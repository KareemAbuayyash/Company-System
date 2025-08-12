
// CompanySystem.Business/Services/InitializationService.cs
using CompanySystem.Data.Data;
using CompanySystem.Data.Models;
using CompanySystem.Business.Interfaces.Auth;
using CompanySystem.Data.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace CompanySystem.Business.Services
{
    public class InitializationService
    {
        private readonly CompanyDbContext _context;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public InitializationService(
            CompanyDbContext context,
            IGenericRepository<User> userRepository,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task InitializeAsync()
        {
            // Check if admin user already exists (including deleted ones)
            var adminUser = await _userRepository.GetFirstOrDefaultIncludeDeletedAsync(u => u.Email == "admin@company.com");
            
            if (adminUser == null)
            {
                // This should not happen as admin user is seeded, but just in case
                var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == Role.RoleNames.Administrator);
                
                if (adminRole != null)
                {
                    var admin = new User
                    {
                        SerialNumber = "ADMIN001",
                        FirstName = "System",
                        LastName = "Administrator",
                        Email = "admin@company.com",
                        PasswordHash = _passwordHasher.HashPassword("Admin123!"),
                        RoleId = adminRole.Id,
                        HireDate = DateTime.UtcNow,
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    await _userRepository.AddAsync(admin, "System");
                    await _userRepository.SaveChangesAsync();
                }
            }
            else
            {
                // Update the seeded admin user with proper password hash if it's still the temp hash
                if (adminUser.PasswordHash == "TEMP_HASH")
                {
                    adminUser.PasswordHash = _passwordHasher.HashPassword("Admin123!");
                    await _userRepository.UpdateAsync(adminUser, "System");
                    await _userRepository.SaveChangesAsync();
                }

                // If admin user exists but is deleted, restore it
                if (adminUser.IsDeleted)
                {
                    await _userRepository.RestoreAsync(adminUser, "System");
                    await _userRepository.SaveChangesAsync();
                }
            }
        }
    }
}