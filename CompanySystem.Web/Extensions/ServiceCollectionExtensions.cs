using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CompanySystem.Data.Data;
using CompanySystem.Data.Models;
using CompanySystem.Data.Repositories.Generic;
using CompanySystem.Data.Repositories.Specific;
using CompanySystem.Business.Interfaces.Auth;
using CompanySystem.Business.Services.Auth;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CompanySystem.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCompanySystemServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Entity Framework
            services.AddDbContext<CompanyDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")),
                    b => b.MigrationsAssembly("CompanySystem.Data")));

            // Add Identity services
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // Add Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Add Specific Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            // Add Business Services
            services.AddScoped<IAuthService, AuthService>();

            // Add Authentication & Authorization
            services.AddAuthentication("CompanySystem")
                .AddCookie("CompanySystem", options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                });

            services.AddAuthorization(options =>
            {
                // Define authorization policies based on roles
                options.AddPolicy("AdministratorOnly", policy =>
                    policy.RequireClaim("Role", Role.RoleNames.Administrator));
                
                options.AddPolicy("HROnly", policy =>
                    policy.RequireClaim("Role", Role.RoleNames.HR));
                
                options.AddPolicy("LeadOnly", policy =>
                    policy.RequireClaim("Role", Role.RoleNames.Lead));
                
                options.AddPolicy("EmployeeOnly", policy =>
                    policy.RequireClaim("Role", Role.RoleNames.Employee));

                // Combined policies
                options.AddPolicy("AdminOrHR", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("Role", Role.RoleNames.Administrator) ||
                        context.User.HasClaim("Role", Role.RoleNames.HR)));

                options.AddPolicy("AdminOrLead", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("Role", Role.RoleNames.Administrator) ||
                        context.User.HasClaim("Role", Role.RoleNames.Lead)));

                options.AddPolicy("ManagementLevel", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("Role", Role.RoleNames.Administrator) ||
                        context.User.HasClaim("Role", Role.RoleNames.HR) ||
                        context.User.HasClaim("Role", Role.RoleNames.Lead)));
            });

            return services;
        }

        public static IServiceCollection AddCompanySystemMvc(this IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Add session support for temporary data
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add TempData
            services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider, 
                Microsoft.AspNetCore.Mvc.ViewFeatures.CookieTempDataProvider>();

            return services;
        }

        public static async Task<IApplicationBuilder> UseCompanySystemAsync(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            // Ensure database is created and migrations are applied
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();
                await context.Database.MigrateAsync();
            }

            return app;
        }
    }
} 