using AutoMapper;
using BlogApp.Business.Helpers;
using BlogApp.Business.Jwt;
using BlogApp.Business.Mapping;
using BlogApp.Business.Services;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Services;
using BlogApp.Core.Utilities.Abstract;
using BlogApp.Core.Utilities.Extensions;
using BlogApp.WEB.Configurations;
using BlogApp.WEB.Mapping;
using BlogApp.WEB.Middlewares;
using BlogApp.WEB.Services;
using BlogApp.WEB.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FileAccess = BlogApp.Business.Helpers.FileAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNToastNotifyToastr();

builder.Services.AddHttpClient<BlogApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

builder.Services.AddHttpClient<CategoryApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

builder.Services.AddHttpClient<CommentApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

builder.Services.AddHttpClient<UserApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

builder.Services.AddHttpClient<AuthApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

builder.Services.AddHttpClient<RoleApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

builder.Services.AddHttpClient<TagApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

//**Mappleme
var profiles = ProfileHelper.GetProfiles();
profiles.Add(new UserProfile());
profiles.Add(new BlogPostProfile());
var configuration = new MapperConfiguration(opt =>
{
    opt.AddProfiles(profiles);
});
var mapper = configuration.CreateMapper();
builder.Services.AddSingleton(mapper);
//**Mappleme

builder.Services.Configure<BlogRightSideBarWidgetOptions>(builder.Configuration.GetSection("BlogRightSideBarWidgetOptions"));
builder.Services.Configure<AboutUsPageInfo>(builder.Configuration.GetSection("AboutUsPageInfo"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<WebsiteInfo>(builder.Configuration.GetSection("WebsiteInfo"));
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.ConfigureWritable<BlogRightSideBarWidgetOptions>(builder.Configuration.GetSection("BlogRightSideBarWidgetOptions"));
builder.Services.ConfigureWritable<AboutUsPageInfo>(builder.Configuration.GetSection("AboutUsPageInfo"));
builder.Services.ConfigureWritable<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.ConfigureWritable<WebsiteInfo>(builder.Configuration.GetSection("WebsiteInfo"));

builder.Services.AddScoped<IValidator<EmailSendDto>, EmailSendDtoValidator>();

builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<TokenGenerator>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.AddScoped<IFileAccess, FileAccess>();

builder.Services.AddTransient<CustomAuthorizationHandler>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Auth/LogIn";
    opt.LogoutPath = "/Auth/Logout";
    opt.AccessDeniedPath = "/Auth/AccessDenied";
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.Cookie.Name = "JwtCookie";
});


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

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
