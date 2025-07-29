using Microsoft.EntityFrameworkCore;
using CompanySystem.Data.Interfaces;
using CompanySystem.Data.Models;
using CompanySystem.Data.Data;

namespace CompanySystem.Data.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(CompanyDbContext context) : base(context)
        {
        }

        // Only methods that require special logic or can't be easily achieved with predicates
        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await FirstOrDefaultAsync(r => r.RoleName == roleName && !r.IsDeleted);
        }
    }
}