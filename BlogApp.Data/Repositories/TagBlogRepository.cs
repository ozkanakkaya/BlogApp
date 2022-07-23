using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;

namespace BlogApp.Data.Repositories
{
    public class TagBlogRepository : GenericRepository<TagBlog>, ITagBlogRepository
    {
        public TagBlogRepository(AppDbContext context) : base(context)
        {
        }
    }
}
