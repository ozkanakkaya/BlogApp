using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class BlogsController : CustomBaseController
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        private readonly IValidator<BlogCreateDto> _blogCreateDtoValidator;
        private readonly IValidator<BlogUpdateDto> _blogUpdateDtoValidotor;

        public BlogsController(IBlogService blogService, IMapper mapper, IValidator<BlogCreateDto> blogCreateDtoValidator, IValidator<BlogUpdateDto> blogUpdateDtoValidotor)
        {
            _blogService = blogService;
            _mapper = mapper;
            _blogCreateDtoValidator = blogCreateDtoValidator;
            _blogUpdateDtoValidotor = blogUpdateDtoValidotor;
        }
        //BlogCreateDto createDto, string tags, [FromBody]List<int> categories
        //createDto, blogDto.Tags, blogDto.CategoryIds
        //[Authorize(Roles = "Admin,Author")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add(BlogCreateDto blogDto)
        {
            var result = _blogCreateDtoValidator.Validate(blogDto);

            if (result.IsValid)
            {
                //var createDto = _mapper.Map<BlogCreateDto>(blogDto);

                var addResult = await _blogService.AddBlogWithTagsAsync(blogDto);
                //if (!addResult.Errors.Any())
                return CreateActionResult(CustomResponse<BlogCreateDto>.Success(201, addResult.Data));
                //return CreateActionResult(CustomResponse<BlogCreateDto>.Fail(addResult.StatusCode, addResult.Errors));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(BlogUpdateDto blogUpdateDto)
        {
            var result = _blogUpdateDtoValidotor.Validate(blogUpdateDto);

            if (result.IsValid)
            {
                var result1 = await _blogService.UpdateBlogAsync(blogUpdateDto);
                if (result1.StatusCode == 204)
                {
                    return CreateActionResult(CustomResponse<NoContent>.Success(204));
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }


    }
}
