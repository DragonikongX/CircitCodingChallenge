using Microsoft.AspNetCore.Mvc;
using WeatherApi.Exceptions;
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
        try
        {
            var result = await _weatherService.GetWeatherDataAsync(city);
            return Ok(result);
        }
        catch (CityNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet("cities")]
    public IActionResult GetCities()
    {
        var cities = _weatherService.GetAllowedCities();
        return Ok(cities);
    }
}
