using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface ICommentService : IService<Comment>
    {
        Task<CustomResponseDto<CommentDto>> AddAsync(CommentCreateDto commentCreateDto, string userId);
        Task<CustomResponseDto<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto);
        Task<CustomResponseDto<CommentDto>> DeleteAsync(int commentId);
        Task<CustomResponseDto<NoContent>> HardDeleteAsync(int commentId);
        Task<CustomResponseDto<int>> CountTotalAsync();
        Task<CustomResponseDto<int>> CountByNonDeletedAsync();
        Task<CustomResponseDto<CommentDto>> ApproveAsync(int commentId);
        Task<CustomResponseDto<CommentDto>> UndoDeleteAsync(int categoryId);
        Task<CustomResponseDto<CommentDto>> GetCommentByIdAsync(int categoryId);
        Task<CustomResponseDto<CommentListDto>> GetAllCommentsByUserIdAsync(int userId);
        Task<CustomResponseDto<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId);
        Task<CustomResponseDto<CommentListDto>> GetAllCommentsAsync();
        Task<CustomResponseDto<CommentListDto>> GetAllByDeletedAsync();
        Task<CustomResponseDto<CommentListDto>> GetAllByNonDeletedAsync();
        Task<CustomResponseDto<CommentListDto>> GetAllByActiveAsync();
    }
}
