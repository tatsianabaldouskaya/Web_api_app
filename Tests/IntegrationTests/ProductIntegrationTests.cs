using System.Net;
using System.Net.Http.Json;
using System.Text;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using WebApplicationApi;
using WebApplicationApi.Data;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Product;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Tests.IntegrationTests;

[Collection("Database")]
public class ProductIntegrationTests
{
    private readonly HttpClient _client;
    private CustomWebAppFactory _factory;
    private AppDbContext _dbContext;

    public ProductIntegrationTests()
    {
        _factory = new CustomWebAppFactory();
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add(Config.ApiKeyHeader, Config.ApiKey);

        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _dbContext.Products.RemoveRange(_dbContext.Products);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Verify_GetProducts_Success()
    {
        // Arrange
        var newProducts = new List<ProductModel>()
        {
            new()
            {
                Id = 1,
                Name = "Test Pt5"
            },
            new()
            {
                Id = 2,
                Name = "Test Pt7"
            }
        };

        _dbContext.Products.AddRange(newProducts);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("api/Products");
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        products.Should().BeEquivalentTo(newProducts);
    }

    [Fact]
    public async Task Verify_GetProduct_By_Id_Success()
    {
        // Arrange
        var newProduct = new ProductModel
        {
            Id = 1,
            Name = "TestProduct1",
            Description = "Test Description1",
            Author = "Test Author1",
            Price = 10.0f
        };

        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/Products/{newProduct.Id}");
        var product = await response.Content.ReadFromJsonAsync<ProductModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        product.Should().BeEquivalentTo(newProduct);
    }


    [Fact]
    public async Task Verify_Product_Is_Created()
    {
        // Arrange
        var newProductDto = new ProductDto()
        {
            Name = "NewProduct",
            Description = "Description",
            Author = "Author",
            Price = 50.0f,
            ImagePath = "image.jpg"
        };

        var content = new StringContent(JsonConvert.SerializeObject(newProductDto), Encoding.UTF8, "application/json");

            // Act
        var response = await _client.PostAsync($"api/Products", content);
        var createdProduct = await response.Content.ReadFromJsonAsync<ProductModel>();
        var expectedCreatedProduct = await _dbContext.Products.FindAsync(createdProduct.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        expectedCreatedProduct.Should().BeEquivalentTo(createdProduct);
    }

    [Fact]
    public async Task Verify_Product_Updated_Successfully()
    {
        // Arrange
        var updateProductDto = new ProductDto()
        {
            Name = "UpdateProduct",
            Description = "Updated Description",
            Author = "Updated Author",
            Price = 100.0f,
            ImagePath = "updateImage.jpg"
        };

        var newProduct = new ProductModel
        {
            Id = 1,
            Name = "TestProduct1",
            Description = "Test Description1",
            Author = "Test Author1",
            Price = 10.0f
        };

        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        var content = new StringContent(JsonConvert.SerializeObject(updateProductDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"api/Products/{newProduct.Id}", content);
        var product = await response.Content.ReadFromJsonAsync<ProductModel>();
        var expectedProduct = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == newProduct.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        expectedProduct.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task Verify_Product_Is_Deleted_Successfully()
    {
        // Arrange
        var newProduct = new ProductModel
        {
            Id = 1,
            Name = "TestProduct1",
            Description = "Test Description1",
            Author = "Test Author1",
            Price = 10.0f
        };

        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"api/Products/{newProduct.Id}");
        var deletedProduct = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == newProduct.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        deletedProduct.Should().BeNull();
    }
}
