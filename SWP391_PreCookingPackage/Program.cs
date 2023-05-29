using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWP391_PreCookingPackage.Models;
using SWP391_PreCookingPackage.MappingProfile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfiles());
}).CreateMapper());
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Database configuration
var serverVersion = new MySqlServerVersion(new Version(8, 0, 32));

builder.Services.AddDbContext<PrecookContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(builder.Configuration["ConnectionStrings:PrecookDatabase"], serverVersion,options => options.EnableRetryOnFailure(maxRetryCount: 5,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null))
        //.UseSnakeCaseNamingConvention()
        // The following three options help with debugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
