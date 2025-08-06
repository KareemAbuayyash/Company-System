using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CompanySystem.Data.Data;
using System.Reflection;

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

        // Basic CRUD methods
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

        // Generic authentication methods (for entities with Email/SerialNumber properties)
        public virtual async Task<T?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var emailProperty = Expression.Property(parameter, "Email");
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var toLowerCall = Expression.Call(emailProperty, toLowerMethod!);
                var emailConstant = Expression.Constant(email.ToLower());
                var equal = Expression.Equal(toLowerCall, emailConstant);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                return await _dbSet.FirstOrDefaultAsync(lambda);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<T?> GetBySerialNumberAsync(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber)) return null;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var serialNumberProperty = Expression.Property(parameter, "SerialNumber");
                var serialNumberConstant = Expression.Constant(serialNumber);
                var equal = Expression.Equal(serialNumberProperty, serialNumberConstant);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                return await _dbSet.FirstOrDefaultAsync(lambda);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var emailProperty = Expression.Property(parameter, "Email");
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var toLowerCall = Expression.Call(emailProperty, toLowerMethod!);
                var emailConstant = Expression.Constant(email.ToLower());
                var equal = Expression.Equal(toLowerCall, emailConstant);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                var query = _dbSet.Where(lambda);

                if (excludeId.HasValue)
                {
                    var idProperty = Expression.Property(parameter, "Id");
                    var idConstant = Expression.Constant(excludeId.Value);
                    var idNotEqual = Expression.NotEqual(idProperty, idConstant);
                    var combined = Expression.AndAlso(equal, idNotEqual);
                    var combinedLambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                    query = _dbSet.Where(combinedLambda);
                }

                return !await query.AnyAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(serialNumber)) return false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var serialNumberProperty = Expression.Property(parameter, "SerialNumber");
                var serialNumberConstant = Expression.Constant(serialNumber);
                var equal = Expression.Equal(serialNumberProperty, serialNumberConstant);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                var query = _dbSet.Where(lambda);

                if (excludeId.HasValue)
                {
                    var idProperty = Expression.Property(parameter, "Id");
                    var idConstant = Expression.Constant(excludeId.Value);
                    var idNotEqual = Expression.NotEqual(idProperty, idConstant);
                    var combined = Expression.AndAlso(equal, idNotEqual);
                    var combinedLambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                    query = _dbSet.Where(combinedLambda);
                }

                return !await query.AnyAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Generic name-based methods (for entities with Name properties)
        public virtual async Task<T?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var nameProperty = Expression.Property(parameter, "Name");
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var toLowerCall = Expression.Call(nameProperty, toLowerMethod!);
                var nameConstant = Expression.Constant(name.ToLower());
                var equal = Expression.Equal(toLowerCall, nameConstant);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                return await _dbSet.FirstOrDefaultAsync(lambda);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var nameProperty = Expression.Property(parameter, "Name");
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var toLowerCall = Expression.Call(nameProperty, toLowerMethod!);
                var nameConstant = Expression.Constant(name.ToLower());
                var equal = Expression.Equal(toLowerCall, nameConstant);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                var query = _dbSet.Where(lambda);

                if (excludeId.HasValue)
                {
                    var idProperty = Expression.Property(parameter, "Id");
                    var idConstant = Expression.Constant(excludeId.Value);
                    var idNotEqual = Expression.NotEqual(idProperty, idConstant);
                    var combined = Expression.AndAlso(equal, idNotEqual);
                    var combinedLambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                    query = _dbSet.Where(combinedLambda);
                }

                return !await query.AnyAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Generic active/inactive filtering
        public virtual async Task<IEnumerable<T>> GetActiveAsync()
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var isActiveProperty = Expression.Property(parameter, "IsActive");
                var trueConstant = Expression.Constant(true);
                var equal = Expression.Equal(isActiveProperty, trueConstant);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                return await _dbSet.Where(lambda).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<IEnumerable<T>> GetInactiveAsync()
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var isActiveProperty = Expression.Property(parameter, "IsActive");
                var falseConstant = Expression.Constant(false);
                var equal = Expression.Equal(isActiveProperty, falseConstant);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                return await _dbSet.Where(lambda).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Generic search method (for entities with searchable string properties)
        public virtual async Task<IEnumerable<T>> SearchAsync(string searchTerm, params Expression<Func<T, object>>[] searchProperties)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchProperties == null || searchProperties.Length == 0)
                return Enumerable.Empty<T>();

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var lowerSearchTerm = searchTerm.ToLower();
                var searchTermConstant = Expression.Constant(lowerSearchTerm);

                Expression? combinedExpression = null;

                foreach (var searchProperty in searchProperties)
                {
                    var propertyExpression = searchProperty.Compile();
                    var propertyValue = Expression.Invoke(searchProperty, parameter);
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var toLowerCall = Expression.Call(propertyValue, toLowerMethod!);
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var containsCall = Expression.Call(toLowerCall, containsMethod!, searchTermConstant);

                    if (combinedExpression == null)
                        combinedExpression = containsCall;
                    else
                        combinedExpression = Expression.OrElse(combinedExpression, containsCall);
                }

                if (combinedExpression == null)
                    return Enumerable.Empty<T>();

                var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                return await _dbSet.Where(lambda).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Generic date range filtering (for entities with date properties)
        public virtual async Task<IEnumerable<T>> GetByDateRangeAsync(Expression<Func<T, DateTime>> dateProperty, DateTime startDate, DateTime endDate)
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyValue = Expression.Invoke(dateProperty, parameter);
                var startDateConstant = Expression.Constant(startDate);
                var endDateConstant = Expression.Constant(endDate);
                var greaterThanOrEqual = Expression.GreaterThanOrEqual(propertyValue, startDateConstant);
                var lessThanOrEqual = Expression.LessThanOrEqual(propertyValue, endDateConstant);
                var combined = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);
                var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);

                return await _dbSet.Where(lambda).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Generic pagination
        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, object>>? orderBy = null, bool ascending = true)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (orderBy != null)
                {
                    query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
                }

                return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<IEnumerable<T>> GetPagedWhereAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, Expression<Func<T, object>>? orderBy = null, bool ascending = true)
        {
            try
            {
                IQueryable<T> query = _dbSet.Where(predicate);

                if (orderBy != null)
                {
                    query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
                }

                return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        // Generic includes
        public virtual async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                
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
    }
} 