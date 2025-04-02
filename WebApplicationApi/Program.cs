using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using WebApplicationApi.Authentication;
using WebApplicationApi.Data;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Repositories;
using WebApplicationApi.Repositories.Interfaces;

[assembly: InternalsVisibleTo("Tests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "The Api key to access the API",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement()
    {
        { scheme, new List<string>() }
    };
    c.AddSecurityRequirement(requirement);
});

// Configure Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookshopDatabase")));


builder.Services.AddScoped<IRepository<BookingModel>, BookingRepository>();
builder.Services.AddScoped<IRepository<ProductModel>, ProductRepository>();
builder.Services.AddScoped<IRepository<StoreItemModel>, StoreItemRepository>();
builder.Services.AddScoped<IRepository<UserModel>, UserRepository>();

builder.Services.AddScoped<ApiKeyAuthFilter>();


var app = builder.Build();

// Ensure database is created automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var isDbRelational = dbContext.Database.IsRelational();
    if (isDbRelational)
    {
        dbContext.Database.Migrate();
    }
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
