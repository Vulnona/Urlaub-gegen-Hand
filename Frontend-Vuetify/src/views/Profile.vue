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

const axiosInstance = axios.create();
axiosInstance.interceptors.request.use(
  config => {
    const token = sessionStorage.getItem('token');
    if (token) {
      const decryptedToken = decryptToken(token);
      if (decryptedToken) {
        config.headers['Authorization'] = `Bearer ${decryptedToken}`; // Set Authorization header
      } else {
        sessionStorage.removeItem('token');
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
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    console.error('Error decrypting token:', e);
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
  name: "UserCard",
  data() {
    return {
      user: {},
      profileImgSrc: '',
      options: [],
      hobbies: '',
      rate: {},
      userRole: '',
    };
  },
  mounted() {
    this.Securitybot();
    this.checkLoginStatus();
    this.fetchUserData(globalToken);
    this.fetchUserRating(globalLogId);
  },
  methods: {
    // Function to check if user is logged in
    Securitybot() {
      if (!sessionStorage.getItem("token")) {
        Swal.fire({
          title: 'You are not logged In!',
          text: 'Login First to continue.',
          icon: 'info',
          confirmButtonText: 'OK'
        });
        router.push('/login');
      }
    },
    // Function to check user login status and retrieve user role from JWT token
    checkLoginStatus() {
      const token = sessionStorage.getItem("token");
      const decryptToken = this.decryptToken(token);
      globalToken = decryptToken;
      const testlogid = this.decryptlogID(sessionStorage.getItem("logId"));
      globalLogId = testlogid;
      if (token) {
        const decryptedToken = this.decryptToken(token);
        if (decryptedToken) {
          const decodedToken = VueJwtDecode.decode(decryptedToken);
          this.userRole = decodedToken[`${process.env.claims_Url}`] || '';
        } else {
          sessionStorage.removeItem('token');
        }
      }
    },
    upload_id() {
      router.push("/uploadID").then(() => {
      });
    },
    // Function to decrypt encrypted token using AES decryption
    decryptToken(encryptedToken) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
        return bytes.toString(CryptoJS.enc.Utf8);
      } catch (e) {
        console.error('Error decrypting token:', e);
        return null;
      }
    },
    // Function to decrypt encrypted log ID and parse it as an integer
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
    // Function to fetch user data using API request
    async fetchUserData(id) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}profile/get-user-profile?token=${id}`);
        this.user = response.data.profile.user;
        this.profileImgSrc = `data:image/jpeg;base64,${response.data.profile.userPic}`;
        this.options = this.processOptions(response.data.profile.options);
        this.hobbies = response.data.profile.hobbies;
      } catch (error) {
        if (error.response && error.response.status === 500) {
          Swal.fire('Session Expired', 'Your session has expired. Please log in again.', 'error');
          sessionStorage.clear();
          router.push('/');
        } else {
          console.error("Error Fetching User data:", error);
          Swal.fire('Error', 'Failed to fetch user data', 'error');
        }
      }
    },
    // Function to process user profile options bitmask and return corresponding array
    processOptions(options) {
      const result = [];
      if (options & ProfileOptions.Smoker) result.push('Smoker');
      if (options & ProfileOptions.PetOwner) result.push('Pet Owner');
      if (options & ProfileOptions.HaveLiabilityInsurance) result.push('Have Liability Insurance');
      return result;
    },
    editProfile() {
      router.push('/editprofile');
    },
    // Function to share user profile and generate shortened link
    async shareProfile() {
      const uniqueLink = `${process.env.baseURL_Frontend}display-profile/?token=${globalToken}`;
      try {
        const response = await axios.post('https://api.shorten.rest/aliases', {
          aliasName: `a257/@rnd`,
          destinations: [{ url: uniqueLink }],
        }, {
          headers: {
            'x-api-key': '79ea7130-21a6-11ef-9447-bb18dd052b84',
            'Content-Type': 'application/json',
          },
        });
        const shortLink = response.data.shortUrl;
        navigator.clipboard.writeText(shortLink)
          .then(() => {
            Swal.fire('Link Copied ✔✔', `Shortened link copied to clipboard! ${shortLink}`, 'success');
          })
          .catch(() => {
            Swal.fire('Error', 'Failed to copy shortened link to clipboard', 'error');
          });
      } catch (error) {
        console.error('Error generating short link:', error);
        Swal.fire('Error', 'Failed to generate short link', 'error');
      }
    },
    // Function to copy text to clipboard
    copyToClipboard(text) {
      navigator.clipboard.writeText(text)
        .then(() => {
          Swal.fire('Copied', 'The URL has been copied to the clipboard', 'success');
        })
        .catch(err => {
          console.error('Could not copy text: ', err);
          Swal.fire('Error', 'Failed to copy the URL', 'error');
        });
    },
    // Function to fetch user rating using API request
    async fetchUserRating(id) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}user-rating/get-rating-by-user-id/${id}`);
        this.rate = response.data;
        if (this.rate.averageRating != null) {
          this.rate.averageRating = parseFloat(response.data.averageRating).toFixed(1);
          this.user.averageRating = parseFloat(response.data.averageRating).toFixed(1);
          this.user.ratingsCount = response.data.ratingsCount;
        }
      } catch (error) {
        console.error("Error Fetching User Rating:", error);
      }
    },
    // Function to dynamically set star class based on rating
    starClass(star) {
      return {
        'ri-star-fill gold': star === 'full',
        'ri-star-half-s-line gold': star === 'half',
        'ri-star-line': star === 'empty'
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