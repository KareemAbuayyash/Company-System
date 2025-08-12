
// CompanySystem.Business/DTOs/Auth/LoginModel.cs
using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs.Auth
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }
}