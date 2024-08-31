<template>
  <div class="v-container account-page" style="margin: auto; max-width: 700px;">
    <div class="card" style="width: 620px;">
      <h2 class="text-center">User Profile</h2>
      <div class="card-body">
        <img :src="profileImgSrc" class="profile-img" alt="User Profile Picture">
        <h5 class="card-title">{{ user.firstName }} {{ user.lastName }}
          <span class="verState" v-if="user.verificationState === 3">
            <i class="ri-shield-star-line" style="color: green;"></i>
          </span>
        </h5>
        <div class="card-text-group ">
          <p class="card-text text-center "><strong>Ratings: </strong>
            <span v-for="(star, index) in stars" :key="index" class="star" :class="starClass(star)"></span>
            <span class="average-rating">({{ rate.averageRating }}/5) - {{ rate.ratingsCount }} votes</span>
          </p>
          <p class="card-text "><strong>Email:</strong> {{ user.email_Address }}</p>
          <p class="card-text"><strong>Geburtsdatum:</strong> {{ user.dateOfBirth }}</p>
          <p class="card-text"><strong>Geschlecht:</strong> {{ user.gender }}</p>
          <p class="card-text"><strong>Land:</strong> {{ user.country }}</p>
          <p class="card-text"><strong>Region/State:</strong> {{ user.state }}</p>
          <p class="card-text"><strong>Stadt:</strong> {{ user.city }}</p>
          <p class="card-text"><strong>Postleitzahl:</strong> {{ user.postCode }}</p>
          <p class="card-text"><strong>Adresse:</strong> {{ user.street }}</p>
          <p class="card-text"><strong>Hausnummmer:</strong> {{ user.houseNumber }}</p>
          <p class="card-text">
            <strong>Facebook Link:</strong><br />
            <a :href="user.facebook_link" target="_blank">{{ user.facebook_link }}</a>&nbsp;
            <button class="btn btn-dark border" @click="copyToClipboard(user.facebook_link)">Copy</button>
          </p>
          <p class="card-text"><strong>Hobbies:</strong><br /> {{ hobbies }}</p>
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

let globalLogId = ''; 
const axiosInstance = axios.create();
// Adding a request interceptor to the axios instance
axiosInstance.interceptors.request.use(
  config => {
    const token = sessionStorage.getItem('token');
    if (token) {
      const decryptedToken = decryptToken(token);
      if (decryptedToken) {
        config.headers['Authorization'] = `Bearer ${decryptedToken}`;
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

// Function to decrypt token
const decryptToken = (encryptedToken) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    console.error('Error decrypting token:', e);
    return null;
  }
};

// Enum-like object for profile options
const ProfileOptions = {
  None: 0,
  Smoker: 1 << 0,
  PetOwner: 1 << 1,
  HaveLiabilityInsurance: 1 << 2
};

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
    this.fetchUserData(sessionStorage.getItem('UserId'));
    this.fetchUserRating(sessionStorage.getItem('UserId'));
    this.checkLoginStatus();
  },
  methods: {
    // Method to check if the user is logged in
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
    // Method to check login status and set the user role
    checkLoginStatus() {
      const token = sessionStorage.getItem("token");
      if (token) {
        const testlogid = this.decryptlogID(sessionStorage.getItem("logId"));
        globalLogId = testlogid;
        const decryptedToken = this.decryptToken(token);
        if (decryptedToken) {
          const decodedToken = VueJwtDecode.decode(decryptedToken);
          this.userRole = decodedToken[`${process.env.claims_Url}`] || '';
        } else {
          sessionStorage.removeItem('token');
        }
      }
    },
    // Method to decrypt log ID
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
    // Method to decrypt token, redefined inside methods for instance access
    decryptToken(encryptedToken) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
        return bytes.toString(CryptoJS.enc.Utf8);
      } catch (e) {
        console.error('Error decrypting token:', e);
        return null;
      }
    },
    // Method to fetch user data
    async fetchUserData(id) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}admin/get-user-profile/${id}`);
        this.user = response.data.profile.user;
        this.profileImgSrc = `data:image/jpeg;base64,${response.data.profile.userPic}`;
        this.options = this.processOptions(response.data.profile.options);
        this.hobbies = response.data.profile.hobbies;
      } catch (error) {
        console.error("Error Fetching User data:", error);
        Swal.fire('Error', 'Failed to fetch user data', 'error');
      }
    },
    // Method to process profile options
    processOptions(options) {
      const result = [];
      if (options & ProfileOptions.Smoker) result.push('raucht');
      if (options & ProfileOptions.PetOwner) result.push('besitzt Tier(e)');
      if (options & ProfileOptions.HaveLiabilityInsurance) result.push('Haftpflichtversichert');
      return result;
    },
    // Method to navigate to edit profile page
    editProfile() {
      router.push('/editprofile');
    },
    // Method to fetch user rating
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
    // Method to get the appropriate class for star rating
    starClass(star) {
      return {
        'ri-star-fill gold': star === 'full',
        'ri-star-half-s-line gold': star === 'half',
        'ri-star-line': star === 'empty'
      };
    }
  },
  computed: {
    // Computed property to generate stars for rating
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
