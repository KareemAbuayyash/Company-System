
// CompanySystem.Business/DTOs/Auth/SetPasswordModel.cs
using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs.Auth
{
    public class SetPasswordModel
    {
        [Required]
        [StringLength(500)]
        public string Token { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}