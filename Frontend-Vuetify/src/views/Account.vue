<template>
  <div>
    <h1>Benutzerkonto</h1>
    <div v-if="userProfile">
      <p>Benutzername: {{ userProfile.username }}</p>
      <!-- Weitere Benutzerdaten hier anzeigen -->
    </div>
    <div v-else>
      <p>Lade Benutzerprofil...</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useUserProfileStore } from '@/store/userProfileStore'; 

const userProfileStore = useUserProfileStore();
const userProfile = ref(null);

onMounted(async () => {
  try {
    const response = await fetch('/api/profile'); 
    const userData = await response.json();
    userProfile.value = userData; 
  } catch (error) {
    console.error('Fehler beim Laden des Benutzerprofils', error);
  }
});
</script>