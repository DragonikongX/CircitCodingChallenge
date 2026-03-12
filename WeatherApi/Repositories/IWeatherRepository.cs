using WeatherApi.Models.External;

namespace WeatherApi.Repositories;

public interface IWeatherRepository
{
    Task<ExternalCurrentWeatherDto> GetCurrentWeatherAsync(string city);
    Task<ExternalTimezoneDto> GetTimezoneAsync(string city);
    Task<ExternalAstronomyDto> GetAstronomyAsync(string city);
}
