<style scoped>
.rating-star {
  font-family: var(--fa-style-family, "Font Awesome 6 Free");
  font-weight: var(--fa-style, 900);
}

.rating-star:before {
  content: "\f005";
}
</style>
<template>
  <div class="bg-ltgrey pt-40 pb-40">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="SearchBox_filter flexBox justify-content-center align-items-center">
            <label class="f-14 mb-0">Suchen Sie nach Angeboten</label>
            <div class="SearchBox">
              <i class="ri-search-line"></i>
              <input type="text" v-model="searchTerm" @input="searchOffers"
                placeholder="Search offers / Region / Skills" class="form-control ">
            </div>
            <div class="btn_outer">
              <button type="button" class="btn themeBtn">Suchen</button>
            </div>
          </div>
        </div>
      </div>

      <div v-if="offers">
        <div v-if="loading" class="text-center">Loading...</div>
        <div v-else class="row">
          <div v-for="offer in filteredOffers" :key="offer.id" class="col-md-4 mb-4">
            <div class="card">
              <img @click="redirectToOfferDetail(offer.id)" v-if="offer.imageData"
                :src="'data:' + offer.imageMimeType + ';base64,' + offer.imageData" class="card-img-top"
                alt="Offer Image">
              <div class="card-body">
                <div @click="redirectToOfferDetail(offer.id)">
                  <h3 class="card-title">{{ offer.title }}</h3>
                  <p class="card-text">{{ truncateDescription(offer.description) }}</p>
                  <p class="card-text"><strong>Location:</strong> {{ offer.location }}</p>
                  <p class="card-text"><strong>Skills:</strong> {{ offer.skills }}</p>
                  <p class="card-text"><strong>Accommodation:</strong> {{ offer.accomodation }}</p>
                  <p class="card-text"><strong>Suitable for:</strong> {{ offer.accomodationsuitable }}</p>
                  <p class="card-text"><strong>Region:</strong> {{ offer.state }}</p>
                </div>
                <div v-if="offer.user.user_Id != logId">

                  <div class="button-container" v-if="userRole != 'Admin'">
                    <button :class="['btn', getButtonColor(offer.id)]" @click.stop="handleButtonClick(offer)">
                      {{ getStatusText(offer) }}
                    </button>

                    <button v-if="getStatus(offer.id) === 'ViewDetails'" class="btn btn-secondary"
                      @click.stop="showAddReviewModal(offer)">
                      Add Review
                    </button>
                    <button v-if="getStatus(offer.id) === 'ViewDetails'"
                      @click="showAddRatingModal(offer.id, currentUserId)" class="btn themeCancelBtn m-0"><i
                        class="ri-star-line"></i></button>
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

    <div id="rating-modal" v-if="showModal">
      <div class="modal-content">
        <h3>Add Rating</h3>
        <div id="rating-stars">
          <span class="star rating-star" v-for="n in 5" :key="n" :data-value="n" @click="selectStar(n)"
            :class="{ 'selected': n <= selectedRating }"></span>
        </div>
        <button @click="submitRating">Submit</button>
        <button @click="cancelRating">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>

import router from '@/router';
import axios from 'axios';
import Swal from 'sweetalert2';
import VueJwtDecode from 'vue-jwt-decode';
import CryptoJS from 'crypto-js';

// Configuration for Font Awesome SVG replacement
window.FontAwesomeConfig = { autoReplaceSvg: false };

