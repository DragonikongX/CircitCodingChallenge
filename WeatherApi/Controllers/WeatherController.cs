using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeather(string city)
    {
        var result = await _weatherService.GetWeatherDataAsync(city);
        return Ok(result);
    }

    [HttpGet("cities")]
    public IActionResult GetCities()
    {
        var cities = _weatherService.GetAllowedCities();
        return Ok(cities);
    }
}
