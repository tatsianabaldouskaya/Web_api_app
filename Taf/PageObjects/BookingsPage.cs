using OpenQA.Selenium;

namespace Taf.PageObjects;

public class BookingsPage : BasePage
{
    private readonly By _bookingCards = By.XPath("//div[contains(@class,'card')]");
    private readonly By _productName = By.XPath(".//h5[contains(@class,'card-title')]");

    public BookingsPage(IWebDriver driver) : base(driver)
    {
    }

    public IList<IWebElement> GetAllBookingCards()
    {
        return Driver.FindElements(_bookingCards);
    }

    public BookingsPage FindBookingByProductName(string productName)
    {
        GetAllBookingCards()
            .FirstOrDefault(x => x.FindElement(_productName).Text == productName);
        return this;
    }

    private void ScrollToBottom()
    {
        var jsExecutor = (IJavaScriptExecutor)Driver;
        jsExecutor.ExecuteScript("window.scrollBy(0, document.body.scrollHeight)");
        Thread.Sleep(500);
    }
}
