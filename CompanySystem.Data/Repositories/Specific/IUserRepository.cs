using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Generic;

namespace CompanySystem.Data.Repositories.Specific
{
    public interface IUserRepository : IGenericRepository<User>
    {
        // Authentication related methods
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetBySerialNumberAsync(string serialNumber);
        Task<User?> GetByEmailWithRoleAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
        Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeUserId = null);
        
        // User management methods
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(int departmentId);
        Task<User?> GetUserWithDetailsAsync(int userId);
        Task<IEnumerable<User>> GetUsersWithDetailsAsync();
        

        Task<IEnumerable<User>> GetUsersByRoleNameAsync(string roleName);
        
        // Department management
        Task<IEnumerable<User>> GetDepartmentManagersAsync();
        Task<User?> GetDepartmentManagerAsync(int departmentId);
        
        // Search and filter
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
        Task<IEnumerable<User>> GetUsersHiredBetweenAsync(DateTime startDate, DateTime endDate);
    }
} 