using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface ICategoryService : IService<Category>
    {
        Task<CustomResponseDto<CategoryDto>> AddAsync(CategoryCreateDto categoryCreateDto);
        Task<CustomResponseDto<CategoryDto>> DeleteAsync(int categoryId);
        Task<CustomResponseDto<CategoryDto>> UndoDeleteAsync(int categoryId);
        Task<CustomResponseDto<CategoryDto>> HardDeleteAsync(int categoryId);
        Task<CustomResponseDto<CategoryListDto>> GetAllByNonDeletedAsync();
        Task<CustomResponseDto<CategoryListDto>> GetAllByActiveAsync();
        Task<CustomResponseDto<CategoryListDto>> GetAllCategoriesAsync();
        Task<CustomResponseDto<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId);
        Task<CustomResponseDto<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto);
        Task<CustomResponseDto<int>> CountTotalAsync();
        Task<CustomResponseDto<int>> CountByNonDeletedAsync();
        Task<CustomResponseDto<CategoryListDto>> GetAllByDeletedAsync();
    }
}
