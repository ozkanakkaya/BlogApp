﻿using BlogApp.Core.Repositories;

namespace BlogApp.Core.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        IBlogRepository Blogs { get; }
        ITagRepository Tags { get; }
        IRoleRepository Roles { get; }
        IBlogCategoryRepository BlogCategory { get; }
        ITagBlogRepository BlogTag { get; }
        ICommentRepository Comments { get; }
        IUserRoleRepository UserRoles { get; }
        Task CommitAsync();
        void Commit();
    }
}
