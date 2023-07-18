using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WEB.Services
{
    public class BlogApiService
    {
        private readonly HttpClient _httpClient;

        public BlogApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<BlogCreateDto>> AddAsync(BlogCreateDto newBlog)
        {
            var response = await _httpClient.PostAsJsonAsync("blog", newBlog);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<BlogCreateDto>>();

            if (responseBody.Errors.Any())
                return CustomResponseDto<BlogCreateDto>.Fail(responseBody.StatusCode, responseBody.Errors);

            return CustomResponseDto<BlogCreateDto>.Success(responseBody.StatusCode, responseBody.Data);
        }

        public async Task<CustomResponseDto<NoContent>> UpdateAsync(BlogUpdateDto newBlog)
        {
            var response = await _httpClient.PutAsJsonAsync("blog", newBlog);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContent>>();

            if (responseBody.Errors.Any())
            {
                return CustomResponseDto<NoContent>.Fail(responseBody.StatusCode, responseBody.Errors);
            }
            else
            {
                return CustomResponseDto<NoContent>.Success(responseBody.StatusCode, responseBody.Data);
            }
        }

        public async Task<CustomResponseDto<BlogListDto>> DeleteAsync(int blogId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<BlogListDto>>($"blog/delete/{blogId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<BlogListDto>>();

            if (responseBody.Errors.Any())
            {
                return CustomResponseDto<BlogListDto>.Fail(responseBody.StatusCode, responseBody.Errors);
            }
            else
            {
                return CustomResponseDto<BlogListDto>.Success(responseBody.StatusCode, responseBody.Data);
            }
        }

        public async Task<List<BlogListDto>> GetAllByActiveAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>("blog");
            if (response.Errors.Any())
            {
                var errorMessage = string.Join(Environment.NewLine, response.Errors);
                throw new Exception(errorMessage);
            }
            return response.Data;
        }

        public async Task<List<BlogListDto>> GetAllByNonDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>("blog/GetAllByNonDeleted");

            if (response.Errors.Any())
            {
                throw new Exception($"Bloglar getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>("blog/GetAllByDeleted");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<BlogListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<BlogListDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByUserIdAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>($"blog/GetAllByUserId/{userId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<BlogListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<BlogListDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>("blog/GetAll");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<BlogListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<BlogListDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<BlogListDto>> HardDeleteAsync(int blogId)
        {
            var response = await _httpClient.DeleteAsync($"blog/{blogId}");

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<BlogListDto>>();

            if (responseBody.Errors.Any())
            {
                return CustomResponseDto<BlogListDto>.Fail(responseBody.StatusCode, responseBody.Errors);
            }
            else
            {
                return CustomResponseDto<BlogListDto>.Success(responseBody.StatusCode, responseBody.Data);
            }
        }

        public async Task<CustomResponseDto<BlogListDto>> UndoDeleteAsync(int blogId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponseDto<BlogListDto>>($"blog/undodelete/{blogId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<BlogListDto>>();

            if (responseBody.Errors.Any())
            {
                return CustomResponseDto<BlogListDto>.Fail(responseBody.StatusCode, responseBody.Errors);
            }
            else
            {
                return CustomResponseDto<BlogListDto>.Success(responseBody.StatusCode, responseBody.Data);
            }
        }

        public async Task<CustomResponseDto<BlogListResultDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<BlogListResultDto>>($"blog/Search?keyword={keyword}&currentPage={currentPage}&pageSize={pageSize}&isAscending={isAscending}");

                return CustomResponseDto<BlogListResultDto>.Success(response.StatusCode, response.Data);
            }
            catch (HttpRequestException ex)
            {

                return CustomResponseDto<BlogListResultDto>.Fail((int)ex.StatusCode, ex.Message);
            }

        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByViewCountAsync(bool isAscending, int takeSize)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>($"blog/GetAllByViewCount?isAscending={isAscending}&takeSize={takeSize}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<BlogListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<BlogListDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<BlogListResultDto> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<BlogListResultDto>>($"blog/GetAllByPaging?categoryId={categoryId}&currentPage={currentPage}&pageSize={pageSize}&isAscending={isAscending}");

            if (response.Errors.Any())
            {
                //_logger.LogWarning(errorMessage);
                throw new Exception($"Bloglar sayfalamaya göre getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CustomResponseDto<int>> CountTotalBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("blog/counttotalblogs");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<int>> CountActiveBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("blog/countactiveblogs");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<int>> CountInactiveBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("blog/countinactiveblogs");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<int>> CountByDeletedBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("blog/countbydeletedblogs");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<int>> CountByNonDeletedBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<int>>("blog/countbynondeletedblogs");

            if (response.Errors.Any())
            {
                return CustomResponseDto<int>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<int>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<string> IncreaseViewCountAsync(int blogId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<string>>($"blog/increaseviewcount/{blogId}");

            if (response.Errors.Any())
            {
                throw new Exception($"Blog görüntülenme sayısı arttırılırken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<List<BlogListDto>> GetAllByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>($"blog/getallbycategory/{categoryId}");

            if (response.Errors.Any())
            {
                throw new Exception($"Bloglar kategoriye göre getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<CustomResponseDto<List<BlogListDto>>> GetAllByUserIdOnFilterAsync(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>($"blog/GetAllByUserIdOnFilter?userId={userId}&filterBy={filterBy}&orderBy={orderBy}&isAscending={isAscending}&takeSize={takeSize}&categoryId={categoryId}&startAt={startAt.ToString("yyyy-MM-ddTHH:mm:ss")}&endAt={endAt.ToString("yyyy-MM-ddTHH:mm:ss")}&minViewCount={minViewCount}&maxViewCount={maxViewCount}&minCommentCount={minCommentCount}&maxCommentCount={maxCommentCount}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<List<BlogListDto>>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<List<BlogListDto>>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<BlogListResultDto>> GetAllFilteredAsync(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeTag, bool includeComments, bool includeUser)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<BlogListResultDto>>($"blog/getallfiltered?categoryId={categoryId}&userId={userId}&isActive={isActive}&isDeleted={isDeleted}&currentPage={currentPage}&pageSize={pageSize}&orderBy={orderBy}&isAscending={isAscending}&includeCategory={includeCategory}&includeTag={includeTag}&includeComments={includeComments}&includeUser={includeUser}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<BlogListResultDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<BlogListResultDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<CustomResponseDto<BlogListDto>> GetByBlogIdAsync(int blogId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<BlogListDto>>($"blog/getbyblogid/{blogId}");

            if (response.Errors.Any())
            {
                return CustomResponseDto<BlogListDto>.Fail(response.StatusCode, response.Errors);
            }
            else
            {
                return CustomResponseDto<BlogListDto>.Success(response.StatusCode, response.Data);
            }
        }

        public async Task<BlogListDto> GetFilteredByBlogIdAsync(int blogId, bool includeCategory, bool includeTag, bool includeComment, bool includeUser)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<BlogListDto>>($"blog/getfilteredbyblogid?blogId={blogId}&includeCategory={includeCategory}&includeTag={includeTag}&includeComment={includeComment}&includeUser={includeUser}");

            if (response.Errors.Any())
            {
                //_logger.LogWarning(errorMessage);
                throw new Exception($"Bloglar filtrelere göre getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<List<BlogListDto>> GetAllByTagAsync(int tagId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<BlogListDto>>>($"blog/getallbytag/{tagId}");

            if (response.Errors.Any())
            {
                //_logger.LogWarning(errorMessage);
                throw new Exception($"Bloglar etikete göre getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

    }
}
