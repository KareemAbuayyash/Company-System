using CompanySystem.Data.Models;
using CompanySystem.Data.Repositories.Generic;

namespace CompanySystem.Data.Repositories.Specific
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        // Role-specific methods
        Task<Role?> GetByNameAsync(string roleName);
        Task<bool> IsRoleNameUniqueAsync(string roleName, int? excludeRoleId = null);
        
        // Role with users
        Task<Role?> GetRoleWithUsersAsync(int roleId);
        Task<IEnumerable<Role>> GetRolesWithUsersAsync();
        
        // Statistical methods
        Task<int> GetUserCountByRoleAsync(int roleId);
        Task<Dictionary<string, int>> GetUserCountByAllRolesAsync();
        
        // Predefined role helpers
        Task<Role?> GetAdministratorRoleAsync();
        Task<Role?> GetHRRoleAsync();
        Task<Role?> GetLeadRoleAsync();
        Task<Role?> GetEmployeeRoleAsync();
    }
} 