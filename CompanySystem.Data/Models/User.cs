using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string EmployeeId { get; set; } = string.Empty;

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

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        // Foreign Keys
        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        // Navigation Properties
        public virtual Role Role { get; set; } = null!;
        public virtual Department? Department { get; set; }

        // Navigation Properties for related entities
        public virtual ICollection<Note> NotesAboutEmployee { get; set; } = new HashSet<Note>();
        public virtual ICollection<Note> NotesCreatedByEmployee { get; set; } = new HashSet<Note>();
        public virtual ICollection<MainPageContent> ContentUpdates { get; set; } = new HashSet<MainPageContent>();
        public virtual ICollection<Department> ManagedDepartments { get; set; } = new HashSet<Department>();

        // Computed Properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string DisplayName => $"{FullName} ({EmployeeId})";
    }
} 