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

namespace BlogApp.Business.Services
{
    public class BlogService : Service<Blog>, IBlogService
    {
        private readonly IImageHelper _imageHelper;
        public BlogService(IGenericRepository<Blog> repository, IUnitOfWork unitOfWork, IMapper mapper, IImageHelper imageHelper) : base(repository, unitOfWork, mapper)
        {
            _imageHelper = imageHelper;
        }
        public async Task<CustomResponse<BlogCreateDto>> AddBlogWithTagsAndCategoriesAsync(BlogCreateDto blogCreateDto)
        {
            var tagSplit = blogCreateDto.Tags.Split(',');

            var blog = Mapper.Map<Blog>(blogCreateDto);
            blog.TagBlogs = new List<TagBlog>();
            blog.BlogCategories = new List<BlogCategory>();

            foreach (var tag in tagSplit)
            {
                var tagName = tag.Trim();
                var checkTag = await UnitOfWork.Tags.Where(x => x.Name == tagName && !x.IsDeleted).FirstOrDefaultAsync();
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

                    await UnitOfWork.Tags.AddAsync(tagModel);
                    await UnitOfWork.CommitAsync();

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

            foreach (var categoryId in blogCreateDto.CategoryIds)
            {
                blog.BlogCategories.Add(new BlogCategory
                {
                    Blog = blog,
                    CategoryId = categoryId
                });
            }
            //blog.CreatedByUsername = (await _appUserRepository.GetByIdAsync(blogDto.AppUserId)).Username;

            var thumbnailResult = await _imageHelper.UploadAsync(blogCreateDto.Title, blogCreateDto.ThumbnailFile, ImageType.Post);
            blog.Thumbnail = thumbnailResult.StatusCode == 200 ? thumbnailResult.Data.FullName : "postImages/defaultThumbnail.png";

            await UnitOfWork.Blogs.AddAsync(blog);
            await UnitOfWork.CommitAsync();

            return CustomResponse<BlogCreateDto>.Success(201, blogCreateDto);
        }

        public async Task<CustomResponse<NoContent>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto)
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

            if (!SetwiseEquivalentTo<string>(blogUpdateDto.Tags.Split(',').Select(x => x.Trim()).ToList(), oldBlog.TagBlogs.Select(x => x.Tag.Name).ToList()))
            {
                blog.TagBlogs.Clear();
                var tagSplit = blogUpdateDto.Tags.Split(',');

                foreach (var tag in tagSplit)
                {
                    var tagName = tag.Trim();
                    var checkTag = await UnitOfWork.Tags.Where(x => x.Name == tagName && !x.IsDeleted).FirstOrDefaultAsync();
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

                        await UnitOfWork.Tags.AddAsync(tagModel);
                        await UnitOfWork.CommitAsync();

                        blog.TagBlogs.Add(new TagBlog
                        {
                            TagId = tagModel.Id
                        });
                    }
                    else
                    {
                        blog.TagBlogs.Add(new TagBlog
                        {
                            TagId = checkTag.Id
                        });
                    }
                }
            }

            //blog.UpdatedByUsername = (await _appUserRepository.GetByIdAsync(blogUpdateDto.AppUserId)).Username;

            bool isNewThumbnailUploaded = false;
            var oldUserThumbnail = oldBlog.Thumbnail;

            if (blogUpdateDto.ThumbnailFile != null)
            {
                var thumbnailResult = await _imageHelper.UploadAsync(blogUpdateDto.Title, blogUpdateDto.ThumbnailFile, ImageType.Post);
                if (thumbnailResult.StatusCode == 200)
                    blog.Thumbnail = thumbnailResult.Data.FullName;

                if (oldUserThumbnail != "postImages/defaultThumbnail.png")
                {
                    isNewThumbnailUploaded = true;
                }
            }

            UnitOfWork.Blogs.Update(blog);
            await UnitOfWork.CommitAsync();

            if (isNewThumbnailUploaded)
                await _imageHelper.DeleteAsync(oldUserThumbnail);

