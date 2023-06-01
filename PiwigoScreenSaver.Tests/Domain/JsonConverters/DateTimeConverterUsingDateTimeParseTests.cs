using PiwigoScreenSaver.Domain.JsonConverters;
using System.Text.Json;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain.JsonConverters;

public class DateTimeConverterUsingDateTimeParseTests
{
    [Fact]
    public void Deserialize()
    {
        var expected = new DateTime(2019, 12, 15, 21, 34, 58);
        var json = @"""2019-12-15 21:34:58""";

        var options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeConverterUsingDateTimeParse());

        var actual = JsonSerializer.Deserialize<DateTime>(json, options);
        Assert.Equal(expected, actual);
    }
}
