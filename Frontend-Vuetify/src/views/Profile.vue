<template>
  <div class="v-container account-page" style="margin: auto; max-width: 700px;">
    <div class="card" style="width: 620px;">
      <h2 class="text-center">User Profile</h2>
      <div class="card-body">
        <img :src="profileImgSrc" class="profile-img" alt="User Profile Picture">
        <div>
          <h5 class="card-title">{{ user.firstName }} {{ user.lastName }}
          </h5>
          <div v-if="user.verificationState === 3">
            <h5 class="card-title"> <span class="verState">
                <i class="ri-shield-star-line" style="color: green;">Verified</i>
              </span> </h5>
          </div>
          <div v-else style="display: flex;justify-content: center;">
            <a class="btn col-3 mb-2" style="background-color:rgb(0, 189, 214);color: white;" @click="upload_id">Upload
              ID</a>
          </div>
        </div>


        <div class="card-text-group ">
          <p v-if="userRole != 'Admin'" class="card-text text-center "><strong>Ratings: </strong>
            <span v-for="(star, index) in stars" :key="index" class="star" :class="starClass(star)"></span>
            <span class="average-rating">({{ rate.averageRating }}/5) - {{ rate.ratingsCount }} votes</span>
          </p>

          <p class="card-text "><strong>Email:</strong> {{ user.email_Address }}</p>
          <p class="card-text"><strong>Date of Birth:</strong> {{ user.dateOfBirth }}</p>
          <p class="card-text"><strong>Gender:</strong> {{ user.gender }}</p>
          <p class="card-text"><strong>Country:</strong> {{ user.country }}</p>
          <p class="card-text"><strong>City:</strong> {{ user.city }}</p>
          <p class="card-text"><strong>State/Region:</strong> {{ user.state }}</p>
          <p class="card-text"><strong>Postal Code:</strong> {{ user.postCode }}</p>
          <p class="card-text"><strong>Street Address:</strong> {{ user.street }}</p>
          <p class="card-text"><strong>House No:</strong> {{ user.houseNumber }}</p>
          <p class="card-text">
            <strong>Facebook Link:</strong><br />
            <a :href="user.facebook_link" target="_blank">{{ user.facebook_link }}</a>&nbsp;
            <button class="btn btn-dark border" @click="copyToClipboard(user.facebook_link)">Copy</button>
          </p>
          <p class="card-text"><strong>Options:</strong><br /> {{ options.join(', ') }}</p>
          <p class="card-text"><strong>Hobbies:</strong><br /> {{ hobbies }}</p>

          <div class="d-flex" style="justify-content: space-around;" v-if="user.verificationState === 3">
            <button class="btn-primary col-5 rounded" style="height: 38px;" @click="editProfile">Edit Profile</button>
            <button class="btn-primary col-5 rounded" style="height: 38px;" @click="shareProfile">Copy Link</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import router from "@/router"; 
import axios from "axios"; 
import Swal from 'sweetalert2'; 
import CryptoJS from 'crypto-js'; 
import VueJwtDecode from 'vue-jwt-decode'; 

const axiosInstance = axios.create(); // Creating axios instance for API requests

axiosInstance.interceptors.request.use(
  config => {
    // Interceptor to modify outgoing request headers
    const token = sessionStorage.getItem('token'); // Retrieve JWT token from sessionStorage
    if (token) {
      const decryptedToken = decryptToken(token); // Decrypt JWT token
      if (decryptedToken) {
        config.headers['Authorization'] = `Bearer ${decryptedToken}`; // Set Authorization header
      } else {
        sessionStorage.removeItem('token'); // Remove token from sessionStorage if decryption fails
      }
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);
// Function to navigate to the upload ID page
const upload_id = () => {
  router.push("/uploadID").then(() => {

  });
};
// Function to decrypt JWT token
const decryptToken = (encryptedToken) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY); // Decrypt token using AES
    return bytes.toString(CryptoJS.enc.Utf8); // Convert decrypted bytes to UTF-8 string
  } catch (e) {
    console.error('Error decrypting token:', e); // Log error if decryption fails
    return null;
  }
};

