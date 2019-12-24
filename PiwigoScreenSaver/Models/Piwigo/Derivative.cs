using PiwigoScreenSaver.Domain.JsonConverters;
using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo
{
    public class Derivative
    {
        public string Url { get; set; }

        [JsonConverter(typeof(NumberDuckTypeConverter))]
        public int Width { get; set; }

        [JsonConverter(typeof(NumberDuckTypeConverter))]
        public int Height { get; set; }
    }
}
