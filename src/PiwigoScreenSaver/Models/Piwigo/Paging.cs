using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo;

public record Paging
{
    public int Page { get; init; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; init; }

    public int Count { get; init; }

    [JsonPropertyName("total_count")]
    public string TotalCount { get; init; } = default!;
}
