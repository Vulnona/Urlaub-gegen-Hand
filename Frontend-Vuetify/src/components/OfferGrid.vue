<template>
  <v-container>
    <v-row v-if="offersSelected">
      <v-col>
        <p>Wir haben <strong>{{ offersSelected }}</strong> Treffer zu deiner Suche gefunden</p>
      </v-col>
    </v-row>
    <v-row>Anzahl der Produkte: {{ result.total }}</v-row>
    <v-row>
      <v-col
        v-for="(item, index) in result.products"
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
import {PropType, ref} from 'vue'
import {Result} from '@/types';

defineProps({
  result: {
    type: Object as PropType<Result>,
    required: true
  }
})

const offersSelected = ref(0)
</script>
