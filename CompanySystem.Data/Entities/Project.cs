using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Entities
{
    [Table("Projects")]
    public class Project : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string ProjectName { get; set; } = string.Empty;

        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Active";

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Budget { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? Priority { get; set; } = "Medium";

        [Column(TypeName = "nvarchar(max)")]
        public string? Requirements { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Notes { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Foreign Keys
        [Required]
        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }



        // Computed Properties
        [NotMapped]
        public bool IsCompleted => EndDate.HasValue;

        [NotMapped]
        public TimeSpan? Duration => EndDate.HasValue ? EndDate.Value - StartDate : null;
    }
} 