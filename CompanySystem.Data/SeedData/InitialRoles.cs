using CompanySystem.Data.Models;

namespace CompanySystem.Data.SeedData
{
    public static class InitialRoles
    {
        public static List<Role> GetInitialRoles()
        {
            return new List<Role>
            {
                new Role 
                { 
                    RoleId = 1, 
                    RoleName = "Administrator",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Role 
                { 
                    RoleId = 2, 
                    RoleName = "HR",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Role 
                { 
                    RoleId = 3, 
                    RoleName = "Lead",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Role 
                { 
                    RoleId = 4, 
                    RoleName = "Employee",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            };
        }
    }
} 