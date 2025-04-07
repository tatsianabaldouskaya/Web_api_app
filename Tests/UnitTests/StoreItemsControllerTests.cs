using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplicationApi.Controllers;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.StoreItem;
using WebApplicationApi.Repositories.Interfaces;

namespace Tests.UnitTests;
public class StoreItemsControllerTests
{
    private readonly Mock<IRepository<StoreItemModel>> _mockRepository;
    private readonly StoreItemsController _controller;

    public StoreItemsControllerTests()
    {
        _mockRepository = new Mock<IRepository<StoreItemModel>>();
        _controller = new StoreItemsController(_mockRepository.Object);
    }

    [Fact]
    public async Task Verify_GetStoreItems_Returns_All_Items()
    {
        // Arrange
        var items = new List<StoreItemModel>()
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
        _mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(items);

        // Act
        var result = await _controller.GetStoreItems();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedItems = okResult.Value.Should().BeOfType<List<StoreItemModel>>().Subject;

        returnedItems.Should().HaveCount(2);
    }

    [Fact]
    public async Task Verify_GetStoreItem_With_ValidId_Returns_Item()
    {
        // Arrange
        var item = new StoreItemModel()
        {
            Id = 1,
            ProductId = 1,
            AvailableQuantity = 5,
            BookedQuantity = 3,
            SoldQuantity = 4
        };
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(item);

        // Act
        var result = await _controller.GetStoreItem(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedItem = okResult.Value.Should().BeOfType<StoreItemModel>().Subject;

        returnedItem.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task Verify_GetStoreItem_With_InvalidId_Returns_NotFound()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((StoreItemModel)null);

        // Act
        var result = await _controller.GetStoreItem(1);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Verify_StoreItem_Added_Successfully()
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

        var newItem = new StoreItemModel()
        {
            Id = 1,
            ProductId = newItemDto.Product.Id,
            AvailableQuantity = newItemDto.AvailableQuantity,
            BookedQuantity = newItemDto.BookedQuantity,
            SoldQuantity = newItemDto.SoldQuantity
        };

        _mockRepository
            .Setup(repo => repo.AddAsync(It.IsAny<StoreItemModel>()))
            .ReturnsAsync(newItem);

        // Act
        var result = await _controller.CreateStoreItem(newItemDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var createdItem = createdResult.Value.Should().BeOfType<StoreItemModel>().Subject;

        createdItem.Should().BeEquivalentTo(newItem);
    }

    [Fact]
    public async Task Verify_StoreItem_Updated_Successfully()
    {
        // Arrange
        var updateDto = new StoreItemDto()
        {
            Product = new ProductModel()
            {
                Id = 1
            },
            AvailableQuantity = 2,
            BookedQuantity = 2,
            SoldQuantity = 2
        };

        var updatedItem = new StoreItemModel()
        {
            Id = 1,
            ProductId = updateDto.Product.Id,
            AvailableQuantity = updateDto.AvailableQuantity,
            BookedQuantity = updateDto.BookedQuantity,
            SoldQuantity = updateDto.SoldQuantity
        };

        _mockRepository
            .Setup(repo => repo.UpdateAsync(1, It.IsAny<StoreItemModel>()))
            .ReturnsAsync(updatedItem);

        // Act
        var result = await _controller.UpdateStoreItem(1, updateDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedItem = okResult.Value.Should().BeOfType<StoreItemModel>().Subject;

        returnedItem.Should().BeEquivalentTo(updatedItem);
    }

    [Fact]
    public async Task Verify_UpdateStoreItem_With_InvalidId_Returns_NotFound()
    {
        // Arrange
        var updateDto = new StoreItemDto()
        {
            Product = new ProductModel()
            {
                Id = 1
            },
            AvailableQuantity = 2,
            BookedQuantity = 2,
            SoldQuantity = 2
        };

        _mockRepository
            .Setup(repo => repo.UpdateAsync(1, It.IsAny<StoreItemModel>()))
            .ReturnsAsync((StoreItemModel)null);

        // Act
        var result = await _controller.UpdateStoreItem(1, updateDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Verify_StoreItem_Deleted_Successfully()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteStoreItem(1);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
