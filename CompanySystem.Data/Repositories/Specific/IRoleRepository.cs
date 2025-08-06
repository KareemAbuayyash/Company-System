using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Generic;

namespace CompanySystem.Data.Repositories.Specific
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        // Role-specific methods that use RoleName property
        Task<Role?> GetByRoleNameAsync(string roleName);
        Task<bool> IsRoleNameUniqueAsync(string roleName, int? excludeRoleId = null);
        
        // Role with users (specific to Role entity)
        Task<Role?> GetRoleWithUsersAsync(int roleId);
        Task<IEnumerable<Role>> GetRolesWithUsersAsync();
        
        // Statistical methods (specific to Role entity)
        Task<int> GetUserCountByRoleAsync(int roleId);
        Task<Dictionary<string, int>> GetUserCountByAllRolesAsync();
    }
} 