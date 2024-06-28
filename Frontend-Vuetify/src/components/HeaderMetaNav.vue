<template>

  <div class="main_header">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <nav class="navbar navbar-expand-lg navbar-white bg-white">
            <a class="navbar-brand" href="#"> <img src="@/assets/images/logo/logo-desktop.svg" class="logo"
                alt="Logo"></a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent"
              aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
              <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
              <ul class="navbar-nav ml-auto">
                <li class="nav-item">
                  <router-link class="nav-link" to="/home"><i class="ri-home-4-line"></i> Home <span
                      class="sr-only">(current)</span></router-link>
                </li>
                <li class="nav-item">
                  <router-link class="nav-link" to="/admin"><i class="ri-admin-line"></i> Admin</router-link>
                </li>
                &nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;
                <li class="nav-item dropdown border">
                  <span class="nav-link dropdown-toggle" id="navbarDropdown" role="button" data-toggle="dropdown"
                    aria-haspopup="true" aria-expanded="false">
                    <i class="ri-user-3-line"></i> {{ username }}
                  </span>
                  <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdown">
                    <a class="dropdown-item" @click="openProfile">Profile</a>
                   
                    <a class="dropdown-item" @click.prevent="doLogout">Abmelden</a>
                  </div>
                </li>
              </ul>
            </div>
          </nav>
        </div>
      </div>
    </div>
  </div>



</template>
<script lang="ts" setup>
import '@fortawesome/fontawesome-free/css/all.css'; // Import FontAwesome CSS for icons
import '@fortawesome/fontawesome-free/js/all.js'; // Import FontAwesome JS for icons
import { ref, onMounted } from 'vue'; // Importing ref and onMounted from Vue for reactivity and lifecycle hooks
import router from "@/router"; // Import the Vue router for navigation

// Declare reactive variables using ref
const isLoggedIn = ref(false); // Reactive variable to track login status
const username = ref(localStorage.getItem("firstName")); // Reactive variable to store the username from local storage

// Function to check login status
const checkLoginStatus = () => {
  // Update isLoggedIn based on whether a token exists in local storage
  isLoggedIn.value = !!localStorage.getItem("token");
};

// Function to handle logout
const doLogout = () => {
  // Clear local storage
  localStorage.clear();
  // Update isLoggedIn to false
  isLoggedIn.value = false;
  // Navigate to the login page and reload the window
  router.push("/login").then(() => {
    window.location.reload();
  });
};

// Function to navigate to the profile page
const openProfile = () => {
  router.push("/profile").then(() => {
    window.location.reload();
  });
};

// Function to navigate to the login page
const doLogin = () => {
  router.push("/login");
};

// Lifecycle hook to execute when the component is mounted
onMounted(() => {
  // Check login status when the component is mounted
  checkLoginStatus();
});
</script>

<style lang="scss" scoped>
.logo {
  inline-size: 200px;
  margin-bottom: 10px;

  @media (min-width: 769px) {
    margin-bottom: 0;
  }
}

.nav-list {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: center;
  list-style: none;
  padding: 0;
  margin: 0;

  @media (max-width: 768px) {
    flex-direction: column;
  }
}
</style>