using System.Reflection;
using System.Runtime.Intrinsics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using Brewserve.Core.Factories;
using Brewserve.Core.Interfaces;
using Brewserve.Data.EF_Core;
using Brewserve.Core.Services;
using Brewserve.Core.Strategies;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Repositories;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Brewserve.Core.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Register UnitOfWork, mapper and strategy
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
mapperConfig.AssertConfigurationIsValid();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddAutoMapper((typeof(Program)));
builder.Services.AddTransient<BeerByAlcoholContentStrategy>();

//configure serilog from app settings
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

//configuration for db
builder.Services.AddScoped<BeerSearchContext>();
builder.Services.AddDbContext<BrewserveDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

// Register Repositories
builder.Services.AddScoped<IBeerRepository, BeerRepository>();
builder.Services.AddScoped<IBarRepository, BarRepository>();
builder.Services.AddScoped<IBreweryRepository, BreweryRepository>();

// Register Services
builder.Services.AddScoped<IBeerService, BeerService>();
builder.Services.AddScoped<IBarService, BarService>();
builder.Services.AddScoped<IBreweryService, BreweryService>();



// Add services to the container.
builder.Services.AddScoped<IBeerSearchStrategy, BeerByAlcoholContentStrategy>();
builder.Services.AddScoped<BeerSearchContext>();
builder.Services.AddScoped<IBeerFactory, BeerFactory>();
builder.Services.AddScoped<IBarFactory, BarFactory>();
builder.Services.AddScoped<IBreweryFactory, Breweryfactory>();

//dd logging
builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=> 
    {
        c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Brewserve API", Version = "v1"});
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

var app = builder.Build();
// Add Middleware
app.UseMiddleware<Brewserve.API.Middleware.ExceptionHandlingMiddleware>();
app.UseMiddleware<Brewserve.API.Middleware.LoggingMiddleware>();

//automatically create database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BrewserveDbContext>();
    dbContext.Database.EnsureCreated();
}


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
