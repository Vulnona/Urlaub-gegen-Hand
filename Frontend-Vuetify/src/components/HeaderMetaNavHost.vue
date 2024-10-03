<template>
  <div class="main_header">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <nav class="navbar navbar-expand-lg navbar-white bg-white">
            <a class="navbar-brand" href="/home"> <img src="@/assets/images/logo/logo-desktop.svg" class="logo"
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
                  <router-link class="nav-link" to="/my-offers"><i class="ri-gift-line"></i> My Offers <span
                      class="sr-only">(current)</span></router-link>
                </li>

                <li class="nav-item">
                  <router-link class="nav-link" to="/offerrequest"><i class="ri-mail-line"></i> Offer
                    Request</router-link>
                </li>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <li class="nav-item dropdown border rounded">
                  <span class="nav-link dropdown-toggle" id="navbarDropdown" role="button" data-toggle="dropdown"
                    aria-haspopup="true" aria-expanded="false">
                    <i class="ri-user-3-line"></i> {{ username }}
                  </span>
                  <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdown">
                    <a class="dropdown-item" @click="openProfile"><i class="ri-file-user-line"></i> Profile</a>
                    <a class="dropdown-item" @click.prevent="doLogout"><i class="ri-logout-circle-r-line"></i> Abmelden</a>
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
import { ref, onMounted } from 'vue';
import router from "@/router";

// Declare reactive variables using ref
const isLoggedIn = ref(false); // Reactive variable to track login status
const username = ref(sessionStorage.getItem("firstName"));

// Function to check login status
const checkLoginStatus = () => {
  isLoggedIn.value = !!sessionStorage.getItem("token");
};

// Function to handle logout
const doLogout = () => {
  sessionStorage.clear();
  isLoggedIn.value = false;
  router.push("/login").then(() => {
    window.location.reload();
  });
};
// Function to navigate to the profile page and reload the window
const openProfile = () => {
  router.push("/profile").then(() => {
    window.location.reload();
  });
};
// Lifecycle hook to execute when the component is mounted
onMounted(() => {
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
