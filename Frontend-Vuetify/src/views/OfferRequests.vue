<template>
  <Navbar />
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2>Offer Request</h2>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div class="section_space offers_request_layout">
    <div class="offers_request_content">
      <div class="card">
        <div class="card-header">
          <h1 class="main-title">Offers Requests</h1>
          <div><router-link class="action-link" to="/my-offers"> <i class="ri-arrow-left-double-fill"></i> Back To
              Offers
              <span class="sr-only">(current)</span></router-link></div>
        </div>
        <div class="card-body">
          <div v-if="loading" class="spinner-container text-center">
            <div class="spinner"></div>
          </div>
          <div v-else>
            <div class="table-responsive">
              <table v-if="offers && offers.length > 0" class="table theme_table">
                <thead>
                  <tr>
                    <th>User</th>
                    <th>Offer title</th>
                    <th>Action</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, index) in offers" :key="item.id">
                    <td><a class="view_user" style="cursor: pointer;" @click="showUserDetails(item.user.user_Id)">
                        <span class="user_img" v-if="item.user.profilePicture != null">
                          <img loading="lazy" class="" alt="User Profile Picture"
                            :src="'data:' + 'image/jpeg' + ';base64,' + item.user.profilePicture">
                        </span>
                        <span class="user_img" v-if="item.user.profilePicture == null">
                          <img loading="lazy" class="" alt="User Profile Picture" :src="defaultProfileImgSrc">
                        </span>
                        {{
                          item.user.firstName }} {{ item.user.lastName }}</a> </td>
                    <td>
                      <a class="view_user" style="cursor: pointer;" @click="redirectToOfferDetail(item.offer.id)">{{
                        item.offer.title }}</a>
                    </td>
                    <td>
                      <div class="btn_flexBox now_wrap buttons_text">
                        <button v-if="item.status === 0" @click="approveOffer(item.offer.id, item.user.user_Id)"
                          class="icon_btn bg_ltgreen" title="Accept">Accept </button>

                        <button v-if="item.status === 0" @click="rejectOffer(item.offer.id, item.user.user_Id)"
                          class="icon_btn bg_ltred" title="Reject">Reject </button>

                        <button v-if="item.status === 2" class="icon_btn bg_ltred" disabled
                          title="Rejected">Rejected</button>

                        <button v-if="item.status === 1"
                          @click="showAddRatingModal(item.offer.id, item.user.user_Id, index)"
                          class="icon_btn bg_ltblue" title="Add Rating and Review">Add Rating
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
              <div v-else>
                <p class="text-center">No offers found for this user.</p>
              </div>
            </div>

          </div>

        </div>

      </div>
    </div>

    <div v-if="showModal" class="overlay"></div>
    <div id="rating-modal" class="rating-modal" v-if="showModal">
      <div class="modal-content rating-modal-content">
        <div class="review_rating_layout">
          <div class="review_header">
            <div class="photo">
              <img loading="lazy"
                :src="'data:' + offers[this.offerRequestIndex].offer.imageMimetype + ';base64,' + offers[this.offerRequestIndex].offer.imageData"
                alt="Offer Image" />
            </div>
            <div class="rightSideBox">
              <h5>{{ offers[this.offerRequestIndex].offer.title }}</h5>
              <p class="hostName">{{ offers[this.offerRequestIndex].user.firstName }} {{
                offers[this.offerRequestIndex].user.lastName }}</p>
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
    <!-- Pagination Section -->
    <div class="pagination">
      <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1"><i
          class="ri-arrow-left-s-line"></i>Previous</button>
      <span>Page {{ currentPage }} of {{ totalPages }}</span>
      <button class="action-link" @click="changePage(currentPage + 1)" :hidden="currentPage === totalPages">Next<i
          class="ri-arrow-right-s-line"></i></button>
    </div>
  </div>
</template>

<script>
import router from '@/router';
import Navbar from '@/components/navbar/Navbar.vue';
import Securitybot from '@/services/SecurityBot';
import axiosInstance from '@/interceptor/interceptor';
import toast from '@/components/toaster/toast';

let globalratingId = '';

