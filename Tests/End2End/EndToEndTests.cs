using FluentAssertions;

using OpenQA.Selenium;

using Taf.Core;
using Taf.Core.Utils;
using Taf.Dtos;
using Taf.PageObjects;

using WebApplicationApi.Data;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Repositories;

namespace Tests.End2End;
public class EndToEndTests : IDisposable
{
    private MainPage _mainPage;
    private BookingsPage _bookingsPage;
    private readonly AppDbContext _dbContext;
    private readonly ProductRepository _productRepository;
    private readonly BookingRepository _bookingRepository;
    private IWebDriver _driver;

    public EndToEndTests()
    {
        _driver = ChromeWebDriver.InitializeDriver();
        _mainPage = new MainPage(_driver);
        _bookingsPage = new BookingsPage(_driver);
        _dbContext = TestDbContext.CreateDbContext();
        _productRepository = new ProductRepository(_dbContext);
        _bookingRepository = new BookingRepository(_dbContext);
    }

    [Fact]
    public async Task Verify_User_Can_Create_Booking()
    {
        // Arrange
        var productModel = new ProductModel
        {
            Name = "Product"+ RandomHelper.NumericString(5),
            Description = "TestDesc",
            Author = "Test Author",
            Price = 10.55f,
        };

        var product = await _productRepository.AddAsync(productModel);

        var preparedBookingDto = new AddBookingRequestDto
        {
            Date = DateTime.Now,
            Time = DateTime.Now,
            Address = "Warsaw"
        };
        Thread.Sleep(1000);

        // Act
        _mainPage
            .OpenLoginPage()
            .LoginAs(Role.Customer)
            .OpenCataloguePage()
            .OpenBookingFormForProduct(product.Name)
            .AddBooking(preparedBookingDto);

        var bookings = await _bookingRepository.GetAllAsync();
        var booking = bookings.FirstOrDefault(p => p.ProductId == product.Id);
        
        // Assert
        booking.Should().NotBeNull();
        _bookingsPage.IsNewBookingDisplayed(productModel.Name).Should().BeTrue();
    }

    [Fact]
    public async Task Verify_Manager_Can_Create_Product()
    {
        // Arrange
        var preparedProductDto = new AddProductRequestDto
        {
            Name = $"TestBook_{RandomHelper.NumericString(6)}",
            Description = "Some description",
            Author = "UnknownAuthor",
            Price = 4.99f,
            ImagePath = null
        };

        // Act
        _mainPage
            .OpenLoginPage()
            .LoginAs(Role.Manager)
            .OpenCataloguePage()
            .OpenAddProductForm()
            .AddNewProduct(preparedProductDto);

        // Assert
        var products = await _productRepository.GetAllAsync();
        var createdProduct = products.FirstOrDefault(p => p.Name == preparedProductDto.Name);
        createdProduct.Should().NotBeNull();
    }

    public void Dispose()
    {
        _mainPage.Logout();
        ChromeWebDriver.QuitDriver();
    }
}
