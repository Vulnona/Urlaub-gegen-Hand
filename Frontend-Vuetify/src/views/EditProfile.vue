<template>
    <div class="col-6 offset-3" style="    margin-top: 20px;
    margin-bottom: 20px;">
      <div class="container col-6">
        <form @submit.prevent="saveProfile" class="profile-form">
          <div class="form-group">
            <label for="firstName">First Name</label>
            <input type="text" id="firstName" v-model="profile.user.firstName" class="form-control">
          </div>
          <div class="form-group">
            <label for="lastName">Last Name</label>
            <input type="text" id="lastName" v-model="profile.user.lastName" class="form-control">
          </div>
          <div class="form-group">
            <label for="dateOfBirth">Date of Birth</label>
            <input type="date" id="dateOfBirth" v-model="profile.user.dateOfBirth" class="form-control">
          </div>
          <div class="form-group">
            <label for="gender">Gender</label>
            <input type="text" id="gender" v-model="profile.user.gender" class="form-control" >
          </div>
          <div class="form-group">
            <label for="street">Street</label>
            <input type="text" id="street" v-model="profile.user.street" class="form-control">
          </div>
          <div class="form-group">
            <label for="houseNumber">House Number</label>
            <input type="text" id="houseNumber" v-model="profile.user.houseNumber" class="form-control">
          </div>
          <div class="form-group">
            <label for="postCode">Post Code</label>
            <input type="text" id="postCode" v-model="profile.user.postCode" class="form-control">
          </div>
          <div class="form-group">
            <label for="city">City</label>
            <input type="text" id="city" v-model="profile.user.city" class="form-control">
          </div>
          <div class="form-group">
            <label for="state">State/Region</label>
            <input type="text" id="state" v-model="profile.user.state" class="form-control">
          </div>
          <div class="form-group">
            <label for="country">Country</label>
            <input type="text" id="country" v-model="profile.user.country" class="form-control">
          </div>
          <div class="form-group">
            <label for="facebookLink">Facebook Link</label>
            <input type="text" id="facebookLink" v-model="profile.user.facebook_link" class="form-control">
          </div>
          <div class="form-group">
            <label for="userPic">User Picture</label>
            <input type="file" id="userPic" multiple @change="onFileChange" class="form-control">
          </div>
          <div class="form-check">
            <input type="checkbox" id="smoker" v-model="profileOptions.smoker" class="form-check-input">
            <label for="smoker" class="form-check-label">Smoker</label>
          </div>
          <div class="form-check">
            <input type="checkbox" id="petOwner" v-model="profileOptions.petOwner" class="form-check-input">
            <label for="petOwner" class="form-check-label">Pet Owner</label>
          </div>
          <div class="form-check">
            <input type="checkbox" id="liabilityInsurance" v-model="profileOptions.haveLiabilityInsurance" class="form-check-input">
            <label for="liabilityInsurance" class="form-check-label">Have Liability Insurance</label>
          </div>
          <div class="form-group">
            <button type="button" class="btn btn-primary col-2" style="color: #fff; background-color: #0d6efd;" @click="toggleHobbiesInput">+ Something Else</button>
          </div>
          <div v-if="showHobbiesInput" class="form-group">
            <label for="hobbies">Hobbies</label>
            <input type="text" id="hobbies" v-model="profile.hobbies" class="form-control">
          </div>
          <button type="submit" class="btn btn-success col-3" style="color: #fff; background-color: #28a745; float: right;">Save Profile</button>
        </form>
      </div>
    </div>
  </template>
  <script>
  import axios from 'axios';
  import Swal from 'sweetalert2';
  import router from '@/router';
  import CryptoJS from 'crypto-js';

  
  let globalToken = '';
  export default {
    data() {
      return {
        profile: {
          id: 0,
          user_Id: 0,
          user: {
            user_Id: 0,
            firstName: '',
            lastName: '',
            dateOfBirth: '',
            gender: '',
            street: '',
            houseNumber: '',
            postCode: '',
            city: '',
            state:'',
            country: '',
            email_Address: '',
            password: '',
            saltKey: '',
            isEmailVerified: false,
            facebook_link: '',
            verificationState: 0,
            currentMembership: {
              membershipID: 0,
              expiration: ''
            }
          },
          userPic: '',
          hobbies: '',
          options: 0, 
        },
        profileOptions: {
          smoker: false,
          petOwner: false,
          haveLiabilityInsurance: false,
        },
        showHobbiesInput: false, 
      };
    },
    mounted() {
      
      const decryptedtoken=this.decryptToken(sessionStorage.getItem("token"));
      globalToken=decryptedtoken;
      const token = decryptedtoken;
      if (token) {
        this.fetchUserProfile(token);
      } else {
        Swal.fire('Error', 'User ID not found', 'error');
        router.push('/login');
      }
      this.Securitybot();
    },
    methods: {
      toggleHobbiesInput() {
        this.showHobbiesInput = !this.showHobbiesInput;
      },
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
      // Method to decrypt log ID from AES encryption
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
      // Method to fetch user profile data from backend API
      async fetchUserProfile(token) {
        try {
          const response = await axios.get(`${process.env.baseURL}profile/get-user-profile?token=${token}`);
          if (response.data.profile) {
            const profile = response.data.profile;
            this.profile = {
              id: profile.id,
              user_Id: profile.user_Id,
              user: {
                user_Id: profile.user.user_Id,
                firstName: profile.user.firstName,
                lastName: profile.user.lastName,
                dateOfBirth: profile.user.dateOfBirth,
                gender: profile.user.gender,
                street: profile.user.street,
                houseNumber: profile.user.houseNumber,
                postCode: profile.user.postCode,
                city: profile.user.city,
                state: profile.user.state,
                country: profile.user.country,
                email_Address: profile.user.email_Address,
                password: profile.user.password,
                saltKey: profile.user.saltKey,
                isEmailVerified: profile.user.isEmailVerified,
                facebook_link: profile.user.facebook_link,
                verificationState: profile.user.verificationState,
                currentMembership: profile.user.currentMembership
              },
              userPic: response.data.userPic,
              hobbies: profile.hobbies,
              options: profile.options,
            };
            const options = profile.options;
            this.profileOptions = {
              smoker: (options & 1) === 1,
              petOwner: (options & 2) === 2,
              haveLiabilityInsurance: (options & 4) === 4,
            };
          } else {
            Swal.fire('Error', 'User profile data not found', 'error');
          }
        } catch (error) {
          console.error('Error fetching user profile:', error);
          Swal.fire('Error', 'Failed to fetch user profile');
        }
      },
      decryptToken(encryptedToken) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
        return bytes.toString(CryptoJS.enc.Utf8); 
      } catch (e) {
        console.error('Error decrypting token:', e);
        return null;
      }
    },
      // Method to save updated profile data
      async saveProfile() {
        try {      
          const optionsValue = this.computeOptionsValue(this.profileOptions);
          const profileData = {
            ...this.profile,
            options: optionsValue,
          };    
          const response = await axios.put(`${process.env.baseURL}profile/add-or-update-profile?token=${globalToken}`, profileData);
          if (response.status === 200) {
            Swal.fire('Success', 'Profile saved successfully', 'success');
            router.push('/profile');
          } else {
            Swal.fire('Error', 'Failed to save profile', 'error');
          }
        } catch (error) {
          console.error('Error saving user profile:', error);
          Swal.fire('Error', 'Failed to save profile');
        }
      },
      computeOptionsValue(options) {
        let value = 0;
        if (options.smoker) value |= 1;
        if (options.petOwner) value |= 2;
        if (options.haveLiabilityInsurance) value |= 4;
        return value;
      },
      // Method to handle file upload and update userPic
      async onFileChange(event) {
        const file = event.target.files[0];
        if (file) {
          const reader = new FileReader();
          reader.onload = (e) => {
            this.profile.userPic = e.target.result.split(',')[1];
          };
          reader.readAsDataURL(file);
        }
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
  </style>
  