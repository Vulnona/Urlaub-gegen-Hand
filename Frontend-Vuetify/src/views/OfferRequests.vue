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
          <div class="flexBox justify-between align-items-center top_headingBox">
            <h1 class="main-title">Offers Requests</h1>
          </div>
          <div class="card mt-25">
            <div v-if="loading" class="text-center">Loading...</div>
            <div v-else>
              <div class="table-responsive">
                <table v-if="offers && offers.length > 0" class="table table-bordered theme_table">
                  <thead>
                    <tr>
                      <th>Title</th>
                      <th>Skills</th>
                      <th>Location</th>
                      <th>User</th>
                      <th>Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="item in offers" :key="item.id">
                      <td>{{ item.offer.title }}</td>
                      <td>{{ item.offer.skills }}</td>
                      <td>{{ item.offer.location }}</td>
                      <td>{{ item.user.firstName }} {{ item.user.lastName }}</td>
                      <td>
                        <div class="btn_flexBox now_wrap">
                          <button v-if="item.status === 1" @click="viewDetail(item.id)" class="btn themeBtn m-0"><i
                              class="ri-eye-line"></i> View Detail</button>
                          <button v-if="item.status === 0" @click="approveOffer(item.id)" class="btn btn-success m-0"><i
                              class="ri-checkbox-circle-line"></i> Approve</button>
                          <button v-if="item.status === 0" @click="rejectOffer(item.id)" class="btn btn-danger m-0"><i
                              class="ri-close-circle-line"></i> Reject</button>
                          <button v-if="item.status === 2" class="btn btn-danger m-0" disabled><i
                              class="ri-close-circle-line"></i> Rejected</button>
                          <button v-if="item.status === 1" @click="showAddReviewModal(item.offer.id)"
                            class="btn themeCancelBtn m-0"><i class="ri-add-circle-line"></i> Add Review</button>
                          <button v-if="item.status === 1" @click="showAddRatingModal(item.offer.id, item.user.user_Id)"
                            class="btn themeCancelBtn m-0"><i class="ri-star-line"></i> Add Rating</button>
                        </div>
                      </td>
                    </tr>
                  </tbody>
                </table>
                <div v-else>
                  <p>No offers found for this user.</p>
                </div>
              </div>
            </div>
          </div>
        </div>
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
import axios from 'axios'; // Import Axios for HTTP requests
import Swal from 'sweetalert2'; // Import SweetAlert2 for alerts
import router from '@/router'; // Import Vue router for navigation
window.FontAwesomeConfig = { autoReplaceSvg: false }; // Configure FontAwesome to not automatically replace SVG icons
import CryptoJS from 'crypto-js'; // Import CryptoJS for encryption

