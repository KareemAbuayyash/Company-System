using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Common;
using CompanySystem.Data.Context;
using CompanySystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanySystem.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly CompanySystemDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(CompanySystemDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<LoginResultDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.Department)
                    .FirstOrDefaultAsync(u => u.Username == loginDto.Username && !u.IsDeleted);

                if (user == null)
                {
                    return ServiceResult<LoginResultDto>.Failure("Invalid username or password");
                }

                if (!user.IsActive)
                {
                    return ServiceResult<LoginResultDto>.Failure("Account is deactivated");
                }

                if (!VerifyPasswordAsync(loginDto.Password, user.PasswordHash))
                {
                    return ServiceResult<LoginResultDto>.Failure("Invalid username or password");
                }

                // Update last login date
                user.LastLoginDate = DateTime.UtcNow;
                user.UpdatedBy = user.Username;
                user.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var result = new LoginResultDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email,
                    RoleName = user.Role?.RoleName ?? string.Empty,
                    RoleId = user.RoleId,
                    DepartmentName = user.Department?.DepartmentName,
                    DepartmentId = user.DepartmentId
                };

                _logger.LogInformation("User {Username} logged in successfully", user.Username);
                return ServiceResult<LoginResultDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Username}", loginDto.Username);
                return ServiceResult<LoginResultDto>.Failure("An error occurred during login");
            }
        }

        public async Task<ServiceResult<bool>> LogoutAsync()
        {
            try
            {
                _logger.LogInformation("User logged out successfully");
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return ServiceResult<bool>.Failure("An error occurred during logout");
            }
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return ServiceResult<bool>.Failure("User not found");
                }

                if (!VerifyPasswordAsync(currentPassword, user.PasswordHash))
                {
                    return ServiceResult<bool>.Failure("Current password is incorrect");
                }

                user.PasswordHash = HashPasswordAsync(newPassword);
                user.UpdatedBy = user.Username;
                user.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Password changed successfully for user {UserId}", userId);
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return ServiceResult<bool>.Failure("An error occurred while changing password");
            }
        }

        public string HashPasswordAsync(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPasswordAsync(string password, string hash)
        {
            var hashedPassword = HashPasswordAsync(password);
            return hashedPassword == hash;
        }
    }
}
