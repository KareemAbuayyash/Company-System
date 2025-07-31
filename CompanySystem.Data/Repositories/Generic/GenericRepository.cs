using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CompanySystem.Data.Data;

namespace CompanySystem.Data.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CompanyDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CompanyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // Get methods
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
                return await _dbSet.FirstOrDefaultAsync(predicate);
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
                return await _dbSet.ToListAsync();
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
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Get with includes
        public virtual async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                
                // Assuming all entities have an Id property
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, "Id");
                var equal = Expression.Equal(property, Expression.Constant(id));
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);
                
                return await query.FirstOrDefaultAsync(lambda);
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
                return await query.FirstOrDefaultAsync(predicate);
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
                return await query.ToListAsync();
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
                return await query.Where(predicate).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Pagination
        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<IEnumerable<T>> GetPagedWhereAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            try
            {
                return await _dbSet.Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
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

        // Add methods
        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                await _dbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                await _dbSet.AddRangeAsync(entities);
                return entities;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update methods
        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbSet.Update(entity);
                return await Task.FromResult(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                _dbSet.UpdateRange(entities);
                return await Task.FromResult(entities);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Delete methods
        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
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
    }
} 