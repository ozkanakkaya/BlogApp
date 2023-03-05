using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface ICommentService : IService<Comment>
    {
        Task<CustomResponse<CommentDto>> AddAsync(CommentCreateDto commentCreateDto, string userId);
        Task<CustomResponse<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto);
        Task<CustomResponse<CommentDto>> DeleteAsync(int commentId);
        Task<CustomResponse<NoContent>> HardDeleteAsync(int commentId);
        Task<CustomResponse<int>> CountTotalAsync();
        Task<CustomResponse<int>> CountByNonDeletedAsync();
        Task<CustomResponse<CommentDto>> ApproveAsync(int commentId);
        Task<CustomResponse<CommentDto>> UndoDeleteAsync(int categoryId);
        Task<CustomResponse<CommentDto>> GetCommentByIdAsync(int categoryId);
        Task<CustomResponse<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId);
        Task<CustomResponse<CommentListDto>> GetAllCommentsAsync();
        Task<CustomResponse<CommentListDto>> GetAllByDeletedAsync();
        Task<CustomResponse<CommentListDto>> GetAllByNonDeletedAsync();
        Task<CustomResponse<CommentListDto>> GetAllByActiveAsync();
    }
}
