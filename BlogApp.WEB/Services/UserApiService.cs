using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace BlogApp.WEB.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<UserDto>> RegisterAsync(UserRegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("user", registerDto);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<UserDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<UserDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<UserDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<List<UserListDto>>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserListDto>>>("user/getallusers");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<UserListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<UserListDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<List<UserListDto>>> GetAllByActiveAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserListDto>>>("user");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<UserListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<UserListDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<UserListDto>> GetUserByIdAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<UserListDto>>($"user/{userId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<UserListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<UserListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<UserDto>> DeleteAsync(int userId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<UserDto>>($"user/delete/{userId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<UserDto>>();

            if (responseBody.Errors.Any())
            {
                return CustomResponseDto<UserDto>.Fail(responseBody.StatusCode, responseBody.Errors);
            }
            else
            {
                return CustomResponseDto<UserDto>.Success(responseBody.StatusCode,responseBody.Data);
            }
        }

        public async Task<CustomResponseDto<UserDto>> UndoDeleteAsync(int userId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<UserDto>>($"user/undodelete/{userId}", null);

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

        public async Task<CustomResponseDto<UserDto>> HardDeleteAsync(int userId)
        {
            var response = await _httpClient.PostAsJsonAsync<CustomResponseDto<UserDto>>($"user/{userId}", null);

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

        public async Task<CustomResponseDto<List<UserDto>>> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserDto>>>($"user/GetAllByDeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<UserDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<UserDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<List<UserListDto>>> GetAllByInactiveAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserListDto>>>($"user/GetAllByInactive");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<UserListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<UserListDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<UserDto>> UpdateAsync([FromForm] UserUpdateDto newUser)
        {
            var response = await _httpClient.PutAsJsonAsync("user", newUser);

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

        public async Task<CustomResponseDto<NoContent>> PasswordChangeAsync(UserPasswordChangeDto newPassword, int userId)
        {
            var response = await _httpClient.PutAsJsonAsync($"user/PasswordChange/{userId}", newPassword);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContent>>();

            if (responseBody.Errors.Any())
            {
                return CustomResponseDto<NoContent>.Fail(responseBody.StatusCode, responseBody.Errors);
            }
            else
            {
                return CustomResponseDto<NoContent>.Success(responseBody.StatusCode);
            }
        }

        public async Task<CustomResponseDto<UserDto>> ActivateUserAsync(int userId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<UserDto>>($"user/ActivateUser/{userId}", null);
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

        public async Task<CustomResponseDto<NoContent>> DeleteUserImageAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<NoContent>>($"user/DeleteUserImage/{userId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<NoContent>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<NoContent>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<int>> CountTotalAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("user/counttotal");

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
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("user/countbynondeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }
    }
}
