using AutoMapper;
using BlogApp.API.Filter;
using BlogApp.Business.Helpers;
using BlogApp.Business.Jwt;
using BlogApp.Business.Mapping;
using BlogApp.Business.Services;
using BlogApp.Business.Validations;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using BlogApp.Core.Utilities.Abstract;
using BlogApp.Data;
using BlogApp.Data.Repositories;
using BlogApp.Data.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Reflection;
using System.Text;
using FileAccess = BlogApp.Business.Helpers.FileAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()));
//builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))/*.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<AppUserRegisterDtoValidator>())*/;

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;//apinin kendi filtresini baskýladýk(true ile)
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//**Mappleme
var profiles = ProfileHelper.GetProfiles();
profiles.Add(new BlogProfile());
profiles.Add(new RoleProfile());
profiles.Add(new TagProfile());
profiles.Add(new CommentProfile());
profiles.Add(new CategoryProfile());
profiles.Add(new UserProfile());

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
//builder.Services.AddValidatorsFromAssemblyContaining<AppUserRegisterDtoValidator>();//diðer kullaným
builder.Services.AddScoped<IValidator<UserRegisterDto>, UserRegisterDtoValidator>();
builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
builder.Services.AddScoped<IValidator<UserUpdateDto>, UserUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<UserPasswordChangeDto>, UserPasswordChangeDtoValidator>();
builder.Services.AddScoped<IValidator<BlogCreateDto>, BlogCreateDtoValidator>();
builder.Services.AddScoped<IValidator<BlogUpdateDto>, BlogUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<CategoryCreateDto>, CategoryCreateDtoValidator>();
builder.Services.AddScoped<IValidator<CategoryUpdateDto>, CategoryUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<CommentCreateDto>, CommentCreateDtoValidator>();
builder.Services.AddScoped<IValidator<CommentUpdateDto>, CommentUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<TagUpdateDto>, TagUpdateDtoValidator>();

builder.Services.AddScoped<CheckUserIdAttribute>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogTagRepository, BlogTagRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.AddScoped<IFileAccess, FileAccess>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<TokenGenerator>();

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

var tokenSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidAudience = tokenSettings.Audience,
        ValidIssuer = tokenSettings.Issuer,
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Key)),
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

//app.UseCustomException();//global hata yakalama için

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
