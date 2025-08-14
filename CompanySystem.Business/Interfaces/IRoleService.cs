using CompanySystem.Data.Models;
using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<IEnumerable<Role>> GetFilteredRolesAsync(string? searchTerm = null, string sortBy = "name");
        Task<RoleSearchResultDto> GetRolesForIndexAsync(string? searchTerm = null, string sortBy = "name");
        Task<IEnumerable<RoleDto>> GetRolesForSearchAsync(string? searchTerm = null, string sortBy = "name");
        Task<Role?> GetRoleByIdAsync(int id);
        Task<RoleDto?> GetRoleDtoByIdAsync(int id);
        Task<Role?> CreateRoleAsync(CreateRoleDto model, string? createdBy = null);
        Task<Role?> UpdateRoleAsync(int roleId, EditRoleDto model, string? updatedBy = null);
        Task<bool> SoftDeleteRoleAsync(int id, string deletedBy);
        Task<bool> RoleExistsAsync(int id);
        Task<bool> RoleNameExistsAsync(string name, int? excludeId = null);
        Task<int> GetRoleCountAsync(string? searchTerm = null);
        Task<IEnumerable<RoleDto>> GetActiveRolesAsync();
    }
}
