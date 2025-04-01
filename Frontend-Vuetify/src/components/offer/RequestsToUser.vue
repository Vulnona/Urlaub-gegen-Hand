<template>
<div class="section_space offers_request_layout" style="text-align: center">
    <div class="offers_request_content" >
      <div class="card" >
        <div class="card-header">
          <h1 class="main-title">Anfragen bei Ihren Angeboten</h1>
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
                    <th>Gast</th>
                    <th>Angebotstitel</th>
                    <th>Datum</th>
                    <th>Aktion</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, index) in offers" :key="item.id">
                    <TableEntryUser :item="item" />
                    <td>
                      <a class="view_user" style="cursor: pointer;" @click="redirectToOfferDetail(item.offer.id)">{{
                        item.offer.title }}</a>
                    </td>
                    <td><a class="view_user" style="cursor: pointer;">{{item.createdAt}} </a></td>
                    <td>
                      <div class="btn_flexBox now_wrap buttons_text">
                        <button v-if="item.status === 0" @click="respondToOffer(item.offer.id, item.user.user_Id, true)"
                                class="icon_btn bg_ltgreen">Akzeptieren </button>
                        <button v-if="item.status === 0" @click="respondToOffer(item.offer.id, item.user.user_Id, false)"
                                class="icon_btn bg_ltred">Ablehnen </button>
                        <button v-if="item.status === 2" class="icon_btn bg_ltred" disabled
                                title="Rejected">Abgelehnt</button>
                        <button v-if="item.status === 1"
                                @click="showAddRatingModal(item.offer.id, item.user.user_Id, index)"
                                class="icon_btn bg_ltblue">Bewerten
                        </button>
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
    <Rate :active="showModal" @update:active="$event => (showModal = $event)" :offer="ratedOffer" :user="ratedUser" :key="showModal.active" />
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
import TableEntryUser from '@/components/offer/TableEntryUser.vue';
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
                isHost: true
            }
        });
        offers = response.data.items;
        totalPages = Math.ceil(response.data.totalCount / pageSize);
    } catch (error) {
        //console.log(error);
    } finally {
        loading.value = false;
    }   
}
const respondToOffer = async (reviewId:Number, userId:Number, approve:Boolean) => {
    try {
        const response = await axiosInstance.put(`${process.env.baseURL}offer/update-application-status?offerId=${reviewId}&isApprove=${approve}&userId=${userId}`);
        const index = offers.findIndex(item => item.id === reviewId);
        if (index !== -1) {
            if(approve)
                offers[index.status = 1];
            else
                offers[index.status = 2];
        }
        fetchOffers(currentPage);
    } catch (error) {
        // console.error('Error responding to offer:', error);
    }
}
const showAddRatingModal = (offerId:Number, userId:String, index:Number) => {
    showModal.value = true;
    ratedOffer.value = offers[index].offer;
    ratedUser.value = offers[index].user;
}
const redirectToOfferDetail = (offerId) => {router.push({ name: 'OfferDetail', params: { id: offerId } })}
onMounted(() => {
    fetchOffers(1);
})

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
