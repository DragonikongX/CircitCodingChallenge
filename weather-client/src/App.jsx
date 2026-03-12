import { useState } from 'react'
import CitySelector from './components/CitySelector.jsx'
import WeatherDashboard from './components/WeatherDashboard.jsx'

export default function App() {
  const [selectedCity, setSelectedCity] = useState(null)

  return (
    <div className="min-vh-100" style={{ background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)' }}>
      <div className="container py-5">
        <header className="text-center text-white mb-5">
          <h1 className="display-4 fw-bold">
            <span className="me-2">⛅</span>Weather Dashboard
          </h1>
          <p className="lead opacity-75">Select a city to view current weather, timezone, and astronomy data</p>
        </header>

        <CitySelector selectedCity={selectedCity} onCitySelected={setSelectedCity} />

        {selectedCity ? (
          <WeatherDashboard city={selectedCity} />
        ) : (
          <div className="text-center text-white-50 py-5">
            <p className="fs-5">Choose a city above to get started</p>
          </div>
        )}

        <footer className="text-center text-white-50 mt-5 pt-4 border-top border-white border-opacity-25">
          <small>Powered by WeatherAPI via RapidAPI &middot; Circit Coding Challenge</small>
        </footer>
      </div>
    </div>
  )
}
