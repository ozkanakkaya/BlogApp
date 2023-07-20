using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class TagController : CustomControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IValidator<TagUpdateDto> _tagUpdateDtoValidator;

        public TagController(ITagService tagService, IValidator<TagUpdateDto> tagUpdateDtoValidator)
        {
            _tagService = tagService;
            _tagUpdateDtoValidator = tagUpdateDtoValidator;
        }

        [HttpPut("[action]/{tagId}")]
        public async Task<IActionResult> Delete(int tagId)
        {
            var result = await _tagService.DeleteAsync(tagId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<TagDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<TagDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpPut("[action]/{tagId}")]
        public async Task<IActionResult> UndoDelete(int tagId)
        {
            var result = await _tagService.UndoDeleteAsync(tagId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<TagDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<TagDto>.Success(result.StatusCode, result.Data));
        }

        [HttpDelete("{tagId}")]
        public async Task<IActionResult> HardDelete(int tagId)
        {
            var result = await _tagService.HardDeleteAsync(tagId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<NoContent>.Success(result.StatusCode));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByNonDeleted()
        {
            var result = await _tagService.GetAllByNonDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<TagListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<TagListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByActive()
        {
            var result = await _tagService.GetAllByActiveAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<TagListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<TagListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tagService.GetAllTagsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<TagListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<TagListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]/{tagId}")]
        public async Task<IActionResult> GetTagUpdateDto(int tagId)
        {
            var result = await _tagService.GetTagUpdateDtoAsync(tagId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<TagUpdateDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<TagUpdateDto>.Success(result.StatusCode, result.Data));
        }

        [HttpPut]
        public async Task<IActionResult> Update(TagUpdateDto tagUpdateDto)
        {
            var result = _tagUpdateDtoValidator.Validate(tagUpdateDto);
            if (result.IsValid)
            {
                var resultUpdated = await _tagService.UpdateAsync(tagUpdateDto);
                if (resultUpdated.Errors.Any())
                {
                    return CreateActionResult(CustomResponseDto<TagDto>.Fail(resultUpdated.StatusCode, resultUpdated.Errors));
                }
                return CreateActionResult(CustomResponseDto<TagDto>.Success(resultUpdated.StatusCode, resultUpdated.Data));
            }

            result.Errors.ForEach(error => ModelState.AddModelError(error.PropertyName, error.ErrorMessage));

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<TagDto>.Fail(200, errors));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountTotal()
        {
            var result = await _tagService.CountTotalAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<int>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByNonDeleted()
        {
            var result = await _tagService.CountByNonDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<int>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByDeleted()
        {
            var deletedUsers = await _tagService.GetAllByDeletedAsync();
            if (deletedUsers.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<TagListDto>.Fail(deletedUsers.StatusCode, deletedUsers.Errors));
            }
            return CreateActionResult(CustomResponseDto<TagListDto>.Success(deletedUsers.StatusCode, deletedUsers.Data));
        }
    }

}

