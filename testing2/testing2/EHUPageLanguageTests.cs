using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebUITests
{
    [TestFixture]
    public class EHUPageLanguageTests
    {
        private IWebDriver driver;
        private string ehuBaseUrl;
        private string lithuanianVersionUrl;

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
            lithuanianVersionUrl = configuration["TestSettings:LithuanianVersionUrl"];

            // Инициализация ChromeDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            // Шаг 1: Перейти на главную страницу EHU
            driver.Navigate().GoToUrl(ehuBaseUrl);

            // Шаг 2: Найти переключатель языка и выбрать "Lietuvių"
            var languageSwitcher = driver.FindElement(By.Id("language-switcher-id")); // Замените на правильный ID или селектор
            languageSwitcher.Click();

            var lithuanianOption = driver.FindElement(By.LinkText("Lietuvių")); // Замените на правильный текст или селектор
            lithuanianOption.Click();

            // Ожидание загрузки страницы
            System.Threading.Thread.Sleep(2000); // Лучше использовать явные ожидания для стабильности

            // Проверка URL версии на литовском языке
            Assert.That(driver.Url, Is.EqualTo(lithuanianVersionUrl), "URL не соответствует литовской версии сайта.");

            // Проверка, что содержимое страницы отображается на литовском
            var content = driver.FindElement(By.TagName("body")).Text; // Проверьте, что текст тела страницы меняется
            Assert.That(content, Does.Contain("lietuvų"), "Содержимое страницы не отображается на литовском языке.");
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
        }
    }
}
