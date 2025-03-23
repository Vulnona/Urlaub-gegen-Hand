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

export const GetUserPrivileges = (): {userRole: string, membershipStatus: string} | null => {
  try {
    const token = sessionStorage.getItem("token");
    if (token) {
      const decryptedToken = decryptToken(token);
      if (decryptedToken) {
        const decodedToken = jwtDecode(decryptedToken); // Corrected import usage
        const userRole = (decodedToken as any)[process.env.claims_Url || ''] || ''; // Type assertion
        return {userRole: userRole, membershipStatus:decodedToken.MembershipStatus, verification:decodedToken.VerificationStatus};
      } else {
        sessionStorage.removeItem('token');
      }
    }
  } catch (error) {
    console.error('Error decoding user role:', error);
  }
  return null;
};

export const isActiveMembership = () => {
    const privileges = GetUserPrivileges();
    if (privileges && privileges.membershipStatus === 'Active')
        return true;
    else 
        return false;       
};
    
export const GetUserRole = (): string | null => {
    const privileges = GetUserPrivileges();    
    if(privileges)
        return privileges.userRole;
    else
        return null;
};
