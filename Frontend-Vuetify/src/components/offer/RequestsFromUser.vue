<template>
<div class="section_space offers_request_layout" style="text-align: center">
    <div class="offers_request_content" >
      <div class="card" >
        <div class="card-header">
          <h1 class="main-title">Eigene Anfragen</h1>
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
                    <th>Gastgeber</th>
                    <th>Angebot</th>
                    <th>Datum</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, index) in offers" :key="item.id">
                    <TableEntryUser :item="item" />
                    <td>
                      <a class="view_user" style="cursor: pointer;" @click="redirectToOfferDetail(item.offer.id)">{{item.offer.title }}</a>
                    </td>
                    <td><a class="view_user" style="cursor: pointer;">{{item.createdAt}} </a></td>
                  <td>
                      <div class="btn_flexBox now_wrap buttons_text">
                        <button v-if="item.status === 0" class="icon_btn bg_ltgreen" disabled >Ausstehend </button>
                        <button v-if="item.status === 2" class="icon_btn bg_ltred" disabled title="Rejected">Abgelehnt</button>
                        <button v-if="item.status === 1"
                                @click="showAddRatingModal(item.offer.id, item.HostId, index)" class="icon_btn bg_ltblue">Bewerten
                        </button>
                      </div>
                  </td>
                  </tr>
                </tbody>
              </table>
              <div v-else> <p class="text-center"> Sie haben keine Anfragen gestellt. </p> </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Pagination Section -->
    <div class="pagination">
      <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1"><i class="ri-arrow-left-s-line"></i>Previous</button>
      <span>Page {{ currentPage }} of {{ totalPages }}</span>
      <button class="action-link" @click="changePage(currentPage + 1)" :hidden="currentPage === totalPages">Next<i class="ri-arrow-right-s-line"></i></button>
    </div>
</div>
<Rate :active="showModal" @update:active="$event => (showModal = $event)" :offer="ratedOffer" :user="ratedUser" :key="showModal.active" />
</template>

<script setup lang="ts">
import {ref, onMounted, computed, toRefs} from "vue";
import toast from '@/components/toaster/toast';
import axiosInstance from '@/interceptor/interceptor';
import TableEntryUser from '@/components/offer/TableEntryUser.vue';
import Rate from '@/components/offer/Rate.vue';

let loading = ref(true)
let offers = []
let showModal = ref(false)
const defaultProfileImgSrc = '/defaultprofile.jpg'
let currentPage = 1
let totalPages = 0
let pageSize = 10
let ratedOffer = ref(null)
let ratedUser = ref(null)

onMounted(() => {
    fetchOffers(1);
})

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
                isHost: false
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
const showAddRatingModal = (offerId:Number, userId:String, index:Number) => {
    showModal.value = true;
    ratedOffer.value = offers[index].offer;
    ratedUser.value = offers[index].user;
}
</script>
