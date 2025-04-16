using OpenQA.Selenium;

using WebApplicationApi.Enums;

namespace Taf.PageObjects;

public class LoginPage : BasePage
{
    private readonly By _loginField = By.Id("loginInput");
    private readonly By _passwordField = By.Id("passInput");
    private readonly By _loginButton = By.XPath("//button[text()='Login']");

    public LoginPage(IWebDriver driver) : base(driver)
    {
    }

    public MainPage LoginAs(Role role)
    {
        var credentials = GetCredentialsByRole(role);

        var loginInput = Driver.FindElement(_loginField);
        loginInput.Clear();
        loginInput.SendKeys(credentials.Key);

        var passwordInput = Driver.FindElement(_passwordField);
        passwordInput.Clear();
        passwordInput.SendKeys(credentials.Value);

        Driver
            .FindElement(_loginButton)
            .Click();

        return new MainPage(Driver);
    }

    private static KeyValuePair<string, string> GetCredentialsByRole(Role role)
    {
        return role switch
        {
            Role.Admin => new KeyValuePair<string, string>("admin", "admin"),
            Role.Manager => new KeyValuePair<string, string>("manager", "manager"),
            Role.Customer => new KeyValuePair<string, string>("customer", "customer"),
            _ => throw new ArgumentOutOfRangeException(nameof(role), $"The role {role} is not supported.")
        };
    }
}
