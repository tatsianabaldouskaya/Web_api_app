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
using WebApplicationApi.Models.Dtos.User;

namespace Tests.IntegrationTests;
public class UserIntegrationTests
{
    private readonly HttpClient _client;
    private CustomWebAppFactory _factory;
    private AppDbContext _dbContext;

    public UserIntegrationTests()
    {
        _factory = new CustomWebAppFactory();
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add(Config.ApiKeyHeader, Config.ApiKey);

        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _dbContext.Users.RemoveRange(_dbContext.Users);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Verify_GetUsers_Success()
    {
        // Arrange
        var newUsers = new List<UserModel>()
        {
            new()
            {
                Id = 1,
                Name = "TestProduct1",
                Login = "Test Login",
            },
            new()
            {
                Id = 2,
                Name = "TestProduct2",
                Login = "Test Login",
            }
        };

        _dbContext.Users.AddRange(newUsers);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("api/Users");
        var products = await response.Content.ReadFromJsonAsync<List<UserModel>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        products.Should().BeEquivalentTo(newUsers);
    }

    [Fact]
    public async Task Verify_GetUser_By_Id_Success()
    {
        // Arrange
        var newUser = new UserModel()
        {
            Id = 1,
            Name = "TestProduct1",
            Login = "Test Login",
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"api/Users/{newUser.Id}");
        var product = await response.Content.ReadFromJsonAsync<UserModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        product.Should().BeEquivalentTo(newUser);
    }


    [Fact]
    public async Task Verify_User_Is_Created()
    {
        // Arrange
        var newUserDto = new UserDto()
        {
            Name = "NewProduct",
            Role = Role.Admin,
            Email = "emailTest",
            Phone = "12345"
        };

        var content = new StringContent(JsonConvert.SerializeObject(newUserDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/Users", content);
        var createdUser = await response.Content.ReadFromJsonAsync<UserModel>();
        var dbEntity = await _dbContext.Users.FindAsync(createdUser.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        dbEntity.Should().BeEquivalentTo(createdUser);
    }

    [Fact]
    public async Task Verify_User_Updated_Successfully()
    {
        // Arrange
        var updateUserDto = new UserDto()
        {
            Name = "New User",
            Role = Role.Admin,
            Email = "emailTest",
            Phone = "12345"
        };

        var entity = new UserModel
        {
            Id = 1,
            Name = "TestUserUpdated",
            Login = "Test Login Updated",
            RoleId = (int)Role.Customer,
            Phone = "new phone"
        };

        _dbContext.Users.Add(entity);
        await _dbContext.SaveChangesAsync();

        var content = new StringContent(JsonConvert.SerializeObject(updateUserDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"api/Users/{entity.Id}", content);
        var user = await response.Content.ReadFromJsonAsync<UserModel>();
        var updatedEntity = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedEntity.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task Verify_Product_Is_Deleted_Successfully()
    {
        // Arrange
        var entity = new UserModel
        {
            Id = 1,
            Name = "TestUser",
            Login = "Test Login",
            RoleId = (int)Role.Customer,
            Phone = "new phone"
        };

        _dbContext.Users.Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"api/Users/{entity.Id}");
        var deletedEntity = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        deletedEntity.Should().BeNull();
    }
}
