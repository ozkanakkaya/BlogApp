using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface ITagService : IService<Tag>
    {
        Task<CustomResponse<TagDto>> DeleteAsync(int tagId);
        Task<CustomResponse<TagDto>> UndoDeleteAsync(int tagId);
        Task<CustomResponse<NoContent>> HardDeleteAsync(int tagId);
        Task<CustomResponse<TagListDto>> GetAllByNonDeletedAsync();
        Task<CustomResponse<TagListDto>> GetAllByActiveAsync();
        Task<CustomResponse<TagListDto>> GetAllTagsAsync();
        Task<CustomResponse<TagUpdateDto>> GetTagUpdateDtoAsync(int tagId);
        Task<CustomResponse<NoContent>> UpdateAsync(TagUpdateDto tagUpdateDto);
        Task<CustomResponse<int>> CountTotalAsync();
        Task<CustomResponse<int>> CountByNonDeletedAsync();
        Task<CustomResponse<TagListDto>> GetAllByDeletedAsync();

    }
}
