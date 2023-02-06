﻿using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Business.Services
{
    public class BlogService : Service<Blog>, IBlogService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITagBlogRepository _tagBlogRepository;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public BlogService(IGenericRepository<Blog> repository, IUnitOfWork unitOfWork, IAppUserRepository appUserRepository, IMapper mapper, IAppRoleRepository appRoleRepository, IBlogRepository blogRepository, ITagRepository tagRepository, ITagBlogRepository tagBlogRepository, IBlogCategoryRepository blogCategoryRepository) : base(repository, unitOfWork)
        {
            _appUserRepository = appUserRepository;
            _mapper = mapper;
            _appRoleRepository = appRoleRepository;
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
            _tagBlogRepository = tagBlogRepository;
            _blogCategoryRepository = blogCategoryRepository;
        }
        public async Task<CustomResponse<BlogCreateDto>> AddBlogWithTagsAndCategoriesAsync(BlogCreateDto blogDto)
        {
            var tagSplit = blogDto.Tags.Split(',');

            var blog = _mapper.Map<Blog>(blogDto);
            blog.TagBlogs = new List<TagBlog>();
            blog.BlogCategories = new List<BlogCategory>();

            foreach (var tag in tagSplit)
            {
                var tagName = tag.Trim();
                var checkTag = _tagRepository.Where(x => x.Name == tagName && !x.IsDeleted).FirstOrDefault();
                if (checkTag == null)
                {
                    var tagModel = new Tag()
                    {
                        Name = tagName,
                        IsDeleted = false,
                        IsActive = true,
                        Description = tagName,
                        //CreatedByUsername = (await _appUserRepository.GetByIdAsync(blogDto.AppUserId)).Username
                    };

                    await _tagRepository.AddAsync(tagModel);
                    await _unitOfWork.CommitAsync();

                    blog.TagBlogs.Add(new TagBlog
                    {
                        Blog = blog,
                        TagId = tagModel.Id
                    });
                }
                else
                {
                    blog.TagBlogs.Add(new TagBlog
                    {
                        Blog = blog,
                        TagId = checkTag.Id
                    });
                }
            }

            foreach (var categoryId in blogDto.CategoryIds)
            {
                blog.BlogCategories.Add(new BlogCategory
                {
                    Blog = blog,
                    CategoryId = categoryId
                });
            }
            //blog.CreatedByUsername = (await _appUserRepository.GetByIdAsync(blogDto.AppUserId)).Username;

            await _blogRepository.AddAsync(blog);
            await _unitOfWork.CommitAsync();

            return CustomResponse<BlogCreateDto>.Success(201, blogDto);
        }

        public async Task<CustomResponse<NoContent>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto)
        {
            var oldBlog = _blogRepository.GetBlogById(blogUpdateDto.Id);
            var blog = _mapper.Map<BlogUpdateDto, Blog>(blogUpdateDto, oldBlog);

            if (!SetwiseEquivalentTo<int>(blogUpdateDto.CategoryIds.ToList(), oldBlog.BlogCategories.Select(x => x.CategoryId).ToList()))
            {
                _blogCategoryRepository.RemoveRange(_blogCategoryRepository.Where(x => x.BlogId == oldBlog.Id).ToList());

                blog.BlogCategories = new List<BlogCategory>();

                foreach (var catId in blogUpdateDto.CategoryIds)
                {
                    blog.BlogCategories.Add(new BlogCategory
                    {
                        Blog = blog,
                        CategoryId = catId
                    });
                }
            }

            if (!SetwiseEquivalentTo<string>(blogUpdateDto.Tags.Split(',').Select(x => x.Trim()).ToList(), oldBlog.TagBlogs.Select(x => x.Tag.Name).ToList()))
            {
                _tagBlogRepository.RemoveRange(_tagBlogRepository.Where(x => x.BlogId == oldBlog.Id).ToList());

                var tagSplit = blogUpdateDto.Tags.Split(',');
                blog.TagBlogs = new List<TagBlog>();

                foreach (var tag in tagSplit)
                {
                    var tagName = tag.Trim();
                    var checkTag = _tagRepository.Where(x => x.Name == tagName && !x.IsDeleted).FirstOrDefault();
                    if (checkTag == null)
                    {
                        var tagModel = new Tag()
                        {
                            Name = tagName,
                            IsDeleted = false,
                            IsActive = true,
                            Description = tagName,
                            //CreatedByUsername = (await _appUserRepository.GetByIdAsync(blogUpdateDto.AppUserId)).Username
                        };

                        await _tagRepository.AddAsync(tagModel);
                        await _unitOfWork.CommitAsync();

                        blog.TagBlogs.Add(new TagBlog
                        {
                            Blog = blog,
                            TagId = tagModel.Id
                        });
                    }
                    else
                    {
                        blog.TagBlogs.Add(new TagBlog
                        {
                            Blog = blog,
                            TagId = checkTag.Id
                        });
                    }
                }
            }

            //blog.UpdatedByUsername = (await _appUserRepository.GetByIdAsync(blogUpdateDto.AppUserId)).Username;

            _blogRepository.Update(blog);
            await _unitOfWork.CommitAsync();

            return CustomResponse<NoContent>.Success(204);
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByNonDeletedAndActive()
        {
            var blogs = await _blogRepository.Where(x => x.IsActive && !x.IsDeleted).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).Include(x => x.AppUser).ToListAsync();
            if (blogs.Any())
            {
                var blogListDto = new List<BlogListDto>();

                foreach (var blog in blogs)
                {
                    var blogList = _mapper.Map<BlogListDto>(blog);
                    blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogListDto.Add(blogList);
                }
                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> DeleteAsync(int blogId)
        {
            var blog = _blogRepository.Where(x => x.Id == blogId).FirstOrDefault();
            if (blog != null)
            {
                blog.IsDeleted = true;
                blog.IsActive = false;
                //blog.UpdatedByUsername = (await _appUserRepository.GetByIdAsync(blog.AppUserId)).Username;
                _blogRepository.Update(blog);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(200);
            }
            return CustomResponse<NoContent>.Fail(404, $"{blogId} idli blog bulunamadı!");
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var blogs = await _blogRepository.Where(x => x.IsDeleted).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).Include(x => x.AppUser).ToListAsync();

            if (blogs.Any())
            {
                var blogListDto = new List<BlogListDto>();
                foreach (var blog in blogs)
                {
                    var blogList = _mapper.Map<BlogListDto>(blog);
                    blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogListDto.Add(blogList);
                }
                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Silinmiş bir blog bulunamadı!");
        }

        public bool SetwiseEquivalentTo<T>(List<T> list, List<T> other) where T : IEquatable<T>
        {
            if (list.Except(other).Any())
                return false;
            if (other.Except(list).Any())
                return false;
            return true;
        }

        public async Task<CustomResponse<PersonalBlogDto>> GetAllByUserIdAsync(int userId)//Kullanıcı Paneli-Bloglarım
        {
            var blogs = await _blogRepository.Where(x => x.AppUserId == userId).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).Include(x => x.AppUser).ToListAsync();

            if (blogs.Any())
            {
                var blogListDto = new List<BlogListDto>();
                foreach (var blog in blogs)
                {
                    var blogList = _mapper.Map<BlogListDto>(blog);
                    blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogListDto.Add(blogList);
                }
                var personalBlogDto = new PersonalBlogDto();
                personalBlogDto.Blogs = blogListDto;
                personalBlogDto.TotalBlogCount = blogs.Count;
                personalBlogDto.TotalActiveBlogCount = await _blogRepository.Where(x => x.IsActive && x.AppUserId == userId).CountAsync();
                personalBlogDto.TotalInactiveBlogCount = await _blogRepository.Where(x => !x.IsActive && x.AppUserId == userId).CountAsync();
                blogs.ForEach(x => { personalBlogDto.TotalBlogsViewedCount += x.ViewsCount; });

                return CustomResponse<PersonalBlogDto>.Success(200, personalBlogDto);
            }
            return CustomResponse<PersonalBlogDto>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<int>> CountTotalBlogsAsync()//Admin-Home
        {
            var blogsTotalCount = await _blogRepository.CountAsync();

            return blogsTotalCount > -1 ? CustomResponse<int>.Success(200, blogsTotalCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");
        }

        public async Task<CustomResponse<int>> CountActiveBlogsAsync()
        {
            var blogsTotalCount = await _blogRepository.CountAsync(x => x.IsActive);

            return blogsTotalCount > -1 ? CustomResponse<int>.Success(200, blogsTotalCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");

        }

        public async Task<CustomResponse<int>> CountInActiveBlogsAsync()
        {
            var blogsTotalCount = await _blogRepository.CountAsync(x => !x.IsActive);

            return blogsTotalCount > -1 ? CustomResponse<int>.Success(200, blogsTotalCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");
        }

        public async Task<CustomResponse<int>> CountByDeletedBlogsAsync()
        {
            var blogsDeletedCount = await _blogRepository.CountAsync(x => x.IsDeleted);

            return blogsDeletedCount > -1 ? CustomResponse<int>.Success(200, blogsDeletedCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsDeletedCount}");
        }

        public async Task<CustomResponse<int>> CountByNonDeletedBlogsAsync()
        {
            var blogsNonDeletedCount = await _blogRepository.CountAsync(x => !x.IsDeleted);

            return blogsNonDeletedCount > -1 ? CustomResponse<int>.Success(200, blogsNonDeletedCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsNonDeletedCount}");
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllBlogsAsync()//Admin-Home-Bloglar
        {
            var blogs = await _blogRepository.GetAll().Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).Include(x => x.AppUser).ToListAsync();

            if (blogs.Any())
            {
                var blogListDto = new List<BlogListDto>();
                foreach (var blog in blogs)
                {
                    var blogList = _mapper.Map<BlogListDto>(blog);
                    blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogListDto.Add(blogList);
                }
                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> HardDeleteAsync(int blogId)//Admin-Arşiv-Blog
        {
            var result = await _blogRepository.AnyAsync(x => x.Id == blogId);
            if (result)
            {
                var blog = _blogRepository.Where(x => x.Id == blogId);
                _blogRepository.RemoveRange(blog);
                _unitOfWork.Commit();
                return CustomResponse<NoContent>.Success(200);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> UndoDeleteAsync(int blogId)//Admin-Arşiv-Blog
        {
            var result = await _blogRepository.AnyAsync(x => x.Id == blogId);
            if (result)
            {
                var blog = _blogRepository.Where(x => x.Id == blogId).FirstOrDefault();
                blog.IsDeleted = false;
                blog.IsActive = true;
                //blog.UpdatedByUsername=
                //blog.UpdatedDate=
                _blogRepository.Update(blog);
                _unitOfWork.Commit();
                return CustomResponse<NoContent>.Success(200);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir blog bulunamadı!");
        }

        public CustomResponse<string> IncreaseViewCountAsync(int blogId)//Anasayfa-Blog Detayda
        {
            var blog = _blogRepository.Where(x => x.Id == blogId).FirstOrDefault();
            if (blog == null)
            {
                return CustomResponse<string>.Fail(404, "Bir blog bulunamadı!");
            }

            blog.ViewsCount += 1;
            _blogRepository.Update(blog);
            _unitOfWork.Commit();
            return CustomResponse<string>.Success(200, $"{blog.Title} adlı bloğun okunmasıyısı arttırıldı.");
        }

        public async Task<CustomResponse<List<BlogListDto>>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            //if (string.IsNullOrWhiteSpace(keyword))
            //{
            //    var blogs = _blogRepository.Where(x => x.IsActive && !x.IsDeleted).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.AppUser).ToList();
            //    var sortedBlogs = isAscending
            //        ? blogs.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
            //        : blogs.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                //return CustomResponse<BlogListDto>.Success(200, new BlogListDto
                //{
                //    Blogs = sortedBlogs,
                //    CurrentPage = currentPage,
                //    PageSize = pageSize,
                //    TotalCount = blogs.Count,
                //    IsAscending = isAscending
                //});
            //}

            var searchedBlogs = await _blogRepository.SearchAsync(new List<Expression<Func<Blog, bool>>>
            {
                (x)=>x.Title.Contains(keyword),
                (x)=>x.BlogCategories.Any(x=>x.Category.Title.Contains(keyword)),
                (x)=>x.TagBlogs.Any(x=>x.Tag.Name.Contains(keyword))
            },
            x=>x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            var searchedAndSortedBlogs = isAscending
                ? searchedBlogs.OrderBy(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : searchedBlogs.OrderByDescending(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();


                var blogListDto = new List<BlogListDto>();
                foreach (var blog in searchedAndSortedBlogs)
                {
                    var searchedBlog = _mapper.Map<BlogListDto>(blog);
                    searchedBlog.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    searchedBlog.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogListDto.Add(searchedBlog);
                }
            return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByViewCountAsync(bool isAscending,int? takeSize)//görüntülenme sayısına göre getir
        {
            var blogs = await _blogRepository.Where(x => x.IsActive && !x.IsDeleted).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x=>x.TagBlogs).ThenInclude(x=>x.Tag).Include(x => x.AppUser).ToListAsync();
            var sortedBlogs = isAscending ? blogs.OrderBy(x => x.ViewsCount) : blogs.OrderByDescending(x => x.ViewsCount);

            if (blogs.Any())
            {
                var blogListDto = new List<BlogListDto>();
                foreach (var blog in sortedBlogs)
                {
                    var blogList = _mapper.Map<BlogListDto>(blog);
                    blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogListDto.Add(blogList);
                }
                return CustomResponse<List<BlogListDto>>.Success(200, takeSize < 1 ? blogListDto.ToList() : blogListDto.Take(takeSize.Value).ToList());

            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");

        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var blogs = categoryId == null
                ? await _blogRepository.Where(x => x.IsActive && !x.IsDeleted).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).Include(x => x.AppUser).ToListAsync()
                : await _blogRepository.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId) && x.IsActive && !x.IsDeleted).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).Include(x => x.AppUser).ToListAsync();

            var sortedBlogs = isAscending
                ? blogs.OrderBy(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : blogs.OrderByDescending(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var blogListDto = new List<BlogListDto>();
            foreach (var blog in sortedBlogs)
            {
                var blogList = _mapper.Map<BlogListDto>(blog);
                blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                blogListDto.Add(blogList);
            }
            return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);

        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByCategoryAsync(int categoryId)
        {
            var blogs = await _blogRepository.Where(x => !x.IsDeleted && x.IsActive && x.BlogCategories.Any(x => x.CategoryId == categoryId)).Include(x=>x.BlogCategories).ThenInclude(x=>x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).Include(x=>x.AppUser).ToListAsync();
                
            if (blogs.Any())
            {
                var blogListDto = new List<BlogListDto>();
                foreach (var blog in blogs)
                {
                    var blogList = _mapper.Map<BlogListDto>(blog);
                    blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogList.CategoryId = categoryId;
                    blogListDto.Add(blogList);
                }
                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bu kategoriye ait gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByUserIdOnFilterAsync(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var anyUser = await _appUserRepository.AnyAsync(x => x.Id == userId);
            if (!anyUser)
            {
                return CustomResponse<List<BlogListDto>>.Fail(404, $"{userId} numarasına ait bir kullanıcı bulunamadı!");
            }

            var userBlogs = await _blogRepository.Where(x => x.IsActive && !x.IsDeleted && x.AppUserId == userId).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).Include(x => x.AppUser).ToListAsync();

            if (userBlogs.Any())
            {
                List<Blog> sortedBlogs = new();
                switch (filterBy)
                {
                    case FilterBy.Category:
                        switch (orderBy)
                        {
                            case OrderBy.Date:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(categoryId == 0 ? x => true : x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderBy(x => x.UpdatedDate).ToList()
                                    : userBlogs.Where(categoryId == 0 ? x => true : x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderByDescending(x => x.UpdatedDate).ToList();
                                break;
                            case OrderBy.ViewCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(categoryId == 0 ? x => true : x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderBy(x => x.ViewsCount).ToList()
                                    : userBlogs.Where(categoryId == 0 ? x => true : x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderByDescending(x => x.ViewsCount).ToList();
                                break;
                            case OrderBy.CommentCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(categoryId == 0 ? x => true : x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderBy(x => x.CommentCount).ToList()
                                    : userBlogs.Where(categoryId == 0 ? x => true : x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderByDescending(x => x.CommentCount).ToList();
                                break;
                        }
                        break;
                    case FilterBy.Date:
                        switch (orderBy)
                        {
                            case OrderBy.Date:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.UpdatedDate >= startAt && x.UpdatedDate <= endAt).Take(takeSize).OrderBy(x => x.UpdatedDate).ToList()
                                    : userBlogs.Where(x => x.UpdatedDate >= startAt && x.UpdatedDate <= endAt).Take(takeSize).OrderByDescending(x => x.UpdatedDate).ToList();
                                break;
                            case OrderBy.ViewCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.UpdatedDate >= startAt && x.UpdatedDate <= endAt).Take(takeSize).OrderBy(x => x.ViewsCount).ToList()
                                    : userBlogs.Where(x => x.UpdatedDate >= startAt && x.UpdatedDate <= endAt).Take(takeSize).OrderByDescending(x => x.ViewsCount).ToList();
                                break;
                            case OrderBy.CommentCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.UpdatedDate >= startAt && x.UpdatedDate <= endAt).Take(takeSize).OrderBy(x =>
                                x.CommentCount).ToList()
                                    : userBlogs.Where(x => x.UpdatedDate >= startAt && x.UpdatedDate <= endAt).Take(takeSize).OrderByDescending(x => x.CommentCount).ToList();
                                break;
                        }
                        break;
                    case FilterBy.ViewCount:
                        switch (orderBy)
                        {
                            case OrderBy.Date:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.ViewsCount >= minViewCount && x.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(x =>
                                x.UpdatedDate).ToList()
                                    : userBlogs.Where(x => x.ViewsCount >= minViewCount && x.ViewsCount <= maxViewCount).Take(takeSize).OrderByDescending(x =>
                                x.UpdatedDate).ToList();
                                break;
                            case OrderBy.ViewCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.ViewsCount >= minViewCount && x.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(x =>
                                x.ViewsCount).ToList()
                                    : userBlogs.Where(x => x.ViewsCount >= minViewCount && x.ViewsCount <= maxViewCount).Take(takeSize).OrderByDescending(x =>
                                x.ViewsCount).ToList();
                                break;
                            case OrderBy.CommentCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.ViewsCount >= minViewCount && x.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(x =>
                                x.CommentCount).ToList()
                                    : userBlogs.Where(x => x.ViewsCount >= minViewCount && x.ViewsCount <= maxViewCount).Take(takeSize).OrderByDescending(x =>
                                x.CommentCount).ToList();
                                break;
                        }
                        break;
                    case FilterBy.CommentCount:
                        switch (orderBy)
                        {
                            case OrderBy.Date:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.CommentCount >= minCommentCount && x.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(x =>
                                x.UpdatedDate).ToList()
                                    : userBlogs.Where(x => x.CommentCount >= minCommentCount && x.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(x =>
                                x.UpdatedDate).ToList();
                                break;
                            case OrderBy.ViewCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.CommentCount >= minCommentCount && x.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(x =>
                                x.ViewsCount).ToList()
                                    : userBlogs.Where(x => x.CommentCount >= minCommentCount && x.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(x =>
                                x.ViewsCount).ToList();
                                break;
                            case OrderBy.CommentCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.CommentCount >= minCommentCount && x.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(x =>
                                x.CommentCount).ToList()
                                    : userBlogs.Where(x => x.CommentCount >= minCommentCount && x.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(x =>
                                x.CommentCount).ToList();
                                break;
                        }
                        break;
                }

                var blogListDto = new List<BlogListDto>();
                foreach (var blog in sortedBlogs)
                {
                    var blogList = _mapper.Map<BlogListDto>(blog);
                    blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogList.CategoryId = categoryId;
                    blogListDto.Add(blogList);
                }
                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Kullanıcıya ait gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllBlogsFilteredAsync(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeTag, bool includeComments, bool includeUser)
        {
            List<Expression<Func<Blog, bool>>> predicates = new List<Expression<Func<Blog, bool>>>();
            List<Expression<Func<Blog, object>>> includes = new List<Expression<Func<Blog, object>>>();

            if (categoryId.HasValue)
            {
                if (!await _blogRepository.AnyAsync(x => x.BlogCategories.Any(x => x.CategoryId == categoryId.Value)))
                {
                    return CustomResponse<List<BlogListDto>>.Fail(404, $"{categoryId} numaralı kategoriye ait bir blog bulunamadı!");
                }
                predicates.Add(x=>x.BlogCategories.Any(x => x.CategoryId == categoryId.Value));
            }

            if (userId.HasValue)
            {
                if (!await _appUserRepository.AnyAsync(x => x.Id == userId.Value))
                {
                    return CustomResponse<List<BlogListDto>>.Fail(404, "Böyle bir kullanıcı bulunamadı!");
                }
                predicates.Add(x => x.AppUserId == userId.Value);
            }

            if (isActive.HasValue) predicates.Add(x => x.IsActive == isActive.Value);
            if (isDeleted.HasValue) predicates.Add(x => x.IsDeleted == isDeleted.Value);

            if (includeCategory) includes.Add(x => x.BlogCategories);
            if (includeTag) includes.Add(x => x.TagBlogs);
            if (includeComments) includes.Add(x => x.Comments);
            if (includeUser) includes.Add(x => x.AppUser);

            var blogs = await _blogRepository.GetAllFilteredAsync(predicates, includes);

            if (blogs.Any())
            {
                IOrderedEnumerable<Blog> sortedBlogs;

                switch (orderBy)
                {
                    case OrderByGeneral.Id:
                        sortedBlogs = isAscending ? blogs.OrderBy(x => x.Id) : blogs.OrderByDescending(x => x.Id);
                        break;
                    case OrderByGeneral.Az:
                        sortedBlogs = isAscending ? blogs.OrderBy(x => x.Title) : blogs.OrderByDescending(x => x.Title);
                        break;
                    default:
                        sortedBlogs = isAscending ? blogs.OrderBy(x => x.CreatedDate) : blogs.OrderByDescending(x => x.CreatedDate);
                        break;
                }

                var blogListDto = new List<BlogListDto>();
                foreach (var blog in sortedBlogs)
                {
                    var blogList = _mapper.Map<BlogListDto>(blog);
                    if (includeCategory)  blogList.Categories = blog.BlogCategories.Select(x => x.Category.Title).ToList();
                    if (includeTag) blogList.Tags = blog.TagBlogs.Select(x => x.Tag.Name).ToList();
                    blogListDto.Add(blogList);
                }
                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList());
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bu filtrelere uygun gösterilecek bir blog bulunamadı!");

        }







        /*
         *+++++++++++++++GetBlogById
         *+++++++++++++++GetAllDeletedBlogs
         *+++++++++++++++UndoDelete
         *+++++++++++++++HardDeleteAsync
         *+++++++++++++++GetAllByViewCount
         *+++++++++++++++GetAllByCategory
         *+++++++++++++++GetAllByPagingAsync
         *+++++++++++++++GetAllByUserIdOnFilter
         *+++++++++++++++SearchAsync
         *+++++++++++++++IncreaseViewCountAsync => görüntüleme sayısını arttır.
         *+++++++++++++++CountAsync => blogların toplam sayısı
         *+++++++++++++++CountByNonDeletedAsync
         *GetAllAsyncV2 =>yorumlar kategoriler kullanıcılar sıralamalar vs göre getirir.
         *GetBlogByIdWithCategoriesAndComments=> GetAsync
         *
         *GetAllByTagAsync
         *
         */
    }
}
