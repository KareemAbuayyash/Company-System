using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CompanySystem.Data.Enums;

namespace CompanySystem.Data.Models
{
    [Table("Notes")]
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }

        [Required]
        public NoteType NoteType { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        // Foreign Keys
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey("CreatedBy")]
        public int CreatedById { get; set; }

        // Navigation Properties
        public virtual User Employee { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }
} 