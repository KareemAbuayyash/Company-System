using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Entities
{
    [Table("Tasks")]
    public class TaskItem : BaseEntity
    {
        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string TaskName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(7);

        [Column(TypeName = "datetime2")]
        public DateTime? CompletedDate { get; set; }

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Pending";

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Priority { get; set; } = "Medium";

        [Column(TypeName = "decimal(18,2)")]
        public decimal? EstimatedHours { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualHours { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Notes { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Foreign Keys
        [Required]
        [ForeignKey("AssignedTo")]
        public int AssignedToId { get; set; }

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }

        [ForeignKey("CreatedBy")]
        public int CreatedById { get; set; }



        // Computed Properties
        [NotMapped]
        public bool IsCompleted => CompletedDate.HasValue;

        [NotMapped]
        public bool IsOverdue => !IsCompleted && DueDate < DateTime.UtcNow;

        [NotMapped]
        public TimeSpan? Duration => CompletedDate.HasValue ? CompletedDate.Value - CreatedDate : null;
    }
} 