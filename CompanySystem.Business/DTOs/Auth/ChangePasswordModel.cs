
// CompanySystem.Business/DTOs/Auth/ChangePasswordModel.cs
using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs.Auth
{
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}