using BlogApp.Business.Exeptions;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace BlogApp.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        /// <summary>
        /// UseExceptionHandler() middleware i uygulamadaki hataları yakalar ve geriye bir model döner. 
        /// Biz bu middeleware in içine girerek ve sonlandırıcı rolündeki Run() middleware sinin de içine girerek hata yakaladığında kendi yazdığımız modelimizi döndürdük(CustomResponseDto isimli genel geri dönüş modelimiz).
        ///IExceptionHandlerFeature: uygulamada fırlatılan hatayı yakalar.
        ///switch: içine girer.
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500 //diğer
                    };

                    context.Response.StatusCode = statusCode;

                    var response = CustomResponseDto<NoContent>.Fail(statusCode, exceptionFeature.Error.Message);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });
            });
        }
    }
}
