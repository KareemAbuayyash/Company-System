// CompanySystem.Data/Repositories/Generic/GenericRepository.cs
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CompanySystem.Data.Data;
using CompanySystem.Data.Models;

namespace CompanySystem.Data.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly CompanyDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CompanyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // Basic CRUD methods (uses query filter - excludes deleted)
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Include deleted methods (bypass query filter)
        public virtual async Task<T?> GetByIdIncludeDeletedAsync(int id)
        {
            try
            {
                return await _dbSet.IgnoreQueryFilters().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<T?> GetFirstOrDefaultIncludeDeletedAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllIncludeDeletedAsync()
        {
            try
            {
                return await _dbSet.IgnoreQueryFilters().AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<IEnumerable<T>> GetWhereIncludeDeletedAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.IgnoreQueryFilters().AsNoTracking().Where(predicate).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Count methods
        public virtual async Task<int> CountAsync()
        {
            try
            {
                return await _dbSet.CountAsync();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public virtual async Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.CountAsync(predicate);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public virtual async Task<int> CountIncludeDeletedAsync()
        {
            try
            {
                return await _dbSet.IgnoreQueryFilters().CountAsync();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public virtual async Task<int> CountWhereIncludeDeletedAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.IgnoreQueryFilters().CountAsync(predicate);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // Existence check
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> ExistsIncludeDeletedAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.IgnoreQueryFilters().AnyAsync(predicate);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Add methods
        public virtual async Task<T> AddAsync(T entity, string? createdBy = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                SetAuditFields(entity, createdBy, false);
                await _dbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, string? createdBy = null)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                var entityList = entities.ToList();
                foreach (var entity in entityList)
                {
                    SetAuditFields(entity, createdBy, false);
                }
                await _dbSet.AddRangeAsync(entityList);
                return entityList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update methods
        public virtual async Task<T> UpdateAsync(T entity, string? updatedBy = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                SetAuditFields(entity, updatedBy, true);
                _dbSet.Update(entity);
                return await Task.FromResult(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, string? updatedBy = null)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                var entityList = entities.ToList();
                foreach (var entity in entityList)
                {
                    SetAuditFields(entity, updatedBy, true);
                }
                _dbSet.UpdateRange(entityList);
                return await Task.FromResult(entityList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Delete methods (hard delete)
        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdIncludeDeletedAsync(id);
                if (entity == null) return false;

                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null) return false;

            try
            {
                _dbSet.Remove(entity);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any()) return false;

            try
            {
                _dbSet.RemoveRange(entities);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Soft delete methods
        public virtual async Task<bool> SoftDeleteAsync(int id, string? deletedBy = null)
        {
            try
            {
                var entity = await GetByIdIncludeDeletedAsync(id);
                if (entity == null) return false;

                SetSoftDeleteFields(entity, deletedBy, true);
                _dbSet.Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> SoftDeleteAsync(T entity, string? deletedBy = null)
        {
            if (entity == null) return false;

            try
            {
                SetSoftDeleteFields(entity, deletedBy, true);
                _dbSet.Update(entity);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> SoftDeleteRangeAsync(IEnumerable<T> entities, string? deletedBy = null)
        {
            if (entities == null || !entities.Any()) return false;

            try
            {
                var entityList = entities.ToList();
                foreach (var entity in entityList)
                {
                    SetSoftDeleteFields(entity, deletedBy, true);
                }
                _dbSet.UpdateRange(entityList);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> RestoreAsync(int id, string? restoredBy = null)
        {
            try
            {
                var entity = await GetByIdIncludeDeletedAsync(id);
                if (entity == null) return false;

                SetSoftDeleteFields(entity, restoredBy, false);
                _dbSet.Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> RestoreAsync(T entity, string? restoredBy = null)
        {
            if (entity == null) return false;

            try
            {
                SetSoftDeleteFields(entity, restoredBy, false);
                _dbSet.Update(entity);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Save changes
        public virtual async Task<bool> SaveChangesAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Include methods
        public virtual async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                
                return await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<T?> GetFirstOrDefaultWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.AsNoTracking().FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<IEnumerable<T>> GetWhereWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.AsNoTracking().Where(predicate).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Helper methods
        private void SetAuditFields(T entity, string? userBy, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(userBy)) return;

            if (isUpdate)
            {
                entity.UpdatedBy = userBy;
                entity.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                entity.CreatedBy = userBy;
                entity.CreatedDate = DateTime.UtcNow;
            }
        }

        private void SetSoftDeleteFields(T entity, string? userBy, bool isDeleted)
        {
            entity.IsDeleted = isDeleted;
            if (!string.IsNullOrWhiteSpace(userBy))
            {
                entity.UpdatedBy = userBy;
                entity.UpdatedDate = DateTime.UtcNow;
            }
        }
    }
}