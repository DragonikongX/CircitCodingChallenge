# CircitCodingChallenge — Weather Dashboard

Aplikacja webowa pokazująca pogodę, strefę czasową i dane astronomiczne dla wybranego miasta. Backend w C# (.NET 8), frontend w React.

W zadaniu sugerowano VueJS, ale wybrałem React — czuję się w nim pewniej niż w Vue, co pozwoliło mi dostarczyć stabilniejsze rozwiązanie. Frontend nigdy nie wywołuje zewnętrznego API bezpośrednio; wszystkie zapytania idą przez backend, który pełni rolę proxy.

## Wymagania

- .NET 8 SDK
- Node.js 18+ (z npm)

## Uruchomienie

1. **Backend** — w jednym terminalu:
   ```
   cd WeatherApi
   dotnet run
   ```
   API startuje na http://localhost:5000

2. **Frontend** — w drugim terminalu:
   ```
   cd weather-client
   npm install
   npm run dev
   ```
   Aplikacja React startuje na http://localhost:5173 i proxy'uje zapytania `/api` do backendu.

3. Otwórz w przeglądarce **http://localhost:5173** i wybierz jedno z miast (Cracow, Warsaw, Dublin).

## Konfiguracja

Lista dozwolonych miast i klucz API są w `WeatherApi/appsettings.json`. Aby dodać nowe miasto, wystarczy dopisać je do tablicy `AllowedCities` — bez zmian w kodzie.

## Struktura projektu

- `WeatherApi/` — API .NET (kontroler, serwis, repozytorium, modele)
- `weather-client/` — aplikacja React z Vite i Bootstrap
