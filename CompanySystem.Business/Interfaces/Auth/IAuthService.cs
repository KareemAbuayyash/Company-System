using CompanySystem.Data.Models;

namespace CompanySystem.Business.Interfaces.Auth
{
    public interface IAuthService
    {
        // Authentication methods
        Task<AuthResult> LoginAsync(string email, string password);
        Task<AuthResult> RegisterAsync(RegisterModel model);
        Task<bool> LogoutAsync(int userId);
        
        // Password management
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> SetNewPasswordAsync(string token, string newPassword);
        
        // User validation
        Task<bool> ValidateUserAsync(int userId);
        Task<bool> IsUserActiveAsync(int userId);
        Task<User?> GetCurrentUserAsync(int userId);
        
        // Token management (for future JWT implementation)
        Task<string> GenerateTokenAsync(User user);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        
        // Password utilities
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string GenerateRandomPassword(int length = 12);
    }

    // Data Transfer Objects
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? User { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiry { get; set; }
    }

    public class RegisterModel
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
        public decimal? Salary { get; set; }
        public string? Skills { get; set; }
        public string? Experience { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }

    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; } = string.Empty;
    }

    public class SetPasswordModel
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
} 