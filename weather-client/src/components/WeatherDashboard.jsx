import { useState, useEffect } from 'react'
import { getWeather } from '../services/apiClient.js'

export default function WeatherDashboard({ city }) {
  const [weather, setWeather] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  useEffect(() => {
    if (!city) return
    setLoading(true)
    setError(null)
    setWeather(null)

    getWeather(city)
      .then(setWeather)
      .catch((e) => {
        const errMsg = e.response?.data
        setError(typeof errMsg === 'string' ? errMsg : errMsg?.detail || 'Failed to fetch weather data. Please try again.')
      })
      .finally(() => setLoading(false))
  }, [city])

  if (loading) {
    return (
      <div className="text-center py-5">
        <div className="spinner-border text-primary" role="status" style={{ width: '3rem', height: '3rem' }}>
          <span className="visually-hidden">Loading...</span>
        </div>
        <p className="mt-3 text-muted">
          Fetching weather data for <strong>{city}</strong>...
        </p>
      </div>
    )
  }

  if (error) {
    return (
      <div className="alert alert-danger shadow-sm" role="alert">
        <h5 className="alert-heading">Something went wrong</h5>
        <p className="mb-0">{error}</p>
      </div>
    )
  }

  if (!weather) return null

  return (
    <div className="row g-4">
      <div className="col-md-6 col-lg-3">
        <div className="card h-100 shadow-sm border-0">
          <div className="card-body text-center">
            <div className="display-6 mb-2">📍</div>
            <h5 className="card-title">Location</h5>
            <p className="fs-4 fw-bold mb-1">{weather.location.name}</p>
            <p className="text-muted mb-1">{weather.location.country}</p>
            <small className="text-muted">
              {weather.location.latitude.toFixed(2)}°N, {weather.location.longitude.toFixed(2)}°E
            </small>
            <div className="mt-2">
              <span className="badge bg-secondary">{weather.location.localTime}</span>
            </div>
          </div>
        </div>
      </div>

      <div className="col-md-6 col-lg-3">
        <div className="card h-100 shadow-sm border-0">
          <div className="card-body text-center">
            {weather.currentWeather.conditionIcon && (
              <img
                src={'https:' + weather.currentWeather.conditionIcon}
                alt={weather.currentWeather.condition}
                width="64"
                height="64"
              />
            )}
            <h5 className="card-title">Current Weather</h5>
            <p className="fs-2 fw-bold mb-0">{weather.currentWeather.tempC}°C</p>
            <p className="text-muted mb-2">{weather.currentWeather.tempF}°F</p>
            <span className="badge bg-info text-dark mb-3">{weather.currentWeather.condition}</span>
            <div className="row text-start small mt-2">
              <div className="col-6 mb-1">💧 Humidity</div>
              <div className="col-6 mb-1 fw-bold">{weather.currentWeather.humidity}%</div>
              <div className="col-6 mb-1">💨 Wind</div>
              <div className="col-6 mb-1 fw-bold">{weather.currentWeather.windKph} km/h</div>
              <div className="col-6 mb-1">🌡️ Feels like</div>
              <div className="col-6 mb-1 fw-bold">{weather.currentWeather.feelsLikeC}°C</div>
              <div className="col-6 mb-1">☀️ UV Index</div>
              <div className="col-6 mb-1 fw-bold">{weather.currentWeather.uv}</div>
            </div>
          </div>
        </div>
      </div>

      <div className="col-md-6 col-lg-3">
        <div className="card h-100 shadow-sm border-0">
          <div className="card-body text-center">
            <div className="display-6 mb-2">🕐</div>
            <h5 className="card-title">Timezone</h5>
            <p className="fs-5 fw-bold mb-1">{weather.timezone.timezoneName}</p>
            <p className="text-muted">UTC {weather.timezone.utcOffset}</p>
          </div>
        </div>
      </div>

      <div className="col-md-6 col-lg-3">
        <div className="card h-100 shadow-sm border-0">
          <div className="card-body text-center">
            <div className="display-6 mb-2">🌙</div>
            <h5 className="card-title">Astronomy</h5>
            <div className="row text-start small">
              <div className="col-6 mb-2">🌅 Sunrise</div>
              <div className="col-6 mb-2 fw-bold">{weather.astronomy.sunrise}</div>
              <div className="col-6 mb-2">🌇 Sunset</div>
              <div className="col-6 mb-2 fw-bold">{weather.astronomy.sunset}</div>
              <div className="col-6 mb-2">🌕 Moonrise</div>
              <div className="col-6 mb-2 fw-bold">{weather.astronomy.moonrise}</div>
              <div className="col-6 mb-2">🌑 Moonset</div>
              <div className="col-6 mb-2 fw-bold">{weather.astronomy.moonset}</div>
            </div>
            <span className="badge bg-dark mt-2">{weather.astronomy.moonPhase}</span>
          </div>
        </div>
      </div>
    </div>
  )
}
