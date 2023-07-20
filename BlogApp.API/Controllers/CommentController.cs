using BlogApp.API.Filter;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class CommentController : CustomControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IValidator<CommentCreateDto> _commentCreateDtoValidator;
        private readonly IValidator<CommentUpdateDto> _commentUpdateDtoValidator;
        public CommentController(ICommentService commentService, IValidator<CommentCreateDto> commentCreateDtoValidator, IValidator<CommentUpdateDto> commentUpdateDtoValidator)
        {
            _commentService = commentService;
            _commentCreateDtoValidator = commentCreateDtoValidator;
            _commentUpdateDtoValidator = commentUpdateDtoValidator;
        }

        [HttpPost]
        [CheckUserId]
        public async Task<IActionResult> Add(CommentCreateDto commentCreateDto)
        {
            var result = _commentCreateDtoValidator.Validate(commentCreateDto);
            if (result.IsValid)
            {
                var userId = HttpContext.Items["userId"] as string;

                var resultAdd = await _commentService.AddAsync(commentCreateDto, userId);

                if (!resultAdd.Errors.Any())
                {
                    return CreateActionResult(CustomResponseDto<CommentDto>.Success(resultAdd.StatusCode, resultAdd.Data));
                }
                else
                {
                    return CreateActionResult(CustomResponseDto<CommentDto>.Fail(resultAdd.StatusCode, resultAdd.Errors));
                }
            }

            result.Errors.ForEach(error => ModelState.AddModelError(error.PropertyName, error.ErrorMessage));

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, errors));

        }

        [HttpPut]
        public async Task<IActionResult> Update(CommentUpdateDto commentUpdateDto)
        {
            var result = _commentUpdateDtoValidator.Validate(commentUpdateDto);
            if (result.IsValid)
            {
                var resultUpdated = await _commentService.UpdateAsync(commentUpdateDto);
                if (resultUpdated.Errors.Any())
                {
                    return CreateActionResult(CustomResponseDto<CommentDto>.Fail(resultUpdated.StatusCode, resultUpdated.Errors));
                }
                return CreateActionResult(CustomResponseDto<CommentDto>.Success(resultUpdated.StatusCode, resultUpdated.Data));
            }

            result.Errors.ForEach(error => ModelState.AddModelError(error.PropertyName, error.ErrorMessage));

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, errors));
        }

        [HttpPut("[action]/{commentId}")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var result = await _commentService.DeleteAsync(commentId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<CommentDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<CommentDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> HardDelete(int commentId)
        {
            var result = await _commentService.HardDeleteAsync(commentId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<NoContent>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountTotal()
        {
            var result = await _commentService.CountTotalAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByNonDeleted()
        {
            var result = await _commentService.CountByNonDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> Approve(int commentId)
        {
            var result = await _commentService.ApproveAsync(commentId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<CommentDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<CommentDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpPut("[action]/{commentId}")]
        public async Task<IActionResult> UndoDelete(int commentId)
        {
            var result = await _commentService.UndoDeleteAsync(commentId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<CommentDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<CommentDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetDetail(int commentId)
        {
            var result = await _commentService.GetCommentByIdAsync(commentId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<CommentDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<CommentDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpGet("[action]/{commentId}")]
        public async Task<IActionResult> GetCommentUpdateDto(int commentId)
        {
            var result = await _commentService.GetCommentUpdateDtoAsync(commentId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<CommentUpdateDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<CommentUpdateDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetAllCommentsByUserId(int userId)
        {
            var result = await _commentService.GetAllCommentsByUserIdAsync(userId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<CommentListDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<CommentListDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _commentService.GetAllCommentsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CommentListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CommentListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByDeleted()
        {
            var result = await _commentService.GetAllByDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CommentListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CommentListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByNonDeleted()
        {
            var result = await _commentService.GetAllByNonDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CommentListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CommentListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByActive()
        {
            var result = await _commentService.GetAllByActiveAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<CommentListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<CommentListDto>.Success(result.StatusCode, result.Data));
        }
    }
}