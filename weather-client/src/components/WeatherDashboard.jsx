import { useState, useEffect } from 'react'
import { Wind, Droplets, Sun, Sunrise, Sunset, Moon, Clock, MapPin, ThermometerSun } from 'lucide-react'
import { getWeather } from '../services/apiClient.js'

export default function WeatherDashboard({ city }) {
  const [weatherData, setWeatherData] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  useEffect(() => {
    if (!city) return

    const fetchWeather = async () => {
      setLoading(true)
      setError(null)
      try {
        const data = await getWeather(city)
        setWeatherData(data)
      } catch (err) {
        const errMsg = err.response?.data
        setError(typeof errMsg === 'string' ? errMsg : errMsg?.detail || 'Failed to fetch weather data.')
      } finally {
        setLoading(false)
      }
    }

    fetchWeather()
  }, [city])

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center mt-5">
        <div className="spinner-border text-light" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    )
  }

  if (error) {
    return (
      <div className="alert alert-danger glass-card mt-4 text-center text-danger" role="alert">
        {error}
      </div>
    )
  }

  if (!weatherData) return null

  const { location, currentWeather, timezone, astronomy } = weatherData
  const conditionIconSrc = currentWeather.conditionIcon
    ? (currentWeather.conditionIcon.startsWith('http') ? currentWeather.conditionIcon : 'https:' + currentWeather.conditionIcon)
    : null

  return (
    <div className="container mt-4 mb-5">
      <div className="text-center mb-4">
        <h2 className="display-4 fw-light mb-0">
          <MapPin className="me-2 mb-2" size={32} />
          {location.name}, {location.country}
        </h2>
      </div>

      <div className="api-section mb-4">
        <h3 className="section-title mb-4">Current Weather</h3>
        <div className="glass-card section-card">
          <div className="row align-items-center">
            <div className="col-lg-4 text-center mb-4 mb-lg-0">
              {conditionIconSrc ? (
                <img src={conditionIconSrc} alt="Weather" style={{ width: '100px' }} />
              ) : (
                <Sun size={80} className="text-warning" />
              )}
              <div className="hero-temp mt-2">{currentWeather.tempC}°</div>
              <p className="fs-4 mb-0 text-capitalize">{currentWeather.condition}</p>
              <p className="fs-6 icon-muted">Feels like: {currentWeather.feelsLikeC}°C</p>
            </div>
            <div className="col-lg-8">
              <div className="row g-4">
                <div className="col-md-6">
                  <div className="d-flex align-items-center">
                    <Wind size={28} className="me-3 text-info" />
                    <div>
                      <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Wind</p>
                      <h4 className="mb-0">{currentWeather.windKph} <span className="fs-6">km/h</span></h4>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="d-flex align-items-center">
                    <Droplets size={28} className="me-3 text-primary" />
                    <div>
                      <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Humidity</p>
                      <h4 className="mb-0">{currentWeather.humidity}%</h4>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="d-flex align-items-center">
                    <ThermometerSun size={28} className="me-3 text-danger" />
                    <div>
                      <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>UV Index</p>
                      <h4 className="mb-0">{currentWeather.uv}</h4>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="d-flex align-items-center">
                    <MapPin size={28} className="me-3 text-success" />
                    <div>
                      <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Coordinates</p>
                      <h4 className="mb-0">{location.latitude.toFixed(2)}°N, {location.longitude.toFixed(2)}°E</h4>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="api-section mb-4">
        <h3 className="section-title mb-4">Timezone</h3>
        <div className="glass-card section-card">
          <div className="row g-4">
            <div className="col-md-4">
              <div className="d-flex align-items-center">
                <Clock size={32} className="me-3 text-success" />
                <div>
                  <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Local time</p>
                  <h4 className="mb-0">{location.localTime}</h4>
                </div>
              </div>
            </div>
            <div className="col-md-4">
              <div className="d-flex align-items-center">
                <Clock size={32} className="me-3 text-info" />
                <div>
                  <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Timezone</p>
                  <h4 className="mb-0">{timezone.timezoneName}</h4>
                </div>
              </div>
            </div>
            <div className="col-md-4">
              <div className="d-flex align-items-center">
                <Clock size={32} className="me-3 text-warning" />
                <div>
                  <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>UTC offset</p>
                  <h4 className="mb-0">{timezone.utcOffset}</h4>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="api-section mb-4">
        <h3 className="section-title mb-4">Astronomy</h3>
        <div className="glass-card section-card">
          <div className="row g-4">
            <div className="col-md-6 col-lg-4">
              <div className="d-flex align-items-center">
                <Sunrise size={32} className="me-3 text-warning" />
                <div>
                  <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Sunrise</p>
                  <h4 className="mb-0">{astronomy.sunrise}</h4>
                </div>
              </div>
            </div>
            <div className="col-md-6 col-lg-4">
              <div className="d-flex align-items-center">
                <Sunset size={32} className="me-3 text-warning" />
                <div>
                  <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Sunset</p>
                  <h4 className="mb-0">{astronomy.sunset}</h4>
                </div>
              </div>
            </div>
            <div className="col-md-6 col-lg-4">
              <div className="d-flex align-items-center">
                <Moon size={32} className="me-3 text-info" />
                <div>
                  <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Moonrise</p>
                  <h4 className="mb-0">{astronomy.moonrise}</h4>
                </div>
              </div>
            </div>
            <div className="col-md-6 col-lg-4">
              <div className="d-flex align-items-center">
                <Moon size={32} className="me-3 text-info" />
                <div>
                  <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Moonset</p>
                  <h4 className="mb-0">{astronomy.moonset}</h4>
                </div>
              </div>
            </div>
            <div className="col-md-6 col-lg-4">
              <div className="d-flex align-items-center">
                <Sun size={32} className="me-3 text-warning" />
                <div>
                  <p className="mb-0 text-uppercase" style={{ fontSize: '0.8rem', letterSpacing: '1px' }}>Moon phase</p>
                  <h4 className="mb-0">{astronomy.moonPhase}</h4>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
