using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo;

public record BaseResult<T>
{
    [JsonPropertyName("stat")]
    public StatusCode Status { get; init; }

    [JsonPropertyName("err")]
    public int? ErrorCode { get; init; }

    [JsonPropertyName("message")]
    public string ErrorMessage { get; init; }

    public T Result { get; init; }
}
