import { NavigationGuardNext, RouteLocationNormalized } from 'vue-router';
import { ref } from 'vue';
import VueJwtDecode from 'vue-jwt-decode';
import CryptoJS from 'crypto-js';

const userRole = ref('');

// Function to decrypt token using AES encryption
const decryptToken = (encryptedToken: string) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY!);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    // Handle error if necessary
    return null;
  }
};

// Get decrypted token
const decryptedToken = decryptToken(sessionStorage.getItem('token') || '');

// Function to check login status
const checkLoginStatus = () => {
  if (decryptedToken) {
    const decodedToken = VueJwtDecode.decode(decryptedToken) as Record<string, any>;
    userRole.value = decodedToken[`${process.env.claims_Url}`] || '';
  }
};

// Auth Guard
export default function authGuard(to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext) {
  const token = sessionStorage.getItem('token');

  // Call login status check before proceeding
  checkLoginStatus();

  // Check if the route requires authentication
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!token) {
      // If the user is not authenticated, redirect to login
      return next({ name: 'Login' });
    }

    // Check if the route requires specific roles
    const requiredRoles = to.meta.roles as string[];
    if (requiredRoles && requiredRoles.length && !requiredRoles.includes(userRole.value)) {
      // Redirect to home or an unauthorized page if the role doesn't match
      return next({ name: 'Home' });
    }
  }

  // Allow the navigation to continue
  next();
}
