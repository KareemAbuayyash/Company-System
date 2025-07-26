using CompanySystem.Data.Models;

namespace CompanySystem.Business.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByEmployeeIdAsync(string employeeId);
        Task<IEnumerable<User>> GetByRoleAsync(int roleId);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetDeletedUsersAsync();
        Task<User?> GetByEmailIncludingDeletedAsync(string email);
        Task<User?> GetByEmployeeIdIncludingDeletedAsync(string employeeId);
    }
} 