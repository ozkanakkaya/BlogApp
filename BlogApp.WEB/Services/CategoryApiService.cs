using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WEB.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CategoryCreateDto> AddAsync([FromForm] CategoryCreateDto newCategory)
        {
            var response = await _httpClient.PostAsJsonAsync("category", newCategory);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CategoryCreateDto>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Ekleme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return responseBody.Data;
        }

        public async Task<bool> UpdateAsync([FromForm] CategoryUpdateDto newCategory)
        {
            var response = await _httpClient.PutAsJsonAsync("category", newCategory);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContent>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Güncelleme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CategoryDto>>($"category/delete/{categoryId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CategoryDto>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return response.IsSuccessStatusCode;
        }

        public async Task<CustomResponseDto<CategoryListDto>> GetAllByActiveAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryListDto>>("category");
            //if (response.Errors.Any())
            //{
            //	var errorMessage = string.Join(Environment.NewLine, response.Errors);
            //	throw new Exception(errorMessage);
            //}
            //return response.Data;
            if (response.Errors.Any())
            {
                return CustomResponseDto<CategoryListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CategoryListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CategoryListDto> GetAllByNonDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryListDto>>("category/getallbynondeleted");

            if (response.Errors.Any())
            {
                throw new Exception($"Kategoriler getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CategoryListDto> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryListDto>>("category/getallbydeleted");

            if (response.Errors.Any())
            {
                throw new Exception($"Kategoriler getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CategoryListDto> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryListDto>>("category/getall");

            if (response.Errors.Any())
            {
                throw new Exception($"Kategoriler getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<bool> HardDeleteAsync(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"category/harddelete/{categoryId}");

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContent>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return response.IsSuccessStatusCode;
        }

        public async Task<CategoryDto> UndoDeleteAsync(int categoryId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CategoryDto>>($"blog/undodelete/{categoryId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CategoryDto>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silmeyi geri alma işlemini sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return responseBody.Data;
        }

        public async Task<CustomResponseDto<int>> CountTotalAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("blog/counttotal");

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
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("category/countbynondeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CategoryUpdateDto> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryUpdateDto>>($"category/getcategoryupdatedto/{categoryId}");

            if (response.Errors.Any())
            {
                //_logger.LogWarning(errorMessage);
                throw new Exception($"Kategori getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }
    }
}
