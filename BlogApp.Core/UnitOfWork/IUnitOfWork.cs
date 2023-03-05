using BlogApp.Core.Repositories;

namespace BlogApp.Core.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ICategoryRepository Categories { get; }
        IAppUserRepository Users { get; }
        IBlogRepository Blogs { get; }
        ITagRepository Tags { get; }
        IAppRoleRepository Roles { get; }
        IBlogCategoryRepository BlogCategory { get; }
        ITagBlogRepository BlogTag { get; }
        ICommentRepository Comments { get; }
        Task CommitAsync();
        void Commit();
    }
}
