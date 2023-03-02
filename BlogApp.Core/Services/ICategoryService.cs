using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface ICategoryService : IService<Category>
    {
        Task<CustomResponse<CategoryCreateDto>> AddAsync(CategoryCreateDto categoryCreateDto);
        Task<CustomResponse<NoContent>> DeleteAsync(int categoryId);
        Task<CustomResponse<NoContent>> UndoDeleteAsync(int categoryId);
        Task<CustomResponse<NoContent>> HardDeleteAsync(int categoryId);
        Task<CustomResponse<CategoryListDto>> GetAllByNonDeletedAsync();
        Task<CustomResponse<CategoryListDto>> GetAllByActiveAsync();
        Task<CustomResponse<CategoryListDto>> GetAllCategoriesAsync();
        Task<CustomResponse<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId);
        Task<CustomResponse<NoContent>> UpdateAsync(CategoryUpdateDto categoryUpdateDto);
        Task<CustomResponse<int>> CountTotalAsync();
        Task<CustomResponse<int>> CountByNonDeletedAsync();
        Task<CustomResponse<CategoryListDto>> GetAllByDeletedAsync();
    }
}
