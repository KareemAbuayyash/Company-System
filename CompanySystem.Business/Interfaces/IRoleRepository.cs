using CompanySystem.Data.Models;

namespace CompanySystem.Business.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
} 