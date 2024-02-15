<template>
  <v-card>
    <RouterLink :to="`/offer-${id}`">
      <img
        height="150"
        :src="image"
        alt=""
      >
    </RouterLink>
    <div class="header pa-3">
      <RouterLink :to="`/offer-${id}`">
        <h3>
          {{ title }}
        </h3>
        <h4>
          <v-icon icon="mdi-map-marker-outline" />
          {{ description }}
        </h4>
      </RouterLink>
    </div>
  </v-card>
</template>

<script lang="ts" setup>
defineProps({
  id: {
    type: Number,
    required: true
  },
  image: {
    type: String,
    required: false,
    default: 'https://via.placeholder.com/768x432/'
  },
  title: {
    type: String,
    required: false,
    default: 'Hier ein tolles Angebot'
  },
  description: {
    type: String,
    required: false,
    default: 'Bundesland / Region / Stadt'
  }
})
</script>

<style lang="scss" scoped>

img {
  object-fit: cover;
  object-position: bottom;
  width: 100%;
  aspect-ratio: 16/9;
}

.header {
  display: flex;
  flex-direction: column-reverse;
  gap: 1rem
}
</style>
