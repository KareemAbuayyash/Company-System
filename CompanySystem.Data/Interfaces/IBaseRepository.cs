using System.Linq.Expressions;

namespace CompanySystem.Data.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        // Basic CRUD operations
        Task<T?> GetByIdAsync<TKey>(TKey id) where TKey : struct;
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteByIdAsync<TKey>(TKey id) where TKey : struct;
        
        // Query operations
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        
        // Soft delete operations (for auditable entities)
        Task SoftDeleteAsync(T entity);
        Task SoftDeleteByIdAsync<TKey>(TKey id) where TKey : struct;
        Task RestoreAsync(T entity);
        Task RestoreByIdAsync<TKey>(TKey id) where TKey : struct;
    }
}