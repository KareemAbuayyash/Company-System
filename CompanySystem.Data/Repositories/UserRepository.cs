using Microsoft.EntityFrameworkCore;
using CompanySystem.Data.Interfaces;
using CompanySystem.Data.Models;
using CompanySystem.Data.Data;

namespace CompanySystem.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CompanyDbContext context) : base(context)
        {
        }

        // Only methods that require special logic or can't be easily achieved with predicates
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<User?> GetByEmployeeIdAsync(string employeeId)
        {
            return await FirstOrDefaultAsync(u => u.EmployeeId == employeeId && !u.IsDeleted);
        }
    }
}