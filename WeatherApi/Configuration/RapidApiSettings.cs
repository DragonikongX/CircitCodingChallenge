namespace WeatherApi.Configuration;

public class RapidApiSettings
{
    public const string SectionName = "RapidApi";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiHost { get; set; } = string.Empty;
    public List<string> AllowedCities { get; set; } = new();
    public int CacheTtlMinutes { get; set; } = 10;
}
