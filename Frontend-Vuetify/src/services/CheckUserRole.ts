import VueJwtDecode from 'vue-jwt-decode';
import CryptoJS from 'crypto-js';

// Function to decrypt token using CryptoJS.
const decryptToken = (encryptedToken) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    return null;
  }
};

// Method to check the login status of the user
const CheckUserRole = () => {
  try {
    const token = sessionStorage.getItem("token");
    if (token) {
      const decryptedToken = decryptToken(token);
      if (decryptedToken) {
        const decodedToken = VueJwtDecode.decode(decryptedToken);
        const userRole = decodedToken[`${process.env.claims_Url}`] || '';
        return userRole;
      } else {
        sessionStorage.removeItem('token');
      }
    }
  }
  catch {
    return null;
  }
};

export default CheckUserRole;
