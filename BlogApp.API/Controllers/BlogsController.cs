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
        private readonly IValidator<BlogCreateDto> _createDtoValidator;

        public BlogsController(IBlogService blogService, IMapper mapper, IValidator<BlogCreateDto> createDtoValidator)
        {
            _blogService = blogService;
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
        }

        //[Authorize(Roles = "Admin,Author")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add(BlogCreateDto createDto, string tags)
        {
            var result = _createDtoValidator.Validate(createDto);

            if (result.IsValid)
            {
                var addResult = await _blogService.AddBlogWithTags(createDto, tags);
                if (!addResult.Errors.Any())
                    return CreateActionResult(CustomResponse<BlogCreateDto>.Success(201, createDto));
                return CreateActionResult(CustomResponse<BlogCreateDto>.Fail(addResult.StatusCode, addResult.Errors));
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
