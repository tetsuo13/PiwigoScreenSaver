using Microsoft.Extensions.Caching.Memory;
using Moq;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain;

public class SettingsServiceTests
{
    [Fact]
    public void Get_NotInCache_NotInRepository_ThrowsException()
    {
        var cacheService = new Mock<IMemoryCache>();
        cacheService.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny!)).Returns(false);

        var settingsRepository = new Mock<ISettingsRepository>();
        settingsRepository.Setup(x => x.GetValue(It.IsAny<string>())).Returns<string>(null);

        var settingsService = new SettingsService(cacheService.Object, settingsRepository.Object);

        Assert.Throws<FormatException>(() => settingsService.Get(SettingKey.Url));
    }

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
        var cacheService = new Mock<IMemoryCache>();
        var settingsRepository = new Mock<ISettingsRepository>();
        var settingsService = new SettingsService(cacheService.Object, settingsRepository.Object);

        Assert.False(settingsService.ValidateSettings(url, username, password));
    }

    [Theory]
    [InlineData("www.example.com", false)]
    [InlineData("https://www.example.com/", true)]
    public void Validation_Urls(string url, bool expected)
    {
        var cacheService = new Mock<IMemoryCache>();
        var settingsRepository = new Mock<ISettingsRepository>();
        var settingsService = new SettingsService(cacheService.Object, settingsRepository.Object);

        Assert.Equal(expected, settingsService.ValidateSettings(url, "jdoe", "monkey"));
    }
}
