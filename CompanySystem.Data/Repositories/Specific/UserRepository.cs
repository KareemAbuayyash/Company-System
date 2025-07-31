using Microsoft.EntityFrameworkCore;
using CompanySystem.Data.Data;
using CompanySystem.Data.Models;
using CompanySystem.Data.Repositories.Generic;

namespace CompanySystem.Data.Repositories.Specific
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CompanyDbContext context) : base(context)
        {
        }

        // Authentication related methods
        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByEmployeeIdAsync(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId)) return null;
            
            return await _dbSet
                .FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
        }

        public async Task<User?> GetByEmailWithRoleAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            
            var query = _dbSet.Where(u => u.Email.ToLower() == email.ToLower());
            
            if (excludeUserId.HasValue)
            {
                query = query.Where(u => u.UserId != excludeUserId.Value);
            }
            
            return !await query.AnyAsync();
        }

        public async Task<bool> IsEmployeeIdUniqueAsync(string employeeId, int? excludeUserId = null)
        {
            if (string.IsNullOrWhiteSpace(employeeId)) return false;
            
            var query = _dbSet.Where(u => u.EmployeeId == employeeId);
            
            if (excludeUserId.HasValue)
            {
                query = query.Where(u => u.UserId != excludeUserId.Value);
            }
            
            return !await query.AnyAsync();
        }

        // User management methods
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
        {
            return await _dbSet
                .Where(u => u.RoleId == roleId && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .Where(u => u.DepartmentId == departmentId && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<User?> GetUserWithDetailsAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Include(u => u.NotesAboutEmployee)
                .Include(u => u.ManagedDepartments)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<IEnumerable<User>> GetUsersWithDetailsAsync()
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Where(u => u.IsActive)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        // Role-based queries using LINQ
        public async Task<IEnumerable<User>> GetAdministratorsAsync()
        {
            return await _dbSet
                .Where(u => u.Role.RoleName == Role.RoleNames.Administrator && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetHRUsersAsync()
        {
            return await _dbSet
                .Where(u => u.Role.RoleName == Role.RoleNames.HR && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetLeadsAsync()
        {
            return await _dbSet
                .Where(u => u.Role.RoleName == Role.RoleNames.Lead && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetEmployeesAsync()
        {
            return await _dbSet
                .Where(u => u.Role.RoleName == Role.RoleNames.Employee && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return Enumerable.Empty<User>();
            
            return await _dbSet
                .Where(u => u.Role.RoleName == roleName && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        // Department management
        public async Task<IEnumerable<User>> GetDepartmentManagersAsync()
        {
            return await _dbSet
                .Where(u => u.ManagedDepartments.Any() && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Include(u => u.ManagedDepartments)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<User?> GetDepartmentManagerAsync(int departmentId)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.ManagedDepartments.Any(d => d.DepartmentId == departmentId) && u.IsActive);
        }

        // Search and filter
        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return Enumerable.Empty<User>();
            
            var lowerSearchTerm = searchTerm.ToLower();
            
            return await _dbSet
                .Where(u => u.IsActive && (
                    u.FirstName.ToLower().Contains(lowerSearchTerm) ||
                    u.LastName.ToLower().Contains(lowerSearchTerm) ||
                    u.Email.ToLower().Contains(lowerSearchTerm) ||
                    u.EmployeeId.ToLower().Contains(lowerSearchTerm)
                ))
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersHiredBetweenAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(u => u.HireDate >= startDate && u.HireDate <= endDate && u.IsActive)
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.HireDate)
                .ToListAsync();
        }
    }
} 