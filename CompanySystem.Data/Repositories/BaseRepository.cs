using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CompanySystem.Data.Interfaces;
using CompanySystem.Data.Data;

namespace CompanySystem.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly CompanyDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(CompanyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Basic CRUD operations
        public virtual async Task<T?> GetByIdAsync<TKey>(TKey id) where TKey : struct
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            // For auditable entities, exclude deleted by default
            if (typeof(IAuditableEntity).IsAssignableFrom(typeof(T)))
            {
                return await _dbSet.Where(GetNotDeletedPredicate()).ToListAsync();
            }
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            // Set audit fields for new entities
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.CreatedAt = DateTime.UtcNow;
                auditableEntity.IsDeleted = false;
            }

            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            // Set audit fields for new entities
            foreach (var entity in entities)
            {
                if (entity is IAuditableEntity auditableEntity)
                {
                    auditableEntity.CreatedAt = DateTime.UtcNow;
                    auditableEntity.IsDeleted = false;
                }
            }

            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            // Set audit fields for updated entities
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.UpdatedAt = DateTime.UtcNow;
            }

            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteByIdAsync<TKey>(TKey id) where TKey : struct
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        // Query operations
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            
            return await _dbSet.CountAsync(predicate);
        }

        // Soft delete operations
        public virtual async Task SoftDeleteAsync(T entity)
        {
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.IsDeleted = true;
                auditableEntity.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(entity);
            }
            else
            {
                await DeleteAsync(entity);
            }
        }

        public virtual async Task SoftDeleteByIdAsync<TKey>(TKey id) where TKey : struct
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await SoftDeleteAsync(entity);
            }
        }

        public virtual async Task RestoreAsync(T entity)
        {
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.IsDeleted = false;
                auditableEntity.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(entity);
            }
        }

        public virtual async Task RestoreByIdAsync<TKey>(TKey id) where TKey : struct
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await RestoreAsync(entity);
            }
        }

        // Helper method for filtering out deleted entities
        protected Expression<Func<T, bool>> GetNotDeletedPredicate()
        {
            return entity => !((IAuditableEntity)entity).IsDeleted;
        }
    }
}