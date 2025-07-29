using CompanySystem.Data.Models;

namespace CompanySystem.Data.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        // Only methods that require special logic or can't be easily achieved with predicates
        Task<Role?> GetByNameAsync(string roleName);
    }
}