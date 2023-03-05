using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using BlogApp.Data.Repositories;

namespace BlogApp.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private CategoryRepository _categoryRepository;
        private AppUserRepository _userRepository;
        private BlogRepository _blogRepository;
        private TagRepository _tagRepository;
        private AppRoleRepository _roleRepository;
        private BlogCategoryRepository _blogCategoryRepository;
        private TagBlogRepository _blogTagRepository;
        private CommentRepository _commentRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);
        public IAppUserRepository Users => _userRepository ??= new AppUserRepository(_context);
        public IBlogRepository Blogs => _blogRepository ??= new BlogRepository(_context);
        public ITagRepository Tags => _tagRepository ??= new TagRepository(_context);
        public IAppRoleRepository Roles => _roleRepository ??= new AppRoleRepository(_context);
        public IBlogCategoryRepository BlogCategory => _blogCategoryRepository ??= new BlogCategoryRepository(_context);
        public ITagBlogRepository BlogTag => _blogTagRepository ??= new TagBlogRepository(_context);
        public ICommentRepository Comments => _commentRepository ??= new CommentRepository(_context);

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}