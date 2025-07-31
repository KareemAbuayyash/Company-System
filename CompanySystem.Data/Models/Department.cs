using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string DepartmentName { get; set; } = string.Empty;

        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        // Foreign Key
        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        // Navigation Properties
        public virtual User? Manager { get; set; }
        public virtual ICollection<User> Employees { get; set; } = new HashSet<User>();
    }
}
