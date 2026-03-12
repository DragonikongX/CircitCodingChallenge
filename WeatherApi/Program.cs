using Microsoft.Extensions.Http.Resilience;
using Polly;
using WeatherApi.Configuration;
using WeatherApi.Middleware;
using WeatherApi.Repositories;
using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

var rapidApiSettings = builder.Configuration
    .GetSection(RapidApiSettings.SectionName)
    .Get<RapidApiSettings>()!;

builder.Services.Configure<RapidApiSettings>(
    builder.Configuration.GetSection(RapidApiSettings.SectionName));

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("RapidApi", client =>
{
    client.BaseAddress = new Uri(rapidApiSettings.BaseUrl);
    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", rapidApiSettings.ApiKey);
    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", rapidApiSettings.ApiHost);
})
.AddStandardResilienceHandler(options =>
{
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromMilliseconds(500);
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors("AllowFrontendDev");
app.MapControllers();

app.Run();

public partial class Program { }
