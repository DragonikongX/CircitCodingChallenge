# CircitCodingChallenge — Weather Dashboard

A full-stack web application that displays weather, timezone, and astronomy data for selected cities. Built with a **C# .NET 8 Web API** backend and a **React 19** frontend.

## Architecture

```
React (Frontend)  -->  .NET 8 Web API (Proxy)  -->  WeatherAPI (RapidAPI)
```

The frontend never calls the external API directly. The .NET backend acts as a proxy, aggregating data from three RapidAPI endpoints (`current.json`, `timezone.json`, `astronomy.json`) into a single unified response.

### Backend Design Patterns

- **Repository-Service Pattern** — `WeatherRepository` handles HTTP communication with RapidAPI; `WeatherService` handles business logic, caching, and DTO mapping.
- **Strongly-Typed External DTOs** — RapidAPI responses are deserialized into dedicated C# classes (`ExternalCurrentWeatherDto`, `ExternalTimezoneDto`, `ExternalAstronomyDto`), then mapped to a clean public `WeatherResponse` DTO.
- **Open/Closed Principle** — The list of allowed cities is stored in `appsettings.json`, not hardcoded. Add new cities without recompilation.
- **IMemoryCache** — Weather data is cached for 10 minutes (configurable) to reduce API calls.
- **Polly Resilience** — HTTP calls use `Microsoft.Extensions.Http.Resilience` with retry (3x exponential backoff), circuit breaker, and per-attempt timeouts.
- **Global Exception Handling** — `IExceptionHandler` (new in .NET 8) catches all unhandled exceptions and formats them as RFC 7807 Problem Details responses.

### API Endpoints

| Method | URL                      | Description                        |
|--------|--------------------------|------------------------------------|
| GET    | `/api/weather/cities`    | Returns the list of allowed cities |
| GET    | `/api/weather/{city}`    | Returns unified weather data       |

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/) (with npm)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/<your-username>/CircitCodingChallenge.git
cd CircitCodingChallenge
```

### 2. Run the Backend

```bash
cd WeatherApi
dotnet run
```

The API will start on `http://localhost:5000`.

### 3. Run the Frontend

In a separate terminal:

```bash
cd weather-client
npm install
npm run dev
```

The React dev server starts on `http://localhost:5173` and proxies `/api` requests to the backend.

### 4. Open the App

Navigate to **http://localhost:5173** in your browser. Select a city to see its weather, timezone, and astronomy data.

## Project Structure

```
CircitCodingChallenge/
├── WeatherApi/                       # .NET 8 Web API
│   ├── Controllers/                  # WeatherController (REST endpoints)
│   ├── Services/                     # IWeatherService / WeatherService (business logic)
│   ├── Repositories/                 # IWeatherRepository / WeatherRepository (HTTP calls)
│   ├── Models/                       # Public DTOs (WeatherResponse)
│   │   └── External/                 # External DTOs (mirror RapidAPI JSON)
│   ├── Configuration/                # RapidApiSettings (strongly-typed config)
│   ├── Exceptions/                   # CityNotFoundException, ExternalApiException
│   ├── Middleware/                    # GlobalExceptionHandler (IExceptionHandler)
│   ├── Program.cs                    # DI, Polly, CORS, middleware pipeline
│   └── appsettings.json              # API keys, allowed cities, cache TTL
├── weather-client/                   # React + Vite + Bootstrap
│   ├── src/
│   │   ├── components/               # CitySelector.jsx, WeatherDashboard.jsx
│   │   ├── services/                 # apiClient.js (Axios)
│   │   └── App.jsx                   # Root component
│   └── vite.config.js                # Dev proxy to backend
└── CircitCodingChallenge.sln         # .NET solution file
```

## Configuration

All external API configuration is in `WeatherApi/appsettings.json`:

```json
{
  "RapidApi": {
    "BaseUrl": "https://weatherapi-com.p.rapidapi.com",
    "ApiKey": "<your-key>",
    "ApiHost": "weatherapi-com.p.rapidapi.com",
    "AllowedCities": ["Cracow", "Warsaw", "Dublin"],
    "CacheTtlMinutes": 10
  }
}
```

To add a new city, simply append it to the `AllowedCities` array — no code changes required.

## Tech Stack

| Layer    | Technology                                      |
|----------|-------------------------------------------------|
| Backend  | .NET 8, ASP.NET Core Web API, Polly, IMemoryCache |
| Frontend | React 19, Vite, Bootstrap 5, Axios              |
| External | WeatherAPI via RapidAPI                          |
