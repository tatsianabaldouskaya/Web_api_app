using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplicationApi;
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
    var apiKeyScheme = new OpenApiSecurityScheme
    {
        Description = "The Api key to access the API",
        Type = SecuritySchemeType.ApiKey,
        Name = Config.ApiKeyHeader,
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        }
    };

    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "'Bearer' [token]",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition(apiKeyScheme.Reference.Id, apiKeyScheme);
    c.AddSecurityDefinition(bearerScheme.Reference.Id, bearerScheme);

    var securityRequirements = new OpenApiSecurityRequirement
    {
        { apiKeyScheme, new List<string>() },
        { bearerScheme, new List<string>() }
    };
    c.AddSecurityRequirement(securityRequirements);
});

// Configure Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookshopDatabase")));

builder.Services.AddScoped<IRepository<BookingModel>, BookingRepository>();
builder.Services.AddScoped<IRepository<ProductModel>, ProductRepository>();
builder.Services.AddScoped<IRepository<StoreItemModel>, StoreItemRepository>();
builder.Services.AddScoped<IRepository<UserModel>, UserRepository>();

builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "BookShop",
            ValidAudience = "BookShopUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.SecurityKey))
        };
    });

builder.Services.AddAuthorization();

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
