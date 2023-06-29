using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.WEB.Services
{
    public class RoleApiService
    {
        private readonly HttpClient _httpClient;

        public RoleApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<RoleListDto>> GetAllByUserIdAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<RoleListDto>>($"role/GetAllByUserId/{userId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<RoleListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<RoleListDto>.Success(response.StatusCode, response.Data);
            }
        }
    }
}
