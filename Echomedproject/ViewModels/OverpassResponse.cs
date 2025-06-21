using Newtonsoft.Json;

namespace Echomedproject.PL.ViewModels
{
    public class OverpassResponse
    {
        [JsonProperty("elements")]
        public List<OverpassElement> Elements { get; set; }
    }
}
