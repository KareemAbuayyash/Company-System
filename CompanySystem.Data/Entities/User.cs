using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        // Primary Key from BaseEntity (Id) will be used for foreign key references
        // SerialNumber is a separate unique identifier, not part of the primary key

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [EmailAddress]
        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string? PhoneNumber { get; set; }

        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? ProfilePhoto { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime HireDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Skills { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Experience { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Foreign Keys
        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        // Navigation Properties
        public virtual Role Role { get; set; } = null!;
        public virtual Department? Department { get; set; }



        public virtual ICollection<Department> ManagedDepartments { get; set; } = new HashSet<Department>();

        // Computed Properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string DisplayName => $"{FullName} ({SerialNumber})";
    }
} 