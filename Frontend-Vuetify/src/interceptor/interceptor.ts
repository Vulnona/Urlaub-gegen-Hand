import axios from 'axios';
import CryptoJS from 'crypto-js';
import router from '@/router';

// Create an Axios instance
const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_BASE_URL || 'http://localhost:8081/api/',
});

// Function to decrypt token using CryptoJS.
const decryptToken = (encryptedToken: string): string | null => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, import.meta.env.VITE_SECRET_KEY || 'thisismytestsecretkey');
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    return null;
  }
};

// Interceptor for adding authorization token to headers
axiosInstance.interceptors.request.use(
  config => {
    const token = sessionStorage.getItem('token');
    if (token) {
      const decryptedToken = decryptToken(token);
      if (decryptedToken) {
        config.headers['Authorization'] = `Bearer ${decryptedToken}`;
      } else {
        sessionStorage.removeItem('token');
      }
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle 401 errors and redirect to login
axiosInstance.interceptors.response.use(
  response => {
    return response;
  },
  error => {
    if (error.response && error.response.status === 401) {
      // Clear session storage
      sessionStorage.clear();
      
      // Redirect to login page using the correct route
      if (router.currentRoute.value.name !== 'Login') {
        router.push({ name: 'Login' });
      }
    }
    return Promise.reject(error);
  }
);

export default axiosInstance;
