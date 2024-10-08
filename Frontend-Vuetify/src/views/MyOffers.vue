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
                placeholder="Search offers / Region / Skills" class="form-control ">
            </div>
            <div class="add-new-offer">
              <router-link class="btn themeBtn" to="/offer"><i class="ri-add-circle-line"></i> Add New Offer
              </router-link>
            </div>
          </div>
          <div v-if="offers" class="offers_group">
            <div v-if="loading" class="text-center">Lädt...</div>
            <div v-else class="row">
              <div v-for="offer in filteredOffers" :key="offer.id" class="col-md-3 mb-4">
                <div class="all_items card-offer">
                  <div class="item_img">
                    <img @click="redirectToOfferDetail(offer.id)" v-if="offer.imageData" loading="lazy"
                      :src="'data:' + offer.imageMimeType + ';base64,' + offer.imageData" class="card-img-top"
                      alt="Offer Image">
                    <div class="rating" v-if="getStatusText(offer) === 'View Details'"
                      @click="showAddRatingModal(offer.id, offer.user.user_Id)"><i class="ri-star-line"></i></div>
                  </div>
                  <div class="item_text">
                    <div @click="redirectToOfferDetail(offer.id)">
                      <h3 class="card-title">{{ offer.title }}</h3>
                      <div class="item_description">
                        <!-- <p class="card-text">{{ truncateDescription(offer.description) }}</p> -->
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
                          <!-- <a href="javascript:void();" class="action-link font-normal"><u>15 reviews</u></a> -->
                        </div>
                      </div>
                    </div>
                    <!-- <div v-if="isActiveMember && offer.user.user_Id != logId"> -->
                    <div v-if="isActiveMember">
                      <div class="button-container" v-if="userRole != 'Admin'">
                        <button :class="['btn', getButtonColor(offer.id)]" @click.stop="handleButtonClick(offer)">
                          {{ getStatusText(offer) }}
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
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
import router from '@/router';
import Swal from 'sweetalert2';
import VueJwtDecode from 'vue-jwt-decode';
import CryptoJS from 'crypto-js';
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from "@/interceptor/interceptor"
import Securitybot from '@/services/SecurityBot';
import toast from '@/components/toaster/toast';
window.FontAwesomeConfig = {
  autoReplaceSvg: false
};
let globalLogid = '';
let globalEmail = '';
let globalRole = '';
export default {
  components: {
    Navbar,
  },
  data() {
    return {
      loading: true,
      offers: [],
      searchTerm: '',
      statusMap: {},
      logId: '',
      showModal: false,
      selectedRating: 0,
      currentOfferId: null,
      userRole: '',
      isActiveMember: false,
      searchTimeout: null,
      reviewText: '',
    };
  },

  mounted() {
    this.checkLoginStatus();
    this.fetchOffers();
    Securitybot();
    // this.Securitybot();
    // this.isActiveMembership();
  },
  methods: {
    // Method to check login status and decrypt relevant data
    checkLoginStatus() {
      const token = sessionStorage.getItem("token");
      if (token) {
        const testlogid = this.decryptlogID(sessionStorage.getItem("logId"));
        globalLogid = testlogid;
        this.logId = testlogid;
        globalEmail = this.decryptToken(sessionStorage.getItem("logEmail"));
        const decryptedToken = this.decryptToken(token);
        if (decryptedToken) {
          const decodedToken = VueJwtDecode.decode(decryptedToken);
          this.userRole = decodedToken[`${process.env.claims_Url}`] || '';
          globalRole = this.userRole;
        } else {
          sessionStorage.removeItem('token');
        }
      }
    },
    // Method to decrypt AES encrypted token
    decryptToken(encryptedToken) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
        return bytes.toString(CryptoJS.enc.Utf8);
      } catch (e) {
        return null;
      }
    },
    // Method to decrypt AES encrypted logID
    decryptlogID(encryptedItem) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY);
        const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
        return parseInt(decryptedString, 10).toString();
      } catch (e) {
        return null;
      }
    },
    // Method to enforce security by verifying token existence
    Securitybot() {
      if (!sessionStorage.getItem("token")) {
        router.push('/login');
      }
    },
    async isActiveMembership() {
      if (globalRole !== 'Admin') {
        try {
          const decLogId = this.decryptlogID(sessionStorage.getItem("logId"));
          const response = await axiosInstance.get(`${process.env.baseURL}membership/check-active-membership-by-userId/${decLogId}`);
          this.isActiveMember = response.data.isActive;
        } catch (error) {
          // console.error('Error checking membership status:', error);
        }
      }
    },
    // Method to fetch all offers based on search term
    async fetchOffers() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}offer/get-offer-by-user`, {
          params: {
            searchTerm: this.searchTerm
          }
        });
        this.offers = response.data;
        // await this.checkAllReviewStatuses();
      } catch (error) {
        // console.error('Error fetching offers:', error);
      } finally {
        this.loading = false;
      }
    },
    // Method to check review status for all offers asynchronously
    async checkAllReviewStatuses() {
      const promises = this.offers.map(offer => this.checkReviewStatus(offer.id));
      await Promise.all(promises);
    },
    // Method to check review status for a specific offer
    async checkReviewStatus(offerId) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}offer/check-offer-status`, {
          params: {
            userId: globalLogid,
            offerId: offerId
          }
        });
        this.statusMap[offerId] = response.data.status;
      } catch (error) { }
    },
    // Method to get status from status map based on offerId
    getStatus(offerId) {
      return this.statusMap[offerId];
    },
    // Method to get text representation of status based on offer status
    getStatusText(offer) {
      const status = this.getStatus(offer.id);
      switch (status) {
        case 'Applied':
          return 'Applied';
        case 'ViewDetails':
          return 'View Details';
        default:
          return 'Apply';
      }
    },
    // Method to get button color based on offer status
    getButtonColor(offerId) {
      const status = this.getStatus(offerId);
      switch (status) {
        case 'Apply':
          return 'themeBtn';
        case 'Applied':
          return 'btn-warning';
        case 'ViewDetails':
          return 'btn-primary';
        default:
          return 'btn-primary';
      }
    },
    // Method to handle button click actions based on offer status
    async handleButtonClick(offer) {
      const status = this.getStatus(offer.id);
      if (status === 'Apply') {
        await this.sendRequest(offer.id);
      } else if (status === 'ViewDetails') {
        await this.fetchUsersByOfferId(offer.id);
      } else { }
    },
    // Method to send request for offer application
    async sendRequest(offerId) {
      const result = await Swal.fire({
        title: 'Bist du sicher?',
        text: 'Möchtest du diese Anfrage senden?',
        icon: 'success',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, send it!'
      });
      if (result.isConfirmed) {
        try {
          await axiosInstance.post(`${process.env.baseURL}offer/apply-offer?email=${globalEmail}&offerId=${offerId}`);
          Swal.fire('Erfolg!', 'Deine Anfrage wurde gesendet.', 'success');
          await this.checkReviewStatus(offerId);
        } catch (error) {
          Swal.fire('Sorry', 'Leider konnte deine Anfrage nicht versendet werden!', '');
          console.error(error);
        }
      }
    },
    // Method to fetch users details based on offerId
    async fetchUsersByOfferId(offerId) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}review/get-user-by-offerId/${offerId}`);
        let users = response.data;
        if (!Array.isArray(users)) {
          users = [users];
        }
        if (users.length > 0) {
          let userDetails = users.map(user => {
            return `
                           <div class="user-detail">
                            <div class="user-info">
                               <div class="leftBox">Name:</div>
                               <div class="rightBox">${user.firstName} ${user.lastName}</div>
                            </div>
                            <div class="user-info">
                                <div class="leftBox">Gender:</div>
                                <div class="rightBox">${user.gender}</div>
                            </div>
                            <div class="user-info">
                                <div class="leftBox">Date of Birth:</div>
                                <div class="rightBox">${user.dateOfBirth}</div>
                            </div>
                            <div class="user-info">
                                <div class="leftBox">Facebook Link:</div>
                                <div class="rightBox">  <button class="action_link" style="border-radius:8px" onclick="window.open('${response.data.facebook_link}', '_blank')"><i class="fa fa-eye"></i> View In Facebook</button></div>
                            </div>
                        </div>
                        `;
          }).join('<br>');
          Swal.fire({
            title: 'User Details',
            html: `<div class="user-details-container">${userDetails}</div>`,
            width: '600px',
            customClass: {
              container: 'custom-swal-container',
              title: 'custom-swal-title',
              htmlContainer: 'custom-swal-html-container',
            },
            showCloseButton: true,
            showConfirmButton: false,
          });
        } else {
          Swal.fire({
            title: 'No Users Found',
            text: 'No users found for this offer.',
            icon: 'info',
          });
        }
      } catch (error) {
        Swal.fire('Error!', 'Failed to fetch users: ' + error.message, 'error');
        console.error(error);
      }
    },
    // Method to show add review modal using Swal
    async showAddReviewModal(offer) {
      const {
        value: review,
        dismiss: dismissAction
      } = await Swal.fire({
        title: 'Review hinzufügen',
        html: `<textarea id="reviewTextArea" class="swal2-textarea" placeholder="Dein Review" readonly>Bitte gebt die gegenseitige Bewertung erst ab, nachdem diese terminlich abgeschlossen ist.</textarea>`,
        showCancelButton: true,
        cancelButtonText: 'Abbrechen',
        confirmButtonText: 'Einreichen',
        preConfirm: () => {
          return document.getElementById('reviewTextArea').value;
        },
      });
      if (review !== undefined) {
        const reviewText = "Bitte gebt die gegenseitige Bewertung erst ab, nachdem diese terminlich abgeschlossen ist.";
        this.addReview(offer.id, reviewText);
      } else if (dismissAction === Swal.DismissReason.cancel) { }
    },
    // Method to add review for a specific offer
    async addReview(offerId, reviewText) {
      try {
        const response = await axiosInstance.post(`${process.env.baseURL}review-login-user/create-review`, {
          offerId,
          userId: globalLogid,
          addReviewForLoginUser: reviewText,
        });
        if (response.status === 200) {
          Swal.fire('Review hinzugefügt', 'Dein Review wurde erfolgreich hinzugefügt.', 'success');
        } else {
          Swal.fire('Error', 'Das Review konnte nicht hinzugefügt werden.', 'error');
        }
      } catch (error) {
        Swal.fire('Bereits hinzugefügt', 'Du hast bereits ein Review hinzugefügt!', '');
      }
    },
    // Method to search offers based on the searchTerm
    searchOffers() {
      this.loading = true;
      this.fetchOffers();
    },
    debouncedSearch() {
      clearTimeout(this.searchTimeout);
      this.searchTimeout = setTimeout(() => {
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
    truncateDescription(description) {
      if (!description) return '';
      const words = description.split(' ');
      return words.slice(0, 15).join(' ') + (words.length > 15 ? '...' : '');
    },
    // Method to show add rating modal
    async showAddRatingModal(offerId, userId) {
      this.selectedRating = 0;
      this.showModal = true;
      this.currentOfferId = offerId;
      globalrating = userId;
    },
    // Method to select star rating
    selectStar(rating) {
      this.selectedRating = rating;
    },
    // Method to submit rating
    async submitRating() {
      if (this.selectedRating > 0) {
        await this.addRating(this.currentOfferId, this.selectedRating, this.reviewText);
        this.showModal = false;
      } else {
        toast.info("Please select a Rating");
      }
    },
    cancelRating() {
      this.showModal = false;
      this.reviewText = '';
    },
    // Method to add rating for a specific offer
    async addRating(offerId, userRating, reviewText) {
      try {
        const response = await axiosInstance.post(`${process.env.baseURL}review/add-review?email=${globalEmail}`, {
          offerId: offerId,
          ratingValue: userRating,
          reviewComment: reviewText,
        });
        if (response.status === 200) {
          Swal.fire('Rating hinzugefügt', 'Dein Rating wurde erfolgreich hinzugefügt.', 'success');
        } else {
          Swal.fire('Etwas ist schief gelaufen', 'Dein Rating konnte nicht abgegeben werden.', 'error');
        }
      } catch (error) {
        Swal.fire('Bereits hinzugefügt', 'Du hast bereits ein Rating abgegeben!', '');
      }
    },
  },
  computed: {
    filteredOffers() {
      return this.offers.filter(offer => {
        const title = offer.title ? offer.title.toLowerCase() : '';
        const description = offer.description ? offer.description.toLowerCase() : '';
        const skills = offer.skills ? offer.skills.toLowerCase() : '';
        const region = offer.region ? offer.region.toLowerCase() : '';
        const isValidOffer = true;
        return isValidOffer && (
          title.includes(this.searchTerm.toLowerCase()) ||
          region.includes(this.searchTerm.toLowerCase()) ||
          description.includes(this.searchTerm.toLowerCase()) ||
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
