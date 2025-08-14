using CompanySystem.Business.DTOs;
using CompanySystem.Business.Common;

namespace CompanySystem.Business.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<LoginResultDto>> LoginAsync(LoginDto loginDto);
        Task<ServiceResult<bool>> LogoutAsync();
        Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        string HashPasswordAsync(string password);
        bool VerifyPasswordAsync(string password, string hash);
    }
}
