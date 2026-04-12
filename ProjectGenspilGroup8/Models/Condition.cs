using System.Text.Json.Serialization;

namespace ProjectGenspilGroup8.Models
{
    // Stored as string in JSON for readability and stability
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Condition
    {
        New,
        Good,
        Worn
    }
}