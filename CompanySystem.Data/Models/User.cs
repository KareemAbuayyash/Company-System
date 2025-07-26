using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using CompanySystem.Data.Interfaces;

namespace CompanySystem.Data.Models
{
    public class User : IAuditableEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? ProfilePhoto { get; set; }

        public DateTime HireDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [StringLength(1000)]
        public string? Skills { get; set; }

        [StringLength(2000)]
        public string? Experience { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        // Audit Fields
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        [StringLength(100)]
        public string? UpdatedBy { get; set; }
        
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

        // Foreign Keys
        public int RoleId { get; set; }
        
        public int? DepartmentId { get; set; }

        // Navigation Properties
        [ForeignKey("RoleId")]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; } = null!;
        
        // Department navigation will be added in Phase 2
    }
} 