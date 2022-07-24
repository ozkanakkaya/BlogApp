using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;

namespace BlogApp.Data.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
