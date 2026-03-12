using System.Text.Json.Serialization;

namespace WeatherApi.Models.External;

public class ExternalAstronomyDto
{
    [JsonPropertyName("location")]
    public ExternalAstronomyLocation? Location { get; set; }

    [JsonPropertyName("astronomy")]
    public ExternalAstronomyData? Astronomy { get; set; }
}

public class ExternalAstronomyLocation
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
}

public class ExternalAstronomyData
{
    [JsonPropertyName("astro")]
    public ExternalAstro? Astro { get; set; }
}

public class ExternalAstro
{
    [JsonPropertyName("sunrise")]
    public string Sunrise { get; set; } = string.Empty;

    [JsonPropertyName("sunset")]
    public string Sunset { get; set; } = string.Empty;

    [JsonPropertyName("moonrise")]
    public string Moonrise { get; set; } = string.Empty;

    [JsonPropertyName("moonset")]
    public string Moonset { get; set; } = string.Empty;

    [JsonPropertyName("moon_phase")]
    public string MoonPhase { get; set; } = string.Empty;
}
