using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace kpi5.Tests;

public class SeleniumTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl = "https://the-internet.herokuapp.com/";

    public SeleniumTests()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [Fact]
    public void TestAddRemoveElements()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "add_remove_elements/");

        var addButton = _driver.FindElement(By.CssSelector("button[onclick='addElement()']"));
        addButton.Click();

        var deleteButton = _driver.FindElement(By.CssSelector("button.added-manually"));
        Assert.True(deleteButton.Displayed);

        deleteButton.Click();

        var deleteButtons = _driver.FindElements(By.CssSelector("button.added-manually"));
        Assert.Empty(deleteButtons);
    }

    [Fact]
    public void TestCheckboxes()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "checkboxes");

        var checkboxes = _driver.FindElements(By.CssSelector("#checkboxes input[type='checkbox']"));
        Assert.Equal(2, checkboxes.Count);

        if (!checkboxes[0].Selected)
        {
            checkboxes[0].Click();
        }
        Assert.True(checkboxes[0].Selected);

        if (checkboxes[1].Selected)
        {
            checkboxes[1].Click();
        }
        Assert.False(checkboxes[1].Selected);
    }

    [Fact]
    public void TestDropdown()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "dropdown");

        var dropdown = new SelectElement(_driver.FindElement(By.Id("dropdown")));
        
        dropdown.SelectByText("Option 1");
        Assert.Equal("Option 1", dropdown.SelectedOption.Text);

        dropdown.SelectByValue("2");
        Assert.Equal("Option 2", dropdown.SelectedOption.Text);
    }

    [Fact]
    public void TestInputs()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "inputs");

        var input = _driver.FindElement(By.CssSelector("input[type='number']"));
        
        input.SendKeys("123");
        Assert.Equal("123", input.GetAttribute("value"));

        input.Clear();
        input.SendKeys("abc");
        
        input.Clear();
        input.SendKeys("456");
        Assert.Equal("456", input.GetAttribute("value"));
    }

    [Fact]
    public void TestStatusCodes()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "status_codes");

        var link200 = _driver.FindElement(By.CssSelector("a[href='status_codes/200']"));
        link200.Click();
        Assert.Contains("200", _driver.Url);
        _driver.Navigate().Back();

        var link404 = _driver.FindElement(By.CssSelector("a[href='status_codes/404']"));
        link404.Click();
        Assert.Contains("404", _driver.Url);
    }

    [Fact]
    public void TestDragAndDrop()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "drag_and_drop");

        var columnA = _driver.FindElement(By.Id("column-a"));
        var columnB = _driver.FindElement(By.Id("column-b"));

        Assert.Equal("A", columnA.Text);
        Assert.Equal("B", columnB.Text);

        var actions = new Actions(_driver);
        actions.DragAndDrop(columnA, columnB).Perform();

        columnA = _driver.FindElement(By.Id("column-a"));
        columnB = _driver.FindElement(By.Id("column-b"));
        Assert.NotNull(columnA);
        Assert.NotNull(columnB);
    }

    [Fact]
    public void TestShiftingContent()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "shifting_content/menu");

        var menuItems = _driver.FindElements(By.CssSelector(".example ul li a"));
        Assert.Equal(5, menuItems.Count);
        
        foreach (var item in menuItems)
        {
            Assert.False(string.IsNullOrEmpty(item.GetAttribute("href")));
        }
    }

    [Fact]
    public void TestGeolocation()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "geolocation");

        var button = _driver.FindElement(By.CssSelector("button[onclick='getLocation()']"));
        button.Click();
        Assert.NotNull(button);
    }

    [Fact]
    public void TestJavaScriptErrors()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "javascript_error");
        var bodyText = _driver.FindElement(By.TagName("body")).Text;
        Assert.Contains("This page has a JavaScript error in the onload event", bodyText);
    }

    [Fact]
    public void TestExitIntent()
    {
        _driver.Navigate().GoToUrl(_baseUrl + "exit_intent");

        var modal = _driver.FindElement(By.Id("ouibounce-modal"));
        Assert.False(modal.Displayed);
        Assert.NotNull(modal);
    }
}
