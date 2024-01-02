<template>
  <v-container>
    <v-row v-if="OffersSelected">
      <v-col>
      <p>Wir haben <strong>{{ OffersSelected }}</strong> Treffer zu deiner Suche gefunden</p>
      </v-col>
    </v-row>
    <v-row>
      <v-col
        v-for="n in Offers"
        :key="n"
        cols='12'
        sm='6'
        md='4'
        lg='3'
      >
        <OfferCard />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import OfferCard from './OfferCard.vue'

const Offers = ref(20)
const OffersSelected = ref(0)
</script>
