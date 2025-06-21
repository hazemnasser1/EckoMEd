using Newtonsoft.Json;

namespace Echomedproject.PL.ViewModels
{
    public class Center
    {
        [JsonProperty("lat")]
        public double? Lat { get; set; }

        [JsonProperty("lon")]
        public double? Lon { get; set; }
    }
}
