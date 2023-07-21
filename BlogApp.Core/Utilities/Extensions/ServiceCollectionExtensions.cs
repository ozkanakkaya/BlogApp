using BlogApp.Core.Utilities.Abstract;
using BlogApp.Core.Utilities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace BlogApp.Core.Utilities.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<T>(this IServiceCollection services, IConfiguration configuration, string file = "appsettings.json") where T : class, new()
        {
            // services.Configure<T>(configuration);
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var fileProvider = provider.GetService<IFileProvider>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new WritableOptions<T>(fileProvider, options, typeof(T).FullName.Split(".").Last(), file);
            });
        }
    }
}
