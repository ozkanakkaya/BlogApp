using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface ITagService : IService<Tag>
    {
        Task<CustomResponseDto<TagDto>> DeleteAsync(int tagId);
        Task<CustomResponseDto<TagDto>> UndoDeleteAsync(int tagId);
        Task<CustomResponseDto<NoContent>> HardDeleteAsync(int tagId);
        Task<CustomResponseDto<TagListDto>> GetAllByNonDeletedAsync();
        Task<CustomResponseDto<TagListDto>> GetAllByActiveAsync();
        Task<CustomResponseDto<TagListDto>> GetAllTagsAsync();
        Task<CustomResponseDto<TagUpdateDto>> GetTagUpdateDtoAsync(int tagId);
        Task<CustomResponseDto<TagDto>> UpdateAsync(TagUpdateDto tagUpdateDto);
        Task<CustomResponseDto<int>> CountTotalAsync();
        Task<CustomResponseDto<int>> CountByNonDeletedAsync();
        Task<CustomResponseDto<TagListDto>> GetAllByDeletedAsync();

    }
}
