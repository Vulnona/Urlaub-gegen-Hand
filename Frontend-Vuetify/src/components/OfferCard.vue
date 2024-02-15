<template>
  <v-card >
    <img height="150" :src="props.image" alt="">
    <div class="header pa-3">
      <h3>{{ props.title }}</h3>
      <h4>
        <v-icon icon="mdi-map-marker-outline" />
        {{ props.description }}
      </h4>
    </div>
  </v-card>
</template>

<script lang="ts" setup>
const props = defineProps({
  id: {
    type: String,
    required: true
  },
  image: {
    type: String,
    required: false,
    default: "https://via.placeholder.com/768x432/"
  },
  title: {
    type: String,
    required: false,
    default: "Hier ein tolles Angebot"
  },
  description: {
    type: String,
    required: false,
    default: "Bundesland / Region / Stadt"
  },
})
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
