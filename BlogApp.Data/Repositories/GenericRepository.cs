using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression = null)
        {
            return await (expression == null ? _dbSet.CountAsync() : _dbSet.CountAsync(expression));
        }

        public async Task<IList<T>> SearchAsync(IList<Expression<Func<T, bool>>> predicates, Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (predicates.Any())
            {
                var predicateChain = PredicateBuilder.New<T>();

                foreach (var predicate in predicates)
                {
                    predicateChain.Or(predicate);
                }
                query = query.Where(predicateChain);
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    switch (includeProperty.ToString())
                    {
                        case string x when x.Contains("BlogCategories"):
                            query = query.Include(x => (x as Blog).BlogCategories).ThenInclude(x => x.Category);
                            break;
                        case string x when x.Contains("BlogTags"):
                            query = query.Include(x => (x as Blog).BlogTags).ThenInclude(x => x.Tag);
                            break;
                        default:
                            query = query.Include(includeProperty);
                            break;
                    }
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IList<T>> GetAllByListedAsync(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    switch (includeProperty.ToString())
                    {
                        case string x when x.Contains("BlogCategories"):
                            query = query.Include(x => (x as Blog).BlogCategories).ThenInclude(x => x.Category);
                            break;
                        case string x when x.Contains("BlogTags"):
                            query = query.Include(x => (x as Blog).BlogTags).ThenInclude(x => x.Tag);
                            break;
                        case string x when x.Contains("UserRoles"):
                            query = query.Include(x => (x as User).UserRoles).ThenInclude(x => x.Role);
                            break;
                        default:
                            query = query.Include(includeProperty);
                            break;
                    }

                    //query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByListedAsync(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    switch (includeProperty.ToString())
                    {
                        case string x when x.Contains("BlogCategories"):
                            query = query.Include(x => (x as Blog).BlogCategories).ThenInclude(x => x.Category);
                            break;
                        case string x when x.Contains("BlogTags"):
                            query = query.Include(x => (x as Blog).BlogTags).ThenInclude(x => x.Tag);
                            break;
                        case string x when x.Contains("UserRoles"):
                            query = query.Include(x => (x as User).UserRoles).ThenInclude(x => x.Role);
                            break;
                        default:
                            query = query.Include(includeProperty);
                            break;
                    }

                    //query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().SingleOrDefaultAsync();
        }


        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    switch (includeProperty.ToString())
                    {
                        case string x when x.Contains("BlogCategories"):
                            query = query.Include(x => (x as Blog).BlogCategories).ThenInclude(x => x.Category);
                            break;
                        case string x when x.Contains("BlogTags"):
                            query = query.Include(x => (x as Blog).BlogTags).ThenInclude(x => x.Tag);
                            break;
                        case string x when x.Contains("UserRoles"):
                            query = query.Include(x => (x as User).UserRoles).ThenInclude(x => x.Role);
                            break;
                        default:
                            query = query.Include(includeProperty);
                            break;
                    }

                    //query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    switch (includeProperty.ToString())
                    {
                        case string x when x.Contains("BlogCategories"):
                            query = query.Include(x => (x as Blog).BlogCategories).ThenInclude(x => x.Category);
                            break;
                        case string x when x.Contains("BlogTags"):
                            query = query.Include(x => (x as Blog).BlogTags).ThenInclude(x => x.Tag);
                            break;
                        case string x when x.Contains("UserRoles"):
                            query = query.Include(x => (x as User).UserRoles).ThenInclude(x => x.Role);
                            break;
                        default:
                            query = query.Include(includeProperty);
                            break;
                    }

                    //query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().SingleOrDefaultAsync();
        }
    }
}
