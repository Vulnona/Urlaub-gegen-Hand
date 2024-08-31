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
import HeaderMetaNav from './components/HeaderMetaNav.vue';
import HeaderMetaNavUser from './components/HeaderMetaNavUser.vue';
const isLoggedIn = ref(false);
const userRole = ref('');
onMounted(() => {
  checkLoginStatus();
});
// Function to check login status based on decrypted token
const checkLoginStatus = () => {
  if (decryptedToken) {
    isLoggedIn.value = true;
    const decodedToken = VueJwtDecode.decode(decryptedToken) as Record<string, any>;
    userRole.value = decodedToken[`${process.env.claims_Url}`] || '';
  }
};
// Function to decrypt token using AES encryption
const decryptToken = (encryptedToken: string) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    console.error('Error decrypting token:', e);
    return null;
  }
};
const decryptedToken = decryptToken(sessionStorage.getItem('token') || ''); 
</script>
<style lang="scss" scoped>
header {
  @media (max-width: 768px) {
    flex-direction: column;
  }
}
</style>
