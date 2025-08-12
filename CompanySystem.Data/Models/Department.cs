using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Models
{
    [Table("Departments")]
    public class Department : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string DepartmentName { get; set; } = string.Empty;

        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        // Foreign Key
        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }
        
    }
}
