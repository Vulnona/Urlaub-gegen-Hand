<template>
  <div class="main_header" v-if="isLoggedIn">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <nav class="navbar navbar-expand-lg navbar-white bg-white">
            <a class="navbar-brand" href="/home">
              <img src="@/assets/images/logo/logo-desktop.svg" class="logo" alt="Logo">
            </a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent"
              aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
              <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
              <ul class="navbar-nav ml-auto">
                <!-- Common Home Link -->
                <li class="nav-item">
                  <router-link class="nav-link" to="/home">
                    <i class="ri-home-4-line"></i> Startseite <span class="sr-only">(current)</span>
                  </router-link>
                </li>
                
                <!-- Admin-specific Links -->
                <li v-if="userRole === 'Admin'" class="nav-item">
                  <router-link class="nav-link" to="/admin">
                    <i class="ri-admin-line"></i> Admin
                  </router-link>
                </li>
                <li v-if="userRole === 'Admin'" class="nav-item">
                  <router-link class="nav-link" to="/reviews">
                    <i class="ri-question-answer-line"></i> Reviews
                  </router-link>
                </li>
                <li v-if="userRole === 'Admin'" class="nav-item">
                  <router-link class="nav-link" to="/coupons">
                    <i class="ri-coupon-line"></i> Coupons
                  </router-link>
                </li>
                <!-- Active Membership-specific Links (for users with active membership) -->
                <li v-if="userRole != 'Admin'" class="nav-item">
                  <router-link class="nav-link" to="/my-offers">
                    <i class="ri-gift-line"></i> Meine Angebote
                  </router-link>
                </li>
                <li v-if="userRole != 'Admin'" class="nav-item">
                  <router-link class="nav-link" to="/offer-request">
                    <i class="ri-mail-line"></i> Anfragen
                  </router-link>
                </li>
                <li v-if="userRole !== 'Admin'" class="nav-item">
                  <router-link class="nav-link" to="/store">
                    <i class="ri-store-line"></i> Store
                  </router-link>
                </li>

                <!-- User Account and Logout (common for all roles) -->
                <li class="nav-item dropdown border rounded">
                  <span class="nav-link dropdown-toggle" id="navbarDropdown" role="button" data-toggle="dropdown"
                    aria-haspopup="true" aria-expanded="false">
                    <i class="ri-user-3-line"></i> {{ username }}
                  </span>
                  <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdown">
                    <a v-if="userRole != 'Admin'" class="dropdown-item" @click="openProfile">
                      <i class="ri-file-user-line"></i> Profil
                    </a>
                    <a v-if="userRole != 'Admin'" class="dropdown-item" @click="openPurchaseHistory">
                      <i class="ri-money-dollar-box-line"></i> Käufe
                    </a>
                    <a class="dropdown-item" @click.prevent="doLogout">
                      <i class="ri-logout-circle-r-line"></i> Logout
                    </a>
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
import {GetUserRole} from "@/services/GetUserPrivileges";
import {isActiveMembership} from "@/services/GetUserPrivileges";
const isLoggedIn = ref(false);
const username = ref(sessionStorage.getItem("firstName"));
const userRole = ref('');

// Handle logout and session clearing
const doLogout = () => {
  sessionStorage.clear();
  // Always redirect to login page after logout
  router.push({ name: 'Login' });
};
// Navigate to profile
const openProfile = () => {
  router.push("/profile");
};

const openPurchaseHistory=()=>{
  router.push("/purchase-history");
}

onMounted(async () => {
  userRole.value = GetUserRole() || '';
  isLoggedIn.value = !!sessionStorage.getItem("token");
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
