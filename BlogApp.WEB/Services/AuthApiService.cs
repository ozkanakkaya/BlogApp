using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BlogApp.WEB.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<TokenResponse>> LogIn(UserLoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/Login", loginDto);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TokenResponse>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<TokenResponse>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<TokenResponse>.Success(responseBody.StatusCode, responseBody.Data);

        }
    }
}
