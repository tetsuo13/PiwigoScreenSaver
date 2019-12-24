using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo
{
    public class BaseResult<T>
    {
        [JsonPropertyName("stat")]
        public StatusCode Status { get; set; }

        [JsonPropertyName("err")]
        public int? ErrorCode { get; set; }

        [JsonPropertyName("message")]
        public string ErrorMessage { get; set; }

        public T Result { get; set; }
    }
}
