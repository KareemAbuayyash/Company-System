using System.Linq.Expressions;

namespace CompanySystem.Data.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        // Basic CRUD methods
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        
        // Count methods
        Task<int> CountAsync();
        Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate);

        // Existence check
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        // Add methods
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        // Update methods
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities);

        // Delete methods
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteRangeAsync(IEnumerable<T> entities);

        // Save changes
        Task<bool> SaveChangesAsync();

        // Generic authentication methods (for entities with Email/SerialNumber properties)
        Task<T?> GetByEmailAsync(string email);
        Task<T?> GetBySerialNumberAsync(string serialNumber);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
        Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeId = null);

        // Generic name-based methods (for entities with Name properties)
        Task<T?> GetByNameAsync(string name);
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);

        // Generic active/inactive filtering
        Task<IEnumerable<T>> GetActiveAsync();
        Task<IEnumerable<T>> GetInactiveAsync();

        // Generic search method (for entities with searchable string properties)
        Task<IEnumerable<T>> SearchAsync(string searchTerm, params Expression<Func<T, object>>[] searchProperties);

        // Generic date range filtering (for entities with date properties)
        Task<IEnumerable<T>> GetByDateRangeAsync(Expression<Func<T, DateTime>> dateProperty, DateTime startDate, DateTime endDate);

        // Generic pagination
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, object>>? orderBy = null, bool ascending = true);
        Task<IEnumerable<T>> GetPagedWhereAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, Expression<Func<T, object>>? orderBy = null, bool ascending = true);

        // Generic includes
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetFirstOrDefaultWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetWhereWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
} 