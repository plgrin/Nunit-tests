using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebUITests
{
    [TestFixture]
    public class EHUPageSearchTests
    {
        private IWebDriver driver;
        private string ehuBaseUrl;
        private string searchTerm;

        [SetUp]
        public void Setup()
        {
            // Загрузка конфигурации из appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Получение ссылок и данных из конфигурационного файла
            ehuBaseUrl = configuration["TestSettings:EHUBaseUrl"];
            searchTerm = configuration["TestSettings:SearchTerm"];

            // Инициализация ChromeDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifySearchFunctionality()
        {
            // Шаг 1: Перейти на главную страницу EHU
            driver.Navigate().GoToUrl(ehuBaseUrl);

            // Шаг 2: Найти поисковую строку и ввести поисковый запрос
            var searchBox = driver.FindElement(By.Name("s")); // Предполагаем, что поисковое поле имеет атрибут name="s"
            searchBox.SendKeys(searchTerm);
            searchBox.SendKeys(Keys.Enter); // Нажимаем Enter

            // Ожидание загрузки страницы результатов поиска
            System.Threading.Thread.Sleep(2000); // Лучше использовать явные ожидания для стабильности

            // Проверка URL страницы результатов поиска
            Assert.That(driver.Url, Does.Contain("/?s=study+programs"), "URL не содержит ожидаемый поисковый запрос.");

            // Проверка наличия результатов поиска
            var searchResults = driver.FindElements(By.CssSelector(".search-result-class")); // Замените .search-result-class на правильный CSS-селектор для результатов поиска
            Assert.That(searchResults.Count, Is.GreaterThan(0), "Результаты поиска не найдены.");
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
        }
    }
}
