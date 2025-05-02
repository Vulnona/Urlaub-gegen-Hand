import axios from 'axios';
import CryptoJS from 'crypto-js';
// Create an Axios instance
const axiosInstance = axios.create({
  baseURL: process.env.baseURL,
});

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
// Function to decrypt token using CryptoJS.
const decryptToken = (encryptedToken) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    return null;
  }
};
export default axiosInstance;
