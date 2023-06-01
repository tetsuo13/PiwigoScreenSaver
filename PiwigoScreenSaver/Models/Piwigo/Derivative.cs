using PiwigoScreenSaver.Domain.JsonConverters;
using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo
{
    public record Derivative
    {
        public string Url { get; init; }

        [JsonConverter(typeof(NumberDuckTypeConverter))]
        public int Width { get; init; }

        [JsonConverter(typeof(NumberDuckTypeConverter))]
        public int Height { get; init; }
    }
}
