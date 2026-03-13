import { useState, useEffect } from 'react'
import { getCities } from '../services/apiClient.js'

export default function CitySelector({ selectedCity, onCitySelected }) {
  const [cities, setCities] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    getCities()
      .then(setCities)
      .catch(() => setError('Failed to load cities. Is the backend running?'))
      .finally(() => setLoading(false))
  }, [])

  return (
    <div className="glass-card mb-4">
      <div className="text-center">
        <h5 className="mb-3">Select a City</h5>
        {loading && <div className="text-muted">Loading cities...</div>}
        {error && <div className="text-danger">{error}</div>}
        {!loading && !error && (
          <div className="d-flex justify-content-center gap-3 flex-wrap">
            {cities.map((city) => (
              <button
                key={city}
                className={`btn btn-lg city-btn ${city === selectedCity ? 'btn-light text-primary' : 'btn-outline-light'}`}
                onClick={() => onCitySelected(city)}
              >
                {city}
              </button>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
