using System.Net;
using System.Net.Http.Json;
using System.Text;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using WebApplicationApi;
using WebApplicationApi.Data;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Booking;

namespace Tests.IntegrationTests;
public class BookingsIntegrationTests
{
    private readonly HttpClient _client;
    private CustomWebAppFactory _factory;
    private AppDbContext _dbContext;

    public BookingsIntegrationTests()
    {
        _factory = new CustomWebAppFactory();
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add(Config.ApiKeyHeader, Config.ApiKey);

        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _dbContext.Bookings.RemoveRange(_dbContext.Bookings);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Verify_GetBookings_Success()
    {
        // Arrange
        var newEntities = new List<BookingModel>()
        {
            new()
            {
                Id = 1,
                ProductId = 1,
                Date = DateTime.Today,
                Quantity = 2
            },
            new()
            {
                Id = 2,
                ProductId = 2,
                Date = DateTime.Today.AddDays(1),
                Quantity = 3
            }
        };

        _dbContext.Bookings.AddRange(newEntities);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("api/Bookings");
        var products = await response.Content.ReadFromJsonAsync<List<BookingModel>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        products.Should().BeEquivalentTo(newEntities);
    }

    [Fact]
    public async Task Verify_GetBooking_By_Id_Success()
    {
        // Arrange
        var entity = new BookingModel()
        {
            Id = 1,
            ProductId = 1,
            Date = DateTime.Today,
            Quantity = 2
        };

        _dbContext.Bookings.Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/Bookings/{entity.Id}");
        var product = await response.Content.ReadFromJsonAsync<BookingModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        product.Should().BeEquivalentTo(entity);
    }


    [Fact]
    public async Task Verify_Booking_Is_Created()
    {
        // Arrange
        var newBookingDto = new BookingDto()
        {
            Product = new ProductModel()
            {
                Id = 1,
                Name = "Product1"
            },
            Description = "Booking1",
            User = new UserModel()
            {
                Id = 1,
            },
            Status = BookingStatus.Approved,
            Quantity = 1,
        };

        var content = new StringContent(JsonConvert.SerializeObject(newBookingDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/Bookings", content);
        var createdUser = await response.Content.ReadFromJsonAsync<BookingModel>();
        var dbEntity = await _dbContext.Bookings.FindAsync(createdUser.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        dbEntity.Should().BeEquivalentTo(createdUser);
    }

    [Fact]
    public async Task Verify_Booking_Updated_Successfully()
    {
        // Arrange
        var updateDto = new BookingDto()
        {
            Product = new ProductModel()
            {
                Id = 2,
                Name = "ProductUpdated"
            },
            Description = "Booking1",
            User = new UserModel()
            {
                Id = 1,
            },
            Status = BookingStatus.Approved,
            Quantity = 1,
        };

        var entity = new BookingModel()
        {
            Id = 1,
            ProductId = 1,
            Date = DateTime.Today,
            Quantity = 2
        };

        _dbContext.Bookings.Add(entity);
        await _dbContext.SaveChangesAsync();

        var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"api/Bookings/{entity.Id}", content);
        var user = await response.Content.ReadFromJsonAsync<BookingModel>();
        var updatedEntity = await _dbContext.Bookings.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedEntity.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task Verify_Booking_Is_Deleted_Successfully()
    {
        // Arrange
        var entity = new BookingModel()
        {
            Id = 1,
            ProductId = 1,
            Date = DateTime.Today,
            Quantity = 2
        };

        _dbContext.Bookings.Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"api/Bookings/{entity.Id}");
        var deletedEntity = await _dbContext.Bookings.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        deletedEntity.Should().BeNull();
    }
}
