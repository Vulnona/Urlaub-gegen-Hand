<template>
  <div class="container">
    <h1 class="title">Reviews</h1>
    <div class="tabs">
      <button :class="{ active: activeTab === 'host' }" @click="activeTab = 'host'">Host Reviews</button>
      <button :class="{ active: activeTab === 'user' }" @click="activeTab = 'user'">User Reviews</button>
    </div>
    <div v-if="activeTab === 'host'">
      <div v-if="hostReviews.length">
        <div v-for="hostReview in hostReviews" :key="hostReview.id" class="review">
          <p><strong>Review:</strong> {{ hostReview.offerUserReviewPost }}</p>
          <p><strong>Created At:</strong> {{ formatDate(hostReview.createdAt) }}</p>
          <hr />
        </div>
      </div>
      <div v-else>
        <p>No Host Reviews Found!</p>
      </div>
    </div>
    <div v-if="activeTab === 'user'">
      <div v-if="userReviews.length">
        <div v-for="userReview in userReviews" :key="userReview.id" class="review">
          <p><strong>Review:</strong> {{ userReview.loginUserReviewPost }}</p>
          <p><strong>Created At:</strong> {{ formatDate(userReview.createdAt) }}</p>
          <hr />
        </div>
      </div>
      <div v-else>
        <p>No User Reviews Found!</p>
      </div>
    </div>
  </div>
</template>
<script>
import Swal from 'sweetalert2';
import router from '@/router';
import CryptoJS from 'crypto-js';
import axiosInstance from '@/interceptor/interceptor';

let globalLogid = '';
export default {
  data() {
    return {
      activeTab: 'host',
      hostReviews: [],
      userReviews: []
    };
  },
  mounted() {
    this.fetchPostReviews();
    this.Securitybot();
  },
  methods: {
    // Method to check login status and redirect if not logged in
    Securitybot() {
      if (!sessionStorage.getItem("token")) {
        Swal.fire({
          title: 'You are not logged In!',
          text: 'Login First to continue.',
          icon: 'info',
          confirmButtonText: 'OK'
        }).then(() => {
          router.push('/login');
        });
      }
    },
    // Method to fetch post reviews from the server
    async fetchPostReviews() {
      try {
        const testlogid = this.decryptlogID(sessionStorage.getItem("logId"));
        globalLogid = testlogid;
        const response = await axiosInstance.get(`${process.env.baseURL}post-review/get-posted-review-by-user-id/${globalLogid}`);
        const reviews = response.data;
        // Separate host reviews and user reviews based on conditions
        this.hostReviews = reviews.filter(review => review.reviewOfferUsersID !== null);
        this.userReviews = reviews.filter(review => review.reviewLoginUsersID !== null);
      } catch (error) {
        console.error('Error fetching post review data:', error);
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
    // Method to format a date into a readable string
    formatDate(date) {
      return new Date(date).toLocaleDateString('en-US', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    }
  }
};
</script>
<style scoped>
/* .container {
  margin-top: 20px;
} */

.title {
  color: #333;
  font-size: 24px;
  margin-bottom: 20px;
}

.tabs {
  margin-bottom: 20px;
}

.tabs button {
  padding: 10px 20px;
  border: none;
  background-color: #e0e0e0;
  cursor: pointer;
  outline: none;
  font-size: 16px;
  margin-right: 10px;
}

.tabs button.active {
  background-color: #007bff;
  color: white;
}

.review {
  background-color: #f9f9f9;
  padding: 15px;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-bottom: 20px;
}

.review p {
  margin: 0;
}

.review p strong {
  font-weight: bold;
}

.review hr {
  border: none;
  border-top: 1px solid #ccc;
  margin: 10px 0;
}
</style>
