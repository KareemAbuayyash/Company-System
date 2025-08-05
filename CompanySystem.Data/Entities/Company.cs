using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Entities
{
    [Table("Companies")]
    public class Company : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string? CompanyCode { get; set; }

        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string? Website { get; set; }

        [StringLength(255)]
        [EmailAddress]
        [Column(TypeName = "nvarchar(255)")]
        public string? Email { get; set; }

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string? PhoneNumber { get; set; }

        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Address { get; set; }

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string? City { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? State { get; set; }

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Country { get; set; }

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Industry { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AnnualRevenue { get; set; }

        [Column(TypeName = "int")]
        public int? EmployeeCount { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? CompanySize { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FoundedDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Computed Properties
        [NotMapped]
        public string FullAddress => $"{Address}, {City}, {State} {PostalCode}, {Country}".Trim(',', ' ');

        [NotMapped]
        public int? CompanyAge => FoundedDate.HasValue ? DateTime.UtcNow.Year - FoundedDate.Value.Year : null;
    }
} 