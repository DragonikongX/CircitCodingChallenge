using System.Text.Json;
using WeatherApi.Exceptions;
using WeatherApi.Models.External;

namespace WeatherApi.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public WeatherRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("RapidApi");
    }

    public async Task<ExternalCurrentWeatherDto> GetCurrentWeatherAsync(string city)
    {
        return await SendRequestAsync<ExternalCurrentWeatherDto>($"/current.json?q={Uri.EscapeDataString(city)}");
    }

    public async Task<ExternalTimezoneDto> GetTimezoneAsync(string city)
    {
        return await SendRequestAsync<ExternalTimezoneDto>($"/timezone.json?q={Uri.EscapeDataString(city)}");
    }

    public async Task<ExternalAstronomyDto> GetAstronomyAsync(string city)
    {
        return await SendRequestAsync<ExternalAstronomyDto>($"/astronomy.json?q={Uri.EscapeDataString(city)}");
    }

    private async Task<T> SendRequestAsync<T>(string endpoint) where T : class
    {
        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new ExternalApiException(
                response.StatusCode,
                $"RapidAPI returned {(int)response.StatusCode}: {errorBody}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, JsonOptions)
            ?? throw new ExternalApiException(
                response.StatusCode,
                $"Failed to deserialize response from {endpoint}");
    }
}
