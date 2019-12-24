using System;
using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Models.Piwigo
{
    public class SessionStatus
    {
        public string Username { get; set; }
        public string Status { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }

        [JsonPropertyName("pwg_token")]
        public string Token { get; set; }

        public string Charset { get; set; }

        [JsonPropertyName("current_datetime")]
        public DateTime CurrentDateTime { get; set; }

        public string Version { get; set; }

        [JsonPropertyName("available_sizes")]
        public string[] AvailableSizes { get; set; }
    }
}
