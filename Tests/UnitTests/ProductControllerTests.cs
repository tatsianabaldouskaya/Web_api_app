using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using WebApplicationApi.Controllers;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Product;
using WebApplicationApi.Repositories.Interfaces;

namespace Tests.UnitTests;

public class ProductControllerTests
{
    private readonly Mock<IRepository<ProductModel>> _mockRepository;
    private readonly ProductsController _controller;

    public ProductControllerTests()
    {
        // Initialize the mocked repository
        _mockRepository = new Mock<IRepository<ProductModel>>();
        _controller = new ProductsController(_mockRepository.Object);
    }

    [Fact]
    public async Task Verify_GetProducts_Returns_All_Products()
    {
        // Arrange
        var products = new List<ProductModel>
        {
            new() { Id = 1, Name = "TestProduct1", Price = 10.0f },
            new() { Id = 2, Name = "TestProduct2", Price = 20.0f }
        };

        _mockRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = result.Result as OkObjectResult;
        var returnedProducts = okResult.Value as List<ProductModel>;

        returnedProducts.Should().HaveCount(2);
    }

    [Fact]
    public async Task Verify_GetProduct_With_ValidId_Returns_Product()
    {
        // Arrange
        var product = new ProductModel
        {
            Id = 1,
            Name = "TestProduct1",
            Description = "Test Description1",
            Author = "Test Author1",
            Price = 10.0f
        };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedProduct = okResult.Value.Should().BeOfType<ProductModel>().Subject;

        returnedProduct.Should().NotBeNull();
        returnedProduct.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetProduct_With_InvalidId_Returns_NotFound()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync((ProductModel)null);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Verify_Product_Added_Successfully()
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

        var newProduct = new ProductModel
        {
            Id = 1,
            Name = newProductDto.Name,
            Description = newProductDto.Description,
            Author = newProductDto.Author,
            Price = newProductDto.Price,
            ImagePath = newProductDto.ImagePath
        };

        _mockRepository
            .Setup(repo => repo.AddAsync(It.IsAny<ProductModel>()))
            .ReturnsAsync(newProduct);

        // Act
        var result = await _controller.CreateProduct(newProductDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var createdProduct = createdResult.Value.Should().BeOfType<ProductModel>().Subject;
        createdProduct.Should().BeEquivalentTo(newProduct);
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

        var updatedProduct = new ProductModel
        {
            Id = 1,
            Name = updateProductDto.Name,
            Description = updateProductDto.Description,
            Author = updateProductDto.Author,
            Price = updateProductDto.Price,
            ImagePath = updateProductDto.ImagePath
        };

        _mockRepository
            .Setup(repo => repo.UpdateAsync(1, It.IsAny<ProductModel>()))
            .ReturnsAsync(updatedProduct);

        // Act
        var result = await _controller.UpdateProduct(1, updateProductDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedUpdatedProduct = okResult.Value.Should().BeOfType<ProductModel>().Subject;

        returnedUpdatedProduct.Should().BeEquivalentTo(updatedProduct);
    }

    [Fact]
    public async Task Verify_UpdateProduct_With_Invalid_Id_Returns_NotFound()
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
        
        _mockRepository
            .Setup(repo => repo.UpdateAsync(1, It.IsAny<ProductModel>()))
            .ReturnsAsync((ProductModel)null);

        // Act
        var result = await _controller.UpdateProduct(1, updateProductDto);

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
        var result = await _controller.DeleteProduct(1);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
