<template>
  <Navbar />
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2></h2>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="container">
    <div class="row profile-content">
      <div class="col-xl-3">
        <div class="card custom-card overflow-hidden border p-0 leftBox-content">
          <div class="card-body border-bottom border-block-end-dashed">
            <div class="text-center">              
              <span class="avatar avatar-xxl avatar-rounded online mb-3">
                <img :src="profileImgSrc || defaultProfileImgSrc" @error="onImageError" class="profile-img"
                  alt="User Profile Picture">
              </span>
              <h5 class="fw-semibold mb-1">{{ user.firstName }} {{ user.lastName }}</h5>
              <span @click="showModal = true" class="action-link fs-13 font-normal">Alle Bewertungen ansehen</span>
            </div>
          </div>
          <div v-if="user.userRating != 0"
            class="rating_block d-flex mb-0 flex-wrap gap-3 p-3 justify-content-center border-bottom border-block-end-dashed">
            <div class="">
              <p class="card-text text-center mb-0">Bewertungen:<span class="average-rating">{{ user.averageRating
                  }}</span>
                {{ " " }} <span class="star ri-star-fill gold"></span>
              </p>
            </div>
          </div>
          <div class="p-3 pb-1 d-flex flex-wrap justify-content-between">
          </div>
          <div class="card-body border-bottom border-block-end-dashed p-0">
            <ul class="list-group list-group-flush basic_info">
              <li class="list-group-item border-0">
                <div>
                  <span class="fw-medium me-2">Name:</span><span class="text-muted">{{ user.firstName }} {{
                    user.lastName
                  }}</span>
                </div>
              </li>
              <li class="list-group-item border-0">
                <div><span class="fw-medium me-2">Alter:</span><span class="text-muted">{{ user.age }}</span>
                </div>
              </li>
              <li class="list-group-item border-0">
                <div><span class="fw-medium me-2">Geschlecht:</span><span class="text-muted">{{ user.gender }}</span>
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="col-xl-9">
        <div class="card custom-card overflow-hidden border p-0">
          <div class="card-body">
            <div>
              <div class="back_link"><a href="#" @click="back()"><i class="ri-arrow-left-double-fill"></i> Back</a>
              </div>
              <ul class="list-group list-group-flush border rounded-3">
                <li class="list-group-item p-3">
                  <div class="text-muted">
                    <p class="mb-3">
                      <span class="fw-medium text-default"><span style="font-size: x-large">&#8962;</span> : {{ user.city }} / {{user.state}}</span> 
                    </p>
                  </div>
                </li>
                <li class="list-group-item p-3 skills_content">
                  <span class="fw-medium fs-15 d-block mb-3">Skills:</span>
                  <div class="w-75">
                    <a href="javascript:void(0);">
                      <span v-for="skills in user.skills" :key="skills" class="badge bg-light text-muted m-1 border">{{
                        skills }}</span>
                    </a>
                  </div>
                </li>
                <li class="list-group-item p-3 hobbies_content">
                  <span class="fw-medium fs-15 d-block mb-3">Hobbies:</span>
                  <div class="w-75">
                    <a href="javascript:void(0);">
                      <span v-for="hobbies in user.hobbies" :key="hobbies"
                        class="badge bg-light text-muted m-1 border">{{ hobbies }}</span>
                    </a>
                  </div>
                </li>
                <li class="list-group-item p-3 social_link">
                  <span class="fw-medium fs-15 d-block mb-3">Social Media:</span>
                  <ul class="d-flex align-items-center flex-wrap">
                    <li class="d-flex align-items-center gap-3">
                      <button v-if="user.facebookLink" @click="redirectToFacebook(user.facebookLink)" type="button" class="btn social_btn">
                        <span class="social_link_outer"><i class="ri-facebook-fill"></i></span>
                        <span class="text-info">Facebook</span>
                      </button>
                    </li>
                  </ul>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div v-if="showModal" class="modal review_modal_layout">
    <div class="modal-content review_modal">
      <div class="modal-header">
        <h4 class="card-title">Rating & Reviews</h4>
        <span class="close" @click="showModal = false">&times;</span>
      </div>
      <div class="modal-body" ref="modalBody" style="" @scroll="handleScroll">
        <div class="review_layout all_reviews_layout">
          <div class="row">
            <div class="col-sm-4">
              <div class="review_rating">
                <div class="rating-score mb-2">
                  <i class="ri-star-fill" aria-hidden="true"></i>{{ user.averageRating }}<span>/5</span>
                </div>
              </div>
            </div>
            <div class="col-sm-8">
              <div class="all_reviews">
                <div class="flex_header">
                  <h5>{{ reviews.length }} Reviews</h5>
                </div>
                <div class="comments-area">
                  <div class="comment-list-wrap">
                    <ol class="comment-list">
                      <li v-for="userReviews in reviews" :key="userReviews" class="comment">
                        <div>
                          <div class="comment_head">
                            <h6 v-if="userReviews.offer" @click="redirectToOffer(userReviews.offer.id)" class="clickable-item">
                              <span v-if="userReviews.offer.isDeleted" class="deleted-offer">
                                {{ userReviews.offer.deletedOfferTitle || 'Gelöschtes Angebot' }}
                              </span>
                              <span v-else>
                                {{ userReviews.offer.title }}
                              </span>
                            </h6>
                            <h6 v-else class="direct-review">
                              Direkte Bewertung
                            </h6>
                            <div class="img-thumb clickable-item" v-if="userReviews.offer.imageData != null"><img
                                @click="redirectToOffer(userReviews.offer.id)"
                                :src="'data:' + userReviews.offer.imageMimeType + ';base64,' + userReviews.offer.imageData">
                            </div>
                            <div class="img-thumb" v-if="userReviews.offer.imageData == null"><img
                                :src="defaultProfileImgSrc">
                            </div>
                          </div>
                          <div class="comment-body">
                            <div class="comment-author vcard" v-if="userReviews.reviewer.profilePicture != null">
                              <img alt="" @click="redirectToProfile(userReviews.reviewer.user_Id)"
                                :src="'data:' + userReviews.offer.imageMimeType + ';base64,' + userReviews.reviewer.profilePicture"
                                class="avatar avatar-80 photo clickable-item" height="80" width="80" decoding="async">
                            </div>
                            <div class="comment-author vcard" v-if="userReviews.reviewer.profilePicture == null">
                              <img @click="redirectToProfile(userReviews.reviewer.user_Id)" alt=""
                                :src="defaultProfileImgSrc" class="avatar avatar-80 photo clickable-item" height="80"
                                width="80" decoding="async">
                            </div>
                            <div class="comment-content">
                              <div class="comment-head">
                                <div class="comment-user">
                                  <div @click="redirectToProfile(userReviews.reviewer.user_Id)"
                                    class="user clickable-item">
                                    <span v-if="userReviews.reviewer.isDeleted" class="deleted-user">
                                      {{ userReviews.reviewer.deletedUserName || 'Gelöschter Nutzer' }}
                                    </span>
                                    <span v-else>
                                      {{ userReviews.reviewer.firstName }} {{
                                        userReviews.reviewer.lastName
                                      }}
                                    </span>
                                  </div>
                                  <div class="comment-date">
                                    <time datetime="2024-08-02T09:54:50+00:00"> <time
                                        :datetime="userReviews.createdAt">{{
                                          formatDate(userReviews.createdAt) }}</time></time>
                                  </div>
                                  <div class="comment-rating-stars stars">
                                    <span class="star">
                                      <i class="ri-star-fill"></i>
                                      {{ userReviews.ratingValue }}
                                    </span>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div>
                            <div class="comment-text">
                              <p class="mb-0">{{ userReviews.reviewComment }}</p>
                            </div>
                          </div>
                        </div>
                      </li>
                    </ol>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

