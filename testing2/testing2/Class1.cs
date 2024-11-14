using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using Microsoft.Extensions.Configuration;
namespace WebUITests
{
    [TestFixture]
    public class GoogleSearchTests
    {
        private IWebDriver driver;
        private string baseUrl;

        [SetUp]
        public void Setup()
        {
            // Загрузка конфигурации из appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Получение BaseUrl из конфигурации
            baseUrl = configuration["TestSettings:BaseUrl"];

            // Инициализация ChromeDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void SearchForKeyword_VerifyTitle()
        {
            // Использование BaseUrl из конфигурации
            driver.Navigate().GoToUrl(baseUrl);

            var searchBox = driver.FindElement(By.Name("q"));
            string searchTerm = "Selenium WebDriver";
            searchBox.SendKeys(searchTerm);
            searchBox.Submit();
            System.Threading.Thread.Sleep(2000);

            // Проверка заголовка страницы
            Assert.That(driver.Title, Does.Contain(searchTerm), "The page title does not contain the search term.");
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
        }
    }
}
