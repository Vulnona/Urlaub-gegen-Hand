<template>  
<Navbar />
<PublicNav />
  <section class="offer-detail-container offer_detail_layout section_space" v-if="!loading">
    <div class="container">
      <div class="row">
        <div class="col-xs-12 col-sm-6">
          <div class="offer-image" v-if="offer.imageData">
            <img :src="'data:' + offer.imageMimeType + ';base64,' + offer.imageData" alt="Offer Image" />
          </div>
        </div>
        <div class="col-xs-12 col-sm-6">
          <div class="offer-content">
            <h1 class="offer-title">{{ offer.title }}</h1>
            <table class="item_description">
              <tbody v-if="offer.hostId != null">
                <tr>
                  <td>
                    <div style="display: flex; align-items: center;"> <a class="b">Gastgeber:&nbsp;</a>
                      <UserLink :hostPic=offer.hostPicture :hostId=offer.hostId :hostName=offer.hostName />
                  </div> </td>
                </tr>
              </tbody>
              <tbody>
                <tr><a class="b">Gesuchte Fähigkeiten:</a> {{ offer.skills }}</tr>
                <tr><a class="b">Unterbringung:</a> {{ offer.accomodation }}</tr>
                <tr><a class="b">Geeignet für:</a> {{ offer.accomodationsuitable }}</tr>
                <tr><a class="b">Ort/Region:</a> {{ offer.location }}</tr>
                <tr><a class="b">Angebotszeitraum:</a> {{offer.fromDate}} - {{offer.toDate}}</tr>
               </tbody>
              </table>
            <Apply :offer=offer :isActiveMember=isActiveMember :logId=logId />
            <div class="offer_btn">
              <button @click="backtooffers()" class="action-link"><i class="ri-arrow-go-back-line"
                  aria-hidden="true"></i> Zurück </button>
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-sm-12">
          <div class="offer_decription mt-4">
            <VueMarkdown :source="offer.description" class="offer-description mb-0" />
          </div>
        </div>
      </div>
      <div v-if="logId == offer.hostId">
        <div v-if="offer.status == '0'">
          <button class="btn btn-danger" @click="closeOffer(offer)">
            Angebot schließen.
          </button>
          <a v-if="!offer.applicationsExist">
           <button class="btn btn-primary" @click="modifyOffer()">
            Angebot modifizieren.
           </button>
          </a>
          <a v-else>
            <button class="btn btn-blocked">
              Angebot schreibgeschützt.
             </button>
          </a>
        </div>
        <div v-else>
          <button class="btn btn-blocked">Angebot geschlossen.</button>
        </div>        
      </div>
    </div>
  </section>
  <div class="loading" v-else>
    Loading...
  </div>
</template>

<script setup lang="ts">
import {
  ref,
  onMounted,
  computed
} from 'vue';
import Navbar from '@/components/navbar/Navbar.vue';
import PublicNav from '@/components/navbar/PublicNav.vue';
import axiosInstance from '@/interceptor/interceptor';
import router from '@/router';
import {isActiveMembership} from '@/services/GetUserPrivileges';
import Apply from '@/components/Apply.vue';
import getLoggedUserId from '@/services/LoggedInUserId';
import UserLink from '@/components/offer/UserLink.vue';
import VueMarkdown from "vue-markdown-render";
import {useRoute} from 'vue-router';
const {params} = useRoute();

const pictureLink = `${process.env.baseURL}offer/get-preview-picture/${params.id}`;
const offer = ref(null);
const loading = ref(true);
const defaultProfileImgSrc = '/defaultprofile.jpg';
const redirectToProfile = (userId) => {
  sessionStorage.setItem("UserId", userId);
  router.push("/account");
}

const fetchOfferDetail = async () => {
  try {
    const response = await axiosInstance.get(`${process.env.baseURL}offer/get-offer-by-id/${params.id}`);
      offer.value = response.data;
  } catch (error) {
    console.error('Error fetching offer detail:', error);
  } finally {
    loading.value = false;
  }
};
const formatDate = (dateString) => {
  const options: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: 'long',
    day: '2-digit',
  };
  return new Date(dateString).toLocaleDateString(undefined, options);
};
const backtooffers = () => {
  window.history.back();
};
const closeOffer = async (offer) => {
    try {
        const response = await axiosInstance.put(`${process.env.baseURL}offer/close-offer/${params.id}`);
        offer.status = 1;
    } catch {
        console.error('Error closing offer');
    }
};
const modifyOffer = () => {
      router.push({
        name: 'ModifyOffer', params: {id: params.id}
      });
};

onMounted(fetchOfferDetail);
</script>

<script lang="ts">    
export default {
    data() {
        return{
            isActiveMember: isActiveMembership(),
            logId: getLoggedUserId(),
        };
}
}
</script>

<style scoped lang="scss">
.loading {
  font-size: 1.5rem;
  color: #888;
  text-align: center;
  padding: 40px;
}

.modal {
  display: block;
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  background-color: rgb(0, 0, 0);
  background-color: rgba(0, 0, 0, 0.4);
  padding: 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.modal-content {
  background-color: #fefefe;
  width: 100%;
  max-width: 800px;
}

.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}

.review-item {
  // padding: 10px;
  // border: 1px solid #ddd;
  margin-bottom: 10px;
}

.outline_Greybtn {
  /* Example button styling */
  border: 1px solid grey;
  background-color: transparent;
  color: grey;
  padding: 8px 12px;
  cursor: pointer;
}

.reviews {
  margin-top: 20px;
}

.b {
    font-weight:bold;
    color:black;
}
.btn-blocked {
   color: #fff;
   background-color: grey;
   border-color: darkgrey;
}
</style>
