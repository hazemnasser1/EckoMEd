using Newtonsoft.Json;

namespace Echomedproject.PL.ViewModels
{
    public class OverpassElement
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }

        [JsonProperty("lat")]
        public double? Lat { get; set; }

        [JsonProperty("lon")]
        public double? Lon { get; set; }

        [JsonProperty("center")]
        public Center Center { get; set; }
    }
}
