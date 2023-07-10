using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
namespace BlogApp.WEB.Services
{
    public class CommentApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CustomResponseDto<CommentDto>> AddAsync(CommentCreateDto newComment)
        {
            var response = await _httpClient.PostAsJsonAsync("comment", newComment);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CommentDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CommentDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CommentDto> UpdateAsync([FromForm] CommentUpdateDto newComment)
        {
            var response = await _httpClient.PutAsJsonAsync("comment", newComment);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Güncelleme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return responseBody.Data;
        }

        public async Task<CommentDto> DeleteAsync(int commentId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CommentDto>>($"comment/delete/{commentId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return responseBody.Data;
        }

        public async Task<bool> HardDeleteAsync(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"comment/{categoryId}");

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContent>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return response.IsSuccessStatusCode;
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

        public async Task<CommentDto> Approve(int commentId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CommentDto>>($"comment/{commentId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Onaylama işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return responseBody.Data;
        }

        public async Task<CommentDto> UndoDeleteAsync(int commentId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CategoryDto>>($"comment/undodelete/{commentId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CommentDto>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silmeyi geri alma işlemini sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return responseBody.Data;
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

        public async Task<CommentUpdateDto> GetCommentUpdateDtoAsync(int commentId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentUpdateDto>>($"comment/getcommentupdatedto/{commentId}");

            if (response.Errors.Any())
            {
                //_logger.LogWarning(errorMessage);
                throw new Exception($"Yorum bilgileri getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CommentListDto> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>("comment/GetAll");

            if (response.Errors.Any())
            {
                throw new Exception($"Yorumlar getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CommentListDto> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>("comment/GetAllByDeleted");

            if (response.Errors.Any())
            {
                throw new Exception($"Yorumlar getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CommentListDto> GetAllByNonDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>("comment/getallbynondeleted");

            if (response.Errors.Any())
            {
                throw new Exception($"Yorumlar getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllByActiveAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CommentListDto>>("comment");
            //if (response.Errors.Any())
            //{
            //	var errorMessage = string.Join(Environment.NewLine, response.Errors);
            //	throw new Exception(errorMessage);
            //}
            //return response.Data;
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
