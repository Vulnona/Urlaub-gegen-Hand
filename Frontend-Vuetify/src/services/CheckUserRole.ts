import { jwtDecode } from 'jwt-decode';
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

// Method to check the login status of the user
const CheckUserRole = (): string | null => {
  try {
    const token = sessionStorage.getItem("token");
    if (token) {
      const decryptedToken = decryptToken(token);
      if (decryptedToken) {
        const decodedToken = jwtDecode(decryptedToken); // Corrected import usage
        const userRole = (decodedToken as any)[process.env.claims_Url || ''] || ''; // Type assertion
        return userRole;
      } else {
        sessionStorage.removeItem('token');
      }
    }
  } catch (error) {
    console.error('Error decoding user role:', error);
  }
  return null;
};

export default CheckUserRole;
