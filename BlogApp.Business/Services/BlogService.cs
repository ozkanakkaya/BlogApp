using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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

        public CustomResponse<List<BlogListDto>> GetAllByNonDeletedAndActive()
        {
            var blogs = _blogRepository.GetAllByNonDeletedAndActive();
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

        public CustomResponse<List<BlogDto>> GetAllByDeleted()
        {
            var blogs = _blogRepository.Where(x => x.IsDeleted).Include(x => x.AppUser).Include(x => x.BlogCategories).ThenInclude(x => x.Category).ToList();

            if (blogs.Any())
            {
                var blogListDto = new List<BlogDto>();
                foreach (var blog in blogs)
                {
                    var deletedBlog = _mapper.Map<BlogDto>(blog);
                    deletedBlog.Category = blog.BlogCategories.Select(x => x.Category.Title).FirstOrDefault();
                    deletedBlog.Username = blog.AppUser.Username;
                    blogListDto.Add(deletedBlog);
                }
                return CustomResponse<List<BlogDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogDto>>.Fail(404, "Silinmiş bir blog bulunamadı!");
        }

        public bool SetwiseEquivalentTo<T>(List<T> list, List<T> other) where T : IEquatable<T>
        {
            if (list.Except(other).Any())
                return false;
            if (other.Except(list).Any())
                return false;
            return true;
        }

        public CustomResponse<List<BlogDto>> GetByUserId(int userId)
        {
            var blogs =_blogRepository.Where(x=>x.AppUserId==userId).Include(x => x.AppUser).Include(x => x.BlogCategories).ThenInclude(x => x.Category).ToList();
            if (blogs.Any())
            {
                var blogListDto = new List<BlogDto>();
                foreach (var blog in blogs)
                {
                    var deletedBlog = _mapper.Map<BlogDto>(blog);
                    deletedBlog.Category = blog.BlogCategories.Select(x => x.Category.Title).FirstOrDefault();
                    deletedBlog.Username = blog.AppUser.Username;
                    blogListDto.Add(deletedBlog);
                }
                return CustomResponse<List<BlogDto>>.Success(200, blogListDto);
            }
            return CustomResponse<List<BlogDto>>.Fail(404, "Silinmiş bir blog bulunamadı!");
        }

        /*
         *GetBlogById
         *+++++++++++++++GetAllDeletedBlogs
         *UndoDelete
         *HardDeleteAsync
         *GetAllByViewCount
         *GetAllByCategory
         *GetAllByPagingAsync
         *GetAllByUserIdOnFilter
         *SearchAsync
         *IncreaseViewCountAsync => görüntüleme sayısını arttır.
         *CountAsync => blogların toplam sayısı
         *CountByNonDeletedAsync
         *GetAllAsyncV2 =>yorumlar kategoriler kullanıcılar sıralamalar vs göre getirir.
         *GetBlogByIdWithCategoriesAndComments=> GetAsync
         */
    }
}
