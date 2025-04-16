using OpenQA.Selenium;

namespace Taf.PageObjects;

public class BasePage
{
    protected readonly IWebDriver Driver;

    private readonly By _catalogLink = By.XPath("//a[@href='/Products']");
    private readonly By _bookingsLink = By.XPath("//a[@href='/Bookings']");
    private readonly By _loginLink = By.XPath("//a[@href='/Login']");
    private readonly By _logoutLink = By.XPath("//a[@href='/Logout']");

    public BasePage(IWebDriver driver)
    {
        Driver = driver;
    }

    public CataloguePage OpenCataloguePage()
    {
        Driver
            .FindElement(_catalogLink)
            .Click();
        return new CataloguePage(Driver);
    }

    public LoginPage OpenLoginPage()
    {
        Driver
            .FindElement(_loginLink)
            .Click();
        return new LoginPage(Driver);
    }

    public MainPage Logout()
    {
        Driver
            .FindElement(_logoutLink)
            .Click();
        return new MainPage(Driver);
    }
}