// Global variable to store decrypted logId
let globalLogid = '';
let globalEmail = '';
let globalrating = '';
let globalIsTrue = '';
let globalRole = '';
export default {
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
      userRole: ''
    };
  },
  mounted() {

    this.checkLoginStatus(); // Check login status and decrypt necessary data
    this.fetchOffers(); // Fetch offers for display
    this.Securitybot(); // Ensure user is authenticated
  //  this.isActiveMembership();
  },
  methods: {
    // Method to check login status and decrypt relevant data
    checkLoginStatus() {
      const token = sessionStorage.getItem("token");
      if (token) {
        // Decrypt logId and user role from sessionStorage
        const testlogid = this.decryptlogID(sessionStorage.getItem("logId"));
        globalLogid = testlogid;
        this.logId = testlogid;
        globalEmail = this.decryptToken(sessionStorage.getItem("logEmail"));
        // Decrypt JWT token to retrieve user role
        const decryptedToken = this.decryptToken(token);
        if (decryptedToken) {
          const decodedToken = VueJwtDecode.decode(decryptedToken);
          this.userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || '';
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
        console.error('Error decrypting token:', e);
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
        console.error('Error decrypting item:', e);
        return null;
      }
    },
    // Method to enforce security by verifying token existence
    Securitybot() {
      if (!sessionStorage.getItem("token")) {
        Swal.fire({
          title: 'You are not logged In!',
          text: 'Login First to continue.',
          icon: 'info',
          confirmButtonText: 'OK'
        });
        router.push('/login'); // Redirect to login page if not authenticated
      }
    },

    async isActiveMembership() {
      if (globalRole != 'Admin') {
        try {
          const decLogId = this.decryptlogID(sessionStorage.getItem("logId"));
          const response = await axios.get(`${process.env.baseURL}membership/check-active-membership-byuserId/${decLogId}`);

          globalIsTrue = response.data.isActive;
          if (globalIsTrue != true) {
            setTimeout(() => {
              Swal.fire({
                title: 'Membership Expired!',
                text: 'Your membership has expired. Please renew your membership.',
                html: `
          <p>Your membership has expired. Please renew your membership.</p>
              <input type="text" id="swal-input1" class="swal2-input" placeholder="Subscription ID">
            <a href="https://alreco.company.site/" target="_blank" class="swal2-confirm swal2-styled" style="display: inline-block; margin-top: 10px;">Click To Buy Membership</a>
        `,
                icon: 'error',
                confirmButtonText: 'Submit',
                confirmButtonText: 'OK'
              }).then(() => {
                // This code will run after the user clicks 'OK'
                router.push('/login'); // Make sure 'router' is properly imported/defined
                sessionStorage.clear();
                setTimeout(() => {
                  window.location.reload();
                }, 500);
              });
            }, 500);
            router.push('/login'); // Redirect to membership page if membership expired
          } else {
            // console.log("Active membership");
          }

        } catch (error) {
          if (error.response) {
            // The request was made and the server responded with a status code
            // that falls out of the range of 2xx
            console.error("Error response:", error.response.data);
            Swal.fire({
              title: 'No Membership Found!',
              html: `
          <p>Your membership not Found. Please Buy your membership.</p>
            
            <a href="https://alreco.company.site/" target="_blank" class="swal2-confirm swal2-styled" style="display: inline-block; margin-top: 10px;">Click To Buy Membership</a>
        `,
              text: 'Your membership not Found. Please Buy your membership.',
              icon: 'error',
              confirmButtonText: 'OK'
            }).then(() => {
              // This code will run after the user clicks 'OK'
              sessionStorage.clear();
              router.push('/login'); // Make sure 'router' is properly imported/defined
              window.location.reload();
            });
          } else if (error.request) {
            // The request was made but no response was received
            console.error("No response received:", error.request);
          } else {
            // Something happened in setting up the request that triggered an Error
            console.error("Error:", error.message);
          }

        }
      }
    },
    // Method to fetch all offers based on search term
    async fetchOffers() {
      try {
        const response = await axios.get(`${process.env.baseURL}offer/get-all-offers`, {
          params: {
            searchTerm: this.searchTerm
          }
        });
        this.offers = response.data; // Update offers array with fetched data
        await this.checkAllReviewStatuses(); // Check review statuses for all fetched offers
      } catch (error) {
        console.error('Error fetching offers:', error);
      } finally {
        this.loading = false; // Set loading state to false after fetching
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
        const response = await axios.get(`${process.env.baseURL}review/check-review-status`, {
          params: {
            userId: globalLogid,
            offerId: offerId
          }
        });
        this.statusMap[offerId] = response.data.status; // Update status map with offerId and its status
      } catch (error) {
        console.error(`Error checking review status for offer ${offerId}:`, error);
      }
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
          return 'btn-success';
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
        await this.sendRequest(offer.id, globalLogid); // Send request if status is 'Apply'
      } else if (status === 'ViewDetails') {
        await this.fetchUsersByOfferId(offer.id); // Fetch users if status is 'ViewDetails'
      } else {

      }
    },
    // Method to send request for offer application
    async sendRequest(offerId, userId) {
      const result = await Swal.fire({
        title: 'Are you sure?',
        text: 'Do you want to send this request?',
        icon: 'success',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, send it!'
      });
      if (result.isConfirmed) {
        try {
          // Send POST request to add review with email as parameter
          await axios.post(`${process.env.baseURL}review/adding-review?email=${globalEmail}`, {
            offerId,
            userId,
            status: 0,
          });
          Swal.fire('Success!', 'Your request has been sent.', 'success');
          await this.checkReviewStatus(offerId); // Update review status after request is sent
        } catch (error) {
          Swal.fire('Error!', 'Failed to send request: Your Request was rejected!', 'error');
          console.error(error);
        }
      }
    },
    // Method to fetch users details based on offerId
    async fetchUsersByOfferId(offerId) {
      try {
        const response = await axios.get(`${process.env.baseURL}review/get-user-by-offerId/${offerId}`);
        let users = response.data;
        if (!Array.isArray(users)) {
          users = [users];
        }
        if (users.length > 0) {
          // Generate user details HTML and display using Swal (SweetAlert)
          let userDetails = users.map(user => {
            return `
          <div class="user-detail">
            <div class="user-info">
              <b>Name:</b> ${user.firstName} ${user.lastName}
            </div>
            <div class="user-info">
              <b>Email:</b> ${user.email_Address}
            </div>
            <div class="user-info">
              <b>Gender:</b> ${user.gender}
            </div>
            <div class="user-info">
              <b>Date of Birth:</b> ${user.dateOfBirth}
            </div>
            <div class="user-info">
              <b>Facebook Link:</b> <a href="${response.data.facebook_link}" target="_blank">${response.data.facebook_link}</a>
            </div>
          </div>
        `;
          }).join('<br>');

          // Display user details using Swal modal
          Swal.fire({
            title: 'User Details',
            html: `<div class="user-details-container">${userDetails}</div>`,
            icon: 'info',
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
      const { value: review, dismiss: dismissAction } = await Swal.fire({
        title: 'Add Review',
        html: `<textarea id="reviewTextArea" class="swal2-textarea" placeholder="Your review" readonly>Bitte gebt die gegenseitige Bewertung erst ab, nachdem diese terminlich abgeschlossen ist.</textarea>`,
        showCancelButton: true,
        cancelButtonText: 'Cancel',
        confirmButtonText: 'Submit',
        preConfirm: () => {
          return document.getElementById('reviewTextArea').value;
        },
      });

      if (review !== undefined) {
        const reviewText = "Bitte gebt die gegenseitige Bewertung erst ab, nachdem diese terminlich abgeschlossen ist.";
        this.addReview(offer.id, reviewText); // Call method to add review
      } else if (dismissAction === Swal.DismissReason.cancel) {
        Swal.fire('Cancelled', 'Review submission cancelled.', 'info');
      }
    },
    // Method to add review for a specific offer
    async addReview(offerId, reviewText) {
      try {
        const response = await axios.post(`${process.env.baseURL}review-login-user/create-review-login-user`, {
          offerId,
          userId: globalLogid,
          addReviewForLoginUser: reviewText,
        });

        if (response.status === 200) {
          Swal.fire('Review Added', 'Your review has been successfully added.', 'success');
        } else {
          Swal.fire('Error', 'Failed to add review.', 'error');
        }
      } catch (error) {
        Swal.fire('Already Added', 'You Have Already Added Review!', '');
      }
    },
    // Method to search offers based on the searchTerm
    searchOffers() {
      this.loading = true;
      this.fetchOffers(); // Fetch offers based on updated searchTerm
    },
    // Method to redirect to offer detail page
    redirectToOfferDetail(offerId) {
      this.$router.push({ name: 'OfferDetail', params: { id: offerId } });
    },
    // Method to truncate description to limit characters for display
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
        await this.addRating(this.currentOfferId, globalLogid, this.selectedRating); // Call method to add rating
        this.showModal = false;
      } else {
        Swal.fire('Error', 'Please select a rating.', 'error');
      }
    },
    // Method to cancel rating submission
    cancelRating() {
      this.showModal = false;
    },
    // Method to add rating for a specific offer
    async addRating(offerId, toUserId, userRating) {
      try {
        const response = await axios.post(`${process.env.baseURL}add-rating-to-host`, {
          user_Id: toUserId,
          offerId: offerId,
          hostRating: userRating,
          submissionDate: new Date().toISOString()
        });

        if (response.status === 200) {
          Swal.fire('Rating Added', 'Your rating has been successfully added.', 'success');
        } else {
          Swal.fire('Something Went Wrong', 'Unable To Add Rating.', 'error');
        }
      } catch (error) {

        Swal.fire('Already Added Rating', 'You have already added rating!', '');
      }
    },
  },
  computed: {
    // Computed property to filter offers based on searchTerm
    filteredOffers() {
      return this.offers.filter(offer => {
        const title = offer.title ? offer.title.toLowerCase() : '';
        const description = offer.description ? offer.description.toLowerCase() : '';
        const skills = offer.skills ? offer.skills.toLowerCase() : '';
        const location = offer.location ? offer.location.toLowerCase() : '';
        const accomodation = offer.accomodation ? offer.accomodation.toLowerCase() : '';
        const accomodationSuitable = offer.accomodationSuitable ? offer.accomodationSuitable.toLowerCase() : '';
        const region = offer.state ? offer.state.toLowerCase() : '';
        return title.includes(this.searchTerm.toLowerCase()) || region.includes(this.searchTerm.toLowerCase()) || description.includes(this.searchTerm.toLowerCase()) || skills.includes(this.searchTerm.toLowerCase());
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

.btn {
  display: inline-block;
  padding: 0.5rem 1rem;
  font-size: 1rem;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s, color 0.3s;
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

.container {
  padding: 20px;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table th,
.table td {
  border: 1px solid #ccc;
  padding: 8px;
}

.table th {
  background-color: #f2f2f2;
}

.table td {
  text-align: left;
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
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  z-index: 1000;
}

.modal-content {
  text-align: center;
}

#rating-stars {
  display: flex;
  justify-content: center;
  margin: 20px 0;
}

.star {
  font-size: 2rem;
  cursor: pointer;
  color: gray;
}

.star.selected {
  color: gold;
}

button {
  margin: 5px;
}
</style>