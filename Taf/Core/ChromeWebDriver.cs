using Microsoft.Extensions.Options;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Taf.Core;

public class ChromeWebDriver
{
    private static IWebDriver? _driver;

    public static IWebDriver InitializeDriver()
    {
        var chromeOptions = new ChromeOptions();
        //chromeOptions.AddArguments("--headless");
        chromeOptions.AddArguments("--no-sandbox");
        chromeOptions.AddArguments("--disable-dev-shm-usage");

        if (_driver == null)
        {
            _driver = new ChromeDriver(chromeOptions);
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Navigate().Refresh();
            _driver.Navigate().GoToUrl(Config.BaseUrl);
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
