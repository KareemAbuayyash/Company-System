using CompanySystem.Data.Entities;
using CompanySystem.Business.DTOs.Auth;

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
} 