// Profile options bitmask enumeration
const ProfileOptions = {
  None: 0,
  Smoker: 1 << 0,
  PetOwner: 1 << 1,
  HaveLiabilityInsurance: 1 << 2
};

// Global variable for storing log ID
let globalLogId = '';
let globalToken = '';
export default {
  name: "UserCard", // Vue component name
  data() {
    return {
      user: {}, // Object to store user data
      profileImgSrc: '', // String to store profile image source
      options: [], // Array to store user profile options
      hobbies: '', // String to store user hobbies
      rate: {}, // Object to store user rating data
      userRole: '', // String to store user role
    };
  },
  mounted() {
    this.Securitybot(); // Function to check login status and redirect if not logged in
    this.checkLoginStatus(); // Function to check and set user login status
    this.fetchUserData(globalToken); // Function to fetch user data using global log ID
    this.fetchUserRating(globalLogId); // Function to fetch user rating using global log ID
  },
  methods: {
    // Function to check if user is logged in; if not, show alert and redirect to login page
    Securitybot() {
      if (!sessionStorage.getItem("token")) {
        Swal.fire({
          title: 'You are not logged In!',
          text: 'Login First to continue.',
          icon: 'info',
          confirmButtonText: 'OK'
        });
        router.push('/login'); // Redirect to login page using Vue router
      }
    },
    // Function to check user login status and retrieve user role from JWT token
    checkLoginStatus() {
      const token = sessionStorage.getItem("token"); // Retrieve JWT token from sessionStorage
      const decryptToken = this.decryptToken(token);
      globalToken = decryptToken;
      const testlogid = this.decryptlogID(sessionStorage.getItem("logId")); // Decrypt log ID from sessionStorage
      globalLogId = testlogid; // Set global log ID
      if (token) {
        const decryptedToken = this.decryptToken(token); // Decrypt JWT token
        if (decryptedToken) {
          const decodedToken = VueJwtDecode.decode(decryptedToken); // Decode JWT token
          this.userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || '';
          // Set user role from decoded JWT token
        } else {
          sessionStorage.removeItem('token'); // Remove token from sessionStorage if decryption fails
        }
      }
    },
    // Function to navigate to the upload ID page
    upload_id() {
      router.push("/uploadID").then(() => {
      });
    },

    // Function to decrypt encrypted token using AES decryption
    decryptToken(encryptedToken) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
        return bytes.toString(CryptoJS.enc.Utf8); // Convert decrypted bytes to UTF-8 string
      } catch (e) {
        console.error('Error decrypting token:', e); // Log error if decryption fails
        return null;
      }
    },
    // Function to decrypt encrypted log ID and parse it as an integer
    decryptlogID(encryptedItem) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY);
        const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
        return parseInt(decryptedString, 10).toString(); // Parse decrypted string as integer
      } catch (e) {
        console.error('Error decrypting item:', e); // Log error if decryption fails
        return null;
      }
    },
    // Function to fetch user data using API request
    async fetchUserData(id) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}profile/get-profile?token=${id}`);
        this.user = response.data.profile.user; // Set user data from API response
        this.profileImgSrc = `data:image/jpeg;base64,${response.data.profile.userPic}`; // Set profile image source
        this.options = this.processOptions(response.data.profile.options); // Process user profile options
        this.hobbies = response.data.profile.hobbies; // Set user hobbies
      } catch (error) {
        if (error.response && error.response.status === 500) {
          Swal.fire('Session Expired', 'Your session has expired. Please log in again.', 'error'); // Show session expired alert
          sessionStorage.clear();
          router.push('/'); // Redirect to login page using Vue router
        } else {
          console.error("Error Fetching User data:", error); // Log error if fetching user data fails
          Swal.fire('Error', 'Failed to fetch user data', 'error'); // Show alert for other errors
        }
      }
    },

    // Function to process user profile options bitmask and return corresponding array
    processOptions(options) {
      const result = [];
      if (options & ProfileOptions.Smoker) result.push('Smoker'); // Check if user is a smoker
      if (options & ProfileOptions.PetOwner) result.push('Pet Owner'); // Check if user is a pet owner
      if (options & ProfileOptions.HaveLiabilityInsurance) result.push('Have Liability Insurance'); // Check if user has liability insurance
      return result; // Return processed options array
    },
    // Function to navigate to edit profile page
    editProfile() {
      router.push('/editprofile'); // Redirect to edit profile page using Vue router
    },
    // Function to share user profile and generate shortened link
    async shareProfile() {

      const uniqueLink = `${process.env.baseURL_Frontend}display-profile/?token=${globalToken}`; // Construct unique profile link
      try {
        const response = await axios.post('https://api.shorten.rest/aliases', {
          aliasName: `a257/@rnd`,
          destinations: [{ url: uniqueLink }],
        }, {
          headers: {
            'x-api-key': '79ea7130-21a6-11ef-9447-bb18dd052b84', // API key for short link generation
            'Content-Type': 'application/json', // Specify content type as JSON
          },
        });

        const shortLink = response.data.shortUrl; // Extract shortened URL from API response
        navigator.clipboard.writeText(shortLink) // Copy shortened link to clipboard
          .then(() => {
            Swal.fire('Link Copied ✔✔', `Shortened link copied to clipboard! ${shortLink}`, 'success'); // Show success alert
          })
          .catch(() => {
            Swal.fire('Error', 'Failed to copy shortened link to clipboard', 'error'); // Show error alert if copying fails
          });
      } catch (error) {
        console.error('Error generating short link:', error); // Log error if short link generation fails
        Swal.fire('Error', 'Failed to generate short link', 'error'); // Show error alert
      }
    },
    // Function to copy text to clipboard
    copyToClipboard(text) {
      navigator.clipboard.writeText(text) // Copy text to clipboard
        .then(() => {
          Swal.fire('Copied', 'The URL has been copied to the clipboard', 'success'); // Show success alert
        })
        .catch(err => {
          console.error('Could not copy text: ', err); // Log error if copying fails
          Swal.fire('Error', 'Failed to copy the URL', 'error'); // Show error alert
        });
    },
    // Function to fetch user rating using API request
    async fetchUserRating(id) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}get-rating-by-user-id/${id}`);
        this.rate = response.data; // Set user rating data from API response
        this.user.averageRating = response.data.averageRating; // Set average rating
        this.user.ratingsCount = response.data.ratingsCount; // Set ratings count
      } catch (error) {
        // console.error("Error Fetching User Rating:", error); // Log error if fetching user rating fails
      }
    },
    // Function to dynamically set star class based on rating
    starClass(star) {
      return {
        'ri-star-fill gold': star === 'full', // Filled star class
        'ri-star-half-s-line gold': star === 'half', // Half-filled star class
        'ri-star-line': star === 'empty' // Empty star class
      };
    }
  },
  computed: {
    stars() {
      const stars = [];
      const integerPart = Math.floor(this.rate.averageRating);
      const decimalPart = this.rate.averageRating - integerPart;
      for (let i = 0; i < 5; i++) {
        if (i < integerPart) {
          stars.push('full');
        } else if (decimalPart >= 0.5 && i === integerPart) {
          stars.push('half');
        } else {
          stars.push('empty');
        }
      }
      return stars;
    }
  }
};
</script>
<style scoped>
.v-container {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
}

.account-page {
  background-color: #f8f9fa;
  padding: 30px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  max-width: 600px;
  width: 100%;
}

.card {
  background-color: #fff;
  border: none;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
}

.card-body {
  padding: 20px;
}

.card-title {
  margin-bottom: 20px;
  font-size: 24px;
  font-weight: bold;
  color: #333;
  text-align: center;
}

.profile-img {
  width: 100%;
  height: auto;
  border-radius: 50%;
  margin-bottom: 20px;
}

.card-text-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.card-text {
  font-size: 16px;
  color: #555;
}

.card-text strong {
  color: #333;
}

.card-text a {
  color: #007bff;
  text-decoration: none;
}

.card-text a:hover {
  text-decoration: underline;
}

.btn-primary {
  margin-top: 20px;
  display: block;
  width: 100%;
  text-align: center;
}

.star {
  font-size: 1.2em;
}

.gold {
  color: gold;
}

.average-rating {
  font-size: 0.9em;
  color: #555;
  margin-left: 10px;
}
</style>