using System;
using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo;

public record SessionStatus
{
    public string Username { get; init; }
    public string Status { get; init; }
    public string Theme { get; init; }
    public string Language { get; init; }

    [JsonPropertyName("pwg_token")]
    public string Token { get; init; }

    public string Charset { get; init; }

    [JsonPropertyName("current_datetime")]
    public DateTime CurrentDateTime { get; init; }

    public string Version { get; init; }

    [JsonPropertyName("available_sizes")]
    public string[] AvailableSizes { get; init; }
}
