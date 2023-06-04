using PiwigoScreenSaver.Domain.JsonConverters;
using System.Text.Json;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain.JsonConverters;

public class NumberDuckTypeConverterTests
{
    [Theory]
    [InlineData("1", 1)]
    [InlineData(@"""1""", 1)]
    public void Deserialize(string json, int expected)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new NumberDuckTypeConverter());

        int actual = JsonSerializer.Deserialize<int>(json, options);
        Assert.Equal(expected, actual);
    }
}
