using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CompanySystem.Data.Enums;

namespace CompanySystem.Data.Models
{
    [Table("Notes")]
    public class Note : BaseEntity
    {
        [Required]
        public NoteType NoteType { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = string.Empty;

        // Foreign Keys
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey("CreatedByUser")]
        public int CreatedById { get; set; }

        // Navigation Properties
        public virtual User? Employee { get; set; }

        public virtual User? CreatedByUser { get; set; }
    }
}
