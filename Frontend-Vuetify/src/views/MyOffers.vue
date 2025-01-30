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
                  <div class="item_img">
                    <img @click="redirectToOfferDetail(offer.id)" v-if="offer.imageData" loading="lazy"
                      :src="'data:' + offer.imageMimeType + ';base64,' + offer.imageData" class="card-img-top"
                      alt="Offer Image">
                  </div>
                  <div class="item_text">
                    <div @click="redirectToOfferDetail(offer.id)">
                      <h3 class="card-title">{{ offer.title }}</h3>
                      <div class="item_description">
                        <p class="card-text"><strong>Fähigkeiten:</strong> {{ offer.skills }}</p>
                        <p class="card-text"><strong>Unterbringung:</strong> {{ offer.accomodation }}</p>
                        <p class="card-text"><strong>Geeignet für:</strong> {{ offer.accomodationsuitable }}</p>
                        <p class="card-text"><strong>Region/Ort:</strong> {{ offer.region }}{{ offer.location }}</p>
                        <div class="rating_flexBox card-text">
                          <strong>Overall Rating:</strong>
                          <div class="rating_star">
                            <span>
                              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32" aria-hidden="true"
                                role="presentation" focusable="false"
                                style="display: block; height: 13px; width: 13px; fill: #f6a716;">
                                <path fill-rule="evenodd"
                                  d="m15.1 1.58-4.13 8.88-9.86 1.27a1 1 0 0 0-.54 1.74l7.3 6.57-1.97 9.85a1 1 0 0 0 1.48 1.06l8.62-5 8.63 5a1 1 0 0 0 1.48-1.06l-1.97-9.85 7.3-6.57a1 1 0 0 0-.55-1.73l-9.86-1.28-4.12-8.88a1 1 0 0 0-1.82 0z">
                                </path>
                              </svg>
                            </span>
                            <span class="vertical_middle">{{ offer.averageRating }} </span>
                          </div>
                        </div>
                      </div>
                    </div>               
                  </div>
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
<script>

import Navbar from '@/components/navbar/Navbar.vue';
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
        const response = await axiosInstance.get(`${process.env.baseURL}offer/get-offer-by-user`, {
          params: {
            searchTerm: this.searchTerm,
            pageSize: this.pageSize,
            pageNumber: this.currentPage
          }
        });
        this.offers = response.data.items;
        this.totalPages = Math.ceil(response.data.totalCount / this.pageSize);
      } catch (error) {
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
    // Method to redirect to offer detail page
    redirectToOfferDetail(offerId) {
      this.$router.push({
        name: 'OfferDetail',
        params: {
          id: offerId
        }
      });
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
