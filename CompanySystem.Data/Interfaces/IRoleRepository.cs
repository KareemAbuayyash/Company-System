using CompanySystem.Data.Models;

namespace CompanySystem.Data.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
} 