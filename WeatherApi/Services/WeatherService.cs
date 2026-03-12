using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeatherApi.Configuration;
using WeatherApi.Exceptions;
using WeatherApi.Models;
using WeatherApi.Models.External;
using WeatherApi.Repositories;

namespace WeatherApi.Services;

public class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly RapidApiSettings _settings;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(
        IWeatherRepository repository,
        IMemoryCache cache,
        IOptions<RapidApiSettings> settings,
        ILogger<WeatherService> logger)
    {
        _repository = repository;
        _cache = cache;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<WeatherResponse> GetWeatherDataAsync(string city)
    {
        var normalizedCity = _settings.AllowedCities
            .FirstOrDefault(c => c.Equals(city, StringComparison.OrdinalIgnoreCase))
            ?? throw new CityNotFoundException(city);

        var cacheKey = $"weather_{normalizedCity.ToLowerInvariant()}";

        if (_cache.TryGetValue(cacheKey, out WeatherResponse? cached) && cached is not null)
        {
            _logger.LogInformation("Cache hit for city: {City}", normalizedCity);
            return cached;
        }

        _logger.LogInformation("Cache miss for city: {City}. Fetching from RapidAPI...", normalizedCity);

        var currentTask = _repository.GetCurrentWeatherAsync(normalizedCity);
        var timezoneTask = _repository.GetTimezoneAsync(normalizedCity);
        var astronomyTask = _repository.GetAstronomyAsync(normalizedCity);

        await Task.WhenAll(currentTask, timezoneTask, astronomyTask);

        var currentWeather = await currentTask;
        var timezone = await timezoneTask;
        var astronomy = await astronomyTask;

        var response = MapToWeatherResponse(currentWeather, timezone, astronomy);

        var cacheOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(_settings.CacheTtlMinutes)
        };
        _cache.Set(cacheKey, response, cacheOptions);

        return response;
    }

    public IReadOnlyList<string> GetAllowedCities()
    {
        return _settings.AllowedCities.AsReadOnly();
    }

    private static WeatherResponse MapToWeatherResponse(
        ExternalCurrentWeatherDto current,
        ExternalTimezoneDto timezone,
        ExternalAstronomyDto astronomy)
    {
        return new WeatherResponse
        {
            Location = new LocationInfo
            {
                Name = current.Location?.Name ?? string.Empty,
                Country = current.Location?.Country ?? string.Empty,
                Latitude = current.Location?.Lat ?? 0,
                Longitude = current.Location?.Lon ?? 0,
                LocalTime = current.Location?.Localtime ?? string.Empty
            },
            CurrentWeather = new CurrentWeatherInfo
            {
                TempC = current.Current?.TempC ?? 0,
                TempF = current.Current?.TempF ?? 0,
                Condition = current.Current?.Condition?.Text ?? string.Empty,
                ConditionIcon = current.Current?.Condition?.Icon ?? string.Empty,
                Humidity = current.Current?.Humidity ?? 0,
                WindKph = current.Current?.WindKph ?? 0,
                FeelsLikeC = current.Current?.FeelslikeC ?? 0,
                UV = current.Current?.Uv ?? 0
            },
            Timezone = new TimezoneInfo
            {
                TimezoneName = timezone.Location?.TzId ?? string.Empty,
                UtcOffset = timezone.Location?.UtcOffset ?? string.Empty
            },
            Astronomy = new AstronomyInfo
            {
                Sunrise = astronomy.Astronomy?.Astro?.Sunrise ?? string.Empty,
                Sunset = astronomy.Astronomy?.Astro?.Sunset ?? string.Empty,
                Moonrise = astronomy.Astronomy?.Astro?.Moonrise ?? string.Empty,
                Moonset = astronomy.Astronomy?.Astro?.Moonset ?? string.Empty,
                MoonPhase = astronomy.Astronomy?.Astro?.MoonPhase ?? string.Empty
            }
        };
    }
}
