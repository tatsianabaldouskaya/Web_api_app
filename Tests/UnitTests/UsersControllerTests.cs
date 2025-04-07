using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using WebApplicationApi.Controllers;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.User;
using WebApplicationApi.Repositories.Interfaces;

namespace Tests.UnitTests;
public class UsersControllerTests
{
    private readonly Mock<IRepository<UserModel>> _mockRepository;
    private readonly UsersController _controller;
    public UsersControllerTests()
    {
        _mockRepository = new Mock<IRepository<UserModel>>();
        _controller = new UsersController(_mockRepository.Object);
    }

    [Fact]
    public async Task Verify_GetUsers_Returns_All_Users()
    {
        // Arrange
        var users = new List<UserModel>
        {
            new() { Id = 1, Name = "TestUser1" },
            new() { Id = 2, Name = "TestUser2" }
        };

        _mockRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _controller.GetUsers();

        // Assert
        var okResult = result.Result as OkObjectResult;
        var returnedProducts = okResult.Value as List<UserModel>;

        returnedProducts.Should().HaveCount(2);
    }

    [Fact]
    public async Task Verify_GetUser_With_ValidId_Returns_User()
    {
        // Arrange
        var user = new UserModel()
        {
            Id = 1,
            Name = "TestProduct1",
            Login = "Test Login",
        };
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.GetUser(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedProduct = okResult.Value.Should().BeOfType<UserModel>().Subject;

        returnedProduct.Should().NotBeNull();
        returnedProduct.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUser_With_InvalidId_Returns_NotFound()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync((UserModel)null);

        // Act
        var result = await _controller.GetUser(1);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Verify_User_Added_Successfully()
    {
        // Arrange
        var newUserDto = new UserDto()
        {
            Name = "NewProduct",
            Role = Role.Admin,
            Email = "emailTest",
            Phone = "12345"
        };

        var newProduct = new UserModel
        {
            Id = 1,
            Name = newUserDto.Name,
            RoleId = (int)newUserDto.Role,
            Email= newUserDto.Email,
        };
        _mockRepository
            .Setup(repo => repo.AddAsync(It.IsAny<UserModel>()))
            .ReturnsAsync(newProduct);

        // Act
        var result = await _controller.CreateUser(newUserDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var createdProduct = createdResult.Value.Should().BeOfType<UserModel>().Subject;
        createdProduct.Should().BeEquivalentTo(newProduct);
    }

    [Fact]
    public async Task Verify_User_Updated_Successfully()
    {
        // Arrange
        var updateDto = new UserDto()
        {
            Name = "UpdateUser",
            Role = Role.Admin,
            Email = "newEmail"
        };

        var updatedModel = new UserModel()
        {
            Id = 1,
            Name = updateDto.Name,
            RoleId = (int)updateDto.Role,
            Email = updateDto.Email,
        };
        _mockRepository
            .Setup(repo => repo.UpdateAsync(1, It.IsAny<UserModel>()))
            .ReturnsAsync(updatedModel);

        // Act
        var result = await _controller.UpdateUser(1, updateDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedUpdatedProduct = okResult.Value.Should().BeOfType<UserModel>().Subject;

        returnedUpdatedProduct.Should().BeEquivalentTo(updatedModel);
    }

    [Fact]
    public async Task Verify_UpdateUser_With_Invalid_Id_Returns_NotFound()
    {
        // Arrange
        var updateDto = new UserDto()
        {
            Name = "UpdateUser",
            Role = Role.Admin,
            Email = "newEmail"
        };

        _mockRepository
            .Setup(repo => repo.UpdateAsync(1, It.IsAny<UserModel>()))
            .ReturnsAsync((UserModel)null);

        // Act
        var result = await _controller.UpdateUser(1, updateDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Verify_Product_Is_Deleted_Successfully()
    {
        _mockRepository
            .Setup(repo => repo.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
