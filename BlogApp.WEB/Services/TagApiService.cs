using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.WEB.Services
{
    public class TagApiService
    {
        private readonly HttpClient _httpClient;

        public TagApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<CustomResponseDto<TagDto>> UpdateAsync(TagUpdateDto newTag)
        {
            var response = await _httpClient.PutAsJsonAsync("tag", newTag);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TagDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<TagDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<TagDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<TagDto>> DeleteAsync(int tagtId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<TagDto>>($"tag/delete/{tagtId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TagDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<TagDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<TagDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<NoContent>> HardDeleteAsync(int tagId)
        {
            var response = await _httpClient.DeleteAsync($"tag/{tagId}");

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContent>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<NoContent>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<NoContent>.Success(responseBody.StatusCode);
        }

        public async Task<CustomResponseDto<int>> CountTotalAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("tag/counttotal");

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
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("tag/countbynondeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<TagDto>> UndoDeleteAsync(int tagId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<TagDto>>($"tag/undodelete/{tagId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TagDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<TagDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<TagDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<TagUpdateDto>> GetTagUpdateDtoAsync(int tagId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<TagUpdateDto>>($"tag/gettagupdatedto/{tagId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<TagUpdateDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<TagUpdateDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<TagListDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<TagListDto>>("tag/GetAll");

            if (response.Errors.Any())
            {
                return CustomResponseDto<TagListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<TagListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<TagListDto>> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<TagListDto>>("tag/GetAllByDeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<TagListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<TagListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<TagListDto>> GetAllByNonDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<TagListDto>>("tag/getallbynondeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<TagListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<TagListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<TagListDto>> GetAllByActiveAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<TagListDto>>("tag");

            if (response.Errors.Any())
            {
                return CustomResponseDto<TagListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<TagListDto>.Success(response.StatusCode, response.Data);
            }
        }
    }
}
