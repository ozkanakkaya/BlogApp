using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;
using System.Net.Http.Json;

namespace BlogApp.WEB.Services
{
    public class RoleApiService
    {
        private readonly HttpClient _httpClient;

        public RoleApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<RoleListDto>> GetAllRolesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<RoleListDto>>("role");

            if (response.Errors.Any())
            {
                return CustomResponseDto<RoleListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<RoleListDto>.Success(response.StatusCode, response.Data);
            }
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

        public async Task<CustomResponseDto<UserRoleAssignDto>> GetUserRoleAssignDtoAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<UserRoleAssignDto>>($"role/GetUserRoleAssignDto/{userId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<UserRoleAssignDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<UserRoleAssignDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<UserDto>> AssignAsync(UserRoleAssignDto userRoleAssignDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"role/Assign", userRoleAssignDto);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<UserDto>>();

            if (responseBody.Errors.Any())
            {
                return CustomResponseDto<UserDto>.Fail(responseBody.StatusCode, responseBody.Errors);
            }
            else
            {
                return CustomResponseDto<UserDto>.Success(responseBody.StatusCode, responseBody.Data);
            }
        }
    }
}
