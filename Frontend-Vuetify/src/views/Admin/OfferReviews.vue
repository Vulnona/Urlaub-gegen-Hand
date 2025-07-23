<template>
  <div v-if="userRole !== 'Admin'">
    <Errorpage />
  </div>
  <div v-else>
    <Navbar />
    <div class="container">
      <!--Review-->
      <div class="review_layout">
        <div class="row">
          <div class="col-sm-12">
            <div class="secondary_title">
              <h2 class="text-center">Reviews</h2>
            </div>
          </div>
          <div class="offer_btn">
            <button @click="backtooffers()" class="action-link"><i class="ri-arrow-go-back-line" aria-hidden="true"></i>
              Back </button>
          </div>
        </div>
        <div class="comments-area">
          <div class="comment-list-wrap">
            <ol class="comment-list">
              <div v-if="reviews.length >= 1">
                <li v-for="(offerReviews, index) in reviews" :key="index" class="comment">
                  <div class="comment-body">
                    <div class="comment-author vcard" v-if="offerReviews.reviewer.profilePicture != null">
                      <img @click="redirectToProfile(offerReviews.reviewer.user_Id)" alt=""
                        :src="'data:' + offer.imageMimeType + ';base64,' + offerReviews.reviewer.profilePicture"
                        class="avatar avatar-80 photo clickable-item" height="80" width="80" decoding="async">
                    </div>
                    <div class="comment-author vcard" v-if="offerReviews.reviewer.profilePicture == null">
                      <img alt="" :src="defaultProfileImgSrc" class="avatar avatar-80 photo" height="80" width="80"
                        decoding="async">
                    </div>
                    <div class="comment-content">
                      <div class="comment-head">
                        <div class="comment-user">
                          <div @click="redirectToProfile(offerReviews.reviewer.user_Id)" class="user clickable-item">{{
                            offerReviews.reviewer.firstName }} {{ offerReviews.reviewer.lastName }}
                          </div>
                          <div class="comment-date"> <time :datetime="offerReviews.createdAt">{{
                            formatDate(offerReviews.createdAt) }}</time>
                          </div>
                          <div class="comment-rating-stars stars">
                            <span class="star star-1"> <i class="ri-star-fill"></i> {{ offerReviews.ratingValue
                              }}</span>
                          </div>
                        </div>
                      </div>
                      <div class="comment-text">
                        <p class="mb-0">{{ offerReviews.reviewComment }}</p>
                      </div>
                    </div>
                  </div>
                </li>
              </div>
              <div v-else>
                <p class="text-center text-bold">Keine Bewertungen bisher</p>
              </div>
            </ol>
            <div class="pagination" v-if="reviews.length >= 1">
              <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1">
                <i class="ri-arrow-left-s-line"></i> Vorherige
              </button>

              <span>Seite {{ currentPage }} von {{ totalPages }}</span>

              <button class="action-link" @click="changePage(currentPage + 1)" :hidden="currentPage === totalPages">
                Nächste <i class="ri-arrow-right-s-line"></i>
              </button>
            </div>

            <!-- <div class="text-center" v-if="reviews.length > 1">
                <button type="button" @click="toggleShowMore" class="btn outline_Greybtn">
                    {{ showAllReviews ? "Verstecke Bewertungen" : "Zeig mehr Bewertungen" }}
                  </button>
              </div> -->
          </div>
        </div>
      </div>
    </div>
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
import {GetUserRole} from "@/services/GetUserPrivileges";
import Errorpage from "../Errorpage.vue";
const userRole = GetUserRole();
const {
  params
} = useRoute();
const loading = ref(true);
const offer = ref(null);
const showAllReviews = ref(false);
var reviews = ref([]);
const defaultProfileImgSrc = '/defaultprofile.jpg';
const currentPage = ref(1); 
const totalPages = ref(1); 
const pageSize = 10;


const changePage = (newPage: number) => {
  if (newPage >= 1 && newPage <= totalPages.value) {
    currentPage.value = newPage;
    fetchReview();
  }
};

const fetchReview = async () => {
  try {
    const response = await axiosInstance.get(
      `${process.env.baseURL}review/get-offer-reviews?offerId=${params.id}`,
      {
        params: {
          pageSize: pageSize,
          pageNumber: currentPage.value
        }
      }
    );
    reviews.value = response.data.items;
    totalPages.value = Math.ceil(response.data.totalCount / pageSize);
  } catch (error) {
    console.error('Error fetching reviews:', error);
  }
};
const fetchOfferDetail = async () => {
  try {
    fetchReview();
    const response = await axiosInstance.get(`${process.env.baseURL}offer/get-offer-by-id/${params.id}`
    );
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
const toggleShowMore = () => {
  showAllReviews.value = !showAllReviews.value;
};
const backtooffers = () => {
  window.history.back();
};
const redirectToProfile = (userId) => {
  sessionStorage.setItem("UserId", userId);
  router.push("/account");
}
onMounted(fetchOfferDetail);
</script>
