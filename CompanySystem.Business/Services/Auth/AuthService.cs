// CompanySystem.Business/Services/Auth/AuthService.cs
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CompanySystem.Business.Interfaces.Auth;
using CompanySystem.Business.DTOs.Auth;
using CompanySystem.Data.Models;
using CompanySystem.Data.Repositories.Generic;
using CompanySystem.Data.Data;

namespace CompanySystem.Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IPasswordHasher _passwordHasher;

        private readonly CompanyDbContext _context;

        public AuthService(
            IGenericRepository<User> userRepository,
            IGenericRepository<Role> roleRepository,
            IPasswordHasher passwordHasher,
            CompanyDbContext context)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Authentication methods
        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Email and password are required."
                    };
                }

                // Get active user with navigation properties
                var user = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.Department)
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted);
                
                if (user == null)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }

                if (!user.IsActive)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Account is inactive. Please contact administrator."
                    };
                }

                if (!VerifyPassword(password, user.PasswordHash))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }

                // Generate token (placeholder for now)
                var token = await GenerateTokenAsync(user);

                return new AuthResult
                {
                    Success = true,
                    Message = "Login successful.",
                    User = user,
                    Token = token,
                    TokenExpiry = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }

        public async Task<AuthResult> RegisterAsync(RegisterModel model)
        {
            return await RegisterAsync(model, "System");
        }

        public async Task<AuthResult> RegisterAsync(RegisterModel model, string? createdBy)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.Email) || 
                    string.IsNullOrWhiteSpace(model.Password) ||
                    string.IsNullOrWhiteSpace(model.FirstName) ||
                    string.IsNullOrWhiteSpace(model.LastName) ||
                    string.IsNullOrWhiteSpace(model.SerialNumber))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "All required fields must be provided."
                    };
                }

                // Check if email is unique (including deleted users)
                if (!await IsEmailUniqueAsync(model.Email))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Email address is already in use."
                    };
                }

                // Check if serial number is unique (including deleted users)
                if (!await IsSerialNumberUniqueAsync(model.SerialNumber))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Serial number is already in use."
                    };
                }

                // Validate role exists and is active
                var role = await GetActiveRoleByIdAsync(model.RoleId);
                if (role == null)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Invalid role specified."
                    };
                }

                // Create new user
                var user = new User
                {
                    SerialNumber = model.SerialNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password),
                    PhoneNumber = model.PhoneNumber,
                    RoleId = model.RoleId,
                    DepartmentId = model.DepartmentId,
                    HireDate = model.HireDate,
                    Salary = model.Salary,
                    Skills = model.Skills,
                    Experience = model.Experience,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user, createdBy);
                
                if (await _userRepository.SaveChangesAsync())
                {
                    return new AuthResult
                    {
                        Success = true,
                        Message = "User registered successfully.",
                        User = user
                    };
                }

                return new AuthResult
                {
                    Success = false,
                    Message = "Failed to save user registration."
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            try
            {
                // For now, just validate user exists
                var user = await _userRepository.GetByIdAsync(userId);
                return user != null;
            }
            catch
            {
                return false;
            }
        }

        // Password management
        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            return await ChangePasswordAsync(userId, currentPassword, newPassword, null);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, string? updatedBy)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || !user.IsActive)
                    return false;

                if (!VerifyPassword(currentPassword, user.PasswordHash))
                    return false;

                user.PasswordHash = HashPassword(newPassword);
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user, updatedBy);
                return await _userRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            return await ResetPasswordAsync(email, null);
        }

        public async Task<bool> ResetPasswordAsync(string email, string? updatedBy)
        {
            try
            {
                var user = await GetActiveUserByEmailAsync(email);
                if (user == null || !user.IsActive)
                    return false;

                // Generate temporary password
                var tempPassword = GenerateRandomPassword();
                user.PasswordHash = HashPassword(tempPassword);
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user, updatedBy);
                var result = await _userRepository.SaveChangesAsync();

                // TODO: Send email with temporary password
                
                return result;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetNewPasswordAsync(string token, string newPassword)
        {
            // TODO: Implement token-based password reset
            // For now, return false as this needs token validation logic
            await Task.CompletedTask;
            return false;
        }

        // User validation and retrieval
        public async Task<bool> ValidateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                return user != null && user.IsActive;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUserActiveAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                return user?.IsActive ?? false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> GetCurrentUserAsync(int userId)
        {
            try
            {
                return await _userRepository.GetByIdAsync(userId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userRepository.GetFirstOrDefaultIncludeDeletedAsync(u => u.Email.ToLower() == email.ToLower());
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> GetActiveUserByEmailAsync(string email)
        {
            try
            {
                return await _userRepository.GetFirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> GetActiveUserByIdAsync(int userId)
        {
            try
            {
                return await _userRepository.GetByIdAsync(userId);
            }
            catch
            {
                return null;
            }
        }

        // Role management
        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            try
            {
                return await _roleRepository.GetByIdIncludeDeletedAsync(roleId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Role?> GetActiveRoleByIdAsync(int roleId)
        {
            try
            {
                return await _roleRepository.GetByIdAsync(roleId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Role>> GetAllActiveRolesAsync()
        {
            try
            {
                return await _roleRepository.GetAllAsync();
            }
            catch
            {
                return Enumerable.Empty<Role>();
            }
        }

        // User management with tracking
        public async Task<bool> ActivateUserAsync(int userId, string? updatedBy = null)
        {
            try
            {
                var user = await _userRepository.GetByIdIncludeDeletedAsync(userId);
                if (user == null) return false;

                user.IsActive = true;
                await _userRepository.UpdateAsync(user, updatedBy);
                return await _userRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateUserAsync(int userId, string? updatedBy = null)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                user.IsActive = false;
                await _userRepository.UpdateAsync(user, updatedBy);
                return await _userRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteUserAsync(int userId, string? deletedBy = null)
        {
            try
            {
                var result = await _userRepository.SoftDeleteAsync(userId, deletedBy);
                if (result)
                {
                    await _userRepository.SaveChangesAsync();
                }
                return result;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RestoreUserAsync(int userId, string? restoredBy = null)
        {
            try
            {
                var result = await _userRepository.RestoreAsync(userId, restoredBy);
                if (result)
                {
                    await _userRepository.SaveChangesAsync();
                }
                return result;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(User user, string? updatedBy = null)
        {
            try
            {
                await _userRepository.UpdateAsync(user, updatedBy);
                return await _userRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        // User queries with soft delete awareness
        public async Task<IEnumerable<User>> GetAllActiveUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch
            {
                return Enumerable.Empty<User>();
            }
        }

        public async Task<IEnumerable<User>> GetActiveUsersByRoleAsync(int roleId)
        {
            try
            {
                return await _userRepository.GetWhereAsync(u => u.RoleId == roleId);
            }
            catch
            {
                return Enumerable.Empty<User>();
            }
        }

        public async Task<IEnumerable<User>> GetActiveUsersByDepartmentAsync(int? departmentId)
        {
            try
            {
                return await _userRepository.GetWhereAsync(u => u.DepartmentId == departmentId);
            }
            catch
            {
                return Enumerable.Empty<User>();
            }
        }

        public async Task<int> GetActiveUserCountAsync()
        {
            try
            {
                return await _userRepository.CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetActiveUserCountByRoleAsync(int roleId)
        {
            try
            {
                return await _userRepository.CountWhereAsync(u => u.RoleId == roleId);
            }
            catch
            {
                return 0;
            }
        }

        // Email and serial number validation
        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            try
            {
                if (excludeUserId.HasValue)
                {
                    return !await _userRepository.ExistsIncludeDeletedAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeUserId.Value);
                }
                return !await _userRepository.ExistsIncludeDeletedAsync(u => u.Email.ToLower() == email.ToLower());
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeUserId = null)
        {
            try
            {
                if (excludeUserId.HasValue)
                {
                    return !await _userRepository.ExistsIncludeDeletedAsync(u => u.SerialNumber == serialNumber && u.Id != excludeUserId.Value);
                }
                return !await _userRepository.ExistsIncludeDeletedAsync(u => u.SerialNumber == serialNumber);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsActiveEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            try
            {
                if (excludeUserId.HasValue)
                {
                    return !await _userRepository.ExistsAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeUserId.Value);
                }
                return !await _userRepository.ExistsAsync(u => u.Email.ToLower() == email.ToLower());
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsActiveSerialNumberUniqueAsync(string serialNumber, int? excludeUserId = null)
        {
            try
            {
                if (excludeUserId.HasValue)
                {
                    return !await _userRepository.ExistsAsync(u => u.SerialNumber == serialNumber && u.Id != excludeUserId.Value);
                }
                return !await _userRepository.ExistsAsync(u => u.SerialNumber == serialNumber);
            }
            catch
            {
                return false;
            }
        }

        // Token management (placeholder implementations)
        public async Task<string> GenerateTokenAsync(User user)
        {
            // TODO: Implement JWT token generation
            // For now, return a simple hash-based token
            await Task.CompletedTask;
            
            var tokenData = $"{user.Id}:{user.Email}:{DateTime.UtcNow:yyyyMMddHHmmss}";
            var tokenBytes = Encoding.UTF8.GetBytes(tokenData);
            var hashBytes = SHA256.HashData(tokenBytes);
            
            return Convert.ToBase64String(hashBytes);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            // TODO: Implement JWT token validation
            await Task.CompletedTask;
            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            // TODO: Implement token revocation (blacklist)
            await Task.CompletedTask;
            return !string.IsNullOrWhiteSpace(token);
        }

        // Password utilities
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            return _passwordHasher.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            try
            {
                return _passwordHasher.VerifyPassword(password, hashedPassword);
            }
            catch
            {
                return false;
            }
        }

        public string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            var random = new Random();
            var chars = new char[length];
            
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }
            
            return new string(chars);
        }
    }
}