using OpenQA.Selenium;
using Taf.Dtos;

namespace Taf.PageObjects;
public class AddBookingPage : BasePage
{
    private readonly By _dateInput = By.Id("dateInput");
    private readonly By _timeInput = By.Id("timeInput");
    private readonly By _addressInput = By.Id("addressInput");
    private readonly By _saveButton = By.XPath("//button[text()='Save']");

    public AddBookingPage(IWebDriver driver) : base(driver)
    {
    }

    public BookingsPage AddBooking(AddBookingRequestDto bookingDto)
    {
        var dateInput = Driver.FindElement(_dateInput);
        dateInput.Clear();
        dateInput.SendKeys(bookingDto.Date.ToString("dd/mm/yyyy"));

        var timeInput = Driver.FindElement(_timeInput);
        timeInput.Clear();
        timeInput.SendKeys(bookingDto.Time.ToString("HH:mm:ss zz"));

        var addressInput = Driver.FindElement(_addressInput);
        addressInput.Clear();
        addressInput.SendKeys(bookingDto.Address);

        Driver.FindElement(_saveButton)
            .Click();

        return new BookingsPage(Driver);
    }
}
