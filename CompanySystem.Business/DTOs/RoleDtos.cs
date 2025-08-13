using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateRoleDto
    {
        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class EditRoleDto
    {
        public int RoleId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }

    public class RoleSearchResultDto
    {
        public IEnumerable<RoleDto> Roles { get; set; } = new List<RoleDto>();
        public int TotalCount { get; set; }
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "name";
        public bool HasSearch { get; set; }
    }
}
