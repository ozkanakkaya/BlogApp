using AutoMapper;
using BlogApp.API.Jwt;
using BlogApp.Business.Helpers;
using BlogApp.Business.Mapping;
using BlogApp.Business.Services;
using BlogApp.Business.Validations;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using BlogApp.Data;
using BlogApp.Data.Repositories;
using BlogApp.Data.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()));
//builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))/*.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<AppUserRegisterDtoValidator>())*/;

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;//apinin kendi filtresini bask�lad�k(true ile)
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//**Mappleme
var profiles = ProfileHelper.GetProfiles();
profiles.Add(new AppRoleProfile());
profiles.Add(new TagProfile());
profiles.Add(new BlogProfile());
var configuration = new MapperConfiguration(opt =>
{
    opt.AddProfiles(profiles);
});
var mapper = configuration.CreateMapper();
builder.Services.AddSingleton(mapper);
//**Mappleme

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});
//builder.Services.AddScoped(typeof());
//builder.Services.AddValidatorsFromAssemblyContaining<AppUserRegisterDtoValidator>();//di�er kullan�m
builder.Services.AddScoped<IValidator<AppUserRegisterDto>, AppUserRegisterDtoValidator>();
builder.Services.AddScoped<IValidator<AppUserLoginDto>, AppUserLoginDtoValidator>();
builder.Services.AddScoped<IValidator<BlogDto>, BlogDtoValidator>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IAppRoleRepository, AppRoleRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<ITagBlogRepository, TagBlogRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidAudience = TokenSettings.Audience,
        ValidIssuer = TokenSettings.Issuer,
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSettings.Key)),
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCustomException();//global hata yakalama i�in

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
