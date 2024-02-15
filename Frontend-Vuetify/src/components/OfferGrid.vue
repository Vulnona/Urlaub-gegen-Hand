<template>
  <v-container>
    <v-row v-if="offersSelected">
      <v-col>
        <p>Wir haben <strong>{{ offersSelected }}</strong> Treffer zu deiner Suche gefunden</p>
      </v-col>
    </v-row>
    <v-row>Anzahl der Produkte: {{ products.total }}</v-row>
    <v-row>
      <v-col
        v-for="(item, index) in products.products"
        :key="index"
        cols="12"
        sm="6"
        md="4"
        lg="3"
      >
        <OfferCard
          :id="item.id"
          :title="item.title"
          :description="item.description"
          :image="item.thumbnail"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import OfferCard from './OfferCard.vue'


import { onMounted, ref } from 'vue'

const products = ref([])
const offersSelected = ref(0)

const getPost = async () => {
  return fetch('https://dummyjson.com/products')
    .then(response => response.json())
}

onMounted(() => {
  getPost().then(data => {
    products.value = data
  })
})

/*
    {
      "id": 1,
      "title": "iPhone 9",
      "description": "An apple mobile which is nothing like apple",
      "price": 549,
      "discountPercentage": 12.96,
      "rating": 4.69,
      "stock": 94,
      "brand": "Apple",
      "category": "smartphones",
      "thumbnail": "https://cdn.dummyjson.com/product-images/1/thumbnail.jpg",
      "images": [
        "https://cdn.dummyjson.com/product-images/1/1.jpg",
        "https://cdn.dummyjson.com/product-images/1/2.jpg",
        "https://cdn.dummyjson.com/product-images/1/3.jpg",
        "https://cdn.dummyjson.com/product-images/1/4.jpg",
        "https://cdn.dummyjson.com/product-images/1/thumbnail.jpg"
      ]
    },
*/
</script>
