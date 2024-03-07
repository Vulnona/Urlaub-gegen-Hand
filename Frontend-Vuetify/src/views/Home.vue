<template>
  <BaseSearch />
  <OfferGrid
    v-if="productsResult"
    :result="productsResult"
  />
</template>

<script lang="ts" setup>
import OfferGrid from '@/components/OfferGrid.vue'
import BaseSearch from '@/components/BaseSearch.vue'
import { onMounted, ref, Ref } from 'vue'
import { Result } from '@/types'

const productsResult: Ref<Result|null> = ref(null)

const fetchProducts = async () => {
  return fetch('https://dummyjson.com/products')
    .then(response => response.json())
}

onMounted(() => {
  fetchProducts().then(data => {
    productsResult.value = data
  })
})
</script>
