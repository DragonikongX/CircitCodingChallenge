import axios from 'axios'

const apiClient = axios.create({
  baseURL: '/api',
  timeout: 15000,
  headers: {
    'Content-Type': 'application/json'
  }
})

export async function getCities() {
  const response = await apiClient.get('/weather/cities')
  return response.data
}

export async function getWeather(city) {
  const response = await apiClient.get(`/weather/${encodeURIComponent(city)}`)
  return response.data
}
