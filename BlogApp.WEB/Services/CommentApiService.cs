using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
namespace BlogApp.WEB.Services
{
    public class CommentApiService
    {
        private readonly HttpClient _httpClient;

        public CommentApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<CommentDto>> AddAsync(CommentCreateDto newComment)
        {
            var response = await _httpClient.PostAsJsonAsync("comment", newComment);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CommentDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CommentDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<CommentDto>> UpdateAsync(CommentUpdateDto newComment)
        {
            var response = await _httpClient.PutAsJsonAsync("comment", newComment);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CommentDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CommentDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<CommentDto>> DeleteAsync(int commentId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CommentDto>>($"comment/delete/{commentId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CommentDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CommentDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<NoContent>> HardDeleteAsync(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"comment/{categoryId}");

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContent>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<NoContent>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<NoContent>.Success(responseBody.StatusCode);
        }

        public async Task<CustomResponseDto<int>> CountTotalAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("comment/counttotal");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<int>> CountByNonDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("comment/countbynondeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CommentDto>> ApproveAsync(int commentId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CommentDto>>($"comment/{commentId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CommentDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CommentDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<CommentDto>> UndoDeleteAsync(int commentId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CommentDto>>($"comment/undodelete/{commentId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CommentDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CommentDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<CommentDto>> GetDetailAsync(int commentId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentDto>>($"comment/{commentId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CommentDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CommentDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentUpdateDto>>($"comment/getcommentupdatedto/{commentId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CommentUpdateDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CommentUpdateDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>("comment/GetAll");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CommentListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CommentListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>("comment/GetAllByDeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CommentListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CommentListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllByNonDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>("comment/getallbynondeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CommentListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CommentListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllByActiveAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>("comment");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CommentListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CommentListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllCommentsByUserIdAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>($"comment/GetAllCommentsByUserId/{userId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CommentListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CommentListDto>.Success(response.StatusCode, response.Data);
            }
        }
    }
}
