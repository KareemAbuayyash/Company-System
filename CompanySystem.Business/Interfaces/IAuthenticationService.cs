using CompanySystem.Data.Models;

namespace CompanySystem.Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<bool> ValidateUserAsync(int userId);
        string GeneratePasswordHash(string password);
        bool VerifyPassword(string password, string hash);
    }
} 