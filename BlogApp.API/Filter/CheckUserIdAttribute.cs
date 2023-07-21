using BlogApp.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BlogApp.API.Filter
{
    [AttributeUsage(AttributeTargets.Method)]

    public class CheckUserIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContent>.Fail(400, "Yorum yapabilmek için üye olun veya giriş yapın!"));
            }
            else
            {
                httpContext.Items["userId"] = userId;
            }
        }
    }
}
