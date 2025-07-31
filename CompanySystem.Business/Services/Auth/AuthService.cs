using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using CompanySystem.Business.Interfaces.Auth;
using CompanySystem.Data.Models;
using CompanySystem.Data.Repositories.Specific;

namespace CompanySystem.Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
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

                var user = await _userRepository.GetByEmailWithRoleAsync(email);
                
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
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.Email) || 
                    string.IsNullOrWhiteSpace(model.Password) ||
                    string.IsNullOrWhiteSpace(model.FirstName) ||
                    string.IsNullOrWhiteSpace(model.LastName) ||
                    string.IsNullOrWhiteSpace(model.EmployeeId))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "All required fields must be provided."
                    };
                }

                // Check if email is unique
                if (!await _userRepository.IsEmailUniqueAsync(model.Email))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Email address is already in use."
                    };
                }

                // Check if employee ID is unique
                if (!await _userRepository.IsEmployeeIdUniqueAsync(model.EmployeeId))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Employee ID is already in use."
                    };
                }

                // Validate role exists
                var role = await _roleRepository.GetByIdAsync(model.RoleId);
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
                    EmployeeId = model.EmployeeId,
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

                await _userRepository.AddAsync(user);
                
                if (await _userRepository.SaveChangesAsync())
                {
                    // Reload user with role information
                    user = await _userRepository.GetByEmailWithRoleAsync(user.Email);
                    
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
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || !user.IsActive)
                    return false;

                if (!VerifyPassword(currentPassword, user.PasswordHash))
                    return false;

                user.PasswordHash = HashPassword(newPassword);
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);
                return await _userRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null || !user.IsActive)
                    return false;

                // Generate temporary password
                var tempPassword = GenerateRandomPassword();
                user.PasswordHash = HashPassword(tempPassword);
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);
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

        // User validation
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
                return await _userRepository.GetUserWithDetailsAsync(userId);
            }
            catch
            {
                return null;
            }
        }

        // Token management (placeholder implementations)
        public async Task<string> GenerateTokenAsync(User user)
        {
            // TODO: Implement JWT token generation
            // For now, return a simple hash-based token
            await Task.CompletedTask;
            
            var tokenData = $"{user.UserId}:{user.Email}:{DateTime.UtcNow:yyyyMMddHHmmss}";
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

            // Using ASP.NET Identity password hasher
            return _passwordHasher.HashPassword(new User(), password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            try
            {
                var result = _passwordHasher.VerifyHashedPassword(new User(), hashedPassword, password);
                return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
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