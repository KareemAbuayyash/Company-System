// CompanySystem.Data/Models/Role.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Models
{
    [Table("Roles")]
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty;

        // Predefined role names as constants
        public static class RoleNames
        {
            public const string Administrator = "Administrator";
            public const string HR = "HR";
            public const string Lead = "Lead";
            public const string Employee = "Employee";
        }
    }
}
