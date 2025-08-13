using Microsoft.EntityFrameworkCore;
using CompanySystem.Business.Interfaces;
using CompanySystem.Data.Context;
using CompanySystem.Data.Models;
using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Services
{
    public class UserService : IUserService
    {
        private readonly CompanySystemDbContext _context;
        private readonly IAuthService _authService;

        public UserService(CompanySystemDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetFilteredUsersAsync(string? searchTerm = null, string sortBy = "name")
        {
            var query = _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var trimmedSearchTerm = searchTerm.Trim();
                query = query.Where(u => 
                    EF.Functions.Like(u.Username, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.Email, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.FirstName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.LastName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.PhoneNumber, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.Role.RoleName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.Department.DepartmentName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.CreatedBy, $"%{trimmedSearchTerm}%"));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => query.OrderBy(u => u.FirstName).ThenBy(u => u.LastName),
                "name_desc" => query.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName),
                "username" => query.OrderBy(u => u.Username),
                "username_desc" => query.OrderByDescending(u => u.Username),
                "email" => query.OrderBy(u => u.Email),
                "email_desc" => query.OrderByDescending(u => u.Email),
                "role" => query.OrderBy(u => u.Role.RoleName),
                "role_desc" => query.OrderByDescending(u => u.Role.RoleName),
                "department" => query.OrderBy(u => u.Department.DepartmentName),
                "department_desc" => query.OrderByDescending(u => u.Department.DepartmentName),
                "date" => query.OrderBy(u => u.CreatedDate),
                "date_desc" => query.OrderByDescending(u => u.CreatedDate),
                "creator" => query.OrderBy(u => u.CreatedBy),
                "creator_desc" => query.OrderByDescending(u => u.CreatedBy),
                _ => query.OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
            };

            return await query.ToListAsync();
        }

        public async Task<UserSearchResultDto> GetUsersForIndexAsync(string? searchTerm = null, string sortBy = "name")
        {
            var users = await GetFilteredUsersAsync(searchTerm, sortBy);
            
            var userDtos = users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive,
                LastLoginDate = u.LastLoginDate,
                RoleId = u.RoleId,
                RoleName = u.Role?.RoleName ?? string.Empty,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department?.DepartmentName,
                FullName = u.FullName,
                CreatedBy = u.CreatedBy,
                CreatedDate = u.CreatedDate,
                UpdatedBy = u.UpdatedBy,
                UpdatedDate = u.UpdatedDate
            }).ToList();

            return new UserSearchResultDto
            {
                Users = userDtos,
                TotalCount = userDtos.Count,
                SearchTerm = searchTerm,
                SortBy = sortBy,
                HasSearch = !string.IsNullOrWhiteSpace(searchTerm)
            };
        }

        public async Task<IEnumerable<UserDto>> GetUsersForSearchAsync(string? searchTerm = null, string sortBy = "name")
        {
            var users = await GetFilteredUsersAsync(searchTerm, sortBy);
            
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive,
                LastLoginDate = u.LastLoginDate,
                RoleId = u.RoleId,
                RoleName = u.Role?.RoleName ?? string.Empty,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department?.DepartmentName,
                FullName = u.FullName,
                CreatedBy = u.CreatedBy,
                CreatedDate = u.CreatedDate,
                UpdatedBy = u.UpdatedBy,
                UpdatedDate = u.UpdatedDate
            }).ToList();
        }

        public async Task<UserDto?> GetUserDtoByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return null;

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                RoleId = user.RoleId,
                RoleName = user.Role?.RoleName ?? string.Empty,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.DepartmentName,
                FullName = user.FullName,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                UpdatedBy = user.UpdatedBy,
                UpdatedDate = user.UpdatedDate
            };
        }

        public async Task<int> GetUserCountAsync(string? searchTerm = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var trimmedSearchTerm = searchTerm.Trim();
                query = query.Where(u => 
                    EF.Functions.Like(u.Username, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.Email, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.FirstName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.LastName, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.PhoneNumber, $"%{trimmedSearchTerm}%") ||
                    EF.Functions.Like(u.CreatedBy, $"%{trimmedSearchTerm}%"));
            }

            return await query.CountAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .IgnoreQueryFilters()
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> CreateUserAsync(CreateUserDto model, string? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                throw new ArgumentNullException(nameof(model.Username));

            if (string.IsNullOrWhiteSpace(model.Email))
                throw new ArgumentNullException(nameof(model.Email));

            if (string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentNullException(nameof(model.Password));

            if (await UsernameExistsAsync(model.Username))
                return null;

            if (await EmailExistsAsync(model.Email))
                return null;

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = _authService.HashPasswordAsync(model.Password),
                IsActive = model.IsActive,
                LastLoginDate = DateTime.UtcNow,
                RoleId = model.RoleId,
                DepartmentId = model.DepartmentId,
                CreatedBy = createdBy ?? "System",
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUserAsync(int userId, EditUserDto model, string? updatedBy = null)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                throw new ArgumentNullException(nameof(model.Username));

            if (string.IsNullOrWhiteSpace(model.Email))
                throw new ArgumentNullException(nameof(model.Email));

            if (await UsernameExistsAsync(model.Username, userId))
                return null;

            if (await EmailExistsAsync(model.Email, userId))
                return null;

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (existingUser == null)
                throw new InvalidOperationException("User not found");

            existingUser.Username = model.Username;
            existingUser.Email = model.Email;
            existingUser.FirstName = model.FirstName;
            existingUser.LastName = model.LastName;
            existingUser.PhoneNumber = model.PhoneNumber;
            existingUser.IsActive = model.IsActive;
            existingUser.RoleId = model.RoleId;
            existingUser.DepartmentId = model.DepartmentId;
            existingUser.UpdatedBy = updatedBy ?? "System";
            existingUser.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> SoftDeleteUserAsync(int id, string deletedBy)
        {
            try
            {
                var user = await _context.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                    return false;

                user.IsDeleted = true;
                user.UpdatedBy = deletedBy;
                user.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"User deletion error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users
                .IgnoreQueryFilters()
                .AnyAsync(u => u.UserId == id);
        }

        public async Task<bool> UsernameExistsAsync(string username, int? excludeId = null)
        {
            var query = _context.Users.Where(u => u.Username.ToLower() == username.ToLower());
            
            if (excludeId.HasValue)
                query = query.Where(u => u.UserId != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _context.Users.Where(u => u.Email.ToLower() == email.ToLower());
            
            if (excludeId.HasValue)
                query = query.Where(u => u.UserId != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return false;

            user.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
