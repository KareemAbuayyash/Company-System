using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Data.Models
{
    public class Department : BaseEntity
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(200)]
        public string DepartmentName { get; set; } = string.Empty;

        public int? ManagerId { get; set; }
    }
}
