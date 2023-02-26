using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        //public CustomBaseController(IImageHelper imageHelper)
        //{
        //    ImageHelper = imageHelper;
        //}

        //protected IImageHelper ImageHelper { get; }

        public IActionResult CreateActionResult<T>(CustomResponse<T> response)
        {
            if (response.StatusCode == 204)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
