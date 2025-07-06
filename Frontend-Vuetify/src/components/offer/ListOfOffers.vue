<template>
<div class="section_space offers_request_layout" style="text-align: center">
    <div class="offers_request_content" >
      <div class="card" >
        <div class="card-header">
          <h1 class="main-title" v-if="isHost">Anfragen auf eigene Angebote</h1>
          <h1 class="main-title" v-else>Eigene Anfragen</h1>
        </div>
        <div class="card-body">
          <div v-if="loading" class="spinner-container text-center">
            <div class="spinner"></div>
          </div>
          <div v-else>
            <div class="table-responsive">
              <table v-if="offers && offers.length > 0" class="table theme_table">
                <thead>
                  <tr>
                    <th v-if="isHost">Gast</th>
                    <th v-else>Gastgeber</th>
                    <th>Angebot</th>
                    <th>Datum</th>
                    <th>Aktion</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, index) in offers" :key="item.id">
                    <td><a class="view_user" style="cursor: pointer;" @click="showUserDetails(item.user.user_Id)">
                        <span class="user_img" v-if="item.user.profilePicture != null">
                          <img loading="lazy" class="" alt="User Profile Picture" :src="'data:' + 'image/jpeg' + ';base64,' + item.user.profilePicture">
                        </span>
                        <span class="user_img" v-if="item.user.profilePicture == null">
                          <img loading="lazy" class="" alt="User Profile Picture" :src="defaultProfileImgSrc">
                        </span>
                        {{item.user.firstName}} {{item.user.lastName}} </a> </td>
                    <td>
                      <a class="view_user" style="cursor: pointer;" @click="redirectToOfferDetail(item.offerId)">{{
                        item.offerTitle }}</a>
                    </td>
                    <td><a class="view_user" style="cursor: pointer;">{{item.createdAt}} </a></td>
                    <td>
                      <div class="btn_flexBox now_wrap buttons_text">
                        <button v-if="item.status === 0 && isHost" @click="respondToOffer(item.offerId, item.user.user_Id, true)"
                                class="icon_btn bg_ltgreen">Akzeptieren </button>
                        <button v-if="item.status === 0 && isHost" @click="respondToOffer(item.offerId, item.user.user_Id, false)"
                                class="icon_btn bg_ltred && isHost">Ablehnen </button>
                        <button v-if="item.status === 0 && !isHost" class="icon_btn bg_ltgreen" disabled >Ausstehend </button>
                        <button v-if="item.status === 2" class="icon_btn bg_ltred" disabled title="Rejected">Abgelehnt</button>
                        <button v-if="item.status === 1 && item.hasReview === false"
                                @click="showAddRatingModal(item.offerId, item.HostId, index)" class="icon_btn bg_ltblue">Bewerten
                        </button>
                        <button v-if="item.status === 1 && item.hasReview" class="icon_btn bg_ltblue">Bereits bewertet. </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
              <div v-else> <p class="text-center"> Es wurden keine Anfragen für Ihre Angebote gefunden.</p> </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <Rate :active="showModal" @update:active="$event => (showModal = $event)" @update:refresh="fetchOffers(currentPage)" :offer="ratedOffer" :user="ratedUser" :key="showModal.active" />
    <!-- Pagination Section -->
    <div class="pagination">
      <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1"><i class="ri-arrow-left-s-line"></i>Previous</button>
      <span>Page {{ currentPage }} of {{ totalPages }}</span>
      <button class="action-link" @click="changePage(currentPage + 1)" :hidden="currentPage === totalPages">Next<i class="ri-arrow-right-s-line"></i></button>
    </div>
</div>
</template>

<script setup lang="ts">
import {ref, onMounted, computed, toRefs} from "vue";
import router from "@/router";
import toast from '@/components/toaster/toast';
import axiosInstance from '@/interceptor/interceptor';
import Rate from '@/components/offer/Rate.vue';

const props = defineProps({isHost:Boolean})
let loading = ref(true)
let offers = []
let showModal = ref(false)
const defaultProfileImgSrc = '/defaultprofile.jpg'
let currentPage = 1
let totalPages = 0
let pageSize = 10
let ratedOffer = ref(null)
let ratedUser = ref(null)

const changePage = (newPage:Number) => {
    if (newPage >= 1 && newPage <= totalPages) {
        currentPage = newPage;        
        fetchOffers(newPage); // fetch new data for the selected page
    }
}
const fetchOffers = async(newPage:Number) => {
    loading.value = true;
    try {
        const response = await axiosInstance.get(`${process.env.baseURL}offer/offer-applications`, {            
            params: {
                pageSize: pageSize,
                pageNumber: newPage,
                isHost: props.isHost
            }
        });
        offers = response.data.items;
        totalPages = Math.ceil(response.data.totalCount / pageSize);
    } catch (error) {       
    } finally {
        loading.value = false;
    }   
}
const respondToOffer = async (reviewId:Number, userId:Number, approve:Boolean) => {
    try {
        const response = await axiosInstance.put(`${process.env.baseURL}offer/update-application-status?offerId=${reviewId}&isApproved=${approve}&userId=${userId}`);
        const index = offers.findIndex(item => item.id === reviewId);
        if (index !== -1) {
            if(approve)
                offers[index.status = 1];
            else
                offers[index.status = 2];
        }
        fetchOffers(currentPage);
    } catch (error) {
    }
}
const showAddRatingModal = (offerId:Number, userId:String, index:Number) => {
    ratedOffer.value = offers[index].offer;
    ratedUser.value = offers[index].user;
    showModal.value = true;
}

const redirectToOfferDetail = (offerId) => {router.push({ name: 'OfferDetail', params: { id: offerId } })}
onMounted(() => {
    fetchOffers(1);
})

const showUserDetails = (userId:String) => {
      sessionStorage.setItem("UserId", userId);
      router.push("/account");
}
</script>

<style scoped>
.table {
  width: 100%;
  border-collapse: collapse;
}
.table th:first-child,
.table td:first-child {
  border-left: none;
}
.table th:last-child,
.table td:last-child {
  border-right: none;
}
button {
  margin: 5px;
}
.btn-success {
  color: #fff;
  background-color: #2e9946;
  border-color: #1e7e34;
}
.btn-primary {
  color: #fff;
}
.swal2-textarea {
  margin: 0;
  width: 100%;
}
</style>
