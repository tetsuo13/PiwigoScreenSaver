using System;
using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo;

public record SessionStatus
{
    public string Username { get; init; } = default!;
    public string Status { get; init; } = default!;
    public string Theme { get; init; } = default!;
    public string Language { get; init; } = default!;

    [JsonPropertyName("pwg_token")]
    public string Token { get; init; } = default!;

    public string Charset { get; init; } = default!;

    [JsonPropertyName("current_datetime")]
    public DateTime CurrentDateTime { get; init; } = default!;

    public string Version { get; init; } = default!;

    [JsonPropertyName("available_sizes")]
    public string[] AvailableSizes { get; init; } = default!;
}
