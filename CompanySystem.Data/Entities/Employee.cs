using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Entities
{
    [Table("Employees")]
    public class Employee : BaseEntity
    {
        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string EmployeeCode { get; set; } = string.Empty;

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

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime HireDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? TerminationDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? Position { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? EmploymentStatus { get; set; } = "Active";

        [Column(TypeName = "nvarchar(max)")]
        public string? Skills { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Experience { get; set; }

        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? ProfilePhoto { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Foreign Keys
        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }



        // Computed Properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string DisplayName => $"{FullName} ({EmployeeCode})";

        [NotMapped]
        public bool IsTerminated => TerminationDate.HasValue;
    }
} 