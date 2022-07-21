using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.DTOs.Concrete.TagDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.API.Controllers
{
    public class BlogsController : CustomBaseController
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;

        public BlogsController(IBlogService blogService, IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPost("[action]")]
        public IActionResult Add(BlogCreateDto createDto,List<TagListDto> tagLists)
        {
            //var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


            //var blogDto = _mapper.Map<Blog>(createDto);

            //var blog = _blogService.AddBlogWithTags
        }
    }
}
