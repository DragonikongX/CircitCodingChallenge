namespace WeatherApi.Exceptions;

public class CityNotFoundException : Exception
{
    public CityNotFoundException(string city)
        : base($"City '{city}' is not in the list of allowed cities.")
    {
    }
}
