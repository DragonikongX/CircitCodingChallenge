namespace WeatherApi.Models;

public class WeatherResponse
{
    public LocationInfo Location { get; set; } = new();
    public CurrentWeatherInfo CurrentWeather { get; set; } = new();
    public TimezoneInfo Timezone { get; set; } = new();
    public AstronomyInfo Astronomy { get; set; } = new();
}

public class LocationInfo
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string LocalTime { get; set; } = string.Empty;
}

public class CurrentWeatherInfo
{
    public double TempC { get; set; }
    public double TempF { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string ConditionIcon { get; set; } = string.Empty;
    public int Humidity { get; set; }
    public double WindKph { get; set; }
    public double FeelsLikeC { get; set; }
    public double UV { get; set; }
}

public class TimezoneInfo
{
    public string TimezoneName { get; set; } = string.Empty;
    public string UtcOffset { get; set; } = string.Empty;
}

public class AstronomyInfo
{
    public string Sunrise { get; set; } = string.Empty;
    public string Sunset { get; set; } = string.Empty;
    public string Moonrise { get; set; } = string.Empty;
    public string Moonset { get; set; } = string.Empty;
    public string MoonPhase { get; set; } = string.Empty;
}
