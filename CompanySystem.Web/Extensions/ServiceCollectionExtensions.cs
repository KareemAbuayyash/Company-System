using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using CompanySystem.Data.Data;
using CompanySystem.Data.Models;
using CompanySystem.Data.Repositories.Generic;
using CompanySystem.Business.Interfaces.Auth;
using CompanySystem.Business.Services.Auth;
using CompanySystem.Business.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CompanySystem.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCompanySystemServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Entity Framework with retry logic
            services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")),
                    mySqlOptions =>
                    {
                        mySqlOptions.MigrationsAssembly("CompanySystem.Data");
                        mySqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                
                // Enable sensitive data logging in development
                if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            // Add Custom Password Hasher
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Add Generic Repository for each entity
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<Role>, GenericRepository<Role>>();
            services.AddScoped<IGenericRepository<Department>, GenericRepository<Department>>();
            services.AddScoped<IGenericRepository<MainPageContent>, GenericRepository<MainPageContent>>();
            services.AddScoped<IGenericRepository<Note>, GenericRepository<Note>>();

            // Add Business Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<InitializationService>();

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
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.Name = "CompanySystem.Auth";
                    
                    // Configure events for better error handling
                    options.Events.OnRedirectToLogin = context =>
                    {
                        // If it's an AJAX request, return 401 instead of redirect
                        if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            context.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                        
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    };
                    
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        // If it's an AJAX request, return 403 instead of redirect
                        if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            context.Response.StatusCode = 403;
                            return Task.CompletedTask;
                        }
                        
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    };
                    
                    // Handle validation failures
                    options.Events.OnValidatePrincipal = async context =>
                    {
                        try
                        {
                            // Add any custom validation logic here
                            // For example, check if user is still active
                            var userIdClaim = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                            {
                                var dbContext = context.HttpContext.RequestServices.GetRequiredService<CompanyDbContext>();
                                var user = await dbContext.Users.FindAsync(userId);
                                
                                if (user == null || !user.IsActive || user.IsDeleted)
                                {
                                    context.RejectPrincipal();
                                    await context.HttpContext.SignOutAsync("CompanySystem");
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // If there's an error validating, reject the principal
                            context.RejectPrincipal();
                        }
                    };
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
            services.AddControllersWithViews(options =>
            {
                // Add global filters
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
                
                // Add custom model binding error handling
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "This field is required.");
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                    _ => "The value entered is not valid.");
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
                    _ => "This field must be a number.");
            });

            // Add session support for temporary data
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "CompanySystem.Session";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            // Add TempData
            services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider, 
                Microsoft.AspNetCore.Mvc.ViewFeatures.CookieTempDataProvider>();

            // Add antiforgery with better configuration
            services.AddAntiforgery(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.HeaderName = "X-CSRF-TOKEN";
                options.Cookie.Name = "CompanySystem.Antiforgery";
                
                // Customize error handling
                options.SuppressXFrameOptionsHeader = false;
            });

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

            // Add security headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
                
                await next();
            });

            // Add custom exception handling middleware
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException)
                {
                    // Handle antiforgery token validation errors
                    context.Response.StatusCode = 400;
                    if (context.Request.Headers["Content-Type"].ToString().Contains("application/json"))
                    {
                        await context.Response.WriteAsync("{\"error\":\"Invalid security token. Please refresh the page and try again.\"}");
                    }
                    else
                    {
                        context.Response.Redirect("/Account/Login?error=InvalidToken");
                    }
                }
                catch (Exception ex)
                {
                    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Unhandled exception occurred");
                    
                    if (!context.Response.HasStarted)
                    {
                        context.Response.StatusCode = 500;
                        if (env.IsDevelopment())
                        {
                            await context.Response.WriteAsync($"Error: {ex.Message}");
                        }
                        else
                        {
                            context.Response.Redirect("/Home/Error");
                        }
                    }
                }
            });

            // Add authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Add session
            app.UseSession();

            // Ensure database is created and migrations are applied
            using (var scope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    
                    logger.LogInformation("Checking database connection...");
                    
                    // Test database connection
                    if (await context.Database.CanConnectAsync())
                    {
                        logger.LogInformation("Database connection successful");
                        await context.Database.MigrateAsync();
                        logger.LogInformation("Database migrations applied successfully");
                        
                        // Initialize application data
                        var initializationService = scope.ServiceProvider.GetRequiredService<InitializationService>();
                        await initializationService.InitializeAsync();
                        logger.LogInformation("Application initialization completed");
                    }
                    else
                    {
                        logger.LogError("Cannot connect to database. Please check your connection string.");
                        throw new InvalidOperationException("Database connection failed");
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing the database");
                    
                    if (env.IsDevelopment())
                    {
                        throw; // Re-throw in development to see the full error
                    }
                    else
                    {
                        // In production, log the error but continue startup
                        logger.LogError("Application started with database initialization errors");
                    }
                }
            }

            return app;
        }
    }
}