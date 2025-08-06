using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Generic;

namespace CompanySystem.Data.Repositories.Specific
{
    public interface IUserRepository : IGenericRepository<User>
    {
        // User-specific methods that use specific User properties
        Task<User?> GetByEmailWithRoleAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(int departmentId);
        Task<User?> GetUserWithDetailsAsync(int userId);
        Task<IEnumerable<User>> GetUsersWithDetailsAsync();
        Task<IEnumerable<User>> GetUsersByRoleNameAsync(string roleName);
        
        // Department management (specific to User entity)
        Task<IEnumerable<User>> GetDepartmentManagersAsync();
        Task<User?> GetDepartmentManagerAsync(int departmentId);
        
        // User-specific search and filter
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
        Task<IEnumerable<User>> GetUsersHiredBetweenAsync(DateTime startDate, DateTime endDate);
    }
} 