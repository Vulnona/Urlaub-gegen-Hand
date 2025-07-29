import { jwtDecode } from 'jwt-decode';
import { decryptItem } from '@/utils/encryption';

// Track if this is the first call after login
let isFirstCall = true;

// Function to decrypt token using centralized encryption utility.
const decryptToken = (encryptedToken: string): string | null => {
  try {
    console.log('Attempting to decrypt token with centralized function...');
    const decrypted = decryptItem(encryptedToken);
    
    if (!decrypted) {
      console.warn('Token decryption failed - clearing invalid token');
      // Clear all session storage to prevent further issues
      sessionStorage.clear();
      return null;
    }
    
    console.log('Token decryption successful');
    return decrypted;
  } catch (e) {
    console.warn('Token decryption error - clearing invalid token');
    // Clear all session storage to prevent further issues
    sessionStorage.clear();
    return null;
  }
};

export const GetUserPrivileges = (): {userRole: string, membershipStatus: string, verification: string} | null => {
  try {
    // Add a small delay on first call to ensure storage is complete
    if (isFirstCall) {
      console.log('First call to GetUserPrivileges, adding delay...');
      isFirstCall = false;
      // Don't add delay here, just mark as not first call
    }
    
    const token = sessionStorage.getItem("token");
    if (!token) {
      console.log('No token found in session storage');
      return null;
    }
    
    console.log('Token found in session storage, attempting decryption...');
    const decryptedToken = decryptToken(token);
    if (!decryptedToken) {
      console.log('Token decryption failed, returning null');
      return null;
    }
    
    console.log('Token decrypted successfully, decoding JWT...');
    const decodedToken = jwtDecode(decryptedToken) as any;
    if (!decodedToken) {
      console.warn('JWT decode failed - clearing invalid token');
      sessionStorage.clear();
      return null;
    }
    
    const userRole = decodedToken[process.env.claims_Url || ''] || '';
    const membershipStatus = decodedToken.MembershipStatus || 'Inactive';
    const verification = decodedToken.VerificationStatus || 'not';
    
    console.log('User privileges retrieved successfully:', { userRole, membershipStatus, verification });
    
    return {
      userRole: userRole, 
      membershipStatus: membershipStatus, 
      verification: verification
    };
  } catch (error) {
    console.error('Error in GetUserPrivileges:', error);
    sessionStorage.clear();
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
