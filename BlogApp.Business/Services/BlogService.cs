using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using BlogApp.Core.Utilities.Abstract;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace BlogApp.Business.Services
{
    public class BlogService : Service<Blog>, IBlogService
    {
        private readonly IImageHelper _imageHelper;
        public BlogService(IGenericRepository<Blog> repository, IUnitOfWork unitOfWork, IMapper mapper, IImageHelper imageHelper) : base(repository, unitOfWork, mapper)
        {
            _imageHelper = imageHelper;
        }
        public async Task<CustomResponseDto<BlogCreateDto>> AddBlogWithTagsAndCategoriesAsync(BlogCreateDto blogCreateDto)
        {
            var tagSplit = blogCreateDto.Tags.TrimEnd(',').Split(',');

            var blog = Mapper.Map<Blog>(blogCreateDto);
            blog.BlogTags = new List<BlogTag>();
            blog.BlogCategories = new List<BlogCategory>();

            foreach (var tag in tagSplit)
            {
                var tagName = !string.IsNullOrWhiteSpace(tag) ? tag.Trim() : null;
                if (tagName == null) continue;

                var checkTag = await UnitOfWork.Tags.Where(x => x.Name == tagName && !x.IsDeleted).FirstOrDefaultAsync();
                if (checkTag == null)
                {
                    var tagModel = new Tag()
                    {
                        Name = tagName,
                        IsDeleted = false,
                        IsActive = true,
                        Description = tagName,
                    };

                    await UnitOfWork.Tags.AddAsync(tagModel);
                    await UnitOfWork.CommitAsync();

                    blog.BlogTags.Add(new BlogTag
                    {
                        Blog = blog,
                        TagId = tagModel.Id
                    });
                }
                else
                {
                    blog.BlogTags.Add(new BlogTag
                    {
                        Blog = blog,
                        TagId = checkTag.Id
                    });
                }
            }

            foreach (var categoryId in blogCreateDto.CategoryIds)
            {
                blog.BlogCategories.Add(new BlogCategory
                {
                    Blog = blog,
                    CategoryId = categoryId
                });
            }

            blog.ImageUrl = blogCreateDto.ImageUrl;

            await UnitOfWork.Blogs.AddAsync(blog);
            await UnitOfWork.CommitAsync();

            return CustomResponseDto<BlogCreateDto>.Success(200, blogCreateDto);
        }

        public async Task<CustomResponseDto<NoContent>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto)
        {
            var oldBlog = await UnitOfWork.Blogs.GetBlogById(blogUpdateDto.Id);
            var blog = Mapper.Map<BlogUpdateDto, Blog>(blogUpdateDto, oldBlog);

            if (!SetwiseEquivalentTo<int>(blogUpdateDto.CategoryIds.ToList(), oldBlog.BlogCategories.Select(x => x.CategoryId).ToList()))
            {
                blog.BlogCategories.Clear();

                foreach (var catId in blogUpdateDto.CategoryIds)
                {
                    blog.BlogCategories.Add(new BlogCategory
                    {
                        CategoryId = catId
                    });
                }
            }

            if (!SetwiseEquivalentTo<string>(blogUpdateDto.Tags.Split(',').Select(x => x.Trim()).ToList(), oldBlog.BlogTags.Select(x => x.Tag.Name).ToList()))
            {
                blog.BlogTags.Clear();

                var tagSplit = blogUpdateDto.Tags.TrimEnd(',').Split(',');

                foreach (var tag in tagSplit)
                {
                    var tagName = !string.IsNullOrWhiteSpace(tag) ? tag.Trim() : null;
                    if (tagName == null) continue;

                    var checkTag = await UnitOfWork.Tags.Where(x => x.Name == tagName && !x.IsDeleted).FirstOrDefaultAsync();
                    if (checkTag == null)
                    {
                        var tagModel = new Tag()
                        {
                            Name = tagName,
                            IsDeleted = false,
                            IsActive = true,
                            Description = tagName,
                        };

                        await UnitOfWork.Tags.AddAsync(tagModel);
                        await UnitOfWork.CommitAsync();

                        blog.BlogTags.Add(new BlogTag
                        {
                            TagId = tagModel.Id
                        });
                    }
                    else
                    {
                        blog.BlogTags.Add(new BlogTag
                        {
                            TagId = checkTag.Id
                        });
                    }
                }
            }

            blog.ImageUrl = blogUpdateDto.ImageUrl;

            UnitOfWork.Blogs.Update(blog);
            await UnitOfWork.CommitAsync();

            return CustomResponseDto<NoContent>.Success(200);
        }

        public async Task<CustomResponseDto<BlogListDto>> DeleteAsync(int blogId)
        {
            var blog = await UnitOfWork.Blogs.Where(x => x.Id == blogId).FirstOrDefaultAsync();
            if (blog != null)
            {
                blog.IsDeleted = true;
                blog.IsActive = false;

                UnitOfWork.Blogs.Update(blog);
                await UnitOfWork.CommitAsync();
                return CustomResponseDto<BlogListDto>.Success(200, Mapper.Map<BlogListDto>(blog));
            }
            return CustomResponseDto<BlogListDto>.Fail(200, $"{blogId} numaralı blog bulunamadı!");
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByActiveAsync()
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponseDto<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByNonDeletedAsync()//Aktif ve Pasif bloglar
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponseDto<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponseDto<List<BlogListDto>>.Fail(200, "Silinmiş bir blog bulunamadı!");
        }

        public bool SetwiseEquivalentTo<T>(List<T> list, List<T> other) where T : IEquatable<T>
        {
            if (list.Except(other).Any())
                return false;
            if (other.Except(list).Any())
                return false;
            return true;
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByUserIdAsync(int userId)//Kullanıcı Paneli-Bloglarım
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.UserId == userId && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            //if (blogs.Any())
            //{
            //    var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

            //    var personalBlogDto = new PersonalBlogDto();
            //    personalBlogDto.Blogs = blogListDto;
            //    personalBlogDto.TotalBlogCount = blogs.Count;
            //    personalBlogDto.TotalActiveBlogCount = await UnitOfWork.Blogs.Where(x => x.IsActive && x.UserId == userId).CountAsync();
            //    personalBlogDto.TotalInactiveBlogCount = await UnitOfWork.Blogs.Where(x => !x.IsActive && x.UserId == userId).CountAsync();
            //    blogs.ForEach(x => { personalBlogDto.TotalBlogsViewedCount += x.ViewCount; });

            //    return CustomResponseDto<PersonalBlogDto>.Success(200, personalBlogDto);
            //}
            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, blogListDto);
            }

            return CustomResponseDto<List<BlogListDto>>.Fail(200, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponseDto<int>> CountTotalBlogsAsync()//Admin-Home
        {
            var blogsTotalCount = await UnitOfWork.Blogs.CountAsync();

            return blogsTotalCount > -1 ? CustomResponseDto<int>.Success(200, blogsTotalCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");
        }

        public async Task<CustomResponseDto<int>> CountActiveBlogsAsync()
        {
            var blogsTotalCount = await UnitOfWork.Blogs.CountAsync(x => x.IsActive);

            return blogsTotalCount > -1 ? CustomResponseDto<int>.Success(200, blogsTotalCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");

        }

        public async Task<CustomResponseDto<int>> CountInactiveBlogsAsync()
        {
            var blogsTotalCount = await UnitOfWork.Blogs.CountAsync(x => !x.IsActive);

            return blogsTotalCount > -1 ? CustomResponseDto<int>.Success(200, blogsTotalCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");
        }

        public async Task<CustomResponseDto<int>> CountByDeletedBlogsAsync()
        {
            var blogsDeletedCount = await UnitOfWork.Blogs.CountAsync(x => x.IsDeleted);

            return blogsDeletedCount > -1 ? CustomResponseDto<int>.Success(200, blogsDeletedCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsDeletedCount}");
        }

        public async Task<CustomResponseDto<int>> CountByNonDeletedBlogsAsync()
        {
            var blogsNonDeletedCount = await UnitOfWork.Blogs.CountAsync(x => !x.IsDeleted);

            return blogsNonDeletedCount > -1 ? CustomResponseDto<int>.Success(200, blogsNonDeletedCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsNonDeletedCount}");
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllBlogsAsync()//Admin-Home-Bloglar
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(null, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponseDto<List<BlogListDto>>.Fail(200, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponseDto<BlogListDto>> HardDeleteAsync(int blogId)//Admin-Arşiv-Blog
        {
            var result = await UnitOfWork.Blogs.AnyAsync(x => x.Id == blogId);
            if (result)
            {
                var blog = UnitOfWork.Blogs.Where(x => x.Id == blogId);
                var blogDto = await blog.Select(u => Mapper.Map<BlogListDto>(u)).FirstOrDefaultAsync();

                UnitOfWork.Blogs.RemoveRange(blog);
                await UnitOfWork.CommitAsync();

                return CustomResponseDto<BlogListDto>.Success(200, blogDto);
            }
            return CustomResponseDto<BlogListDto>.Fail(200, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponseDto<BlogListDto>> UndoDeleteAsync(int blogId)//Admin-Arşiv-Blog
        {
            var result = await UnitOfWork.Blogs.AnyAsync(x => x.Id == blogId);
            if (result)
            {
                var blog = await UnitOfWork.Blogs.Where(x => x.Id == blogId).FirstOrDefaultAsync();
                blog.IsDeleted = false;
                blog.IsActive = true;

                UnitOfWork.Blogs.Update(blog);
                await UnitOfWork.CommitAsync();
                return CustomResponseDto<BlogListDto>.Success(200, Mapper.Map<BlogListDto>(blog));
            }
            return CustomResponseDto<BlogListDto>.Fail(200, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponseDto<string>> IncreaseViewCountAsync(int blogId)//Anasayfa-Blog Detayda
        {
            var blog = await UnitOfWork.Blogs.Where(x => x.Id == blogId).FirstOrDefaultAsync();
            if (blog == null)
            {
                return CustomResponseDto<string>.Fail(404, "Bir blog bulunamadı!");
            }

            blog.ViewCount += 1;
            UnitOfWork.Blogs.Update(blog);
            await UnitOfWork.CommitAsync();
            return CustomResponseDto<string>.Success(200, $"{blog.Title} adlı bloğun okunmasayısı arttırıldı.");
        }

        public async Task<CustomResponseDto<BlogListResultDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            //if (string.IsNullOrWhiteSpace(keyword))
            //{
            //    //var blogs = await UnitOfWork.Blogs.GetBlogsByDetailsAsync();

            //    //var sortedBlogs = isAscending
            //    //    ? blogs.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
            //    //    : blogs.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            //    //var sortedBlogsListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

            //    //return CustomResponse<List<BlogListDto>>.Success(200, sortedBlogsListDto);
            //    return CustomResponse<BlogListResultDto>.Fail(400, "Lütfen anahtar kelime giriniz!");

            //}

            var searchedBlogs = await UnitOfWork.Blogs.SearchAsync(new List<Expression<Func<Blog, bool>>>
            {
                (x)=>x.Title.Contains(keyword),
                (x)=>x.BlogCategories.Any(x=>x.Category.Name.Contains(keyword)),
                (x)=>x.BlogTags.Any(x=>x.Tag.Name.Contains(keyword))
            },
            x => x.IsActive && !x.IsDeleted,
            x => x.BlogCategories, x => x.BlogTags, x => x.User);

            var searchedAndSortedBlogs = isAscending
                ? searchedBlogs.OrderBy(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : searchedBlogs.OrderByDescending(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var blogListDto = searchedAndSortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

            return CustomResponseDto<BlogListResultDto>.Success(200, new BlogListResultDto
            {
                BlogListDto = blogListDto,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = searchedBlogs.Count,
                IsAscending = isAscending,
                Keyword = keyword
            });
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            var sortedBlogs = isAscending ? blogs.OrderBy(x => x.ViewCount) : blogs.OrderByDescending(x => x.ViewCount);

            if (blogs.Any())
            {
                var blogListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, takeSize < 1 ? blogListDto : blogListDto.Take(takeSize.Value).ToList());

            }
            return CustomResponseDto<List<BlogListDto>>.Fail(200, "Bir blog bulunamadı!");

        }

        public async Task<CustomResponseDto<BlogListResultDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var blogs = categoryId == null
                ? await UnitOfWork.Blogs.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User)
                : await UnitOfWork.Blogs.GetAllAsync(x => x.BlogCategories.Any(x => x.CategoryId == categoryId) && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            var sortedBlogs = isAscending
                ? blogs.OrderBy(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : blogs.OrderByDescending(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var blogListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

            return CustomResponseDto<BlogListResultDto>.Success(200, new BlogListResultDto
            {
                BlogListDto = blogListDto,
                CategoryId = categoryId.HasValue ? categoryId.Value : null,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = blogs.Count,
                IsAscending = isAscending
            });

        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByCategoryAsync(int categoryId)
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.BlogCategories.Any(x => x.CategoryId == categoryId) && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponseDto<List<BlogListDto>>.Fail(404, "Bu kategoriye ait gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByUserIdOnFilterAsync(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var anyUser = await UnitOfWork.Users.AnyAsync(x => x.Id == userId);
            if (!anyUser)
            {
                return CustomResponseDto<List<BlogListDto>>.Fail(404, $"{userId} numarasına ait bir kullanıcı bulunamadı!");
            }

            var userBlogs = await UnitOfWork.Blogs.GetAllAsync(x => x.UserId == userId && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

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
                                    ? userBlogs.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderBy(x => x.UpdatedDate).ToList()
                                    : userBlogs.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderByDescending(x => x.UpdatedDate).ToList();
                                break;
                            case OrderBy.ViewCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderBy(x => x.ViewCount).ToList()
                                    : userBlogs.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderByDescending(x => x.ViewCount).ToList();
                                break;
                            case OrderBy.CommentCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderBy(x => x.CommentCount).ToList()
                                    : userBlogs.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderByDescending(x => x.CommentCount).ToList();
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
                                    ? userBlogs.Where(x => x.UpdatedDate >= startAt && x.UpdatedDate <= endAt).Take(takeSize).OrderBy(x => x.ViewCount).ToList()
                                    : userBlogs.Where(x => x.UpdatedDate >= startAt && x.UpdatedDate <= endAt).Take(takeSize).OrderByDescending(x => x.ViewCount).ToList();
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
                                    ? userBlogs.Where(x => x.ViewCount >= minViewCount && x.ViewCount <= maxViewCount).Take(takeSize).OrderBy(x =>
                                x.UpdatedDate).ToList()
                                    : userBlogs.Where(x => x.ViewCount >= minViewCount && x.ViewCount <= maxViewCount).Take(takeSize).OrderByDescending(x =>
                                x.UpdatedDate).ToList();
                                break;
                            case OrderBy.ViewCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.ViewCount >= minViewCount && x.ViewCount <= maxViewCount).Take(takeSize).OrderBy(x =>
                                x.ViewCount).ToList()
                                    : userBlogs.Where(x => x.ViewCount >= minViewCount && x.ViewCount <= maxViewCount).Take(takeSize).OrderByDescending(x =>
                                x.ViewCount).ToList();
                                break;
                            case OrderBy.CommentCount:
                                sortedBlogs = isAscending
                                    ? userBlogs.Where(x => x.ViewCount >= minViewCount && x.ViewCount <= maxViewCount).Take(takeSize).OrderBy(x =>
                                x.CommentCount).ToList()
                                    : userBlogs.Where(x => x.ViewCount >= minViewCount && x.ViewCount <= maxViewCount).Take(takeSize).OrderByDescending(x =>
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
                                x.ViewCount).ToList()
                                    : userBlogs.Where(x => x.CommentCount >= minCommentCount && x.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(x =>
                                x.ViewCount).ToList();
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
                if (!sortedBlogs.Any())
                    return CustomResponseDto<List<BlogListDto>>.Fail(404, "Kullanıcının bu kriterlere ait gösterilecek bir bloğu bulunamadı!");

                var blogListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponseDto<List<BlogListDto>>.Fail(404, "Kullanıcıya ait gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponseDto<BlogListResultDto>> GetAllBlogsFilteredAsync(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeTag, bool includeComments, bool includeUser)
        {
            List<Expression<Func<Blog, bool>>> predicates = new List<Expression<Func<Blog, bool>>>();
            List<Expression<Func<Blog, object>>> includes = new List<Expression<Func<Blog, object>>>();

            if (categoryId.HasValue)
            {
                if (!await UnitOfWork.Blogs.AnyAsync(x => x.BlogCategories.Any(x => x.CategoryId == categoryId.Value)))
                {
                    return CustomResponseDto<BlogListResultDto>.Fail(404, $"{categoryId} numaralı kategoriye ait bir blog bulunamadı!");
                }
                predicates.Add(x => x.BlogCategories.Any(x => x.CategoryId == categoryId.Value));
            }

            if (userId.HasValue)
            {
                if (!await UnitOfWork.Users.AnyAsync(x => x.Id == userId.Value))
                {
                    return CustomResponseDto<BlogListResultDto>.Fail(404, "Böyle bir kullanıcı bulunamadı!");
                }
                predicates.Add(x => x.UserId == userId.Value);
            }

            if (isActive.HasValue) predicates.Add(x => x.IsActive == isActive.Value);
            if (isDeleted.HasValue) predicates.Add(x => x.IsDeleted == isDeleted.Value);

            if (includeCategory) includes.Add(x => x.BlogCategories);
            if (includeTag) includes.Add(x => x.BlogTags);
            if (includeComments) includes.Add(x => x.Comments.Where(x => x.IsActive && !x.IsDeleted));
            if (includeUser) includes.Add(x => x.User);

            var blogs = await UnitOfWork.Blogs.GetAllByListedAsync(predicates, includes);

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

                var blogListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                return CustomResponseDto<BlogListResultDto>.Success(200, new BlogListResultDto
                {
                    BlogListDto = blogListDto,
                    CategoryId = categoryId.HasValue ? categoryId.Value : null,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = blogListDto.Count,
                    IsAscending = isAscending,
                });
            }
            return CustomResponseDto<BlogListResultDto>.Fail(404, "Bu filtrelere uygun gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponseDto<BlogListDto>> GetByBlogIdAsync(int blogId)//blog detayda kul.
        {
            var blog = await UnitOfWork.Blogs.GetAsync(x => x.Id == blogId, x => x.User, x => x.BlogCategories, x => x.BlogTags, x => x.Comments.Where(x => x.IsActive && !x.IsDeleted));

            if (blog != null)
            {
                var blogDto = Mapper.Map<BlogListDto>(blog);

                return CustomResponseDto<BlogListDto>.Success(200, blogDto);
            }
            return CustomResponseDto<BlogListDto>.Fail(200, "Gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponseDto<BlogListDto>> GetFilteredByBlogIdAsync(int blogId, bool includeCategory, bool includeTag, bool includeComment, bool includeUser)
        {
            List<Expression<Func<Blog, bool>>> predicates = new List<Expression<Func<Blog, bool>>>();
            List<Expression<Func<Blog, object>>> includes = new List<Expression<Func<Blog, object>>>();

            if (includeCategory) includes.Add(x => x.BlogCategories);
            if (includeTag) includes.Add(x => x.BlogTags);
            if (includeComment) includes.Add(x => x.Comments);
            if (includeUser) includes.Add(x => x.User);
            predicates.Add(x => x.Id == blogId);

            var blog = await UnitOfWork.Blogs.GetByListedAsync(predicates, includes);

            if (blog == null)
                return CustomResponseDto<BlogListDto>.Fail(404, $"{blogId} numaralı bir blog bulunamadı!");

            var blogDto = Mapper.Map<BlogListDto>(blog);

            return CustomResponseDto<BlogListDto>.Success(200, blogDto);

        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByTagAsync(int tagId)
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.BlogTags.Any(x => x.TagId == tagId) && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.BlogTags, x => x.User);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponseDto<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponseDto<List<BlogListDto>>.Fail(404, "Bu etikete ait gösterilecek bir blog bulunamadı!");

        }
    }
}
