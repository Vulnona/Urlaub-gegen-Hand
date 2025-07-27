<style>
body .custom-card {
  padding: 0;
}

body .custom-card .card-title {
  font-size: 20px;
  font-weight: 500;
  text-transform: capitalize;
}

body .custom-card .card-text {
  font-size: 14px;
}

body .custom-card .button-container .btn {
  padding: 5px 9px;
  font-size: 14px !important;
}

/*
  .rating-buttons {
    margin-bottom: 20px;
  }
  */
.rating-modal-content {
  padding: 0;
  border: none;
}

.swal2-textarea {
  margin: 0;
  width: 100%;
}

.user-info {
  width: 100%;
  margin: auto;
}

.user-details-container {
  font-size: 16px;
}

.user-info a {
  width: 250px;
  overflow: hidden;
  text-overflow: ellipsis;
  display: inline-block;
  white-space: nowrap;
  vertical-align: bottom;
}

.card-offer {
  cursor: pointer;
}

.card-offer:hover .card-title {
  color: rgb(0, 189, 214);
}

body .custom-card .card-text strong {
  font-weight: 500;
}
</style>
<template>
  <PublicNav />
  <Navbar />
  <section class="section_space  Offer_search_layout">
    <div class="offer_search_overlay"></div>
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="text-center main_center_title">
            <h2>Suchen Sie nach Angeboten</h2>
            <p>Du suchst nach deinem Traumurlaubsziel, weißt aber nicht, wo du anfangen sollst? Plane ganz einfach die Reise deines Lebens.</p>
          </div>
          <div class="offer_search_content" ref="searchContent">
            <div class="SearchBox_filter flexBox align-items-center">
              <div class="SearchBox">
                <i class="ri-search-line"></i>
                <input type="text"  v-model="searchTerm" @input="resetSearch(this.searchTerm)"
                  placeholder="Suche Angebote / Regionen / Skills" class="form-control "
                  @keyup.enter="debouncedSearch">
              </div>
              <div class="btn_outer">
                <button type="button"  @click="debouncedSearch" class="btn themeBtn">Suchen</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
  <section class="section_space">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div v-if="offers" class="offers_group">
            <div v-if="loading" class="spinner-container text-center">
              <div class="spinner"></div>
            </div>
            <div v-else class="row">
              <div v-for="(offer, index) in offers" :key="offer.id" class="col-md-3 mb-4">
                <OfferCard :offer=offer :logId=logId :isActiveMember=isActiveMember />
              </div>
            </div>
            <!-- Pagination Section -->
            <div class="pagination">
              <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1"><i
                  class="ri-arrow-left-s-line"></i>Zurück</button>
              <span>Seite {{ currentPage }} von {{ totalPages }}</span>
              <button class="action-link" @click="changePage(currentPage + 1)"
                :hidden="currentPage === totalPages">Weiter<i class="ri-arrow-right-s-line"></i></button>
            </div>
          </div>
          <div v-else>
            <h2 class="text-center">No Offers Found!</h2>
          </div>
        </div>
      </div>
    </div>
  </section>
  <div v-if="showRatingModal" class="overlay"></div>
  <div id="rating-modal" class="rating-modal" v-if="showRatingModal">
    <div class="modal-content rating-modal-content">
      <div class="review_rating_layout">
        <div class="review_header">
          <div class="photo">
            <img
              :src="'data:' + offers[this.currentIndex].imageMimetype + ';base64,' + offers[this.currentIndex].imageData"
              alt="Offer Image" />
          </div>
          <div class="rightSideBox">
            <h5>{{ offers[this.currentIndex].title }}</h5>
            <p class="hostName"> {{ offers[this.currentIndex].hostName }}</p>
          </div>
        </div>
        <div class="rating_flexBox">
          <p>Add Rating</p>
          <div id="rating-stars">
            <span class="star ri-star-fill" v-for="n in 5" :key="n" :data-value="n" @click="selectStar(n)"
              :class="{ 'selected': n <= selectedRating }"></span>
          </div>
        </div>
        <div class="review_box">
          <p>Write a review</p>
          <textarea class="textarea form-control" style="height: 120px;" v-model="reviewText">
                        Bitte gebt die gegenseitige Bewertung erst ab, nachdem diese terminlich abgeschlossen ist.
                      </textarea>
        </div>
      </div>
    </div>
    <div class="modal-footer">
      <div class="rating-buttons">
        <button @click="submitRating" class="btn common-btn themeBtn">Bewertung abschicken</button>
        <button @click="cancelRating" class="btn common-btn btn-cancel">Abbrechen</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import OfferCard from '@/components/offer/OfferCard.vue';
