using Microsoft.EntityFrameworkCore;
using CompanySystem.Business.Interfaces;
using CompanySystem.Data.Context;
using CompanySystem.Data.Models;
using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly CompanySystemDbContext _context;

        public RoleService(CompanySystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.RoleName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetFilteredRolesAsync(string? searchTerm = null, string sortBy = "name")
        {
            var query = _context.Roles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var trimmedSearchTerm = searchTerm.Trim();
                query = query.Where(r => 
                    EF.Functions.Like(r.RoleName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(r.Description, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(r.CreatedBy, $"%{trimmedSearchTerm}%"));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => query.OrderBy(r => r.RoleName),
                "name_desc" => query.OrderByDescending(r => r.RoleName),
                "date" => query.OrderBy(r => r.CreatedDate),
                "date_desc" => query.OrderByDescending(r => r.CreatedDate),
                "creator" => query.OrderBy(r => r.CreatedBy),
                "creator_desc" => query.OrderByDescending(r => r.CreatedBy),
                _ => query.OrderBy(r => r.RoleName)
            };

            return await query.ToListAsync();
        }

        public async Task<RoleSearchResultDto> GetRolesForIndexAsync(string? searchTerm = null, string sortBy = "name")
        {
            var roles = await GetFilteredRolesAsync(searchTerm, sortBy);
            
            var roleDtos = roles.Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description,
                IsActive = r.IsActive,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                UpdatedBy = r.UpdatedBy,
                UpdatedDate = r.UpdatedDate
            }).ToList();

            return new RoleSearchResultDto
            {
                Roles = roleDtos,
                TotalCount = roleDtos.Count,
                SearchTerm = searchTerm,
                SortBy = sortBy,
                HasSearch = !string.IsNullOrWhiteSpace(searchTerm)
            };
        }

        public async Task<IEnumerable<RoleDto>> GetRolesForSearchAsync(string? searchTerm = null, string sortBy = "name")
        {
            var roles = await GetFilteredRolesAsync(searchTerm, sortBy);
            
            return roles.Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description,
                IsActive = r.IsActive,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                UpdatedBy = r.UpdatedBy,
                UpdatedDate = r.UpdatedDate
            }).ToList();
        }

        public async Task<RoleDto?> GetRoleDtoByIdAsync(int id)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
                return null;

            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Description = role.Description,
                IsActive = role.IsActive,
                CreatedBy = role.CreatedBy,
                CreatedDate = role.CreatedDate,
                UpdatedBy = role.UpdatedBy,
                UpdatedDate = role.UpdatedDate
            };
        }

        public async Task<int> GetRoleCountAsync(string? searchTerm = null)
        {
            var query = _context.Roles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var trimmedSearchTerm = searchTerm.Trim();
                query = query.Where(r => 
                    EF.Functions.Like(r.RoleName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(r.Description, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(r.CreatedBy, $"%{trimmedSearchTerm}%"));
            }

            return await query.CountAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task<Role?> CreateRoleAsync(CreateRoleDto model, string? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(model.RoleName))
                throw new ArgumentNullException(nameof(model.RoleName));

            if (await RoleNameExistsAsync(model.RoleName))
                return null;

            var role = new Role
            {
                RoleName = model.RoleName,
                Description = model.Description,
                IsActive = model.IsActive,
                CreatedBy = createdBy ?? "System",
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role?> UpdateRoleAsync(int roleId, EditRoleDto model, string? updatedBy = null)
        {
            if (string.IsNullOrWhiteSpace(model.RoleName))
                throw new ArgumentNullException(nameof(model.RoleName));

            if (await RoleNameExistsAsync(model.RoleName, roleId))
                return null;

            var existingRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (existingRole == null)
                throw new InvalidOperationException("Role not found");

            existingRole.RoleName = model.RoleName;
            existingRole.Description = model.Description;
            existingRole.IsActive = model.IsActive;
            existingRole.UpdatedBy = updatedBy ?? "System";
            existingRole.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingRole;
        }

        public async Task<bool> SoftDeleteRoleAsync(int id, string deletedBy)
        {
            try
            {
                var role = await _context.Roles
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(r => r.RoleId == id);

                if (role == null)
                    return false;

                // Check if there are any users assigned to this role
                var usersWithRole = await _context.Users
                    .IgnoreQueryFilters()
                    .Where(u => u.RoleId == id && u.IsDeleted == false)
                    .CountAsync();

                if (usersWithRole > 0)
                {
                    // Cannot delete role because it has users assigned
                    return false;
                }

                role.IsDeleted = true;
                role.UpdatedBy = deletedBy;
                role.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"Role deletion error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> RoleExistsAsync(int id)
        {
            return await _context.Roles
                .IgnoreQueryFilters()
                .AnyAsync(r => r.RoleId == id);
        }

        public async Task<bool> RoleNameExistsAsync(string name, int? excludeId = null)
        {
            var query = _context.Roles.Where(r => r.RoleName.ToLower() == name.ToLower());
            
            if (excludeId.HasValue)
                query = query.Where(r => r.RoleId != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<RoleDto>> GetActiveRolesAsync()
        {
            var roles = await _context.Roles
                .Where(r => r.IsActive)
                .OrderBy(r => r.RoleName)
                .ToListAsync();

            return roles.Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description,
                IsActive = r.IsActive,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                UpdatedBy = r.UpdatedBy,
                UpdatedDate = r.UpdatedDate
            }).ToList();
        }
    }
}
