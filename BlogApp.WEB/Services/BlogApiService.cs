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

        public async Task<BlogCreateDto> AddAsync([FromForm] BlogCreateDto newBlog)
        {
            var response = await _httpClient.PostAsJsonAsync("blog", newBlog);
            
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponse<BlogCreateDto>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Ekleme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return responseBody.Data;
        }

        public async Task<bool> UpdateAsync([FromForm] BlogUpdateDto newBlog)
        {
            var response = await _httpClient.PutAsJsonAsync("blog", newBlog);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Güncelleme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int blogId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponse<NoContent>>($"blog/delete/{blogId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<BlogListDto>> GetAllByActiveAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>("blog");
            if (response.Errors.Any())
            {
                var errorMessage = string.Join(Environment.NewLine, response.Errors);
                throw new Exception(errorMessage);
            }
            return response.Data;
        }

        public async Task<List<BlogListDto>> GetAllByNonDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>("blog/GetAllByNonDeleted");

            if (response.Errors.Any())
            {
                throw new Exception($"Bloglar getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<List<BlogListDto>> GetAllByDeletedAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>("blog/GetAllByDeleted");

            if (response.Errors.Any())
            {
                throw new Exception($"Bloglar getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<List<BlogListDto>> GetAllByUserIdAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>($"blog/GetAllByDeleted/{userId}");

            if (response.Errors.Any())
            {
                throw new Exception($"Bloglar getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<List<BlogListDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>("blog/GetAll");

            if (response.Errors.Any())
            {
                throw new Exception($"Bloglar getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<bool> HardDeleteAsync(int blogId)
        {
            var response = await _httpClient.DeleteAsync($"blog/HardDelete/{blogId}");

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silme işlemi sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UndoDeleteAsync(int blogId)
        {
            var response = await _httpClient.PutAsJsonAsync<CustomResponse<NoContent>>($"blog/undodelete/{blogId}", null);

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();

            if (responseBody.Errors.Any())
                throw new Exception($"Silmeyi geri alma işlemini sırasında hata oluştu. Hata mesajları: {string.Join(',', responseBody.Errors)}");

            return response.IsSuccessStatusCode;
        }

        public async Task<BlogViewModel> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<BlogViewModel>>($"blog/Search?keyword={keyword}&currentPage={currentPage}&pageSize={pageSize}&isAscending={isAscending}");

            if (response.Errors.Any())
            {
                throw new Exception($"Bloglar anahtar kelimeye göre getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<List<BlogListDto>> GetAllByViewCountAsync(bool isAscending, int takeSize)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>($"blog/GetAllByViewCount?isAscending={isAscending}&takeSize={takeSize}");

            if (response.Errors.Any())
            {
                //_logger.LogWarning(errorMessage);
                throw new Exception($"Bloglar görüntüleme sayısına göre getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<BlogViewModel> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<BlogViewModel>>($"blog/GetAllByPaging?categoryId={categoryId}&currentPage={currentPage}&pageSize={pageSize}&isAscending={isAscending}");

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

        public async Task<int> CountTotalBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<int>>("blog/counttotalblogs");

            if (response.Errors.Any())
            {
                throw new Exception($"Toplam blog sayısı getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<int> CountActiveBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<int>>("blog/countactiveblogs");

            if (response.Errors.Any())
            {
                throw new Exception($"Aktif blog sayısı getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<int> CountInactiveBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<int>>("blog/countinactiveblogs");

            if (response.Errors.Any())
            {
                throw new Exception($"Pasif blog sayısı getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<int> CountByDeletedBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<int>>("blog/countbydeletedblogs");

            if (response.Errors.Any())
            {
                throw new Exception($"Silinmiş blog sayısı getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<int> CountByNonDeletedBlogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<int>>("blog/countbynondeletedblogs");

            if (response.Errors.Any())
            {
                throw new Exception($"Silinmemiş blog sayısı getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<string> IncreaseViewCountAsync(int blogId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<string>>($"blog/increaseviewcount/{blogId}");

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
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>($"blog/getallbycategory/{categoryId}");

            if (response.Errors.Any())
            {
                throw new Exception($"Bloglar kategoriye göre getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<List<BlogListDto>> GetAllByUserIdOnFilterAsync(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>($"blog/getallbyuseridonfilter?userId={userId}&filterBy={filterBy}&orderBy={orderBy}&isAscending={isAscending}&takeSize={takeSize}&categoryId={categoryId}&startAt={startAt}&endAt={endAt}&minViewCount={minViewCount}&maxViewCount={maxViewCount}&minCommentCount={minCommentCount}&maxCommentCount={maxCommentCount}");

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

        public async Task<BlogViewModel> GetAllFilteredAsync(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeTag, bool includeComments, bool includeUser)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<BlogViewModel>>($"blog/getallfiltered?categoryId={categoryId}&userId={userId}&isActive={isActive}&isDeleted={isDeleted}&currentPage={currentPage}&pageSize={pageSize}&orderBy={orderBy}&isAscending={isAscending}&includeCategory={includeCategory}&includeTag={includeTag}&includeComments={includeComments}&includeUser={includeUser}");

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

        public async Task<BlogListDto> GetByBlogIdAsync(int blogId)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<BlogListDto>>($"blog/getbyblogid/{blogId}");

            if (response.Errors.Any())
            {
                //_logger.LogWarning(errorMessage);
                throw new Exception($"Blog getirilirken hata oluştu. Hata mesajları: {string.Join(',', response.Errors)}");
            }
            else
            {
                return response.Data;
            }
        }

        public async Task<BlogListDto> GetFilteredByBlogIdAsync(int blogId, bool includeCategory, bool includeTag, bool includeComment, bool includeUser)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<BlogListDto>>($"blog/getfilteredbyblogid?blogId={blogId}&includeCategory={includeCategory}&includeTag={includeTag}&includeComment={includeComment}&includeUser={includeUser}");

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
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<List<BlogListDto>>>($"blog/getallbytag/{tagId}");

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
