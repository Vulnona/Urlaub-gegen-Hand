<template>
  <v-app>
    <v-container>
      <header>
        <HeaderLogo />
        <HeaderMetaNav />
      </header>
    </v-container>
    <router-view />
  </v-app>
</template>

<script setup lang="ts">
import HeaderMetaNav from '@/components/HeaderMetaNav.vue';
import HeaderLogo from '@/components/HeaderLogo.vue';
</script>

<style lang="scss" scoped>
header {
  display: flex;
  justify-content: space-between;
  align-items: center; /* Center the items vertically */
  padding: 10px 0; /* Add some padding */
  background-color: #f5f5f5; /* Light grey background */
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* Subtle shadow for header */
}
</style>
