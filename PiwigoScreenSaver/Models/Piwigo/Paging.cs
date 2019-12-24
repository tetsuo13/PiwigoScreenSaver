using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo
{
    public class Paging
    {
        public int Page { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }

        public int Count { get; set; }

        [JsonPropertyName("total_count")]
        public string TotalCount { get; set; }
    }
}
