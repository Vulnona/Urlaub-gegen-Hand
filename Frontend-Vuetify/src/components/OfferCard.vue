<template>
  <v-card >
    <img height="150" src="https://cdn.vuetifyjs.com/images/cards/sunshine.jpg" alt="">
    <div class="header pa-3">
      <h3>Angebots Titel, gern auch mal mehrzeilig Lorem ipsum dolor sit amet.</h3>
      <h4>
        <v-icon icon="mdi-map-marker-outline" />
        Bundesland / Region / Stadt
      </h4>
    </div>
  </v-card>
</template>

<script lang="ts" setup>
</script>

<style lang="scss" scoped>

img {
  object-fit: cover;
  object-position: bottom;
  width: 100%;
  //aspect-ratio: 16/9;
}
.header {
  display: flex;
  flex-direction: column-reverse;
  gap: 1rem
}
</style>
