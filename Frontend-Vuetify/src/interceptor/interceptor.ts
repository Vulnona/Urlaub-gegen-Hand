import axios from 'axios';
import CryptoJS from 'crypto-js';
import router from '@/router';

// Create an Axios instance
const axiosInstance = axios.create({
  baseURL: '/api/',  // Use relative URL for Vite proxy
});


// Function to decrypt token using CryptoJS AES
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
    // Only log in development environment
    if (process.env.NODE_ENV === 'development') {
      console.log('🔍 AXIOS REQUEST:', {
        url: config.url,
        method: config.method
      });
    }
    
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
    console.error('🚨 AXIOS REQUEST ERROR:', error);
    return Promise.reject(error);
  }
);

// Response interceptor to handle 401 errors and redirect to login
axiosInstance.interceptors.response.use(
  response => {
    // Only log in development environment
    if (process.env.NODE_ENV === 'development') {
      console.log('✅ AXIOS RESPONSE SUCCESS:', {
        status: response.status,
        url: response.config.url
      });
    }
    return response;
  },
  error => {
    // Only log in development environment, without sensitive data
    if (process.env.NODE_ENV === 'development') {
      console.error('❌ AXIOS RESPONSE ERROR:', {
        status: error.response?.status,
        url: error.config?.url,
        message: error.message
      });
    }
    
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