</template>
<script>
import axiosInstance from "@/interceptor/interceptor"
import Navbar from "@/components/navbar/Navbar.vue";
import Securitybot from '@/services/SecurityBot';
import toast from "@/components/toaster/toast";
import { nextTick } from 'vue';
export default {
  components: {
    Navbar,
  },
  name: "UserCard",
  data() {
    return {
      user: {},
      profileImgSrc: '',
      defaultProfileImgSrc: '/defaultprofile.jpg',
      options: [],
      hobbies: '',
      rate: {},
      userRole: '',
      showModal: false,
      reviews: [],
      currentPage: 1,
      totalPages: 1,
      loading: false,
    };
  },
  watch: {
    profileImgSrc(newVal) {
      if (!newVal) {
        this.profileImgSrc = this.defaultProfileImgSrc;
      }
    },
  },

  mounted() {
    Securitybot();
    nextTick(() => {
      if (this.$refs.modalBody) {
        this.$refs.modalBody.addEventListener('scroll', this.handleScroll);
        this.showReviews(sessionStorage.getItem('UserId'));
      }
    });
    this.showReviews(sessionStorage.getItem('UserId'));
    this.fetchUserData(sessionStorage.getItem('UserId'));
  },
  beforeDestroy() {
    if (this.$refs.modalBody) {
      this.$refs.modalBody.removeEventListener('scroll', this.handleScroll); // Cleanup
    }
  },
  methods: {
    handleScroll(event) {
      const bottom = event.target.scrollHeight === event.target.scrollTop + event.target.clientHeight;
      if (bottom && !this.loading && this.currentPage < this.totalPages) {
        this.loadMoreReviews();
      }
    },
    loadMoreReviews() {
      this.loading = true;
      this.currentPage += 1;
      this.showReviews(sessionStorage.getItem('UserId')).finally(() => {
        this.loading = false;
      });
    },

    redirectToOffer(offerId) {
      this.$router.push({
        name: 'OfferDetail',
        params: {
          id: offerId
        }
      });
    },
    redirectToProfile(userId) {
      sessionStorage.setItem("UserId", userId);
      window.location.reload();
    },
    back() {
      window.history.back();
    },
    formatDate(dateString) {
      const options = { year: 'numeric', month: 'long', day: '2-digit' };
      return new Date(dateString).toLocaleDateString(undefined, options);
    },
    async showReviews(userId) {
      try {
        const response = await axiosInstance.get(`review/get-user-reviews?userId=${userId}&page=${this.currentPage}`);
        this.reviews.push(...response.data.items); // Append new reviews
        this.totalPages = response.data.totalPages; // Update total pages
      } catch (error) {
        console.error('Fehler beim Laden der Reviews:', error);
      }
    },
    onImageError(event) {
      event.target.src = this.defaultProfileImgSrc;
    },
    // Method to fetch user data
    async fetchUserData(id) {
      try {
        const response = await axiosInstance.get(`profile/get-user-profile/${id}`);
        this.user = response.data;
        this.profileImgSrc = `data:image/jpeg;base64,${response.data.profilePicture}`;
      } catch (error) {
        toast.info("Fehler beim Laden der Benutzerdaten!");
      }
    },

    redirectToFacebook(fblink) {
      window.open(fblink);
    },
    starClass(star) {
      return {
        'ri-star-fill gold': star === 'full',
        'ri-star-half-s-line gold': star === 'half',
        'ri-star-line': star === 'empty'
      };
    }
  },
  computed: {
    imageSrc() {
      return this.profileImgSrc || this.defaultProfileImgSrc;
    },
  }
};
</script>
<style scoped>
.v-container {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
}

