<template>
  <div class="container">
    <div class="row">
      <div class="col-sm-12">
        <div class="flexBox justify-between align-items-center top_headingBox mb-25">
          <h1 class="main-title">Offers</h1>
          <div class="SearchBox">
            <i class="ri-search-line"></i>
            <input type="text" v-model="searchTerm" @input="searchOffers" placeholder="Search offers..."
              class="form-control">
          </div>
        </div>
      </div>
    </div>
    <div v-if="loading" class="text-center">Loading...</div>
    <div v-else class="row">
      <div v-for="offer in offers" :key="offer.id" class="col-md-4 mb-4">
        <div class="card">
          <div class="card-body">
            <h3 class="card-title">{{ offer.title }}</h3>
            <p class="card-text">{{ offer.description }}</p>
            <p class="card-text"><strong>Location:</strong> {{ offer.location }}</p>
            <p class="card-text"><strong>Skill:</strong> {{ offer.skill.skillDescrition }}</p>
            <p class="card-text"><strong>Accomodation:</strong> {{ offer.accomodation.nameAccomodationType }}</p>
            <p class="card-text"><strong>Suitable for:</strong> {{ offer.accomodationSuitable.name }}</p>
            <button @click="SendRequest()" class="btn btn-success" style="float: right;">Request</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import axiosInstance from '@/interceptor/interceptor';
import axios from 'axios';

export default {
  data() {
    return {
      loading: true,
      offers: [],
      searchTerm: ''
    };
  },
  mounted() {
    this.fetchOffers();
  },
  methods: {
    async fetchOffers() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}offer/get-all-offers`, {
          params: {
            searchTerm: this.searchTerm
          }
        });
        this.offers = response.data;
        console.log(response.data);
        this.loading = false;
      } catch (error) {
        console.error('Error fetching offers:', error);
        this.loading = false;
      }
    },
    searchOffers() {
      this.loading = true;
      this.fetchOffers();
    }
  },
  computed: {
    filteredOffers() {
      if (!this.searchTerm) {
        return this.offers;
      }
      return this.offers.filter(offer =>
        offer.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        offer.description.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }
  }
};
</script>
<style scoped>
.card {
  border: 1px solid #ccc;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.card-title {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
}

.card-text {
  margin-bottom: 0.5rem;
}

.container {
  padding-top: 20px;
}
</style>
