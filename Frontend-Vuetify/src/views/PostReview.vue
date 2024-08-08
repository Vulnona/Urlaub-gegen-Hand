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
import axios from 'axios'; // Import Axios for HTTP requests
import Swal from 'sweetalert2'; // Import SweetAlert2 for alerts
import router from '@/router'; // Import Vue router for navigation
import CryptoJS from 'crypto-js'; // Import CryptoJS for encryption

let globalLogid = ''; // Variable to store decrypted log ID

export default {
  data() {
    return {
      activeTab: 'host', // Active tab state for reviews (default: 'host')
      hostReviews: [], // Array to store host reviews
      userReviews: [] // Array to store user reviews
    };
  },
  mounted() {
    this.fetchPostReviews(); // Fetch post reviews on component mount
    this.Securitybot(); // Check login status on component mount
  },
  methods: {
    // Method to check login status and redirect if not logged in
    Securitybot() {
      if (!sessionStorage.getItem("token")) { // If token is not present in sessionStorage
        Swal.fire({ // Show SweetAlert2 modal
          title: 'You are not logged In!', // Modal title
          text: 'Login First to continue.', // Modal message
          icon: 'info', // Info icon
          confirmButtonText: 'OK' // Confirmation button text
        }).then(() => {
          router.push('/login'); // Redirect to login page using Vue router
        });
      }
    },
    // Method to fetch post reviews from the server
    async fetchPostReviews() {
      try {
        const testlogid = this.decryptlogID(sessionStorage.getItem("logId")); // Decrypt log ID from sessionStorage
        globalLogid = testlogid; // Assign decrypted log ID to global variable
        const response = await axios.get(`${process.env.baseURL}post-review/${globalLogid}`); // HTTP GET request to fetch reviews
        const reviews = response.data; // Assign fetched reviews to a variable
        // Separate host reviews and user reviews based on conditions
        this.hostReviews = reviews.filter(review => review.reviewOfferUsersID !== null); // Filter host reviews
        this.userReviews = reviews.filter(review => review.reviewLoginUsersID !== null); // Filter user reviews
      } catch (error) {
        console.error('Error fetching post review data:', error); // Log error if fetching reviews fails
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
    // Method to format a date into a readable string
    formatDate(date) {
      return new Date(date).toLocaleDateString('en-US', { // Format date in US English locale
        weekday: 'long', // Full weekday name (e.g., "Monday")
        year: 'numeric', // Full numeric year (e.g., "2024")
        month: 'long', // Full month name (e.g., "January")
        day: 'numeric' // Day of the month (e.g., "1")
      });
    }
  }
};
</script>


<style scoped>
.container {
  margin-top: 20px;
}

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
