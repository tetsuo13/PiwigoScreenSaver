using Moq;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using PiwigoScreenSaver.Presenters;
using PiwigoScreenSaver.Tests.Mocks;
using Xunit;

namespace PiwigoScreenSaver.Tests.Presenters
{
    public class SettingsFormPresenterTests
    {
        [Theory]
        [InlineData("https://example.com/", "", "monkey")]
        [InlineData("https://example.com/", null, "monkey")]
        [InlineData("https://example.com/", " ", "monkey")]
        [InlineData("https://example.com/", "jdoe", "")]
        [InlineData("https://example.com/", "jdoe", null)]
        [InlineData("https://example.com/", "jdoe", " ")]
        [InlineData("", "jdoe", "monkey")]
        [InlineData(null, "jdoe", "monkey")]
        [InlineData(" ", "jdoe", "monkey")]
        public void Validation_NullOrWhiteSpace_ShouldFail(string url, string username, string password)
        {
            var view = new MockSettingsFormView
            {
                Url = url,
                Username = username,
                Password = password
            };
            var settingsRepository = new Mock<ISettingsRepository>();
            var presenter = new SettingsFormPresenter(view, new SettingsService(null, settingsRepository.Object));
            Assert.False(presenter.ValidateSettings());
        }

        [Theory]
        [InlineData("www.example.com", false)]
        [InlineData("https://www.example.com/", true)]
        public void Validation_Urls(string url, bool expected)
        {
            var view = new MockSettingsFormView
            {
                Url = url,
                Username = "jdoe",
                Password = "monkey"
            };
            var settingsRepository = new Mock<ISettingsRepository>();
            var presenter = new SettingsFormPresenter(view, new SettingsService(null, settingsRepository.Object));
            Assert.Equal(expected, presenter.ValidateSettings());
        }

        [Fact]
        public void Initialize_NoSettings_DoesntThrowException()
        {
            var view = new MockSettingsFormView();
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.Get(It.IsAny<SettingKey>())).Throws<Exception>();
            var presenter = new SettingsFormPresenter(view, settingsService.Object);

            presenter.Initialize();
        }
    }
}
