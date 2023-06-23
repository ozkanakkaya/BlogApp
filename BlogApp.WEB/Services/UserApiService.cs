﻿using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WEB.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<UserRegisterDto>> RegisterAsync(UserRegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("user", registerDto);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<UserRegisterDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<UserRegisterDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<UserRegisterDto>.Success(responseBody.StatusCode, responseBody.Data);
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

        public async Task<CustomResponseDto<NoContent>> DeleteAsync(int userId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<NoContent>>($"user/delete/{userId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContent>>();

            if (responseBody.Errors.Any())
            {
                return CustomResponseDto<NoContent>.Fail(responseBody.StatusCode, responseBody.Errors.FirstOrDefault());
            }
            else
            {
                return CustomResponseDto<NoContent>.Success(responseBody.StatusCode);
            }
        }

        public async Task<CustomResponseDto<NoContent>> UndoDeleteAsync(int userId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<NoContent>>($"user/undodelete/{userId}", null);

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

        public async Task<CustomResponseDto<NoContent>> HardDeleteAsync(int userId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<NoContent>>($"user/harddelete/{userId}", null);

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

        public async Task<CustomResponseDto<List<UserListDto>>> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserListDto>>>($"user/GetAllByDeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<UserListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<UserListDto>>.Success(response.StatusCode, response.Data);
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

        public async Task<CustomResponseDto<NoContent>> UpdateAsync([FromForm] UserUpdateDto newUser)
        {
            var response = await _httpClient.PutAsJsonAsync("user", newUser);

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

        public async Task<CustomResponseDto<NoContent>> PasswordChangeAsync([FromForm] UserPasswordChangeDto newPassword)
        {
            var response = await _httpClient.PutAsJsonAsync("user/PasswordChange", newPassword);

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

        public async Task<CustomResponseDto<NoContent>> ActivateUserAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<NoContent>>($"user/ActivateUser/{userId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<NoContent>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<NoContent>.Success(response.StatusCode, response.Data);
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
