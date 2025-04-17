using OpenQA.Selenium;
using Taf.Dtos;

namespace Taf.PageObjects;
public class AddProductPage : BasePage
{
    private readonly By _nameInput = By.Id("nameInput");
    private readonly By _priceInput = By.Id("priceInput");
    private readonly By _descriptionInput = By.Id("descInput");
    private readonly By _authorInput = By.Id("authorInput");
    private readonly By _saveButton = By.XPath("//button[text()='Save']");

    public AddProductPage(IWebDriver driver) : base(driver)
    {
    }

    public CataloguePage AddNewProduct(AddProductRequestDto productDto)
    {
        Driver
            .FindElement(_nameInput)
            .SendKeys(productDto.Name);
        Driver
            .FindElement(_priceInput)
            .SendKeys(productDto.Price.ToString());
        Driver
            .FindElement(_descriptionInput)
            .SendKeys(productDto.Description);
        Driver
            .FindElement(_authorInput)
            .SendKeys(productDto.Author);
        Driver
            .FindElement(_saveButton)
            .Click();

        return new CataloguePage(Driver);
    }
}
