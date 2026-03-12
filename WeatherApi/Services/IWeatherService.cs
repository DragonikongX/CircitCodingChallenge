using WeatherApi.Models;

namespace WeatherApi.Services;

public interface IWeatherService
{
    Task<WeatherResponse> GetWeatherDataAsync(string city);
    IReadOnlyList<string> GetAllowedCities();
}
