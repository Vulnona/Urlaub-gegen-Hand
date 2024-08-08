[12:45 PM] Sombir Redhu
<template>
  <div class="user-profile">
    <div class="card">
      <h2>User Profile</h2>
      <div class="card-body">
        <div v-if="user">
          <div class="profile-image">
            <img :src="user.profileImgSrc" alt="Profile Image" />
          </div>
          <div class="profile-info">
            <p><strong>Name:</strong> {{ user.firstName }} {{ user.lastName }}</p>
            <p><strong>Email:</strong> {{ user.email_Address }}</p>
            <p><strong>Date of Birth:</strong> {{ user.dateOfBirth }}</p>
            <p><strong>Gender:</strong> {{ user.gender }}</p>
            <p><strong>Country:</strong> {{ user.country }}</p>
            <p><strong>City:</strong> {{ user.city }}</p>
            <p><strong>Postal Code:</strong> {{ user.postCode }}</p>
          </div>
          <button type="button" class="btn" @click="goHome" style="float: right; background: #00AFFF;">Home</button>
        </div>
        <div v-else>
          <p>Loading user profile...</p>
        </div> 
      </div>
    </div>
  </div>
</template>
 
<script>
import axios from 'axios';
 
export default {
  data() {
    return {
      user: null,
    };
  },
  mounted() {
    this.fetchUserProfile();
  },
  methods: {
    async fetchUserProfile() {
      try {
        const sentToken = this.$route.query.token;
        console.log('Retrieved token from URL:', sentToken); // Debugging log
        if (!sentToken) {
          throw new Error('No token provided');
        }
        console.log("sent token dis : ", sentToken);
        const response = await axios.get(`${process.env.baseURL}profile/display-profile?token=${sentToken}`);
        console.log('API response:', response); // Debugging log
 
        this.user = response.data;
        this.user.profileImgSrc = `data:image/jpeg;base64,${response.data.userPic}`;
      } catch (error) {
        console.error('Error fetching user profile:', error);
      }
    },
    goHome() {
      this.$router.push("/");
    }
  },
};
</script>
 
<style scoped>
.user-profile {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
}
 
.card {
  background-color: #ffffff;
  border: none;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
  max-width: 600px;
  width: 100%;
}
 
.card-body {
  padding: 30px;
}
 
h2 {
  text-align: center;
  margin-bottom: 30px;
  font-size: 24px;
  color: #333;
}
 
.profile-info {
  margin-bottom: 20px;
}
 
.profile-info p {
  font-size: 18px;
  color: #555;
}
 
.profile-info strong {
  color: #333;
}
 
.profile-image {
  text-align: center;
}
 
.profile-image img {
  width: 100%;
  height: 100%;
  border-radius: 5%;
  object-fit: cover;
  border: 3px solid #fff;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  margin-bottom: 2cm;
}
</style>
 