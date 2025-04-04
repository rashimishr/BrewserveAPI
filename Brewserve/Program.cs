using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Mapping;
using BrewServe.Core.Services;
using BrewServe.Core.Strategies;
using Brewserve.Data.Interfaces;
using BrewServe.Data.Interfaces;
using Brewserve.Data.Repositories;
using BrewServe.Data.Repositories;
using BrewServe.Infrastructure.Middleware;
using BrewServeData.EF_Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Register UnitOfWork, mapper and strategy
var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
var mapper = mapperConfig.CreateMapper();
mapperConfig.AssertConfigurationIsValid();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<BeerByAlcoholContentStrategy>();

// Configure serilog from app settings
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

// Register for db context
builder.Services.AddDbContext<BrewServeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped<IBeerRepository, BeerRepository>();
builder.Services.AddScoped<IBarRepository, BarRepository>();
builder.Services.AddScoped<IBreweryRepository, BreweryRepository>();

// Register Services
builder.Services.AddScoped<IBeerService, BeerService>();
builder.Services.AddScoped<IBarService, BarService>();
builder.Services.AddScoped<IBreweryService, BreweryService>();
builder.Services.AddScoped<IBeerSearchStrategy, BeerByAlcoholContentStrategy>();

// Register Logging
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BrewServe API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
var app = builder.Build();
// Add Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

// Create database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BrewServeDbContext>();
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