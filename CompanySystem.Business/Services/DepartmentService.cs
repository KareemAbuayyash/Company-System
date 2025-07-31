using Microsoft.EntityFrameworkCore;
using CompanySystem.Business.Interfaces;
using CompanySystem.Data.Context;
using CompanySystem.Data.Models;

namespace CompanySystem.Business.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly CompanySystemDbContext _context;

        public DepartmentService(CompanySystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetFilteredDepartmentsAsync(string searchTerm = null, string sortBy = "name")
        {
            var query = _context.Departments.AsQueryable();

            // Apply search filter at database level - now works from first character
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var trimmedSearchTerm = searchTerm.Trim();
                query = query.Where(d => 
                    EF.Functions.Like(d.DepartmentName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(d.CreatedBy, $"%{trimmedSearchTerm}%"));
            }

            // Apply sorting at database level
            query = sortBy?.ToLower() switch
            {
                "name" => query.OrderBy(d => d.DepartmentName),
                "name_desc" => query.OrderByDescending(d => d.DepartmentName),
                "date" => query.OrderBy(d => d.CreatedDate),
                "date_desc" => query.OrderByDescending(d => d.CreatedDate),
                "creator" => query.OrderBy(d => d.CreatedBy),
                "creator_desc" => query.OrderByDescending(d => d.CreatedBy),
                _ => query.OrderBy(d => d.DepartmentName)
            };

            return await query.ToListAsync();
        }

        public async Task<int> GetDepartmentCountAsync(string searchTerm = null)
        {
            var query = _context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var trimmedSearchTerm = searchTerm.Trim();
                query = query.Where(d => 
                    EF.Functions.Like(d.DepartmentName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(d.CreatedBy, $"%{trimmedSearchTerm}%"));
            }

            return await query.CountAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            department.CreatedDate = DateTime.UtcNow;
            department.IsDeleted = false;

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == department.DepartmentId);

            if (existingDepartment == null)
                throw new InvalidOperationException("Department not found");

            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.UpdatedBy = department.UpdatedBy;
            existingDepartment.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingDepartment;
        }

        public async Task<bool> SoftDeleteDepartmentAsync(int id, string deletedBy)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
                return false;

            department.IsDeleted = true;
            department.UpdatedBy = deletedBy;
            department.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DepartmentExistsAsync(int id)
        {
            return await _context.Departments
                .AnyAsync(d => d.DepartmentId == id);
        }

        public async Task<bool> DepartmentNameExistsAsync(string name, int? excludeId = null)
        {
            var query = _context.Departments.Where(d => d.DepartmentName.ToLower() == name.ToLower());
            
            if (excludeId.HasValue)
                query = query.Where(d => d.DepartmentId != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
