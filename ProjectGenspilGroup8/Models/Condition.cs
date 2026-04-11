using System.Text.Json.Serialization;

namespace ProjectGenspilGroup8.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Condition
    {
        New,
        Good,
        Worn
    }
}