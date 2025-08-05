using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanySystem.Data.Entities
{
    [Table("Roles")]
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string RoleName { get; set; } = string.Empty;

        // Navigation Properties
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();



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