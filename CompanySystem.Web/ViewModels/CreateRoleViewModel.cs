using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Web.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = string.Empty;
    }
}
