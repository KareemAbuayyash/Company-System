using CompanySystem.Data.Data;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Enums;
using CompanySystem.Business.Interfaces.Auth;
using Microsoft.EntityFrameworkCore;

namespace CompanySystem.Business.Services
{
    public class InitializationService
    {
        private readonly CompanyDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public InitializationService(CompanyDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task InitializeAsync()
        {
            // Check if admin user already exists
            var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "admin@company.com");
            
            if (adminUser == null)
            {
                // Create admin user with secure password
                var admin = new User
                {
                    SerialNumber = "ADMIN001",
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@company.com",
                    RoleId = 1, // Administrator role
                    HireDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                // Hash the password securely
                admin.PasswordHash = _passwordHasher.HashPassword("Admin123!");

                await _context.Users.AddAsync(admin);
                await _context.SaveChangesAsync();

                // Create main page content
                var mainPageContents = new List<MainPageContent>
                {
                    new MainPageContent
                    {
                        SectionName = SectionName.Overview,
                        Title = "Company Overview",
                        Content = "Welcome to our company management system.",
                        UpdatedById = admin.Id,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    },
                    new MainPageContent
                    {
                        SectionName = SectionName.AboutUs,
                        Title = "About Us",
                        Content = "We are a leading company in our industry.",
                        UpdatedById = admin.Id,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    },
                    new MainPageContent
                    {
                        SectionName = SectionName.Services,
                        Title = "Our Services",
                        Content = "We provide comprehensive business solutions.",
                        UpdatedById = admin.Id,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    }
                };

                await _context.MainPageContent.AddRangeAsync(mainPageContents);
                await _context.SaveChangesAsync();
            }
        }
    }
} 