            return CustomResponse<NoContent>.Success(204);
        }

        public async Task<CustomResponse<NoContent>> DeleteAsync(int blogId)
        {
            var blog = await UnitOfWork.Blogs.Where(x => x.Id == blogId).FirstOrDefaultAsync();
            if (blog != null)
            {
                blog.IsDeleted = true;
                blog.IsActive = false;
                //blog.UpdatedByUsername = (await _appUserRepository.GetByIdAsync(blog.AppUserId)).Username;
                UnitOfWork.Blogs.Update(blog);
                await UnitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, $"{blogId} numaralı blog bulunamadı!");
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByActiveAsync()
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByNonDeletedAsync()//Aktif ve Pasif bloglar
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

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
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.AppUserId == userId && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                var personalBlogDto = new PersonalBlogDto();
                personalBlogDto.Blogs = blogListDto;
                personalBlogDto.TotalBlogCount = blogs.Count;
                personalBlogDto.TotalActiveBlogCount = await UnitOfWork.Blogs.Where(x => x.IsActive && x.AppUserId == userId).CountAsync();
                personalBlogDto.TotalInactiveBlogCount = await UnitOfWork.Blogs.Where(x => !x.IsActive && x.AppUserId == userId).CountAsync();
                blogs.ForEach(x => { personalBlogDto.TotalBlogsViewedCount += x.ViewsCount; });

                return CustomResponse<PersonalBlogDto>.Success(200, personalBlogDto);
            }
            return CustomResponse<PersonalBlogDto>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<int>> CountTotalBlogsAsync()//Admin-Home
        {
            var blogsTotalCount = await UnitOfWork.Blogs.CountAsync();

            return blogsTotalCount > -1 ? CustomResponse<int>.Success(200, blogsTotalCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");
        }

        public async Task<CustomResponse<int>> CountActiveBlogsAsync()
        {
            var blogsTotalCount = await UnitOfWork.Blogs.CountAsync(x => x.IsActive);

            return blogsTotalCount > -1 ? CustomResponse<int>.Success(200, blogsTotalCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");

        }

        public async Task<CustomResponse<int>> CountInactiveBlogsAsync()
        {
            var blogsTotalCount = await UnitOfWork.Blogs.CountAsync(x => !x.IsActive);

            return blogsTotalCount > -1 ? CustomResponse<int>.Success(200, blogsTotalCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsTotalCount}");
        }

        public async Task<CustomResponse<int>> CountByDeletedBlogsAsync()
        {
            var blogsDeletedCount = await UnitOfWork.Blogs.CountAsync(x => x.IsDeleted);

            return blogsDeletedCount > -1 ? CustomResponse<int>.Success(200, blogsDeletedCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsDeletedCount}");
        }

        public async Task<CustomResponse<int>> CountByNonDeletedBlogsAsync()
        {
            var blogsNonDeletedCount = await UnitOfWork.Blogs.CountAsync(x => !x.IsDeleted);

            return blogsNonDeletedCount > -1 ? CustomResponse<int>.Success(200, blogsNonDeletedCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {blogsNonDeletedCount}");
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllBlogsAsync()//Admin-Home-Bloglar
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(null, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> HardDeleteAsync(int blogId)//Admin-Arşiv-Blog
        {
            var result = await UnitOfWork.Blogs.AnyAsync(x => x.Id == blogId);
            if (result)
            {
                var blog = UnitOfWork.Blogs.Where(x => x.Id == blogId);

                var thumbnail = await blog.Select(x => x.Thumbnail).FirstOrDefaultAsync();

                UnitOfWork.Blogs.RemoveRange(blog);
                await UnitOfWork.CommitAsync();

                if (thumbnail != "postImages/defaultThumbnail.png")
                    await _imageHelper.DeleteAsync(thumbnail);

                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> UndoDeleteAsync(int blogId)//Admin-Arşiv-Blog
        {
            var result = await UnitOfWork.Blogs.AnyAsync(x => x.Id == blogId);
            if (result)
            {
                var blog = await UnitOfWork.Blogs.Where(x => x.Id == blogId).FirstOrDefaultAsync();
                blog.IsDeleted = false;
                blog.IsActive = true;
                //blog.UpdatedByUsername=
                //blog.UpdatedDate=
                UnitOfWork.Blogs.Update(blog);
                await UnitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir blog bulunamadı!");
        }

        public async Task<CustomResponse<string>> IncreaseViewCountAsync(int blogId)//Anasayfa-Blog Detayda
        {
            var blog = await UnitOfWork.Blogs.Where(x => x.Id == blogId).FirstOrDefaultAsync();
            if (blog == null)
            {
                return CustomResponse<string>.Fail(404, "Bir blog bulunamadı!");
            }

            blog.ViewsCount += 1;
            UnitOfWork.Blogs.Update(blog);
            await UnitOfWork.CommitAsync();
            return CustomResponse<string>.Success(200, $"{blog.Title} adlı bloğun okunmasıyısı arttırıldı.");
        }

        public async Task<CustomResponse<BlogViewModel>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                //var blogs = await UnitOfWork.Blogs.GetBlogsByDetailsAsync();

                //var sortedBlogs = isAscending
                //    ? blogs.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                //    : blogs.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                //var sortedBlogsListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                //return CustomResponse<List<BlogListDto>>.Success(200, sortedBlogsListDto);
                return CustomResponse<BlogViewModel>.Fail(400, "Lütfen anahtar kelime giriniz!");

            }

            var searchedBlogs = await UnitOfWork.Blogs.SearchAsync(new List<Expression<Func<Blog, bool>>>
            {
                (x)=>x.Title.Contains(keyword),
                (x)=>x.BlogCategories.Any(x=>x.Category.Title.Contains(keyword)),
                (x)=>x.TagBlogs.Any(x=>x.Tag.Name.Contains(keyword))
            },
            x => x.IsActive && !x.IsDeleted,
            x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            if (!searchedBlogs.Any())
                return CustomResponse<BlogViewModel>.Fail(404, "Aradığınız anahtar kelime bulunamadı!");

            var searchedAndSortedBlogs = isAscending
                ? searchedBlogs.OrderBy(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : searchedBlogs.OrderByDescending(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var blogListDto = searchedAndSortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

            return CustomResponse<BlogViewModel>.Success(200, new BlogViewModel
            {
                BlogListDto = blogListDto,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = blogListDto.Count,
                IsAscending = isAscending,
                Keyword = keyword
            });
        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            var sortedBlogs = isAscending ? blogs.OrderBy(x => x.ViewsCount) : blogs.OrderByDescending(x => x.ViewsCount);

            if (blogs.Any())
            {
                var blogListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponse<List<BlogListDto>>.Success(200, takeSize < 1 ? blogListDto : blogListDto.Take(takeSize.Value).ToList());

            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bir blog bulunamadı!");

        }

        public async Task<CustomResponse<BlogViewModel>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var blogs = categoryId == null
                ? await UnitOfWork.Blogs.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser)
                : await UnitOfWork.Blogs.GetAllAsync(x => x.BlogCategories.Any(x => x.CategoryId == categoryId) && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            var sortedBlogs = isAscending
                ? blogs.OrderBy(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : blogs.OrderByDescending(x => x.UpdatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var blogListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

            return CustomResponse<BlogViewModel>.Success(200, new BlogViewModel
            {
                BlogListDto = blogListDto,
                CategoryId = categoryId.HasValue ? categoryId.Value : null,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = blogListDto.Count,
                IsAscending = isAscending
            });

        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByCategoryAsync(int categoryId)
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.BlogCategories.Any(x => x.CategoryId == categoryId) && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bu kategoriye ait gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByUserIdOnFilterAsync(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var anyUser = await UnitOfWork.Users.AnyAsync(x => x.Id == userId);
            if (!anyUser)
            {
                return CustomResponse<List<BlogListDto>>.Fail(404, $"{userId} numarasına ait bir kullanıcı bulunamadı!");
            }

            var userBlogs = await UnitOfWork.Blogs.GetAllAsync(x => x.AppUserId == userId && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

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
                                    ? userBlogs.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderBy(x => x.ViewsCount).ToList()
                                    : userBlogs.Where(x => x.BlogCategories.Any(x => x.CategoryId == categoryId)).Take(takeSize).OrderByDescending(x => x.ViewsCount).ToList();
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
                if (!sortedBlogs.Any())
                    return CustomResponse<List<BlogListDto>>.Fail(404, "Kullanıcının bu kriterlere ait gösterilecek bir bloğu bulunamadı!");

                var blogListDto = sortedBlogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Kullanıcıya ait gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponse<BlogViewModel>> GetAllBlogsFilteredAsync(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeTag, bool includeComments, bool includeUser)
        {
            List<Expression<Func<Blog, bool>>> predicates = new List<Expression<Func<Blog, bool>>>();
            List<Expression<Func<Blog, object>>> includes = new List<Expression<Func<Blog, object>>>();

            if (categoryId.HasValue)
            {
                if (!await UnitOfWork.Blogs.AnyAsync(x => x.BlogCategories.Any(x => x.CategoryId == categoryId.Value)))
                {
                    return CustomResponse<BlogViewModel>.Fail(404, $"{categoryId} numaralı kategoriye ait bir blog bulunamadı!");
                }
                predicates.Add(x => x.BlogCategories.Any(x => x.CategoryId == categoryId.Value));
            }

            if (userId.HasValue)
            {
                if (!await UnitOfWork.Users.AnyAsync(x => x.Id == userId.Value))
                {
                    return CustomResponse<BlogViewModel>.Fail(404, "Böyle bir kullanıcı bulunamadı!");
                }
                predicates.Add(x => x.AppUserId == userId.Value);
            }

            if (isActive.HasValue) predicates.Add(x => x.IsActive == isActive.Value);
            if (isDeleted.HasValue) predicates.Add(x => x.IsDeleted == isDeleted.Value);

            if (includeCategory) includes.Add(x => x.BlogCategories);
            if (includeTag) includes.Add(x => x.TagBlogs);
            if (includeComments) includes.Add(x => x.Comments.Where(x => x.IsActive && !x.IsDeleted));
            if (includeUser) includes.Add(x => x.AppUser);

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

                return CustomResponse<BlogViewModel>.Success(200, new BlogViewModel
                {
                    BlogListDto = blogListDto,
                    CategoryId = categoryId.HasValue ? categoryId.Value : null,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = blogListDto.Count,
                    IsAscending = isAscending,
                });
            }
            return CustomResponse<BlogViewModel>.Fail(404, "Bu filtrelere uygun gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponse<BlogListDto>> GetByBlogIdAsync(int blogId)//blog detayda kul.
        {
            var blog = await UnitOfWork.Blogs.GetAsync(x => x.Id == blogId, x => x.AppUser, x => x.BlogCategories, x => x.TagBlogs, x => x.Comments.Where(x => x.IsActive && !x.IsDeleted));

            if (blog != null)
            {
                var blogDto = Mapper.Map<BlogListDto>(blog);

                return CustomResponse<BlogListDto>.Success(200, blogDto);
            }
            return CustomResponse<BlogListDto>.Fail(404, "Gösterilecek bir blog bulunamadı!");

        }

        public async Task<CustomResponse<BlogListDto>> GetFilteredByBlogIdAsync(int blogId, bool includeCategory, bool includeTag, bool includeComment, bool includeUser)
        {
            List<Expression<Func<Blog, bool>>> predicates = new List<Expression<Func<Blog, bool>>>();
            List<Expression<Func<Blog, object>>> includes = new List<Expression<Func<Blog, object>>>();

            if (includeCategory) includes.Add(x => x.BlogCategories);
            if (includeTag) includes.Add(x => x.TagBlogs);
            if (includeComment) includes.Add(x => x.Comments);
            if (includeUser) includes.Add(x => x.AppUser);
            predicates.Add(x => x.Id == blogId);

            var blog = await UnitOfWork.Blogs.GetByListedAsync(predicates, includes);

            if (blog == null)
                return CustomResponse<BlogListDto>.Fail(404, $"{blogId} numaralı bir blog bulunamadı!");

            var blogDto = Mapper.Map<BlogListDto>(blog);

            return CustomResponse<BlogListDto>.Success(200, blogDto);

        }

        public async Task<CustomResponse<List<BlogListDto>>> GetAllByTagAsync(int tagId)
        {
            var blogs = await UnitOfWork.Blogs.GetAllAsync(x => x.TagBlogs.Any(x => x.TagId == tagId) && x.IsActive && !x.IsDeleted, x => x.BlogCategories, x => x.TagBlogs, x => x.AppUser);

            if (blogs.Any())
            {
                var blogListDto = blogs.Select(blog => Mapper.Map<BlogListDto>(blog)).ToList();

                return CustomResponse<List<BlogListDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogListDto>>.Fail(404, "Bu etikete ait gösterilecek bir blog bulunamadı!");

        }
    }
}
