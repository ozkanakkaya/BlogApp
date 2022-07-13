using AutoMapper;
using BlogApp.API.Middlewares;
using BlogApp.Business.Helpers;
using BlogApp.Business.Services;
using BlogApp.Business.Validations;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using BlogApp.Data;
using BlogApp.Data.Repositories;
using BlogApp.Data.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()));
//builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))/*.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<AppUserRegisterDtoValidator>())*/;

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;//apinin kendi filtresini baskýladýk(true ile)
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//**Mappleme
var profiles = ProfileHelper.GetProfiles();
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
builder.Services.AddScoped<IValidator<AppUserRegisterDto>, AppUserRegisterDtoValidator>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCustomException();//global hata yakalama için

app.UseAuthorization();

app.MapControllers();

app.Run();
