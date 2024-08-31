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
import axios from 'axios';
import Swal from 'sweetalert2';
import router from '@/router';
window.FontAwesomeConfig = { autoReplaceSvg: false };
import CryptoJS from 'crypto-js';

let globalLogid = '';
let globalratingId = '';
export default {
  data() {
    return {
      email: sessionStorage.getItem("logEmail"),
      loading: true,
      offers: [],
      showModal: false,
      selectedRating: 0,
      currentOfferId: null,
    };
  },
  mounted() {
    this.fetchUserOffers();
    this.Securitybot();
  },
  methods: {
    // Method to check login status and redirect if not logged in
    Securitybot() {
      if (!sessionStorage.getItem("token")) {
        Swal.fire({
          title: 'You are not logged In !',
          text: 'Login First to continue.',
          icon: 'info',
          confirmButtonText: 'OK'
        }).then(() => {
          router.push('/login');
        });
      }
    },
    // Method to check if user can add a rating
    async checkRatings() {
      try {
        const response = await axios.post(`${process.env.baseURL}user-rating/add-rating-to-user`, {
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
        const testlogid = this.decryptlogID(sessionStorage.getItem("logId"));
        globalLogid = testlogid;
        const response = await axios.get(`${process.env.baseURL}review/get-reviews-for-user-offers/${globalLogid}`);
        this.offers = response.data;
      } catch (error) {
        console.error('Error fetching user offers:', error);
      } finally {
        this.loading = false;
      }
    },
    // Method to decrypt an encrypted item
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
    // Method to approve an offer
    async approveOffer(reviewId) {
      try {
        const response = await axios.put(`${process.env.baseURL}review/update-review?reviewId=${reviewId}&newStatus=1`);
        const index = this.offers.findIndex(item => item.id === reviewId);
        if (index !== -1) {
          this.offers[index].status = 1;
        }
      } catch (error) {
        console.error('Error approving offer:', error);
      }
    },
    // Method to reject an offer
    async rejectOffer(reviewId) {
      try {
        const response = await axios.put(`${process.env.baseURL}review/update-review?reviewId=${reviewId}&newStatus=2`);
        const index = this.offers.findIndex(item => item.id === reviewId);
        if (index !== -1) {
          this.offers[index].status = 2;
        }
      } catch (error) {
        console.error('Error rejecting offer:', error);
      }
    },
    // Method to view details of a review
    async viewDetail(reviewId) {
      try {
        const response = await axios.get(`${process.env.baseURL}review/get-user-by-review-id/${reviewId}`);
        await Swal.fire({
          title: "Review Details",
          html: `<p>User: ${response.data.firstName} ${response.data.lastName}</p>` +
            `<p>Email: ${response.data.email_Address}</p>` +
            `<p>Facebook Link: <a href="${response.data.facebook_link}">${response.data.facebook_link}</a></p>`,
          icon: "info",
          confirmButtonText: "Close"
        });
      } catch (error) {
        console.error('Error fetching review details:', error);
        await Swal.fire("Error", "Failed to fetch review details.", "error");
      }
    },
    // Method to show modal for adding a review
    async showAddReviewModal(offerId) {
      const predefinedReview = "Bitte gebt die gegenseitige Bewertung erst ab, nachdem diese terminlich abgeschlossen ist.";
      const { value: review, dismiss: dismissAction } = await Swal.fire({
        title: 'Add Review',
        html: `<textarea id="reviewTextArea" class="swal2-textarea" readonly>${predefinedReview}</textarea>`,
        focusConfirm: false,
        showCancelButton: true,
        cancelButtonText: 'Cancel',
        confirmButtonText: 'Submit',
      });
      if (review !== undefined) {
        await this.addReview(offerId, predefinedReview);
      } else if (dismissAction === Swal.DismissReason.cancel) {
      }
    },
    // Method to add a review
    async addReview(offerId, reviewText) {
      try {
        const response = await axios.post(`${process.env.baseURL}review-user-offer/create-review`, {
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
        Swal.fire('Already Added', 'You Have Already Added Review');
      }
    },
    // Method to show modal for adding a rating
    async showAddRatingModal(offerId, userId) {
      this.selectedRating = 0;
      this.showModal = true;
      this.currentOfferId = offerId;
      globalratingId = userId;
    },
    // Method to select a star rating
    selectStar(rating) {
      this.selectedRating = rating;
    },
    // Method to submit a rating
    async submitRating() {
      if (this.selectedRating > 0) {
        await this.addRating(this.currentOfferId, globalratingId, this.selectedRating);
        this.showModal = false;
      } else {
        Swal.fire('Error', 'Please select a rating.', 'error');
      }
    },
    cancelRating() {
      this.showModal = false;
    },
    // Method to add a rating
    async addRating(offerId, toUserId, userRating) {
      try {
        const response = await axios.post(`${process.env.baseURL}user-rating/add-rating-to-user`, {
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
        Swal.fire('Already Added', 'You Have Already Added Rating!', '');
      }
    }
  },
  async addReview(offerId, reviewText) {
    try {
      const response = await axios.post(`${process.env.baseURL}review-user-offer/create-review`, {
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
