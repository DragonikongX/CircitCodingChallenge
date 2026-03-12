using System.Text.Json.Serialization;

namespace WeatherApi.Models.External;

public class ExternalTimezoneDto
{
    [JsonPropertyName("location")]
    public ExternalTimezoneLocation? Location { get; set; }
}

public class ExternalTimezoneLocation
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("tz_id")]
    public string TzId { get; set; } = string.Empty;

    [JsonPropertyName("localtime")]
    public string Localtime { get; set; } = string.Empty;

    [JsonPropertyName("utc_offset")]
    public string UtcOffset { get; set; } = string.Empty;
}
