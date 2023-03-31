using BlogApp.Business.Services;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Services;
using BlogApp.WEB.Configurations;
using BlogApp.WEB.Services;
using BlogApp.WEB.Validations;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNToastNotifyToastr();

builder.Services.AddHttpClient<BlogApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

builder.Services.AddHttpClient<CategoryApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

builder.Services.AddHttpClient<CommentApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

builder.Services.Configure<BlogRightSideBarWidgetOptions>(builder.Configuration.GetSection("BlogRightSideBarWidgetOptions"));
builder.Services.Configure<AboutUsPageInfo>(builder.Configuration.GetSection("AboutUsPageInfo"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<IValidator<EmailSendDto>, EmailSendDtoValidator>();

builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseNToastNotify();

//var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();

////API'ye yapýlan her istekte, cookie içindeki token'a eriþmek için bu middleware kullanýldý.
//app.Use(async (context, next) =>
//{
//	var accessToken = httpContextAccessor.HttpContext.Request.Cookies["access_token"];
//	if (!string.IsNullOrEmpty(accessToken))
//	{
//		context.Request.Headers.Add("Authorization", "Bearer" + accessToken);
//	}
//	await next();
//});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
