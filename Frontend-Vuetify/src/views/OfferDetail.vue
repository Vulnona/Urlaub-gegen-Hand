<script setup lang="ts">
import {useRoute} from 'vue-router';

const {params} = useRoute()
</script>

<template>
  <article>
    <h1> Hier ID: {{ params.id }}</h1>
  </article>
</template>

<style scoped lang="scss">

</style>
