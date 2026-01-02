import axios from 'axios';

// Configuração da API
const API_URL = process.env.REACT_APP_API_URL || 'https://household-budget-api.onrender.com/';
const API_KEY = process.env.REACT_APP_API_KEY || 'e24dd2210803b4737a9bd9e3163a4ca807b63201c3bc32b68fb122ca52efff36';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
    'x-api-key': API_KEY
  }
});

// Interceptor para adicionar token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor para tratar erros
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default api;