import router from '@/router';
import { jwtDecode } from 'jwt-decode'; // Replaced VueJwtDecode with jwtDecode
import CryptoJS from 'crypto-js';

// Function to decrypt token using CryptoJS.
const decryptToken = (encryptedToken: string): string | null => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY || '');
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    return null;
  }
};

const Securitybot = (): void => {
  const token = sessionStorage.getItem('token');

  if (!token) {
    router.push('/');
    return;
  }
  
  const decryptedToken = decryptToken(token);
  if (decryptedToken) {
    const decoded = jwtDecode(decryptedToken); // Using jwtDecode instead of VueJwtDecode
    const currentTime = Math.floor(Date.now() / 1000);

    if (decoded.exp < currentTime) {
      sessionStorage.removeItem('token');
      router.push('/');
    }
  }
};

export default Securitybot;
