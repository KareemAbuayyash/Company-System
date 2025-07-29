using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using CompanySystem.Data.Interfaces;

namespace CompanySystem.Data.Models
{
    public class Role : IAuditableEntity
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty;

        // Audit Fields
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public int? CreatedById { get; set; }
        
        public int? UpdatedById { get; set; }
        
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

        // Navigation property
        [InverseProperty("Role")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
} 