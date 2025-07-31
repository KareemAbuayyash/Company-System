using System.Linq.Expressions;

namespace CompanySystem.Data.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        // Get methods
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        
        // Get with includes
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetFirstOrDefaultWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetWhereWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        // Pagination
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<T>> GetPagedWhereAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);

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
    }
} 