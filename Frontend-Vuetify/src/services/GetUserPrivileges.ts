import { jwtDecode } from 'jwt-decode';
import CryptoJS from 'crypto-js';

// Function to decrypt token using CryptoJS.
const decryptToken = (encryptedToken: string): string | null => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, import.meta.env.VITE_SECRET_KEY || 'thisismytestsecretkey');
    const decrypted = bytes.toString(CryptoJS.enc.Utf8);
    // If decryption failed, the result will be empty
    if (!decrypted) {
      console.warn('Token decryption failed - clearing invalid token');
      sessionStorage.removeItem('token');
      return null;
    }
    return decrypted;
  } catch (e) {
    console.warn('Token decryption error - clearing invalid token:', e);
    sessionStorage.removeItem('token');
    return null;
  }
};

export const GetUserPrivileges = (): {userRole: string, membershipStatus: string, verification: string} | null => {
  try {
    const token = sessionStorage.getItem("token");
    if (!token) {
      return null;
    }
    
    const decryptedToken = decryptToken(token);
    if (!decryptedToken) {
      return null;
    }
    
    const decodedToken = jwtDecode(decryptedToken) as any;
    if (!decodedToken) {
      console.warn('JWT decode failed - clearing invalid token');
      sessionStorage.removeItem('token');
      return null;
    }
    
    const userRole = decodedToken[process.env.claims_Url || ''] || '';
    const membershipStatus = decodedToken.MembershipStatus || 'Inactive';
    const verification = decodedToken.VerificationStatus || 'not';
    
    return {
      userRole: userRole, 
      membershipStatus: membershipStatus, 
      verification: verification
    };
  } catch (error) {
    console.error('Error in GetUserPrivileges:', error);
    sessionStorage.removeItem('token');
    return null;
  }
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
