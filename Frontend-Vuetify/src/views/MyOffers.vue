<template>
  <Navbar />
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2>My Offers</h2>
          </div>
        </div>
      </div>
    </div>
  </div>
  <section class="section_space offers_list">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="sort-new-button ">
            <div class="sort-select"><label>Sort by</label>
              <select class="form-control">
                <option value="1">Most Recent</option>
                <option value="4">Positive First</option>
                <option value="3">Negative First</option>
              </select>
            </div>
            <div class="SearchBox">
              <i class="ri-search-line"></i>
              <input type="text" v-model="searchTerm" @input="debouncedSearch"
                placeholder="Suche Angebote / Regionen / Skills" class="form-control ">
            </div>
            <div class="add-new-offer">
              <router-link class="btn themeBtn" to="/add-offer"><i class="ri-add-circle-line"></i> Add New Offer
              </router-link>
            </div>
          </div>
          <div v-if="offers.length" class="offers_group">
            <div v-if="loading" class="spinner-container text-center">
              <div class="spinner"></div>
            </div>
            <div v-else class="row">
              <div v-for="offer in filteredOffers" :key="offer.id" class="col-md-3 mb-4">
                <div class="all_items card-offer">
                <OfferCard :offer=offer />
                </div>
              </div>
            </div>
             <!-- Pagination Section -->
             <div class="pagination">
              <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1"><i
                  class="ri-arrow-left-s-line"></i>Previous</button>
              <span>Page {{ currentPage }} of {{ totalPages }}</span>
              <button class="action-link" @click="changePage(currentPage + 1)"
                :hidden="currentPage === totalPages">Next<i class="ri-arrow-right-s-line"></i></button>
            </div>
          </div>
          <div v-else>
            <h2 class="text-center">No Offers Found!</h2>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
<script setup lang="ts">
  import OfferCard from '@/components/offer/OfferCard.vue';
</script>
<script lang="ts">

import Navbar from '@/components/navbar/Navbar.vue';
import toast from '@/components/toaster/toast';
import axiosInstance from "@/interceptor/interceptor"
import Securitybot from '@/services/SecurityBot';


window.FontAwesomeConfig = {
  autoReplaceSvg: false
};

export default {
  components: {
    Navbar,
  },
  data() {
    return {
      loading: true,
      offers: [],
      searchTerm: '',
      searchTimeout: null,
      currentIndex: 0,
      currentPage: 1,
      totalPages: 1,
      pageSize: 8,
    };
  },

  mounted() {
    this.fetchOffers();
    Securitybot();
  },
  methods: {  
    async fetchOffers() {
      try {
        const response = await axiosInstance.get(`offer/get-offer-by-user`, {
          params: {
            searchTerm: this.searchTerm,
            pageSize: this.pageSize,
            pageNumber: this.currentPage
          }
        });
        this.offers = response.data.items;
        this.totalPages = Math.ceil(response.data.totalCount / this.pageSize);
      } catch (error) {
        console.error('Fehler beim Laden der eigenen Angebote:', error);
        toast.error('Eigene Angebote konnten nicht geladen werden. Bitte versuchen Sie es erneut.');
      } finally {
        this.loading = false;
      }
    },
    changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.currentPage = newPage;
        this.fetchOffers(); // fetch new data for the selected page
      }
    },
    searchOffers() {
      this.loading = true;
      this.fetchOffers();
    },
    debouncedSearch() {
      clearTimeout(this.searchTimeout);
      this.searchTimeout = setTimeout(() => {
        this.currentPage= 1,
        this.searchOffers();
      }, 1000);
    },
  },
  computed: {
    filteredOffers() {
      return this.offers.filter(offer => {
        const title = offer.title ? offer.title.toLowerCase() : '';
        const skills = offer.skills ? offer.skills.toLowerCase() : '';
        const region = offer.region ? offer.region.toLowerCase() : '';
        const isValidOffer = true;
        return isValidOffer && (
          title.includes(this.searchTerm.toLowerCase()) ||
          region.includes(this.searchTerm.toLowerCase()) ||   
          skills.includes(this.searchTerm.toLowerCase())
        );
      });
    }
  }
};
</script>

<style>
.offers_list_page {
  margin-top: 30px;
}
</style>