export default {
  components: {
    Navbar,
  },
  data() {
    return {
      email: sessionStorage.getItem("logEmail"),
      loading: true,
      offers: [],
      showModal: false,
      selectedRating: 0,
      currentOfferId: null,
      reviewText: '',
      ratingModalData: [],
      offerRequestIndex: 0,
      defaultProfileImgSrc: '/defaultprofile.jpg',
      currentIndex: 0,
      currentPage: 1,
      totalPages: 1,
      pageSize: 10,
    };
  },
  mounted() {
    this.fetchUserOffers();
    Securitybot();
  },
  methods: {
    changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.currentPage = newPage;
        this.fetchUserOffers(); // fetch new data for the selected page
      }
    },
    async checkRatings() {
      try {
        const response = await axiosInstance.post(`${process.env.baseURL}user-rating/add-rating-to-user`, {
          user_Id: toUserId,
          offerId: offerId,
          userRating: userRating,
          submissionDate: new Date().toISOString()
        });
      } catch (error) {
      } finally {
        this.loading = false;
      }
    },
    // Method to fetch user offers from the server
    async fetchUserOffers() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}offer/offer-applications`, {
          params: {
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
    // Method to approve an offer
    async approveOffer(reviewId, userId) {
      try {
        const response = await axiosInstance.put(`${process.env.baseURL}offer/update-application-status?offerId=${reviewId}&isApprove=true&userId=${userId}`);
        const index = this.offers.findIndex(item => item.id === reviewId);
        if (index !== -1) {
          this.offers[index].status = 1;
        }
        this.fetchUserOffers();
      } catch (error) {
        this.fetchUserOffers();

      }
    },
    // Method to redirect to offer detail page
    redirectToOfferDetail(offerId) {
      this.$router.push({ name: 'OfferDetail', params: { id: offerId } });
    },
    // Method to reject an offer
    async rejectOffer(reviewId, userId) {
      try {
        const response = await axiosInstance.put(`${process.env.baseURL}offer/update-application-status?offerId=${reviewId}&isApprove=false&userId=${userId}`);
        const index = this.offers.findIndex(item => item.id === reviewId);
        if (index !== -1) {
          this.offers[index].status = 2;
        }
        this.fetchUserOffers();
      } catch (error) {
        // console.error('Error rejecting offer:', error);
      }
    },
    showUserDetails(userId) {
      sessionStorage.setItem("UserId", userId);
      router.push("/account");
    },

    // Method to show modal for adding a rating
    async showAddRatingModal(offerId, userId, index) {
      this.selectedRating = 0;
      this.showModal = true;
      this.currentOfferId = offerId;
      globalratingId = userId;
      this.offerRequestIndex = index;
    },
    // Method to select a star rating
    selectStar(rating) {
      this.selectedRating = rating;
    },
    // Method to submit rating
    async submitRating() {
      if (this.selectedRating > 0) {
        await this.addRating(this.currentOfferId, this.selectedRating, this.reviewText, globalratingId);
        this.cancelRating();
      } else {
        toast.info("Please select a Rating");
      }
    },
    cancelRating() {
      this.showModal = false;
      this.reviewText = '';
    },
    // Method to add rating for a specific offer
    async addRating(offerId, userRating, reviewText, userId) {
      try {
        const response = await axiosInstance.post(`${process.env.baseURL}review/add-review`, {

          offerId: offerId,
          ratingValue: userRating,
          reviewComment: reviewText,
          reviewedUserId: userId,
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
};
</script>

<style scoped>
.rating-star {
  font-family: var(--fa-style-family, "Font Awesome 6 Free");
  font-weight: var(--fa-style, 900);
}

.rating-star:before {
  content: "\f005";
}

.rating-modal-content {
  padding: 0;
  border: none;

}

.table {
  width: 100%;
  border-collapse: collapse;
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
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  max-width: 500px;
  width: 100%;
}

.modal-content {
  text-align: center;
}

.rating-stars {
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

.btn-primary {
  color: #fff;
}

.swal2-textarea {
  margin: 0;
  width: 100%;
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
</style>
