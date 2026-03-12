using System.Text.Json.Serialization;

namespace WeatherApi.Models.External;

public class ExternalCurrentWeatherDto
{
    [JsonPropertyName("location")]
    public ExternalLocation? Location { get; set; }

    [JsonPropertyName("current")]
    public ExternalCurrent? Current { get; set; }
}

public class ExternalLocation
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("tz_id")]
    public string TzId { get; set; } = string.Empty;

    [JsonPropertyName("localtime")]
    public string Localtime { get; set; } = string.Empty;
}

public class ExternalCurrent
{
    [JsonPropertyName("temp_c")]
    public double TempC { get; set; }

    [JsonPropertyName("temp_f")]
    public double TempF { get; set; }

    [JsonPropertyName("condition")]
    public ExternalCondition? Condition { get; set; }

    [JsonPropertyName("wind_kph")]
    public double WindKph { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("feelslike_c")]
    public double FeelslikeC { get; set; }

    [JsonPropertyName("uv")]
    public double Uv { get; set; }
}

public class ExternalCondition
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;
}
