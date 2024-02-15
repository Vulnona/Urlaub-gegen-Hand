<template>
  <BaseSearch />
  <OfferGrid
    v-if="productsResult"
    :result="productsResult"
  />
</template>

<script setup>
import OfferGrid from '@/components/OfferGrid.vue'
import BaseSearch from '@/components/BaseSearch.vue'

</script>
