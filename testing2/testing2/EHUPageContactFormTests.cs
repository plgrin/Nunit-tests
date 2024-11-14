using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebUITests
{
    [TestFixture]
    public class EHUPageContactFormTests
    {
        private IWebDriver driver;
        private string contactPageUrl;
        private string contactName;
        private string contactEmail;
        private string contactMessage;

        [SetUp]
        public void Setup()
        {
            // Загрузка конфигурации из appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Получение данных из конфигурационного файла
            contactPageUrl = configuration["TestSettings:ContactPageUrl"];
            contactName = configuration["TestSettings:ContactName"];
            contactEmail = configuration["TestSettings:ContactEmail"];
            contactMessage = configuration["TestSettings:ContactMessage"];

            // Инициализация ChromeDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifyContactFormSubmission()
        {
            // Шаг 1: Перейти на страницу контактной формы
            driver.Navigate().GoToUrl(contactPageUrl);

            // Шаг 2: Заполнить поле "Name"
            var nameField = driver.FindElement(By.Id("name-field-id")); // Замените на правильный ID или селектор
            nameField.SendKeys(contactName);

            // Шаг 3: Заполнить поле "Email"
            var emailField = driver.FindElement(By.Id("email-field-id")); // Замените на правильный ID или селектор
            emailField.SendKeys(contactEmail);

            // Шаг 4: Заполнить поле "Message"
            var messageField = driver.FindElement(By.Id("message-field-id")); // Замените на правильный ID или селектор
            messageField.SendKeys(contactMessage);

            // Шаг 5: Нажать кнопку "Send"
            var sendButton = driver.FindElement(By.Id("send-button-id")); // Замените на правильный ID или селектор
            sendButton.Click();

            // Ожидание загрузки сообщения об успешной отправке
            System.Threading.Thread.Sleep(2000); // Лучше использовать явные ожидания для стабильности

            // Проверка наличия сообщения об успешной отправке
            var successMessage = driver.FindElement(By.Id("success-message-id")).Text; // Замените на правильный ID или селектор
            Assert.That(successMessage, Does.Contain("Thank you for your message. It has been sent."), "Сообщение об успешной отправке не отображается.");
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
        }
    }
}
