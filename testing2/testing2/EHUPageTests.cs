using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebUITests
{
    [TestFixture]
    public class EHUPageTests
    {
        private IWebDriver driver;
        private string ehuBaseUrl;
        private string aboutPageUrl;

        [SetUp]
        public void Setup()
        {
            // Загрузка конфигурации из appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Получение ссылок из конфигурационного файла
            ehuBaseUrl = configuration["TestSettings:EHUBaseUrl"];
            aboutPageUrl = configuration["TestSettings:AboutPageUrl"];

            // Инициализация ChromeDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifyNavigationToAboutEHUPage()
        {
            // Шаг 1: Перейти на главную страницу EHU
            driver.Navigate().GoToUrl(ehuBaseUrl);

            // Шаг 2: Найти и нажать на ссылку "About EHU"
            var aboutLink = driver.FindElement(By.LinkText("About EHU"));
            aboutLink.Click();

            // Ожидание загрузки страницы
            System.Threading.Thread.Sleep(2000); // Лучше заменить на явные ожидания

            // Проверка URL страницы "About EHU"
            Assert.That(driver.Url, Is.EqualTo(aboutPageUrl), "URL не соответствует ожидаемому.");

            // Проверка заголовка страницы
            Assert.That(driver.Title, Is.EqualTo("About EHU"), "Заголовок страницы не соответствует ожидаемому.");

            // Проверка заголовка контента
            var header = driver.FindElement(By.TagName("h1")).Text;
            Assert.That(header, Is.EqualTo("About European Humanities University"), "Заголовок контента не соответствует ожидаемому.");
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
        }
    }
}
