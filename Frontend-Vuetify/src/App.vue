<template>
  <v-app>
    <router-view />
    <Footer />
  </v-app>
</template>

<script lang="ts">
import Footer from "@/components/Footer.vue";

export default {
  components: {
    Footer,
  },
};
</script>
