using BlogApp.Business.Services;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class CategoryController : CustomControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CategoryCreateDto> _categoryCreateDtoValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateDtoValidator;

        public CategoryController(ICategoryService categoryService, IValidator<CategoryCreateDto> categoryCreateDtoValidator, IValidator<CategoryUpdateDto> categoryUpdateDtoValidator)
        {
            _categoryService = categoryService;
            _categoryCreateDtoValidator = categoryCreateDtoValidator;
            _categoryUpdateDtoValidator = categoryUpdateDtoValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryCreateDto categoryCreateDto)
        {
            var result = _categoryCreateDtoValidator.Validate(categoryCreateDto);

            if (result.IsValid)
            {
                var resultAdd = await _categoryService.AddAsync(categoryCreateDto);

                if (!resultAdd.Errors.Any())
                {
                    return CreateActionResult(CustomResponseDto<CategoryDto>.Success(resultAdd.StatusCode, resultAdd.Data));
                }
                else
                {
                    return CreateActionResult(CustomResponseDto<CategoryDto>.Fail(resultAdd.StatusCode, resultAdd.Errors));
                }
            }

            result.Errors.ForEach(error => ModelState.AddModelError(error.PropertyName, error.ErrorMessage));

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<NoContent>.Fail(200, errors));
        }

        [HttpPut("[action]/{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var result = await _categoryService.DeleteAsync(categoryId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<CategoryDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<CategoryDto>.Fail(200, result.Errors));
        }

        [HttpPut("[action]/{categoryId}")]
        public async Task<IActionResult> UndoDelete(int categoryId)
        {
            var result = await _categoryService.UndoDeleteAsync(categoryId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CategoryDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CategoryDto>.Success(result.StatusCode, result.Data));
        }

        [HttpDelete("[action]/{categoryId}")]
        public async Task<IActionResult> HardDelete(int categoryId)
        {
            var result = await _categoryService.HardDeleteAsync(categoryId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CategoryDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CategoryDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByNonDeleted()
        {
            var result = await _categoryService.GetAllByNonDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CategoryListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CategoryListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByActive()
        {
            var result = await _categoryService.GetAllByActiveAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CategoryListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CategoryListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CategoryListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CategoryListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetCategoryUpdateDto(int categoryId)
        {
            var result = await _categoryService.GetCategoryUpdateDtoAsync(categoryId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CategoryUpdateDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CategoryUpdateDto>.Success(result.StatusCode, result.Data));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            var result = _categoryUpdateDtoValidator.Validate(categoryUpdateDto);
            if (result.IsValid)
            {
                var resultUpdated = await _categoryService.UpdateAsync(categoryUpdateDto);
                if (resultUpdated.Errors.Any())
                {
                    return CreateActionResult(CustomResponseDto<CategoryDto>.Fail(resultUpdated.StatusCode, resultUpdated.Errors));
                }
                return CreateActionResult(CustomResponseDto<CategoryDto>.Success(resultUpdated.StatusCode, resultUpdated.Data));
            }

            result.Errors.ForEach(error => ModelState.AddModelError(error.PropertyName, error.ErrorMessage));

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<CategoryDto>.Fail(200, errors));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountTotal()
        {
            var result = await _categoryService.CountTotalAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByNonDeleted()
        {
            var result = await _categoryService.CountByNonDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByDeleted()
        {
            var deletedUsers = await _categoryService.GetAllByDeletedAsync();
            if (deletedUsers.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(deletedUsers.StatusCode, deletedUsers.Errors));
            }
            return CreateActionResult(CustomResponseDto<CategoryListDto>.Success(deletedUsers.StatusCode, deletedUsers.Data));
        }
    }
}
