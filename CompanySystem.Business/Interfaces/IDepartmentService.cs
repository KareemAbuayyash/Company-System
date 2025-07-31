using CompanySystem.Data.Models;

namespace CompanySystem.Business.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department> UpdateDepartmentAsync(Department department);
        Task<bool> SoftDeleteDepartmentAsync(int id, string deletedBy);
        Task<bool> DepartmentExistsAsync(int id);
        Task<bool> DepartmentNameExistsAsync(string name, int? excludeId = null);
    }
}
