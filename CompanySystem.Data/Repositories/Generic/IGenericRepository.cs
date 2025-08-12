
// CompanySystem.Data/Repositories/Generic/IGenericRepository.cs
using System.Linq.Expressions;
using CompanySystem.Data.Models;

namespace CompanySystem.Data.Repositories.Generic
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Basic CRUD methods
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        
        // Include deleted items (bypass query filter)
        Task<T?> GetByIdIncludeDeletedAsync(int id);
        Task<T?> GetFirstOrDefaultIncludeDeletedAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllIncludeDeletedAsync();
        Task<IEnumerable<T>> GetWhereIncludeDeletedAsync(Expression<Func<T, bool>> predicate);
        
        // Count methods
        Task<int> CountAsync();
        Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountIncludeDeletedAsync();
        Task<int> CountWhereIncludeDeletedAsync(Expression<Func<T, bool>> predicate);

        // Existence check
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsIncludeDeletedAsync(Expression<Func<T, bool>> predicate);

        // Add methods
        Task<T> AddAsync(T entity, string? createdBy = null);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, string? createdBy = null);

        // Update methods
        Task<T> UpdateAsync(T entity, string? updatedBy = null);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, string? updatedBy = null);

        // Delete methods (hard delete - not recommended for tracked entities)
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteRangeAsync(IEnumerable<T> entities);

        // Soft delete methods
        Task<bool> SoftDeleteAsync(int id, string? deletedBy = null);
        Task<bool> SoftDeleteAsync(T entity, string? deletedBy = null);
        Task<bool> SoftDeleteRangeAsync(IEnumerable<T> entities, string? deletedBy = null);
        Task<bool> RestoreAsync(int id, string? restoredBy = null);
        Task<bool> RestoreAsync(T entity, string? restoredBy = null);

        // Save changes
        Task<bool> SaveChangesAsync();

        // Includes
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetFirstOrDefaultWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetWhereWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}