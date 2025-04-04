using System.Net;
using System.Net.Http.Json;
using System.Text;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using WebApplicationApi;
using WebApplicationApi.Authorization;
using WebApplicationApi.Data;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.StoreItem;

namespace Tests.IntegrationTests;
public class StoreItemsIntegrationTests
{
    private readonly HttpClient _client;
    private CustomWebAppFactory _factory;
    private AppDbContext _dbContext;

    public StoreItemsIntegrationTests()
    {
        _factory = new CustomWebAppFactory();
        _client = _factory.CreateClient();
        var token = new TokenGenerator().GenerateSuperUserJwtToken();
        _client.DefaultRequestHeaders.Add(Config.ApiKeyHeader, Config.ApiKey);
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _dbContext.BookStore.RemoveRange(_dbContext.BookStore);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Verify_GetStoreItems_Success()
    {
        // Arrange
        var newEntities = new List<StoreItemModel>()
        {
            new()
            {
                Id = 1,
                ProductId = 1,
                AvailableQuantity = 5,
                BookedQuantity = 3,
                SoldQuantity = 4
            },
            new()
            {
                Id = 2,
                ProductId = 2,
                AvailableQuantity = 5,
                BookedQuantity = 3,
                SoldQuantity = 4
            },
        };

        _dbContext.BookStore.AddRange(newEntities);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("api/StoreItems");
        var items = await response.Content.ReadFromJsonAsync<List<StoreItemModel>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        items.Should().BeEquivalentTo(newEntities);
    }

    [Fact]
    public async Task Verify_GetStoreItem_By_Id_Success()
    {
        // Arrange
        var newEntity = new StoreItemModel
        {
            Id = 1,
            ProductId = 1,
            AvailableQuantity = 5,
            BookedQuantity = 3,
            SoldQuantity = 4
        };

        _dbContext.BookStore.Add(newEntity);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/StoreItems/{newEntity.Id}");
        var storeItem = await response.Content.ReadFromJsonAsync<StoreItemModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        storeItem.Should().BeEquivalentTo(newEntity);
    }


    [Fact]
    public async Task Verify_Item_Is_Created()
    {
        // Arrange
        var newItemDto = new StoreItemDto()
        {
            Product = new ProductModel()
            {
                Id = 1
            },
            AvailableQuantity = 2,
            BookedQuantity = 2,
            SoldQuantity = 2
        };

        var content = new StringContent(JsonConvert.SerializeObject(newItemDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/StoreItems", content);
        var createdItem = await response.Content.ReadFromJsonAsync<StoreItemModel>();
        var dbEntity = await _dbContext.BookStore.FindAsync(createdItem.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        dbEntity.Should().BeEquivalentTo(createdItem);
    }

    [Fact]
    public async Task Verify_StoreItem_Updated_Successfully()
    {
        // Arrange
        var updateItemDto = new StoreItemDto()
        {
            Product = new ProductModel()
            {
                Id = 2
            },
            AvailableQuantity = 3,
            BookedQuantity = 4,
            SoldQuantity = 5
        };

        var entity = new StoreItemModel
        {
            Id = 1,
            ProductId = 1,
            AvailableQuantity = 5,
            BookedQuantity = 3,
            SoldQuantity = 4
        };

        _dbContext.BookStore.Add(entity);
        await _dbContext.SaveChangesAsync();

        var content = new StringContent(JsonConvert.SerializeObject(updateItemDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"api/StoreItems/{entity.Id}", content);
        var item = await response.Content.ReadFromJsonAsync<StoreItemModel>();
        var updatedEntity = await _dbContext.BookStore.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedEntity.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task Verify_StoreItem_Is_Deleted_Successfully()
    {
        // Arrange
        var entity = new StoreItemModel
        {
            Id = 1,
            ProductId = 1,
            AvailableQuantity = 5,
            BookedQuantity = 3,
            SoldQuantity = 4
        };

        _dbContext.BookStore.Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"api/StoreItems/{entity.Id}");
        var deletedEntity = await _dbContext.BookStore.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        deletedEntity.Should().BeNull();
    }
}
