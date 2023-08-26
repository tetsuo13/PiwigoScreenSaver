using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain;

public class SettingsServiceTests
{
    [Fact]
    public void Get_NotInCache_NotInRepository_ThrowsException()
    {
        var cacheService = Substitute.For<IMemoryCache>();
        cacheService.TryGetValue(Arg.Any<object>(), out Arg.Any<object?>()).Returns(false);

        var settingsRepository = Substitute.For<ISettingsRepository>();
        settingsRepository.GetValue(Arg.Any<string>()).ReturnsNull();

        var settingsService = new SettingsService(cacheService, settingsRepository);

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
        var cacheService = Substitute.For<IMemoryCache>();
        var settingsRepository = Substitute.For<ISettingsRepository>();
        var settingsService = new SettingsService(cacheService, settingsRepository);

        Assert.False(settingsService.ValidateSettings(url, username, password));
    }

    [Theory]
    [InlineData("www.example.com", false)]
    [InlineData("https://www.example.com/", true)]
    public void Validation_Urls(string url, bool expected)
    {
        var cacheService = Substitute.For<IMemoryCache>();
        var settingsRepository = Substitute.For<ISettingsRepository>();
        var settingsService = new SettingsService(cacheService, settingsRepository);

        Assert.Equal(expected, settingsService.ValidateSettings(url, "jdoe", "monkey"));
    }
}
