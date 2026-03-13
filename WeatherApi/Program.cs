using WeatherApi.Configuration;
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
});

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? "http://localhost:5173";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev", policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowFrontendDev");
app.MapControllers();

app.Run();

public partial class Program { }
