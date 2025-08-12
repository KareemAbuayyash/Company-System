using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Web.ViewModels
{
    public class EditRoleViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = string.Empty;
    }
}
