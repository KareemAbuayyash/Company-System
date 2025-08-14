using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Data.Models
{
    public class Role : TrackingEntity
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

    }
}