let globalLogid = ''; // Variable to store decrypted log ID
let globalratingId='';
export default {
  data() {
    return {
      email: sessionStorage.getItem("logEmail"), // Retrieve email from sessionStorage
      loading: true, // Loading state for async operations
      offers: [], // Array to store user offers
      showModal: false, // Modal visibility state
      selectedRating: 0, // Selected rating for review
      currentOfferId: null, // ID of the current offer being reviewed
    };
  },
  mounted() {
    this.fetchUserOffers(); // Fetch user offers on component mount
    this.Securitybot(); // Check login status on component mount
  },
  methods: {
    // Method to check login status and redirect if not logged in
    Securitybot() {
      if (!sessionStorage.getItem("token")) { // If token is not present in sessionStorage
        Swal.fire({ // Show SweetAlert2 modal
          title: 'You are not logged In !', // Modal title
          text: 'Login First to continue.', // Modal message
          icon: 'info', // Info icon
          confirmButtonText: 'OK' // Confirmation button text
        }).then(() => {
          router.push('/login'); // Redirect to login page using Vue router
        });
      }
    },
    // Method to check if user can add a rating
    async checkRatings() {
      try {
        const response = await axios.post(`${process.env.baseURL}add-rating-to-user`, {
          user_Id: toUserId,
          offerId: offerId,
          userRating: userRating,
          submissionDate: new Date().toISOString()
        });
      } catch (error) {
        console.error('Error checking ratings:', error);
      } finally {
        this.loading = false;
      }
    },
    // Method to fetch user offers from the server
    async fetchUserOffers() {
      try {
        const testlogid = this.decryptlogID(sessionStorage.getItem("logId")); // Decrypt log ID from sessionStorage
        globalLogid = testlogid; // Assign decrypted log ID to global variable
        const response = await axios.get(`${process.env.baseURL}review/get-reviews-for-user-offers/${globalLogid}`); // HTTP GET request to fetch user offers
        this.offers = response.data; // Assign fetched user offers to component data
      } catch (error) {
        console.error('Error fetching user offers:', error); // Log error if fetching user offers fails
      } finally {
        this.loading = false; // Set loading state to false after fetch completes
      }
    },
    // Method to decrypt an encrypted item
    decryptlogID(encryptedItem) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY); // Decrypt encrypted item
        const decryptedString = bytes.toString(CryptoJS.enc.Utf8); // Convert decrypted bytes to UTF-8 string
        return parseInt(decryptedString, 10).toString(); // Parse decrypted string as integer and return
      } catch (e) {
        console.error('Error decrypting item:', e); // Log error if decryption fails
        return null; // Return null if decryption fails
      }
    },
    // Method to approve an offer
    async approveOffer(reviewId) {
      try {
        const response = await axios.put(`${process.env.baseURL}review/update-review-status?reviewId=${reviewId}&newStatus=1`); // HTTP PUT request to approve offer
        const index = this.offers.findIndex(item => item.id === reviewId); // Find index of offer in array
        if (index !== -1) {
          this.offers[index].status = 1; // Update status of the approved offer
        }
      } catch (error) {
        console.error('Error approving offer:', error); // Log error if approving offer fails
      }
    },
    // Method to reject an offer
    async rejectOffer(reviewId) {
      try {
        const response = await axios.put(`${process.env.baseURL}review/update-review-status?reviewId=${reviewId}&newStatus=2`); // HTTP PUT request to reject offer
        const index = this.offers.findIndex(item => item.id === reviewId); // Find index of offer in array
        if (index !== -1) {
          this.offers[index].status = 2; // Update status of the rejected offer
        }
      } catch (error) {
        console.error('Error rejecting offer:', error); // Log error if rejecting offer fails
      }
    },
    // Method to view details of a review
    async viewDetail(reviewId) {
      try {
        const response = await axios.get(`${process.env.baseURL}review/get-user-by-review-id/${reviewId}`); // HTTP GET request to fetch review details
        await Swal.fire({ // Show SweetAlert2 modal with review details
          title: "Review Details", // Modal title
          html: `<p>User: ${response.data.firstName} ${response.data.lastName}</p>` + // Display user details in modal
            `<p>Email: ${response.data.email_Address}</p>` +
            `<p>Facebook Link: <a href="${response.data.facebook_link}">${response.data.facebook_link}</a></p>`,
          icon: "info", 
          confirmButtonText: "Close" // Confirmation button text
        });
      } catch (error) {
        console.error('Error fetching review details:', error); // Log error if fetching review details fails
        await Swal.fire("Error", "Failed to fetch review details.", "error"); // Show error modal for failed fetch
      }
    },
    // Method to show modal for adding a review
    async showAddReviewModal(offerId) {
      const predefinedReview = "Bitte gebt die gegenseitige Bewertung erst ab, nachdem diese terminlich abgeschlossen ist."; // Predefined review text
      const { value: review, dismiss: dismissAction } = await Swal.fire({ // Show SweetAlert2 modal for adding review
        title: 'Add Review', // Modal title
        html: `<textarea id="reviewTextArea" class="swal2-textarea" readonly>${predefinedReview}</textarea>`, // Textarea with predefined review text
        focusConfirm: false, // Do not focus on confirm button initially
        showCancelButton: true, // Show cancel button
        cancelButtonText: 'Cancel', // Cancel button text
        confirmButtonText: 'Submit', // Confirm button text
      });
      if (review !== undefined) { // If review is defined (not cancelled)
        await this.addReview(offerId, predefinedReview); // Add the review
      } else if (dismissAction === Swal.DismissReason.cancel) { // If modal was cancelled
      //  Swal.fire('Cancelled', 'Review submission cancelled.', 'info'); // Show info message for cancelled review submission
      }
    },
    // Method to add a review
    async addReview(offerId, reviewText) {
      try {
        const response = await axios.post(`${process.env.baseURL}review-offer-user/create-review-offer-user`, { // HTTP POST request to add review
          offerId,
          userId: globalLogid,
          addReviewForOfferUser: reviewText,
        });
        if (response.status === 200) {
          Swal.fire('Review Added', 'Your review has been successfully added.', 'success'); // Show success message for added review
        } else {
          Swal.fire('Error', 'Failed to add review.', 'error'); // Show error message for failed review addition
        }
      } catch (error) {
        Swal.fire('Already Added', 'You Have Already Added Review'); // Show error message for failed review addition
      }
    },
    // Method to show modal for adding a rating
    async showAddRatingModal(offerId, userId) {
      this.selectedRating = 0; // Reset selected rating
      this.showModal = true; // Show modal
      this.currentOfferId = offerId; // Set current offer ID
      globalratingId=userId; // Set global log ID
    },
    // Method to select a star rating
    selectStar(rating) {
      this.selectedRating = rating; // Set selected rating
    },
    // Method to submit a rating
    async submitRating() {
      if (this.selectedRating > 0) { // If a rating is selected
        await this.addRating(this.currentOfferId, globalratingId, this.selectedRating); // Add the rating
        this.showModal = false; // Hide the modal
      } else {
        Swal.fire('Error', 'Please select a rating.', 'error'); // Show error message for no selected rating
      }
    },
    // Method to cancel rating submission
    cancelRating() {
      this.showModal = false; // Hide the modal
    },
    // Method to add a rating
    async addRating(offerId, toUserId, userRating) {
      try {
        const response = await axios.post(`${process.env.baseURL}add-rating-to-user`, { // HTTP POST request to add rating
          user_Id: toUserId,
          offerId: offerId,
          userRating: userRating,
          submissionDate: new Date().toISOString
        });

        if (response.status === 200) {
          Swal.fire('Rating Added', 'Your rating has been successfully added.', 'success');
        } else {
          Swal.fire('Error', 'Failed to add rating.', 'error');
        }
      } catch (error) {
        Swal.fire('Already Added', 'You Have Already Added Rating!' , '');
      }
    }
  },
  async addReview(offerId, reviewText) {
      try {
        const response = await axios.post(`${process.env.baseURL}review-offer-user/create-review-offer-user`, {
          offerId,
          userId: globalLogid,
          addReviewForOfferUser: reviewText,
        });
 
        if (response.status === 200) {
          Swal.fire('Review Added', 'Your review has been successfully added.', 'success');
        } else {
          Swal.fire('Error', 'Failed to add review.', 'error');
        }
      } catch (error) {
        Swal.fire('Error', 'Failed to add review: ' + error.message, 'error');
      }
    },
 
 
};
</script>


<style scoped>
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

.btn-success {
  color: #fff;
  background-color: #2e9946;
  border-color: #1e7e34;
}
</style>
