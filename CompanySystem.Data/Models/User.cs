// CompanySystem.Data/Models/User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Models
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string SerialNumber { get; set; } = string.Empty;

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

        [StringLength(500)]
        public string? ProfilePhoto { get; set; }

        [Required]
        public DateTime HireDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Skills { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Experience { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Foreign Keys
        [Required]
        public int RoleId { get; set; }

        public int? DepartmentId { get; set; }

        // Navigation Properties
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        // Computed Properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string DisplayName => $"{FullName} ({SerialNumber})";
    }
}
