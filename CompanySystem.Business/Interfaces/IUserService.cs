using CompanySystem.Data.Models;
using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetFilteredUsersAsync(string? searchTerm = null, string sortBy = "name");
        Task<UserSearchResultDto> GetUsersForIndexAsync(string? searchTerm = null, string sortBy = "name");
        Task<IEnumerable<UserDto>> GetUsersForSearchAsync(string? searchTerm = null, string sortBy = "name");
        Task<User?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserDtoByIdAsync(int id);
        Task<User?> CreateUserAsync(CreateUserDto model, string? createdBy = null);
        Task<User?> UpdateUserAsync(int userId, EditUserDto model, string? updatedBy = null);
        Task<bool> SoftDeleteUserAsync(int id, string deletedBy);
        Task<bool> UserExistsAsync(int id);
        Task<bool> UsernameExistsAsync(string username, int? excludeId = null);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<int> GetUserCountAsync(string? searchTerm = null);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UpdateLastLoginAsync(int userId);
    }
}
