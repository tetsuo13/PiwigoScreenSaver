using PiwigoScreenSaver.Domain.JsonConverters;
using System.Text.Json;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain.JsonConverters
{
    public class NumberDuckTypeConverterTests
    {
        [Fact]
        public void Deserialize()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new NumberDuckTypeConverter());

            int actual = JsonSerializer.Deserialize<int>("1", options);
            Assert.Equal(1, actual);

            actual = JsonSerializer.Deserialize<int>(@"""1""", options);
            Assert.Equal(1, actual);
        }
    }
}
