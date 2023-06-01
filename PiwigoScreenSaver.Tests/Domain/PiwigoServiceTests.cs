using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using PiwigoScreenSaver.Models.Piwigo;
using System.Drawing;
using System.Net;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain;

public class PiwigoServiceTests
{
    private readonly IDictionary<string, Derivative> derivatives = new Dictionary<string, Derivative>
    {
        { "square", new Derivative { Url = "http://example.com/1", Width = 120, Height = 120 } },
        { "thumb", new Derivative { Url = "http://example.com/2", Width = 144, Height = 108 } },
        { "2small", new Derivative { Url = "http://example.com/3", Width = 240, Height = 180 } },
        { "xsmall", new Derivative { Url = "http://example.com/4", Width = 432, Height = 324 } },
        { "small", new Derivative { Url = "http://example.com/5", Width = 576, Height = 432 } },
        { "medium", new Derivative { Url = "http://example.com/6", Width = 792, Height = 594 } },
        { "large", new Derivative { Url = "http://example.com/7", Width = 1008, Height = 756 } },
        { "xlarge", new Derivative { Url = "http://example.com/8", Width = 1224, Height = 918 } },
        { "xxlarge", new Derivative { Url = "http://example.com/9", Width = 1656, Height = 1242 } },
    };

    [Fact]
    public async Task NotAuthenticated_Guest_ReturnsTrue()
    {
        var json = @"{""stat"":""ok"",""result"":{""username"":""guest"",""status"":""guest"",""theme"":""modus"",""language"":""en_GB"",""pwg_token"":""a55282f9b21754bf3b31f478e68e42ff"",""charset"":""utf-8"",""current_datetime"":""2019-12-15 21:34:58"",""version"":""2.10.1"",""available_sizes"":[""square"",""thumb"",""2small"",""xsmall"",""small"",""medium"",""large"",""xlarge"",""xxlarge""]}}";
        var httpClient = CreateMockHttpClient(HttpStatusCode.OK, json);
        var logger = new Mock<ILogger>();

        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), httpClient);
        var actual = await piwigoService.NotAuthenticated();

        Assert.True(actual);
    }

    [Fact]
    public async Task NotAuthenticated_AsAuthenticatedUser_ReturnsFalse()
    {
        var json = @"{""stat"":""ok"",""result"":{""username"":""jdoe"",""status"":""generic"",""theme"":""modus"",""language"":""en_GB"",""pwg_token"":""a55282f9b21754bf3b31f478e68e42ff"",""charset"":""utf-8"",""current_datetime"":""2019-12-15 21:34:58"",""version"":""2.10.1"",""available_sizes"":[""square"",""thumb"",""2small"",""xsmall"",""small"",""medium"",""large"",""xlarge"",""xxlarge""]}}";
        var httpClient = CreateMockHttpClient(HttpStatusCode.OK, json);
        var logger = new Mock<ILogger>();

        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), httpClient);
        var actual = await piwigoService.NotAuthenticated();

        Assert.False(actual);
    }

    [Fact]
    public async Task Authenticate_InvalidCredentials_ThrowsException()
    {
        var json = @"{""stat"":""fail"",""err"":999,""message"":""Invalid username\/password""}";
        var httpClient = CreateMockHttpClient(HttpStatusCode.OK, json);
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), httpClient);

        await Assert.ThrowsAsync<Exception>(piwigoService.Authenticate);
    }

    [Fact]
    public async Task Authenticate_ValidCredentials_NoException()
    {
        var json = @"{""stat"":""ok"",""result"":true}";
        var httpClient = CreateMockHttpClient(HttpStatusCode.OK, json);
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), httpClient);

        await piwigoService.Authenticate();
    }

    [Fact]
    public void MapJson_SessionStatus()
    {
        var json = @"{""stat"":""ok"",""result"":{""username"":""guest"",""status"":""guest"",""theme"":""modus"",""language"":""en_GB"",""pwg_token"":""a55282f9b21754bf3b31f478e68e42ff"",""charset"":""utf-8"",""current_datetime"":""2019-12-15 21:34:58"",""version"":""2.10.1"",""available_sizes"":[""square"",""thumb"",""2small"",""xsmall"",""small"",""medium"",""large"",""xlarge"",""xxlarge""]}}";
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), new HttpClient());
        var actual = piwigoService.MapJson<SessionStatus>(json);

        Assert.NotNull(actual);
        Assert.Equal(StatusCode.OK, actual.Status);
        Assert.Null(actual.ErrorCode);
        Assert.Null(actual.ErrorMessage);
        Assert.NotNull(actual.Result);
        Assert.Equal("guest", actual.Result.Username);
        Assert.Equal("guest", actual.Result.Status);
        Assert.Equal("modus", actual.Result.Theme);
        Assert.Equal("en_GB", actual.Result.Language);
        Assert.Equal("a55282f9b21754bf3b31f478e68e42ff", actual.Result.Token);
        Assert.Equal("utf-8", actual.Result.Charset);
        Assert.Equal(new DateTime(2019, 12, 15, 21, 34, 58), actual.Result.CurrentDateTime);
        Assert.Equal("2.10.1", actual.Result.Version);
        Assert.Equal(new string[] { "square", "thumb", "2small", "xsmall", "small", "medium", "large", "xlarge", "xxlarge" },
            actual.Result.AvailableSizes);
    }

    [Fact]
    public void MapJson_SessionStatus_Error()
    {
        var json = @"{""stat"":""fail"",""err"":501,""message"":""Method name is not valid""}";
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), new HttpClient());
        var actual = piwigoService.MapJson<SessionStatus>(json);

        Assert.NotNull(actual);
        Assert.Equal(StatusCode.Fail, actual.Status);
        Assert.Equal(501, actual.ErrorCode);
        Assert.Equal("Method name is not valid", actual.ErrorMessage);
        Assert.Null(actual.Result);
    }

    [Fact]
    public void MapJson_CategoryImages()
    {
        var json = @"{""stat"":""ok"",""result"":{""paging"":{""page"":0,""per_page"":1,""count"":1,""total_count"":""976""},""images"":[{""id"":9578,""width"":2592,""height"":1944,""hit"":0,""file"":""20180218_113956.jpg"",""name"":""20180218 113956"",""comment"":null,""date_creation"":""2018-02-18 11:39:56"",""date_available"":""2018-11-29 21:49:54"",""page_url"":""https:\/\/example.com\/picture.php?\/8178"",""element_url"":""https:\/\/example.com\/upload\/2018\/11\/29\/20181129214954-8ce196fc.jpg"",""derivatives"":{""square"":{""url"":""https:\/\/example.com\/_data\/i\/upload\/2018\/11\/29\/20181129214954-8ce196fc-sq.jpg"",""width"":120,""height"":120},""thumb"":{""url"":""https:\/\/example.com\/_data\/i\/upload\/2018\/11\/29\/20181129214954-8ce196fc-th.jpg"",""width"":144,""height"":108},""2small"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2018\/11\/29\/20181129214954-8ce196fc-2s.jpg"",""width"":240,""height"":180},""xsmall"":{""url"":""https:\/\/example.com\/_data\/i\/upload\/2018\/11\/29\/20181129214954-8ce196fc-xs.jpg"",""width"":432,""height"":324},""small"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2018\/11\/29\/20181129214954-8ce196fc-sm.jpg"",""width"":576,""height"":432},""medium"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2018\/11\/29\/20181129214954-8ce196fc-me.jpg"",""width"":792,""height"":594},""large"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2018\/11\/29\/20181129214954-8ce196fc-la.jpg"",""width"":1008,""height"":756},""xlarge"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2018\/11\/29\/20181129214954-8ce196fc-xl.jpg"",""width"":1224,""height"":918},""xxlarge"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2018\/11\/29\/20181129214954-8ce196fc-xx.jpg"",""width"":1656,""height"":1242}},""categories"":[{""id"":38,""url"":""https:\/\/example.com\/index.php?\/category\/38"",""page_url"":""https:\/\/example.com\/picture.php?\/8178\/category\/38""}]}]}}";
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), new HttpClient());
        var actual = piwigoService.MapJson<CategoryImages>(json);

        Assert.NotNull(actual);
        Assert.Equal(StatusCode.OK, actual.Status);
        Assert.NotNull(actual.Result);
        Assert.NotNull(actual.Result.Paging);
        Assert.Equal(0, actual.Result.Paging.Page);
        Assert.Equal(1, actual.Result.Paging.PerPage);
        Assert.Equal(1, actual.Result.Paging.Count);
        Assert.Equal("976", actual.Result.Paging.TotalCount);
        Assert.NotNull(actual.Result.Images);
        Assert.Single(actual.Result.Images);
        Assert.NotNull(actual.Result.Images.Single().Derivatives);
        Assert.Equal(1656, actual.Result.Images.Single().Derivatives["xxlarge"].Width);
        Assert.Equal(1242, actual.Result.Images.Single().Derivatives["xxlarge"].Height);
        Assert.Equal("20180218 113956", actual.Result.Images.Single().Name);
    }

    [Fact]
    public void MapJson_CategoryImages_StringWidthAndHeight()
    {
        var json = @"{""stat"":""ok"",""result"":{""paging"":{""page"":0,""per_page"":1,""count"":1,""total_count"":""976""},""images"":[{""id"":8587,""width"":389,""height"":800,""hit"":0,""file"":""1568733725341_photo.jpg"",""name"":""1568733725341 photo"",""comment"":null,""date_creation"":""2019-09-17 11:21:00"",""date_available"":""2019-09-23 20:57:32"",""page_url"":""https:\/\/example.com\/picture.php?\/8587"",""element_url"":""https:\/\/example.com\/upload\/2019\/09\/23\/20190923205732-d3f0e81a.jpg"",""derivatives"":{""square"":{""url"":""https:\/\/example.com\/_data\/i\/upload\/2019\/09\/23\/20190923205732-d3f0e81a-sq.jpg"",""width"":120,""height"":120},""thumb"":{""url"":""https:\/\/example.com\/_data\/i\/upload\/2019\/09\/23\/20190923205732-d3f0e81a-th.jpg"",""width"":70,""height"":144},""2small"":{""url"":""https:\/\/example.com\/_data\/i\/upload\/2019\/09\/23\/20190923205732-d3f0e81a-2s.jpg"",""width"":116,""height"":240},""xsmall"":{""url"":""https:\/\/example.com\/_data\/i\/upload\/2019\/09\/23\/20190923205732-d3f0e81a-xs.jpg"",""width"":157,""height"":324},""small"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2019\/09\/23\/20190923205732-d3f0e81a-sm.jpg"",""width"":210,""height"":432},""medium"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2019\/09\/23\/20190923205732-d3f0e81a-me.jpg"",""width"":288,""height"":594},""large"":{""url"":""https:\/\/example.com\/i.php?\/upload\/2019\/09\/23\/20190923205732-d3f0e81a-la.jpg"",""width"":367,""height"":756},""xlarge"":{""url"":""https:\/\/example.com\/upload\/2019\/09\/23\/20190923205732-d3f0e81a.jpg"",""width"":""389"",""height"":""800""},""xxlarge"":{""url"":""https:\/\/example.com\/upload\/2019\/09\/23\/20190923205732-d3f0e81a.jpg"",""width"":""389"",""height"":""800""}},""categories"":[{""id"":38,""url"":""https:\/\/example.com\/index.php?\/category\/38"",""page_url"":""https:\/\/example.com\/picture.php?\/8587\/category\/38""}]}]}}";
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), new HttpClient());
        var actual = piwigoService.MapJson<CategoryImages>(json);

        Assert.NotNull(actual);
        Assert.Equal(StatusCode.OK, actual.Status);
        Assert.NotNull(actual.Result);
        Assert.NotNull(actual.Result.Paging);
        Assert.Equal(0, actual.Result.Paging.Page);
        Assert.Equal(1, actual.Result.Paging.PerPage);
        Assert.Equal(1, actual.Result.Paging.Count);
        Assert.Equal("976", actual.Result.Paging.TotalCount);
        Assert.NotNull(actual.Result.Images);
        Assert.Single(actual.Result.Images);
        Assert.NotNull(actual.Result.Images.Single().Derivatives);
        Assert.Equal(389, actual.Result.Images.Single().Derivatives["xxlarge"].Width);
        Assert.Equal(800, actual.Result.Images.Single().Derivatives["xxlarge"].Height);
        Assert.Equal("1568733725341 photo", actual.Result.Images.Single().Name);
    }

    [Fact]
    public void MapJson_CategoryImages_NoResults()
    {
        var json = @"{""stat"":""ok"",""result"":{""paging"":{""page"":0,""per_page"":1,""count"":0,""total_count"":""0""},""images"":[]}}";
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), new HttpClient());
        var actual = piwigoService.MapJson<CategoryImages>(json);

        Assert.NotNull(actual);
        Assert.Equal(StatusCode.OK, actual.Status);
        Assert.NotNull(actual.Result);
        Assert.NotNull(actual.Result.Paging);
        Assert.Equal(0, actual.Result.Paging.Page);
        Assert.Equal(1, actual.Result.Paging.PerPage);
        Assert.Equal(0, actual.Result.Paging.Count);
        Assert.Equal("0", actual.Result.Paging.TotalCount);
        Assert.NotNull(actual.Result.Images);
        Assert.False(actual.Result.Images.Any());
    }

    [Theory]
    [InlineData(3840, 2160, "xxlarge")]
    [InlineData(1000, 700, "medium")]
    public void FindLargestImageWithinBounds(int boundingWidth, int boundingHeight, string expectedSize)
    {
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), new HttpClient());
        var boundingSize = new Size(boundingWidth, boundingHeight);

        var actual = piwigoService.FindLargestImageWithinBounds(derivatives, boundingSize);

        Assert.Equal(derivatives[expectedSize].Url, actual);
    }

    [Fact]
    public void FindLargestImageWithinBounds_NoneFound_ThrowsException()
    {
        var logger = new Mock<ILogger>();
        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), new HttpClient());

        Assert.Throws<Exception>(() => piwigoService.FindLargestImageWithinBounds(derivatives, new Size(42, 42)));
    }

    [Fact]
    public async Task ImageStreamFromUrl_ThrowsException_ContainsPhotoNameInMessage()
    {
        const string imageName = "Rockabye, Sheriff, just you relax.gif";
        var httpClient = new HttpClient();
        var logger = new Mock<ILogger>();

        var piwigoService = new PiwigoService(logger.Object, CreateMockSettingsService(), httpClient);

        var e = await Assert.ThrowsAsync<Exception>(async () => await piwigoService.ImageStreamFromUrl(string.Empty, imageName));
        Assert.Contains(imageName, e.Message);
    }

    private HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string json)
    {
        var messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        messageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(json)
            });

        return new HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("https://www.example.com/")
        };
    }

    private ISettingsService CreateMockSettingsService()
    {
        var service = new Mock<ISettingsService>();
        service.Setup(x => x.Get(It.IsAny<SettingKey>())).Returns("https://www.example.com/");
        return service.Object;
    }
}