</script>
<script lang="ts">
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from '@/interceptor/interceptor';
import PublicNav from '@/components/navbar/PublicNav.vue';
import {isActiveMembership,GetUserRole} from '@/services/GetUserPrivileges';
import getLoggedUserId from '@/services/LoggedInUserId';
import Swal from "sweetalert2";
import router from "@/router";
import toast from '@/components/toaster/toast';
import debounce from 'lodash/debounce';


function scrollToTargetElement() {
  const targetElement = document.querySelector('.Offer_search_layout');
  if (targetElement) {
    targetElement.scrollIntoView({ behavior: 'smooth' });
    return true;
  }
  return false; 
}

export default {
  components: {
    Navbar,
      PublicNav,
  },
  data() {
    return {
      loading: true,
      offers: [],
      searchTerm: '',
      statusMap: {},
      logId: getLoggedUserId(),
      showModal: false,
      selectedRating: 0,
      currentOfferId: null,
      userRole: GetUserRole(),
      isActiveMember: isActiveMembership(),
      searchTimeout: null,
      reviewText: '',
      showRatingModal: false,
      currentIndex: 0,
      currentPage: 1,
      totalPages: 1,
      pageSize: 12,
    };
  },
  mounted() {
    this.debouncedSearchOffers = debounce(this.searchOffers, 300);
    console.log('[DEBUG] Home.vue userRole:', this.userRole);
    if (this.userRole !== 'Admin') {
      this.fetchOffers();
    }
  },
  methods: {
    resetSearch(search) {
      if (search == '' && this.userRole !== 'Admin') {
        this.fetchOffers();
      }
    },
    async fetchOffers() {
      this.loading = true;
      try {
        const response = await axiosInstance.get(`offer/get-all-offers`, {
          params: {
            searchTerm: this.searchTerm,
            pageSize: this.pageSize,
            pageNumber: this.currentPage
          }
        });
        this.offers = response.data.items;
        this.totalPages = Math.ceil(response.data.totalCount / this.pageSize);
      } catch (error) {
        console.error('Fehler beim Laden der Angebote:', error);
        toast.info('Angebote konnten nicht geladen werden. Bitte versuchen Sie es erneut.');
      } finally {
        this.loading = false;
      }
    },

    async changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages && this.userRole !== 'Admin') {
        this.currentPage = newPage;
        await this.fetchOffers(); 

        this.$nextTick(() => {
          if (!scrollToTargetElement()) {
            console.log('Starting MutationObserver...');
            const observer = new MutationObserver((mutations, obs) => {
              if (scrollToTargetElement()) {
                console.log('Element found and scrolled. Disconnecting observer.');
                obs.disconnect(); 
              }
            });
            observer.observe(document.body, {
              childList: true,
              subtree: true,
            });
          }
        });
      }
    },

    // Method to send request for offer application
    async sendRequest(offerId) {
      const result = await Swal.fire({
        title: 'Bist du sicher?',
        text: 'Möchtest du diese Anfrage senden?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Apply',
        customClass: {
          popup: 'custom-apply-modal dialog_box',
          confirmButton: 'themeBtn',
          cancelButton: 'Cancel_btn',
        }
      });
      if (result.isConfirmed) {
        try {
          await axiosInstance.post(`offer/apply-offer?offerId=${offerId}`);
          toast.success("Deine Anfrage wurde gesendet.!");
          // await this.checkReviewStatus(offerId);
          if (this.userRole !== 'Admin') {
            this.fetchOffers();
          }
        } catch (error) {
          console.error('Fehler beim Versenden der Anfrage:', error);
          toast.info("Leider konnte deine Anfrage nicht versendet werden. Bitte versuchen Sie es erneut.");
        }
      }
    },
    // Method to search offers based on the searchTerm
    searchOffers() {
      if (this.userRole !== 'Admin') {
        this.loading = true;
        this.fetchOffers();
      }
    },
    debouncedSearch() {
      this.currentPage = 1;
      this.debouncedSearch();
    },
    async showAddRatingModal(offerId, userId, index) {
      this.selectedRating = 0;
      this.showRatingModal = true;
      this.currentOfferId = offerId;
      this.currentIndex = index;
    },
    // Method to select star rating
    selectStar(rating) {
      this.selectedRating = rating;
    },
    // // Method to submit rating
    async submitRating() {
      if (this.selectedRating > 0) {
        await this.addRating(this.currentOfferId, this.selectedRating, this.reviewText);
        this.cancelRating();
      } else {
        toast.info("Please select a Rating");
      }
    },
    cancelRating() {
      this.showRatingModal = false;
      this.reviewText = '';
    },
    // Method to add rating for a specific offer
    async addRating(offerId, userRating, reviewText) {
      try {
        const response = await axiosInstance.post(`review/add-review`, {
          offerId: offerId,
          ratingValue: userRating,
          reviewComment: reviewText,
          reviewedUserId: null,
        });
        if (response.status === 200) {
          toast.success("Dein Rating wurde erfolgreich hinzugefügt.");
        }
      } catch (error) {
        console.error('Fehler beim Absenden der Bewertung:', error);
        toast.error("Bewertung konnte nicht abgesendet werden. Bitte versuchen Sie es erneut.");
      }
    },
  },
  computed: {
    filteredOffers() {
      return this.offers.filter(offer => {
        const title = offer.title ? offer.title.toLowerCase() : '';
        const skills = offer.skills ? offer.skills.toLowerCase() : '';
        const region = offer.region ? offer.region.toLowerCase() : '';
        const isValidOffer = offer.hostId != offer.id;
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
<style lang="scss" scoped>
.offers-title {
  font-size: 2.5rem;
  color: #333;
  margin-bottom: 1rem;
  font-weight: bold;
}

.test {
  font-size: 5.5rem;
  color: #ca0000;
  margin-bottom: 1rem;
}

.search-input {
  padding: 0.75rem 1rem;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 1rem;
  width: 100%;
  max-width: 400px;
  margin: 0 auto;
  display: block;
  transition: border-color 0.3s ease;
}

.search-input:focus {
  outline: none;
  border-color: #1ee94a;
  box-shadow: 0 0 5px rgba(63, 248, 27, 0.5);
}

.card {
  max-width: 100%;
  overflow: hidden;
  border: 1px solid #e0e0e0;
  border-radius: 10px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s, box-shadow 0.2s;
}

.card:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

.card-body {
  padding: 1rem;
}

.card-img-top {
  max-height: 200px;
  min-height: 200px;
  object-fit: cover;
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
}

.card-title {
  font-size: 1.5rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
}

.card-text {
  font-size: 1rem;
  margin-bottom: 0.5rem;
}

.btn-success {
  background-color: #28a745;
  color: white;
}

.btn-success:hover {
  background-color: #218838;
}

.btn-warning {
  background-color: #ffc107;
  color: black;
}

.modal-content {
  text-align: center;
}

.btn-warning:hover {
  background-color: #e0a800;
}

.btn-primary {
  background-color: #007bff;
  color: white;
}

.btn-primary:hover {
  background-color: #0056b3;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background-color: #5a6268;
}

.mt-2 {
  margin-top: 0.5rem;
}

.button-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 10px;
}

.btn {
  flex: 1;
  margin: 0 5px;
}

.btn-secondary.mt-2 {
  margin-top: 0;
}

.custom-swal-container {
  font-family: 'Arial', sans-serif;
}

.custom-swal-title {
  font-size: 1.5rem;
  color: #333;
  font-weight: bold;
}

.custom-swal-html-container {
  font-size: 1rem;
  color: #555;
  text-align: left;
}

.user-details-container {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.user-detail {
  background-color: #f9f9f9;
  border-radius: 8px;
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.user-info {
  margin-bottom: 0.5rem;
}

.user-info b {
  color: #333;
}

.user-info a {
  color: #007bff;
  text-decoration: none;
}

.user-info a:hover {
  text-decoration: underline;
}

// .container {
//   padding: 20px;
// }
.table {
  width: 100%;
  border-collapse: collapse;
}

.overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.5);
  /* Semi-transparent black */
  z-index: 100;
  /* Behind the modal */
}

.table th:first-child,
.table td:first-child {
  border-left: none;
}

.table th:last-child,
.table td:last-child {
  border-right: none;
}

#rating-modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: white;
  padding: 0px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  max-width: 500px;
  width: 100%;
}

.modal-content {
  text-align: center;
}

#rating-stars {
  display: flex;
  justify-content: center;
  margin: 0px 0;
}

.star {
  font-size: 2rem;
  cursor: pointer;
  color: gray;
}

.star.selected {
  color: #f6a716;
}

button {
  margin: 5px;
}

.rating-star {
  font-family: var(--fa-style-family, "Font Awesome 6 Free");
  font-weight: var(--fa-style, 900);
}

.rating-star:before {
  content: "\f005";
}

.OfferButtons {
  width: 100%;
}

</style>
