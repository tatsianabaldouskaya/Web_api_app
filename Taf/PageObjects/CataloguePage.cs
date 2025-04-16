using OpenQA.Selenium;

namespace Taf.PageObjects;

public class CataloguePage : BasePage
{
    private readonly By _addProductButton = By.XPath("//button[text()='Create']");
    private readonly By _bookButton = By.XPath("//button[text()='Book']");
    private readonly By _productCard = By.XPath("//div[contains(@class,'col mb-4')]");
    private readonly By _productName = By.XPath(".//h5[contains(@class,'card-title')]");

    public CataloguePage(IWebDriver driver) : base(driver)
    {
    }

    public AddBookingPage OpenBookingFormForProduct(string productName)
    {
        var product = FindProductByProductName(productName);
        ScrollToElement(product);

        var bookButton = product.FindElement(_bookButton);

        ScrollToElement(bookButton);
        bookButton.Click();

        return new AddBookingPage(Driver);
    }

    public AddProductPage OpenAddProductForm()
    {
        var addProductButton = Driver.FindElement(_addProductButton);
        ScrollToElement(addProductButton);
        addProductButton.Click();

        return new AddProductPage(Driver);
    }

    public IList<IWebElement> GetAllProducts()
    {
        return Driver.FindElements(_productCard);
    }

    public IWebElement FindProductByProductName(string productName)
    {
        var product = GetAllProducts()
            .FirstOrDefault(x => x.FindElement(_productName).Text.Trim().Equals(productName, StringComparison.OrdinalIgnoreCase));
        return product;
    }

    private void ScrollToElement(IWebElement element)
    {
        var jsExecutor = (IJavaScriptExecutor)Driver;
        jsExecutor.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", element);
    }

    private void ScrollToBottom()
    {
        var jsExecutor = (IJavaScriptExecutor)Driver;
        jsExecutor.ExecuteScript("window.scrollBy(0, document.body.scrollHeight)");
        Thread.Sleep(500);
    }
}
