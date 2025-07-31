using Microsoft.EntityFrameworkCore;
using CompanySystem.Data.Data;
using CompanySystem.Data.Models;
using CompanySystem.Data.Repositories.Generic;

namespace CompanySystem.Data.Repositories.Specific
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(CompanyDbContext context) : base(context)
        {
        }

        // Role-specific methods
        public async Task<Role?> GetByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return null;
            
            return await _dbSet
                .FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.ToLower());
        }

        public async Task<bool> IsRoleNameUniqueAsync(string roleName, int? excludeRoleId = null)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return false;
            
            var query = _dbSet.Where(r => r.RoleName.ToLower() == roleName.ToLower());
            
            if (excludeRoleId.HasValue)
            {
                query = query.Where(r => r.RoleId != excludeRoleId.Value);
            }
            
            return !await query.AnyAsync();
        }

        // Role with users
        public async Task<Role?> GetRoleWithUsersAsync(int roleId)
        {
            return await _dbSet
                .Include(r => r.Users.Where(u => u.IsActive))
                .FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task<IEnumerable<Role>> GetRolesWithUsersAsync()
        {
            return await _dbSet
                .Include(r => r.Users.Where(u => u.IsActive))
                .OrderBy(r => r.RoleName)
                .ToListAsync();
        }

        // Statistical methods
        public async Task<int> GetUserCountByRoleAsync(int roleId)
        {
            return await _context.Users
                .CountAsync(u => u.RoleId == roleId && u.IsActive);
        }

        public async Task<Dictionary<string, int>> GetUserCountByAllRolesAsync()
        {
            return await _dbSet
                .Select(r => new { r.RoleName, UserCount = r.Users.Count(u => u.IsActive) })
                .ToDictionaryAsync(x => x.RoleName, x => x.UserCount);
        }

        // Predefined role helpers
        public async Task<Role?> GetAdministratorRoleAsync()
        {
            return await GetByNameAsync(Role.RoleNames.Administrator);
        }

        public async Task<Role?> GetHRRoleAsync()
        {
            return await GetByNameAsync(Role.RoleNames.HR);
        }

        public async Task<Role?> GetLeadRoleAsync()
        {
            return await GetByNameAsync(Role.RoleNames.Lead);
        }

        public async Task<Role?> GetEmployeeRoleAsync()
        {
            return await GetByNameAsync(Role.RoleNames.Employee);
        }
    }
} 