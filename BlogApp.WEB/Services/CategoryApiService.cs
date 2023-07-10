using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace BlogApp.WEB.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<CategoryDto>> AddAsync(CategoryCreateDto newCategory)
        {
            var response = await _httpClient.PostAsJsonAsync("category", newCategory);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CategoryDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CategoryDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CategoryDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<CategoryDto>> UpdateAsync(CategoryUpdateDto newCategory)
        {
            var response = await _httpClient.PutAsJsonAsync("category", newCategory);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CategoryDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CategoryDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CategoryDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<CategoryDto>> DeleteAsync(int categoryId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CategoryDto>>($"category/delete/{categoryId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CategoryDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CategoryDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CategoryDto>.Success(responseBody.StatusCode, responseBody.Data);
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

        public async Task<CustomResponseDto<CategoryListDto>> GetAllByNonDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryListDto>>("category/getallbynondeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CategoryListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CategoryListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CategoryListDto>> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryListDto>>("category/getallbydeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CategoryListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CategoryListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CategoryListDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryListDto>>("category/getall");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CategoryListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CategoryListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<CategoryDto>> HardDeleteAsync(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"category/harddelete/{categoryId}");

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CategoryDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CategoryDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CategoryDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<CategoryDto>> UndoDeleteAsync(int categoryId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<CategoryDto>>($"category/undodelete/{categoryId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CategoryDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<CategoryDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<CategoryDto>.Success(responseBody.StatusCode, responseBody.Data);
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

        public async Task<CustomResponseDto<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CategoryUpdateDto>>($"category/getcategoryupdatedto/{categoryId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<CategoryUpdateDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<CategoryUpdateDto>.Success(response.StatusCode, response.Data);
            }
        }
    }
}
