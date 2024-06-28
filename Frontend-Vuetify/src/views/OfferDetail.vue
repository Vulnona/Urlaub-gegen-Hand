<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';

const { params } = useRoute();
const offer = ref(null);
const loading = ref(true);

const fetchOfferDetail = async () => {
  try {
    const response = await axios.get(`${process.env.baseURL}offer/get-offer-by-id/${params.id}`);
    offer.value = response.data;
  } catch (error) {
    console.error('Error fetching offer detail:', error);
  } finally {
    loading.value = false;
  }
};

const backtooffers = () => {
  window.location.href = '/home';
};

onMounted(fetchOfferDetail);
</script>

<template>
  <div class="offer-detail-container" v-if="!loading">
    <div class="offer-image" v-if="offer.imageData">
      <img :src="'data:' + offer.imageMimeType + ';base64,' + offer.imageData" alt="Offer Image" />
    </div>
    <div class="offer-content">
      <h1 class="offer-title">{{ offer.title }}</h1>
      <p class="offer-description">{{ offer.description }}</p>
      <div class="offer-details">
        <p><strong>Location:</strong> {{ offer.location }}</p>
        <p><strong>Skills:</strong> {{ offer.skills }}</p>
        <p><strong>Accommodation:</strong> {{ offer.accomodation }}</p>
        <p><strong>Suitable for:</strong> {{ offer.accomodationSuitable }}</p>
        <p><strong>Region:</strong> {{ offer.region.regionName }}</p>
      </div>

      <button @click="backtooffers()" class="btn btn-primary" style="background-color: cornflowerblue;">Back To
        Offers</button>
    </div>
  </div>
  <div class="loading" v-else>
    Loading...
  </div>
</template>

<style scoped lang="scss">
.offer-detail-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 10px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  max-width: 800px;
  margin: 40px auto;
}

.offer-image img {
  max-width: 100%;
  border-radius: 10px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.offer-content {
  margin-top: 20px;
  text-align: center;
}

.offer-title {
  font-size: 2.5rem;
  font-weight: bold;
  color: #333;
}

.offer-description {
  font-size: 1.2rem;
  color: #555;
  margin: 20px 0;
}

.offer-details {
  font-size: 1rem;
  color: #666;
  text-align: left;
  width: 100%;
  max-width: 600px;
  margin: 0 auto;
}

.offer-details p {
  margin: 10px 0;
}

.loading {
  font-size: 1.5rem;
  color: #888;
  text-align: center;
  padding: 40px;
}
</style>
