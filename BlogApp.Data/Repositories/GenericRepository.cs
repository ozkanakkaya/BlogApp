using BlogApp.Core.Repositories;
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

        public async Task<IList<T>> GetAllFilteredAsync(IList<Expression<Func<T,bool>>> predicates, IList<Expression<Func<T,object>>> includeProperties)
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
                            query = query.Include(x => (x as Core.Entities.Concrete.Blog).BlogCategories).ThenInclude(x => x.Category);
                            break;
                        case string x when x.Contains("TagBlogs"):
                            query = query.Include(x => (x as Core.Entities.Concrete.Blog).TagBlogs).ThenInclude(x => x.Tag);
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
    }
}
