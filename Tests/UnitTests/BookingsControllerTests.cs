using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using WebApplicationApi.Controllers;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Booking;
using WebApplicationApi.Repositories.Interfaces;

namespace Tests.UnitTests;
public class BookingsControllerTests
{
    private readonly Mock<IRepository<BookingModel>> _mockRepository;
    private readonly BookingsController _controller;

    public BookingsControllerTests()
    {
        _mockRepository = new Mock<IRepository<BookingModel>>();
        _controller = new BookingsController(_mockRepository.Object);
    }

    [Fact]
    public async Task Verify_GetBookings_Returns_All_Bookings()
    {
        // Arrange
        var bookings = new List<BookingModel>()
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
        _mockRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(bookings);

        // Act
        var result = await _controller.GetBookings();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedBookings = okResult.Value.Should().BeOfType<List<BookingModel>>().Subject;

        returnedBookings.Should().HaveCount(2);
    }

    [Fact]
    public async Task Verify_GetBooking_With_ValidId_Returns_Booking()
    {
        // Arrange
        var booking = new BookingModel()
        {
            Id = 1,
            ProductId = 1,
            Date = DateTime.Today,
            Quantity = 2
        };
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(booking);

        // Act
        var result = await _controller.GetBooking(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedBooking = okResult.Value.Should().BeOfType<BookingModel>().Subject;

        returnedBooking.Should().BeEquivalentTo(booking);
    }

    [Fact]
    public async Task Verify_GetBooking_With_InvalidId_Returns_NotFound()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((BookingModel)null);

        // Act
        var result = await _controller.GetBooking(1);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Verify_Booking_Added_Successfully()
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

        var newBooking = new BookingModel()
        {
            Id = 1,
            ProductId = newBookingDto.Product.Id,
            UserId = 1,
            StatusId = (int)newBookingDto.Status,
            Quantity = newBookingDto.Quantity
        };

        _mockRepository
            .Setup(repo => repo.AddAsync(It.IsAny<BookingModel>()))
            .ReturnsAsync(newBooking);

        // Act
        var result = await _controller.CreateBooking(newBookingDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var createdBooking = createdResult.Value.Should().BeOfType<BookingModel>().Subject;

        createdBooking.Should().BeEquivalentTo(newBooking);
    }

    [Fact]
    public async Task Verify_Booking_Updated_Successfully()
    {
        // Arrange
        var updateDto = new BookingDto()
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

        var updatedBooking = new BookingModel()
        {
            Id = 1,
            ProductId = updateDto.Product.Id,
            UserId = 1,
            StatusId = (int)updateDto.Status,
            Quantity = updateDto.Quantity
        };

        _mockRepository
            .Setup(repo => repo.UpdateAsync(1, It.IsAny<BookingModel>()))
            .ReturnsAsync(updatedBooking);

        // Act
        var result = await _controller.UpdateBooking(1, updateDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedBooking = okResult.Value.Should().BeOfType<BookingModel>().Subject;

        returnedBooking.Should().BeEquivalentTo(updatedBooking);
    }

    [Fact]
    public async Task Verify_Booking_Deleted_Successfully()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteBooking(1);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
