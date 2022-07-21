using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.DTOs.Concrete.TagDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public class BlogService : Service<Blog>, IBlogService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public BlogService(IGenericRepository<Blog> repository, IUnitOfWork unitOfWork, IAppUserRepository appUserRepository, IMapper mapper, IAppRoleRepository appRoleRepository, IBlogRepository blogRepository, ITagRepository tagRepository) : base(repository, unitOfWork)
        {
            _appUserRepository = appUserRepository;
            _mapper = mapper;
            _appRoleRepository = appRoleRepository;
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
        }

        public async Task<CustomResponse<BlogCreateDto>> AddBlogWithTags(BlogCreateDto createDto, List<TagListDto> tagList)
        {
            /*"tagList"ten gelen taglerin bilgileri Tag tablosuna eklenecek*/

            //var blog = _mapper.Map<Blog>(createDto);

            //blog.TagBlogs = new List<TagBlog>();
            //blog.TagBlogs.Add()
            //blog.TagBlogs.Add(new TagBlog
            //{
            //    Blog = blog,
            //    TagId=/*Ekllenen taglerin Id leri sırasıyla ilgili bloğun TagId'sine eklenecek  */
            //});


            //var tags = _mapper.Map<Tag>(tagList);

            //foreach (var tag in tagList)
            //{
            //    await _tagRepository.AddAsync(_mapper.Map<Tag>(tag));

            //    _blogRepository.(x=>x.TagBlogs
            //}




            //await _blogRepository.AddAsync(blog);
            //await _unitOfWork.CommitAsync();

            return CustomResponse<BlogCreateDto>.Success(201, createDto);
        }
    }
}
