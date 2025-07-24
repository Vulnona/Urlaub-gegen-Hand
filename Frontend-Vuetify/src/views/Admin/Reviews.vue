<template>
  <div v-if="userRole !== 'Admin'">
    <Errorpage />
  </div>
  <div v-else>
    <Navbar />
    <div class="inner_banner_layout">
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <div class="inner_banner">
              <h2>Offer Reviews</h2>
            </div>
          </div>
        </div>
      </div>
    </div>
    <section class="section_space offers_list">
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <div class="sort-new-button justify-content-center">
              <div class="SearchBox">
                <i class="ri-search-line"></i>
                <input type="text" v-model="searchTerm" @input="debouncedSearch" placeholder="Search offers"
                  class="form-control ">
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
                      <img @click="redirectToOfferReviews(offer.id)" v-if="offer.imageData" loading="lazy"
                        :src="'data:' + offer.imageMimeType + ';base64,' + offer.imageData" class="card-img-top"
                        alt="Offer Image">
                    </div>
                    <div class="">
                      <div @click="redirectToOfferReviews(offer.id)">
                        <h4 class="card-title">{{ offer.title }}</h4>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <!-- Pagination Section -->
              <div class="pagination">
                <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1"><i
                    class="ri-arrow-left-s-line"></i>Vorherige</button>
                <span>Seite {{ currentPage }} von {{ totalPages }}</span>
                <button class="action-link" @click="changePage(currentPage + 1)"
                  :hidden="currentPage === totalPages">Nächste<i class="ri-arrow-right-s-line"></i></button>
              </div>
            </div>
            <div v-else>
              <h2 class="text-center">Keine Angebote gefunden!</h2>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from "@/interceptor/interceptor"
import Securitybot from '@/services/SecurityBot';
import {GetUserRole} from "@/services/GetUserPrivileges";
import Errorpage from "../Errorpage.vue";

window.FontAwesomeConfig = {
  autoReplaceSvg: false
};

export default {
  components: {
    Navbar,
    Errorpage
  },
  data() {
    return {
      userRole: GetUserRole(),
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
        const response = await axiosInstance.get(`review/get-all-offers-for-reviews-admin`, {
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
        this.fetchOffers(); 
      }
    },
    searchOffers() {
      this.loading = true;
      this.fetchOffers();
    },
    debouncedSearch() {
      clearTimeout(this.searchTimeout);
      this.searchTimeout = setTimeout(() => {
        this.currentPage = 1,
          this.searchOffers();
      }, 1000);
    },
    // Method to redirect to offer detail page
    redirectToOfferReviews(offerId) {
      this.$router.push({
        name: 'OfferReviews',
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
<style scoped>
.all_items {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  width: 300px; /* Fixed card width */
  height: 400px; /* Fixed card height */
  border: 1px solid #ccc;
  border-radius: 8px;
  overflow: hidden;
  background: #fff;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.item_img {
  flex-grow: 1; /* Allows the image container to grow and fill available space */
  display: flex;
  align-items: stretch; /* Ensure the image container stretches completely */
  justify-content: center;
  padding: 0; /* Remove any padding */
  margin: 0; /* Remove any margin */
}

.item_img img {
  width: 100%; /* Ensures the image fills the width of the container */
  height: 100%; /* Ensures the image fills the height of the container */
  object-fit: cover; /* Adjust the image to cover the container without distortion */
}

.card-title {
  text-align: center;
  font-size: 1rem;
  padding: 10px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  background-color: #f8f8f8;
}

.card-title:hover {
  white-space: normal; /* Show full text on hover */
  overflow: visible;
}

.all_items > div {
  display: flex;
  flex-direction: column;
}

</style>
