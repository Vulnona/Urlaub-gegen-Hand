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
            <p>Looking for your dream vacation destination but don't know where to start? With the help of experienced
              <br> and knowledgeable travel agents, you can plan the trip of a lifetime with ease.
            </p>
          </div>
          <div class="offer_search_content" ref="searchContent">
            <div class="SearchBox_filter flexBox align-items-center">
              <div class="SearchBox">
                <i class="ri-search-line"></i>
                <input type="text"  v-model="searchTerm" @input="resetSearch(this.searchTerm)"
                  placeholder="Search offers / Region / Skills" class="form-control "
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
                <div class="all_items card-offer">
                  <div class="item_img">
                    <img loading="lazy" :src="'data:' + offer.imageMimeType + ';base64,' + offer.imageData"
                      class="card-img-top" alt="Offer Image">
                    <div class="rating"
                      v-if="isActiveMember && offer.hostId != logId && offer.appliedStatus == 'Approved'"
                      @click="showAddRatingModal(offer.id, offer.hostId, index)"><i class="ri-star-line"></i></div>
                  </div>
                  <div class="item_text">
                    <div @click="redirectToOfferDetail(offer.id)">
                      <h3 class="card-title">{{ offer.title }}</h3>
                      <div class="item_description">
                        <p class="card-text"><strong>Fähigkeiten:</strong> {{ offer.skills }}</p>
                        <p class="card-text"><strong>Unterbringung:</strong> {{ offer.accomodation }}</p>
                        <p class="card-text"><strong>Geeignet für:</strong> {{ offer.accomodationsuitable }}</p>
                        <p class="card-text"><strong>Region/Ort:</strong> {{ offer.region }} {{ offer.location }}</p>
                      </div>
                    </div>
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
                    <div v-if="isActiveMember && offer.hostId != logId && userRole != 'Admin'">
                      <button v-if="offer.appliedStatus === 'CanApply'" @click="sendRequest(offer.id)"
                        class="btn btn-success OfferButtons">Apply</button>
                      <button v-else-if="offer.appliedStatus === 'Applied'" class="btn btn-info OfferButtons"
                        disabled>Applied</button>
                      <button v-else-if="offer.appliedStatus === 'Rejected'" class="btn btn-danger OfferButtons"
                        disabled>Rejected</button>
                      <button v-else-if="offer.appliedStatus === 'Approved'" @click="showUserDetails(offer.hostId)"
                        class="btn btn-primary OfferButtons">View Host Details</button>
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
        <button @click="submitRating" class="btn common-btn themeBtn">Submit</button>
        <button @click="cancelRating" class="btn common-btn btn-cancel">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from '@/interceptor/interceptor';
import PublicNav from '@/components/navbar/PublicNav.vue';
import isActiveMembership from '@/services/CheckActiveMembership';
import getLoggedUserId from '@/services/LoggedInUserId';
import Swal from "sweetalert2";
import CheckUserRole from "@/services/CheckUserRole";
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
    PublicNav
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
      userRole: CheckUserRole(),
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
    this.fetchOffers();
  },
  methods: {
    resetSearch(search) {
      if (search == '') {
        this.fetchOffers();
      }
    },
    showUserDetails(userId) {
      sessionStorage.setItem("UserId", userId);
      router.push("/account")
    },
    async fetchOffers() {
      this.loading = true;
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}offer/get-all-offers`, {
          params: {
            searchTerm: this.searchTerm,
            pageSize: this.pageSize,
            pageNumber: this.currentPage
          }
        });
        this.offers = response.data.items;
        this.totalPages = Math.ceil(response.data.totalCount / this.pageSize);
      } catch (error) {
        console.error(error);
      } finally {
        this.loading = false;
      }
    },

    async changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
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
        icon: '',
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
          await axiosInstance.post(`${process.env.baseURL}offer/apply-offer?offerId=${offerId}`);
          toast.success("Deine Anfrage wurde gesendet.!");
          // await this.checkReviewStatus(offerId);
          this.fetchOffers();
        } catch (error) {
          toast.info("Leider konnte deine Anfrage nicht versendet werden!");
        }
      }
    },
    // Method to search offers based on the searchTerm
    searchOffers() {
      this.loading = true;
      this.fetchOffers();
    },
    debouncedSearch() {
      this.currentPage = 1;
      this.debouncedSearchOffers();
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
        const response = await axiosInstance.post(`${process.env.baseURL}review/add-review`, {
          offerId: offerId,
          ratingValue: userRating,
          reviewComment: reviewText,
          reviewedUserId: null,
        });
        if (response.status === 200) {
          toast.success("Dein Rating wurde erfolgreich hinzugefügt.");
        }
      } catch (error) {
        if (error.response.data.message == "The Review already exists.") {
          toast.info("Du hast bereits ein Rating abgegeben.");
        }
        else {
          toast.error("Fehler beim Absenden der Bewertungen!");
        }
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
