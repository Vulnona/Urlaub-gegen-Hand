import CryptoJS from 'crypto-js';
import VueJwtDecode from 'vue-jwt-decode';
// Function to decrypt token using CryptoJS.
const decryptToken = (encryptedToken) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    return null;
  }
};

const isActiveMembership = () => {
  try {
    const token = sessionStorage.getItem("token");
    if (token) {
      const decryptedToken = decryptToken(token);
      if (decryptedToken) {
        const decodedToken = VueJwtDecode.decode(decryptedToken);
        const membershipStatus = decodedToken['MembershipStatus'] || '';
        if (membershipStatus === 'Active') {
          return true;
        } else if (membershipStatus === 'Inactive') {
          return false;
        }
      } else {
        sessionStorage.removeItem('token');
      }
    }
  } catch (error) {
    console.error('Error checking membership status:', error);
    return false;
  }
  return false;
};


export default isActiveMembership;