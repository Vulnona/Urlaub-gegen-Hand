<template>
  <v-app>
    <div v-if="isLoggedIn">
      <header class="main_header2" v-if="userRole === 'Admin'">
        <HeaderMetaNav />
      </header>
      <header class="main_header2" v-else="userRole === 'User'">
        <HeaderMetaNavUser />
      </header>
    </div>
    <router-view />
  </v-app>
</template>

<script lang="ts" setup>
import { onMounted, ref } from 'vue';
import VueJwtDecode from 'vue-jwt-decode';
import CryptoJS from 'crypto-js';

// Importing components
import HeaderMetaNav from './components/HeaderMetaNav.vue';
import HeaderMetaNavUser from './components/HeaderMetaNavUser.vue';

// Reactive variables
const isLoggedIn = ref(false); // Reactive variable to track login status
const userRole = ref(''); // Reactive variable to store user role

onMounted(() => {
  checkLoginStatus();
});

// Function to check login status based on decrypted token
const checkLoginStatus = () => {
  if (decryptedToken) {
    isLoggedIn.value = true; // User is logged in
    const decodedToken = VueJwtDecode.decode(decryptedToken) as Record<string, any>;
    userRole.value = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || '';
    // Extract and store user role from decoded JWT token
  }
};

// Function to decrypt token using AES encryption
const decryptToken = (encryptedToken: string) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY); 
    return bytes.toString(CryptoJS.enc.Utf8); // Convert decrypted bytes to UTF-8 string
  } catch (e) {
    console.error('Error decrypting token:', e); // Log error if decryption fails
    return null; // Return null if decryption fails
  }
};

const decryptedToken = decryptToken(localStorage.getItem('token') || ''); // Decrypt token stored in localStorage
</script>

<style lang="scss" scoped>
header {
  @media (max-width: 768px) {
    flex-direction: column;
  }
}
</style>
