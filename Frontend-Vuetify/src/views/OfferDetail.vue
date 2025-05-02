<template>
<Navbar />
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
              <div class="item_description">
                <tr><td>
                    <div style="display: flex; align-items: center;"> <strong>Gastgeber:&nbsp;</strong>
                      <UserLink :hostPic=offer.hostPicture :hostId=offer.hostId :hostName=offer.hostName />
                  </div> </td>
                </tr>
                <tr><strong>Gesuchte Fähigkeiten:</strong> {{ offer.skills }}</tr>
                <tr><strong>Annehmlichkeiten:</strong> {{ offer.accomodation }}</tr>
                <tr><strong>Geeignet für:</strong> {{ offer.accomodationsuitable }}</tr>
                <tr><strong>Ort/Region:</strong> {{ offer.location }}</tr>
                <tr><strong>Angebotszeitraum:</strong> {{offer.fromDate}} - {{offer.toDate}}</tr>
              </div>
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
            <p class="offer-description mb-0">{{ offer.description }}</p>
          </div>
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
import {
  useRoute
} from 'vue-router';
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from '@/interceptor/interceptor';
import router from '@/router';
import {isActiveMembership} from '@/services/GetUserPrivileges';
import Apply from '@/components/Apply.vue';
import getLoggedUserId from '@/services/LoggedInUserId';
import UserLink from '@/components/offer/UserLink.vue';

const {
  params
} = useRoute();
const offer = ref(null);
const loading = ref(true);
const showAllReviews = ref(false);
var reviews = ref([]);
const defaultProfileImgSrc = '/defaultprofile.jpg';
const redirectToProfile = (userId) => {
  sessionStorage.setItem("UserId", userId);
  router.push("/account");
}

const fetchOfferDetail = async () => {
  try {
    fetchReview();
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
const fetchReview = async () => {
  try {
    const response = await axiosInstance.get(`${process.env.baseURL}review/get-offer-reviews?offerId=${params.id}`);
    reviews.value = response.data.items;
  } catch (error) {
    console.error('Error fetching reviews:', error);
  }
};
const displayedReviews = computed(() => {
  return showAllReviews.value ? reviews.value : reviews.value.slice(0, 1);
});
const toggleShowMore = () => {
  showAllReviews.value = !showAllReviews.value;
};
const backtooffers = () => {
  window.history.back();
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
</style>
