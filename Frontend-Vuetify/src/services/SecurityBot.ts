import router from '@/router';
import VueJwtDecode from 'vue-jwt-decode';
import CryptoJS from 'crypto-js';

const decryptToken = (encryptedToken) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    return null;
  }
};

const Securitybot = () => {
  const token = sessionStorage.getItem('token');

  if (!token) {
    router.push('/');
    return;
  }
  const decryptedToken = decryptToken(token);
  const decoded = VueJwtDecode.decode(decryptedToken);
  const currentTime = Math.floor(Date.now() / 1000); 

  if (decoded.exp < currentTime) {
    sessionStorage.removeItem('token');
    router.push('/');
  }
};

export default Securitybot;
