using OpenQA.Selenium;

namespace Taf.PageObjects;

public class BookingsPage : BasePage
{
    private readonly By _bookingCards = By.XPath("//div[contains(@class,'col mb-4')]");
    private readonly By _productName = By.XPath(".//h4[contains(@class,'card-title')]");

    public BookingsPage(IWebDriver driver) : base(driver)
    {
    }

    public IList<IWebElement> GetAllBookingCards()
    {
        return Driver.FindElements(_bookingCards);
    }

    public bool IsNewBookingDisplayed(string productName)
    {
        var booking = GetAllBookingCards()
            .FirstOrDefault(x => x.FindElement(_productName).Text.Trim().Equals(productName, StringComparison.OrdinalIgnoreCase));
        return booking != null;
    }
}
