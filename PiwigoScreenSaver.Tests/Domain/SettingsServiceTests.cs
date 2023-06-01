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
        cacheService.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny)).Returns(false);

        var settingsRepository = new Mock<ISettingsRepository>();
        settingsRepository.Setup(x => x.GetValue(It.IsAny<string>())).Returns<string>(null);

        var settingsService = new SettingsService(cacheService.Object, settingsRepository.Object);

        Assert.Throws<Exception>(() => settingsService.Get(SettingKey.Url));
    }
}
