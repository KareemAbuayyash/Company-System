
// CompanySystem.Business/DTOs/Auth/ResetPasswordModel.cs
using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs.Auth
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
    }
}