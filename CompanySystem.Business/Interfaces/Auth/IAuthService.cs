
// CompanySystem.Business/Interfaces/Auth/IAuthService.cs
using CompanySystem.Data.Models;
using CompanySystem.Business.DTOs.Auth;

namespace CompanySystem.Business.Interfaces.Auth
{
    public interface IAuthService
    {
        // Authentication methods
        Task<AuthResult> LoginAsync(string email, string password);
        Task<AuthResult> RegisterAsync(RegisterModel model);
        Task<AuthResult> RegisterAsync(RegisterModel model, string? createdBy);
        Task<bool> LogoutAsync(int userId);
        
        // Password management
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, string? updatedBy);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string? updatedBy);
        Task<bool> SetNewPasswordAsync(string token, string newPassword);
        
        // User validation and retrieval
        Task<bool> ValidateUserAsync(int userId);
        Task<bool> IsUserActiveAsync(int userId);
        Task<User?> GetCurrentUserAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetActiveUserByEmailAsync(string email);
        Task<User?> GetActiveUserByIdAsync(int userId);
        
        // Role management
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<Role?> GetActiveRoleByIdAsync(int roleId);
        Task<IEnumerable<Role>> GetAllActiveRolesAsync();
        
        // User management with tracking
        Task<bool> ActivateUserAsync(int userId, string? updatedBy = null);
        Task<bool> DeactivateUserAsync(int userId, string? updatedBy = null);
        Task<bool> SoftDeleteUserAsync(int userId, string? deletedBy = null);
        Task<bool> RestoreUserAsync(int userId, string? restoredBy = null);
        Task<bool> UpdateUserAsync(User user, string? updatedBy = null);
        
        // User queries with soft delete awareness
        Task<IEnumerable<User>> GetAllActiveUsersAsync();
        Task<IEnumerable<User>> GetActiveUsersByRoleAsync(int roleId);
        Task<IEnumerable<User>> GetActiveUsersByDepartmentAsync(int? departmentId);
        Task<int> GetActiveUserCountAsync();
        Task<int> GetActiveUserCountByRoleAsync(int roleId);
        
        // Email and serial number validation (active users only)
        Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
        Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeUserId = null);
        Task<bool> IsActiveEmailUniqueAsync(string email, int? excludeUserId = null);
        Task<bool> IsActiveSerialNumberUniqueAsync(string serialNumber, int? excludeUserId = null);
        
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