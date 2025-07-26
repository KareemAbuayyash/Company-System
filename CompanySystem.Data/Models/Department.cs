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

        // Foreign Key to Users table (will be created by your team)
        public int? ManagerId { get; set; }
    }
}
