using System.Net;

namespace WeatherApi.Exceptions;

public class ExternalApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ExternalApiException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