.account-page {
  padding: 30px;
  border-radius: 8px;
  width: 100%;
}

.card {
  background-color: #fff;
  border: none;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
}

.card-body {
  padding: 20px;
}

.card-title {
  margin-bottom: 20px;
  font-size: 24px;
  font-weight: bold;
  color: #333;
  text-align: center;
}


.card-text-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.card-text {
  font-size: 16px;
  color: #555;
}

.card-text strong {
  color: #333;
}

.card-text a {
  color: #007bff;
  text-decoration: none;
}

.card-text a:hover {
  text-decoration: underline;
}

.star {
  font-size: 1.2em;
}

.gold {
  color: gold;
}

.average-rating {
  font-size: 0.9em;
  color: #555;
  margin-left: 10px;
}

.action-link {
  cursor: pointer;
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
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.modal-content {
  background-color: #fefefe;
  /* padding: 20px; */
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

.profile-content {
  margin-block-start: -5rem;
}


body .account-page {
  padding: 0 !important;
}

.leftBox-content .avatar.avatar-xxl {
  width: 8rem;
  height: 8rem;
  line-height: 5rem;
  font-size: 1.5rem;
  display: inline-block;
  position: relative;
}

.leftBox-content .avatar img {
  width: 100%;
  height: 100%;
  border-radius: 100px;
}

.account-page .border,
.account-page .list-group-item,
.account-page .border-bottom {
  border-color: #ecf3fb !important;
}

.profile-content,
.profile-content p {
  font-size: 14px;
}

.rating_block .star.gold {
  color: #f6a716;
}

.basic_info li {
  padding: 6px 15px;
}



.social_link_outer {
  width: 20px;
  height: 20px;
  line-height: 20px;
  display: inline-block;
  font-size: 12px;
  text-align: center;
  background-color: rgb(13 110 253) !important;
  color: #fff;
  margin-right: 2px;
  border-radius: 50px;
}

.fw-medium {
  font-weight: 500;
}

.skills_content .badge {
  font-size: 12px;
  font-weight: 500;
  background: #f0f6fd !important;
}

.basic_info {
  margin-bottom: 10px;
}


.icon {
  margin-inline-end: 8px;
  width: 1.75rem;
  height: 1.75rem;
  line-height: 1.65rem;
  font-size: .85rem;
  display: inline-block;
  text-align: center;
  border-radius: 50px;
}



.profile-banner-img {
  position: relative;
}

.icon4 {
  color: #ff8e6f !important;
  background-color: rgb(255 142 111 / 10%) !important;
}

.icon3 {
  color: #ff5d9f;
  background-color: rgb(255 93 159 / 10%);
}

.icon2 {
  color: rgb(227 84 212) !important;
  opacity: 1;
  background-color: rgb(227 84 212 / 10%) !important;
}

.icon1 {
  color: #5c67f7 !important;
  opacity: 1;
  background-color: rgb(92 103 247 / 10%) !important;
}


.profile-content .card-body {
  padding: 1rem;
}

.rating_block .star {
  font-size: 16px;
}

.themeColor {
  color: #0f97cb;
}

.hobbies_content .badge {
  font-size: 12px;
  font-weight: 500;
}

.social_btn {
  color: rgb(13 110 253) !important;
  font-size: 14px;
  padding: 0;
}

.social_btn .text-info {
  text-decoration: underline;
  margin-left: 5px;
}

.inner_banner_layout {
  position: relative;
  background: #f1f1f1;
  min-height: 170px;
  text-align: center;
  display: flex;
  align-items: center;
  background-size: cover;
  background-position: 90% center;
  background-size: cover;
  background-image: url(/images/profile_banner.webp);
}

.inner_banner_layout:before {
  content: "";
  position: absolute;
  top: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.65);
}

.deleted-user {
  color: #999;
  font-style: italic;
  text-decoration: line-through;
}

.deleted-offer {
  color: #999;
  font-style: italic;
  text-decoration: line-through;
}

.direct-review {
  color: #666;
  font-style: italic;
  font-size: 0.9em;
}

.clickable-item {
  cursor: pointer;
}

.clickable-item:hover {
  text-decoration: underline;
}
</style>
