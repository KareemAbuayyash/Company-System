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
            return await GetBaseUserQuery()
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetFilteredUsersAsync(string? searchTerm = null, string sortBy = "name")
        {
            var query = ApplySearchFilter(GetBaseUserQuery(), searchTerm);
            query = ApplySorting(query, sortBy);

            return await query.ToListAsync();
        }

        public async Task<UserSearchResultDto> GetUsersForIndexAsync(string? searchTerm = null, string sortBy = "name")
        {
            var users = await GetFilteredUsersAsync(searchTerm, sortBy);
            var userDtos = users.Select(MapToUserDto).ToList();

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
            return users.Select(MapToUserDto).ToList();
        }

        public async Task<UserDto?> GetUserDtoByIdAsync(int id)
        {
            var user = await GetBaseUserQuery()
                .FirstOrDefaultAsync(u => u.UserId == id);

            return user == null ? null : MapToUserDto(user);
        }

        public async Task<int> GetUserCountAsync(string? searchTerm = null)
        {
            var query = ApplySearchFilter(_context.Users.AsQueryable(), searchTerm);
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
            ValidateRequiredFields(model.Username, model.Email, model.Password);

            if (await UsernameExistsAsync(model.Username) || await EmailExistsAsync(model.Email))
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
            ValidateRequiredFields(model.Username, model.Email);

            if (await UsernameExistsAsync(model.Username, userId) || await EmailExistsAsync(model.Email, userId))
                return null;

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (existingUser == null)
                throw new InvalidOperationException("User not found");

            UpdateUserProperties(existingUser, model, updatedBy);
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
            return await CheckFieldExistsAsync(u => u.Username.ToLower() == username.ToLower(), excludeId);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await CheckFieldExistsAsync(u => u.Email.ToLower() == email.ToLower(), excludeId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await GetBaseUserQuery()
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await GetBaseUserQuery()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return false;

            user.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        // Private helper methods to eliminate duplication
        private IQueryable<User> GetBaseUserQuery()
        {
            return _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department);
        }

        private static IQueryable<User> ApplySearchFilter(IQueryable<User> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return query;

            var trimmedSearchTerm = searchTerm.Trim();
            return query.Where(u =>
                EF.Functions.Like(u.Username, $"%{trimmedSearchTerm}%") ||
                EF.Functions.Like(u.Email, $"%{trimmedSearchTerm}%") ||
                EF.Functions.Like(u.FirstName, $"%{trimmedSearchTerm}%") ||
                EF.Functions.Like(u.LastName, $"%{trimmedSearchTerm}%") ||
                EF.Functions.Like(u.PhoneNumber, $"%{trimmedSearchTerm}%") ||
                EF.Functions.Like(u.Role.RoleName, $"%{trimmedSearchTerm}%") ||
                EF.Functions.Like(u.Department.DepartmentName, $"%{trimmedSearchTerm}%") ||
                EF.Functions.Like(u.CreatedBy, $"%{trimmedSearchTerm}%"));
        }

        private static IQueryable<User> ApplySorting(IQueryable<User> query, string sortBy)
        {
            return sortBy?.ToLower() switch
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
        }

        private static UserDto MapToUserDto(User user)
        {
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

        private static void ValidateRequiredFields(string username, string email, string? password = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (password != null && string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));
        }

        private async Task<bool> CheckFieldExistsAsync(System.Linq.Expressions.Expression<Func<User, bool>> predicate, int? excludeId = null)
        {
            var query = _context.Users.Where(predicate);

            if (excludeId.HasValue)
                query = query.Where(u => u.UserId != excludeId.Value);

            return await query.AnyAsync();
        }

        private static void UpdateUserProperties(User existingUser, EditUserDto model, string? updatedBy)
        {
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
        }
    }
}
