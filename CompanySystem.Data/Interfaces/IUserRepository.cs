using CompanySystem.Data.Models;

namespace CompanySystem.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        // Only methods that require special logic or can't be easily achieved with predicates
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByEmployeeIdAsync(string employeeId);
    }
}