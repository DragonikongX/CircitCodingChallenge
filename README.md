# CircitCodingChallenge — Weather Dashboard

A web application that displays weather, timezone, and astronomy data for a selected city. Backend in C# (.NET 8), frontend in React.

The task suggested VueJS, but I chose React — I feel more confident in it than in Vue, which allowed me to deliver a more stable solution. The frontend never calls the external API directly; all requests go through the backend, which acts as a proxy.

## Prerequisites

- .NET 8 SDK
- Node.js 18+ (with npm)

## Getting Started

1. **Backend** — in one terminal:
   ```
   cd WeatherApi
   dotnet run
   ```
   The API starts on http://localhost:5000

2. **Frontend** — in a second terminal:
   ```
   cd weather-client
   npm install
   npm run dev
   ```
   The React app starts on http://localhost:5173 and proxies `/api` requests to the backend.

3. Open **http://localhost:5173** in your browser and select a city (Cracow, Warsaw, Dublin).

## Configuration

The list of allowed cities and the API key are in `WeatherApi/appsettings.json`. To add a new city, append it to the `AllowedCities` array — no code changes required.

**Note:** The RapidAPI key is stored explicitly in `appsettings.json` to meet the task requirement that the project must be runnable immediately after cloning from GitHub, without additional setup. In a production environment, this key would be stored securely using .NET User Secrets or as an environment variable.

## Project Structure

- `WeatherApi/` — .NET API (controller, service, repository, models)
- `weather-client/` — React app with Vite and Bootstrap
