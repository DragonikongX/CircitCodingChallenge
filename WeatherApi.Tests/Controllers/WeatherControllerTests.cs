using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApi.Controllers;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Tests.Controllers;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherService> _serviceMock;
    private readonly WeatherController _sut;

    public WeatherControllerTests()
    {
        _serviceMock = new Mock<IWeatherService>();
        _sut = new WeatherController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetWeather_ValidCity_ReturnsOkWithData()
    {
        var expected = new WeatherResponse
        {
            Location = new LocationInfo { Name = "Dublin", Country = "Ireland" },
            CurrentWeather = new CurrentWeatherInfo { TempC = 12.0, Condition = "Rainy" },
            Timezone = new TimezoneInfo { TimezoneName = "Europe/Dublin" },
            Astronomy = new AstronomyInfo { Sunrise = "07:00 AM" }
        };
        _serviceMock
            .Setup(s => s.GetWeatherDataAsync("Dublin"))
            .ReturnsAsync(expected);

        var result = await _sut.GetWeather("Dublin");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<WeatherResponse>(okResult.Value);
        Assert.Equal("Dublin", response.Location.Name);
        Assert.Equal(12.0, response.CurrentWeather.TempC);
    }

    [Fact]
    public void GetCities_ReturnsOkWithCityList()
    {
        var cities = new List<string> { "Cracow", "Warsaw", "Dublin" }.AsReadOnly();
        _serviceMock.Setup(s => s.GetAllowedCities()).Returns(cities);

        var result = _sut.GetCities();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCities = Assert.IsAssignableFrom<IReadOnlyList<string>>(okResult.Value);
        Assert.Equal(3, returnedCities.Count);
    }

    [Fact]
    public async Task GetWeather_ServiceThrows_ExceptionPropagates()
    {
        _serviceMock
            .Setup(s => s.GetWeatherDataAsync("Berlin"))
            .ThrowsAsync(new WeatherApi.Exceptions.CityNotFoundException("Berlin"));

        await Assert.ThrowsAsync<WeatherApi.Exceptions.CityNotFoundException>(
            () => _sut.GetWeather("Berlin"));
    }
}
