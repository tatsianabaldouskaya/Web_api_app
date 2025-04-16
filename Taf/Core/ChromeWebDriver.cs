using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Taf.Core;

public class ChromeWebDriver
{
    private static IWebDriver? _driver;

    public static IWebDriver InitializeDriver()
    {
        var chromeOptions = new ChromeOptions();

        if (_driver == null)
        {
            _driver = new ChromeDriver(chromeOptions);

            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Navigate().Refresh();
            _driver.Navigate().GoToUrl("https://localhost:44388");
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        return _driver;
    }

    public static void QuitDriver()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
            _driver = null;
        }
    }
}
