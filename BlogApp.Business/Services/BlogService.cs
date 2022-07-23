using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;

namespace BlogApp.Business.Services
{
    public class BlogService : Service<Blog>, IBlogService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITagBlogRepository _tagBlogRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public BlogService(IGenericRepository<Blog> repository, IUnitOfWork unitOfWork, IAppUserRepository appUserRepository, IMapper mapper, IAppRoleRepository appRoleRepository, IBlogRepository blogRepository, ITagRepository tagRepository, ITagBlogRepository tagBlogRepository) : base(repository, unitOfWork)
        {
            _appUserRepository = appUserRepository;
            _mapper = mapper;
            _appRoleRepository = appRoleRepository;
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
            _tagBlogRepository = tagBlogRepository;
        }

        public async Task<CustomResponse<BlogCreateDto>> AddBlogWithTags(BlogCreateDto createDto, string tags)
        {
            if (!string.IsNullOrEmpty(tags))
            {
                var tagSplit = tags.Split(',');

                var blog = _mapper.Map<Blog>(createDto);
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
                await _blogRepository.AddAsync(blog);
                await _unitOfWork.CommitAsync();

                return CustomResponse<BlogCreateDto>.Success(201, createDto);
            }

            return CustomResponse<BlogCreateDto>.Fail(400, "Bloğa ait tanımlayıcı etiketler girilmeli!");
        }
    }
}
