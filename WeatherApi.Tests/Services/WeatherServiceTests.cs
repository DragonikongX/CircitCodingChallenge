using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using WeatherApi.Configuration;
using WeatherApi.Exceptions;
using WeatherApi.Models.External;
using WeatherApi.Repositories;
using WeatherApi.Services;

namespace WeatherApi.Tests.Services;

public class WeatherServiceTests
{
    private readonly Mock<IWeatherRepository> _repositoryMock;
    private readonly IMemoryCache _cache;
    private readonly IOptions<RapidApiSettings> _options;
    private readonly WeatherService _sut;

    public WeatherServiceTests()
    {
        _repositoryMock = new Mock<IWeatherRepository>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _options = Options.Create(new RapidApiSettings
        {
            AllowedCities = new List<string> { "Cracow", "Warsaw", "Dublin" },
            CacheTtlMinutes = 10
        });
        var logger = new Mock<ILogger<WeatherService>>();

        _sut = new WeatherService(_repositoryMock.Object, _cache, _options, logger.Object);
    }

    [Fact]
    public async Task GetWeatherDataAsync_ValidCity_ReturnsMappedResponse()
    {
        SetupRepositoryWithTestData("Cracow");

        var result = await _sut.GetWeatherDataAsync("Cracow");

        Assert.Equal("Cracow", result.Location.Name);
        Assert.Equal("Poland", result.Location.Country);
        Assert.Equal(50.06, result.Location.Latitude);
        Assert.Equal(19.94, result.Location.Longitude);
        Assert.Equal(15.0, result.CurrentWeather.TempC);
        Assert.Equal(59.0, result.CurrentWeather.TempF);
        Assert.Equal("Partly cloudy", result.CurrentWeather.Condition);
        Assert.Equal(65, result.CurrentWeather.Humidity);
        Assert.Equal(12.5, result.CurrentWeather.WindKph);
        Assert.Equal(13.0, result.CurrentWeather.FeelsLikeC);
        Assert.Equal(4.0, result.CurrentWeather.UV);
        Assert.Equal("Europe/Warsaw", result.Timezone.TimezoneName);
        Assert.Equal("1.0", result.Timezone.UtcOffset);
        Assert.Equal("06:30 AM", result.Astronomy.Sunrise);
        Assert.Equal("07:45 PM", result.Astronomy.Sunset);
        Assert.Equal("Waxing Crescent", result.Astronomy.MoonPhase);
    }

    [Fact]
    public async Task GetWeatherDataAsync_CaseInsensitiveCity_ReturnsData()
    {
        SetupRepositoryWithTestData("Dublin");

        var result = await _sut.GetWeatherDataAsync("dublin");

        Assert.NotNull(result);
        _repositoryMock.Verify(r => r.GetCurrentWeatherAsync("Dublin"), Times.Once);
    }

    [Fact]
    public async Task GetWeatherDataAsync_InvalidCity_ThrowsCityNotFoundException()
    {
        await Assert.ThrowsAsync<CityNotFoundException>(
            () => _sut.GetWeatherDataAsync("Berlin"));
    }

    [Fact]
    public async Task GetWeatherDataAsync_RepositoryThrows_PropagatesException()
    {
        _repositoryMock
            .Setup(r => r.GetCurrentWeatherAsync(It.IsAny<string>()))
            .ThrowsAsync(new ExternalApiException(System.Net.HttpStatusCode.ServiceUnavailable, "API down"));
        _repositoryMock
            .Setup(r => r.GetTimezoneAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateTestTimezone());
        _repositoryMock
            .Setup(r => r.GetAstronomyAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateTestAstronomy());

        await Assert.ThrowsAsync<ExternalApiException>(
            () => _sut.GetWeatherDataAsync("Cracow"));
    }

    [Fact]
    public async Task GetWeatherDataAsync_SecondCall_ReturnsCachedResult()
    {
        SetupRepositoryWithTestData("Warsaw");

        await _sut.GetWeatherDataAsync("Warsaw");
        await _sut.GetWeatherDataAsync("Warsaw");

        _repositoryMock.Verify(r => r.GetCurrentWeatherAsync("Warsaw"), Times.Once);
        _repositoryMock.Verify(r => r.GetTimezoneAsync("Warsaw"), Times.Once);
        _repositoryMock.Verify(r => r.GetAstronomyAsync("Warsaw"), Times.Once);
    }

    [Fact]
    public void GetAllowedCities_ReturnsConfiguredCities()
    {
        var cities = _sut.GetAllowedCities();

        Assert.Equal(3, cities.Count);
        Assert.Contains("Cracow", cities);
        Assert.Contains("Warsaw", cities);
        Assert.Contains("Dublin", cities);
    }

    [Fact]
    public async Task GetWeatherDataAsync_CallsAllThreeEndpointsInParallel()
    {
        SetupRepositoryWithTestData("Dublin");

        await _sut.GetWeatherDataAsync("Dublin");

        _repositoryMock.Verify(r => r.GetCurrentWeatherAsync("Dublin"), Times.Once);
        _repositoryMock.Verify(r => r.GetTimezoneAsync("Dublin"), Times.Once);
        _repositoryMock.Verify(r => r.GetAstronomyAsync("Dublin"), Times.Once);
    }

    private void SetupRepositoryWithTestData(string city)
    {
        _repositoryMock
            .Setup(r => r.GetCurrentWeatherAsync(city))
            .ReturnsAsync(CreateTestCurrentWeather(city));
        _repositoryMock
            .Setup(r => r.GetTimezoneAsync(city))
            .ReturnsAsync(CreateTestTimezone());
        _repositoryMock
            .Setup(r => r.GetAstronomyAsync(city))
            .ReturnsAsync(CreateTestAstronomy());
    }

    private static ExternalCurrentWeatherDto CreateTestCurrentWeather(string city) => new()
    {
        Location = new ExternalLocation
        {
            Name = city,
            Country = "Poland",
            Lat = 50.06,
            Lon = 19.94,
            TzId = "Europe/Warsaw",
            Localtime = "2026-03-12 14:30"
        },
        Current = new ExternalCurrent
        {
            TempC = 15.0,
            TempF = 59.0,
            Condition = new ExternalCondition { Text = "Partly cloudy", Icon = "//cdn.weatherapi.com/weather/64x64/day/116.png" },
            WindKph = 12.5,
            Humidity = 65,
            FeelslikeC = 13.0,
            Uv = 4.0
        }
    };

    private static ExternalTimezoneDto CreateTestTimezone() => new()
    {
        Location = new ExternalTimezoneLocation
        {
            Name = "Cracow",
            TzId = "Europe/Warsaw",
            Localtime = "2026-03-12 14:30",
            UtcOffset = "1.0"
        }
    };

    private static ExternalAstronomyDto CreateTestAstronomy() => new()
    {
        Location = new ExternalAstronomyLocation
        {
            Name = "Cracow",
            Country = "Poland"
        },
        Astronomy = new ExternalAstronomyData
        {
            Astro = new ExternalAstro
            {
                Sunrise = "06:30 AM",
                Sunset = "07:45 PM",
                Moonrise = "10:00 AM",
                Moonset = "01:30 AM",
                MoonPhase = "Waxing Crescent"
            }
        }
    };
}
