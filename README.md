# CircitCodingChallenge — Weather Dashboard

A web application that displays weather, timezone, and astronomy data for a selected city. Backend in C# (.NET 8), frontend in React with Vite.

---

## Installation on a Clean Machine

The following steps assume you start with a fresh system and need to install everything from scratch.

### 1. Install .NET 8 SDK

The backend requires .NET 8.

**Windows:**
- Download the installer from https://dotnet.microsoft.com/download/dotnet/8.0
- Run the SDK installer (not just the Runtime)
- Restart the terminal and verify:
  ```
  dotnet --version
  ```
  Expected output: `8.0.x` or similar

**macOS:**
- Using Homebrew (install from https://brew.sh if needed):
  ```
  brew install dotnet@8
  ```
- Or download from https://dotnet.microsoft.com/download/dotnet/8.0
- Verify: `dotnet --version`

**Linux (Ubuntu/Debian):**
  ```
  wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  rm packages-microsoft-prod.deb
  sudo apt-get update
  sudo apt-get install -y dotnet-sdk-8.0
  dotnet --version
  ```

### 2. Install Node.js 18+ (with npm)

The frontend requires Node.js 18 or newer. npm is included with Node.js.

**Windows:**
- Download the LTS installer from https://nodejs.org
- Run the installer and follow the wizard (ensure "Add to PATH" is checked)
- Restart the terminal and verify:
  ```
  node --version
  npm --version
  ```
  Node should report v18.x or higher

**macOS:**
- Using Homebrew:
  ```
  brew install node
  ```
- Or download from https://nodejs.org
- Verify: `node --version` and `npm --version`

**Linux (Ubuntu/Debian):**
  ```
  curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
  sudo apt-get install -y nodejs
  node --version
  npm --version
  ```

### 3. Install Git (if not present)

**Windows:** Download from https://git-scm.com/download/win  
**macOS:** `xcode-select --install` or `brew install git`  
**Linux:** `sudo apt-get install git` (Ubuntu/Debian)

### 4. Clone the Repository

  ```
  git clone https://github.com/YOUR_USERNAME/CircitCodingChallenge.git
  cd CircitCodingChallenge
  ```

Replace `YOUR_USERNAME` with the actual GitHub username or organization.

---

## Running the Application

You need two terminals: one for the backend, one for the frontend.

### Terminal 1 — Backend (WeatherApi)

  ```
  cd WeatherApi
  dotnet restore
  dotnet run
  ```

The API starts on **http://localhost:5000**. You should see output similar to:
  ```
  info: Microsoft.Hosting.Lifetime[14]
        Now listening on: http://localhost:5000
  ```

Leave this terminal running.

### Terminal 2 — Frontend (weather-client)

  ```
  cd weather-client
  npm install
  npm run dev
  ```

The React app starts on **http://localhost:5173**. You should see:
  ```
  VITE v8.x.x  ready in xxx ms
  ➜  Local:   http://localhost:5173/
  ```

### Open the Application

In your browser, go to **http://localhost:5173**. Select a city (Cracow, Warsaw, or Dublin) to view weather, timezone, and astronomy data.

---

## Configuration

All configuration is in `WeatherApi/appsettings.json`:

- **RapidApi.ApiKey** — API key for WeatherAPI via RapidAPI (required for live data)
- **RapidApi.AllowedCities** — List of cities available in the selector
- **AllowedOrigins** — CORS origins (default: `http://localhost:5173` for Vite dev server)
- **CacheTtlMinutes** — How long weather data is cached (default: 10)

To add a new city, append its name to the `AllowedCities` array. The city must be recognized by the WeatherAPI service.

---

## Project Structure

- `WeatherApi/` — .NET 8 Web API (Controller, Service, Repository, Models)
- `weather-client/` — React app with Vite, Bootstrap, Lucide icons

---

## Troubleshooting

**Backend fails to start**
- Ensure .NET 8 SDK is installed: `dotnet --version`
- Ensure port 5000 is not in use by another application

**Frontend fails to start**
- Ensure Node.js 18+ is installed: `node --version`
- Delete `node_modules` and run `npm install` again
- Ensure port 5173 is free

**"Failed to load cities" or "Failed to fetch weather data"**
- Ensure the backend is running on http://localhost:5000
- Check that the RapidAPI key in `appsettings.json` is valid
- Verify your internet connection (the backend calls RapidAPI)

**CORS errors in the browser**
- Ensure `AllowedOrigins` in `appsettings.json` includes `http://localhost:5173`
- Ensure you access the app via http://localhost:5173, not 127.0.0.1 or another port
