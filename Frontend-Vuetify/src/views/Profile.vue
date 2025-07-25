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
  <div class="row profile-content container mx-auto">
    <div class="col-xl-3">
      <div class="card custom-card overflow-hidden border p-0 leftBox-content">
        <div class="card-body border-bottom border-block-end-dashed">
          <div class="text-center">

            <span class="avatar avatar-xxl avatar-rounded online mb-3">
              <i v-if="user.verificationState === 'Verified'" @click="editProfilePic"
                 class="ri-pencil-line edit_icon"></i>
              <img :src="profileImgSrc || defaultProfileImgSrc" @error="onImageError" class="profile-img"
                   alt="User Profile Picture">
            </span>
            <h5 class="fw-semibold mb-1">{{ user.firstName }} {{ user.lastName }}</h5>
            <span @click="openReviewsModal()" class="action-link fs-13 font-normal">Alle Bewertungen anzeigen</span><br/>
            <span class="fs-13 font-normal" v-if="isActiveMember">Mitgliedschaft gültig bis: {{ formatMembershipDate(user.membershipEndDate) }}</span>
          </div>
        </div>

        <div v-if="userRole != 'Admin' && user.userRating != 0"
             class="rating_block d-flex mb-0 flex-wrap gap-3 p-3 justify-content-center border-bottom border-block-end-dashed">
          <div class="">
            <p class="card-text text-center mb-0">
              Nutzerbewertung:<span class="average-rating">{{ user.userRating }}</span>
              {{ " " }} <span class="star ri-star-fill gold"></span>
            </p>
          </div>

        </div>
        <div class="p-3 pb-1 d-flex flex-wrap justify-content-between">
          <div class="fw-medium fs-15 themeColor">
            Basisdaten:

          </div>
        </div>
        <div class="card-body border-bottom border-block-end-dashed p-0">
          <ul class="list-group list-group-flush basic_info">
            <li class="list-group-item border-0">
              <div>
                <span class="fw-medium me-2">Name:</span><span class="text-muted">
                  {{ user.firstName }} {{
 user.lastName
                  }}
                </span>
              </div>
            </li>
            <li class="list-group-item border-0">
              <div>
                <span class="fw-medium me-2">Geburtsdatum:</span><span class="text-muted">{{ user.dateOfBirth }}</span>
              </div>
            </li>
            <li class="list-group-item border-0">
              <div>
                <span class="fw-medium me-2">Geschlecht:</span><span class="text-muted">{{ user.gender }}</span>
              </div>
            </li>
          </ul>
          <div class="upload-btn text-center" v-if="user.verificationState != 'Verified'">
            <a class="btn btn-primary" @click="upload_id">
              Upload
              ID
            </a>
          </div>
          <div class="upload-btn text-center" v-if="user.verificationState == 'Verified' && !isActiveMember">
            <button class="btn btn-primary" @click="redeemCoupon()">Redeem Coupon</button>
          </div>
        </div>
      </div>
    </div>
    <div class="col-xl-9">
      <div class="card custom-card overflow-hidden border p-0">
        <div class="card-body">

          <div>
            <ul class="list-group list-group-flush border rounded-3">
              <li class="list-group-item p-3">
                <span class="fw-medium fs-15 d-block mb-3">General Info:</span>
                <div class="text-muted">
                  <div v-if="user.address" class="address-section">
                    <p class="mb-3">
                      <span class="icon icon2">
                        <i class="ri-map-pin-line align-middle fs-15"></i>
                      </span>
                      <span class="fw-medium text-default">Adresse: </span> {{ user.address.displayName }}
                    </p>
                    <!-- Nur Adresse und Koordinaten anzeigen, Details und Global entfernt -->
                    <p class="mb-0">
                      <span class="icon">
                        <i class="ri-navigation-line align-middle fs-15"></i>
                      </span>
                      <span class="fw-medium text-default">Koordinaten: </span> 
                      {{ user.address.latitude?.toFixed(6) }}, {{ user.address.longitude?.toFixed(6) }}
                      <button 
                        class="btn btn-sm btn-outline-primary ms-2" 
                        @click="showAddressOnMap"
                        title="Auf Karte anzeigen"
                      >
                        <i class="ri-map-2-line"></i>
                      </button>
                    </p>
                  </div>
                  <div v-else class="no-address">
                    <p class="mb-0 text-muted">
                      <span class="icon">
                        <i class="ri-map-pin-line align-middle fs-15"></i>
                      </span>
                      Keine Adresse hinterlegt
                    </p>
                  </div>
                </div>
              </li>
              <li class="list-group-item p-3 skills_content">
                <span class="fw-medium fs-15 d-block mb-3">Skills:</span>
                <div class="w-75">
                  <a v-for="skill in user.skills" :key="skill" href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">{{ skill }}</span>
                  </a>
                </div>
              </li>

              <li class="list-group-item p-3 hobbies_content">
                <span class="fw-medium fs-15 d-block mb-3">Hobbies:</span>
                <div class="w-75">
                  <a v-for="hobbies in user.hobbies" :key="hobbies" href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">{{ hobbies }}</span>
                  </a>
                </div>
              </li>

              <li class="list-group-item p-3 social_link">
                <span class="fw-medium fs-15 d-block mb-3">Social Media:</span>
                <ul class="d-flex align-items-center flex-wrap">
                  <li class="d-flex align-items-center gap-3 me-2">
                    <button v-if="user.facebookLink" @click="redirectToFacebook(user.facebookLink)" type="button" class="btn social_btn">
                      <span class="social_link_outer"><i class="ri-facebook-fill"></i></span>
                      <span class="text-info">Facebook</span>
                    </button>
                  </li>
                </ul>
              </li>
            </ul>
          </div>
          <div class="profile_btn">
            <div class="profile_group_btn" v-if="user.verificationState === 'Verified'">
              <button class="btn  btn-primary rounded" @click="editProfile">
                Editiere
                Profil
              </button>
            </div>
            <div class="profile_group_btn" v-if="user.verificationState === 'Verified'">
              <button class="btn  btn-primary rounded" @click="editUserData">
                Editiere Userdaten              </button>
            </div>
            <div class="profile_group_btn">
              <button class="btn  btn-primary rounded" @click="changePassword">
                Passwort ändern</button>
            </div>
            <button class="btn btn-danger" @click="deleteSkillsAndHobbies()">
              Skills & Hobbies löschen
            </button>
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
      <div class="modal-body">
        <div class="review_layout all_reviews_layout">
          <div class="row">
            <div class="col-sm-4">
              <div class="review_rating">
                <div class="rating-score mb-2">
                  <i class="ri-star-fill" aria-hidden="true"></i>{{ user.userRating }}<span>/5</span>
                </div>
              </div>
            </div>
            <div class="col-sm-8">
              <div class="all_reviews">
                <div class="flex_header">
                  <h5>{{ reviews.length }} Reviews</h5>
                  <div class="filter_search">
                  </div>
                </div>
                <div class="comments-area">
                  <div class="comment-list-wrap">
                    <ol class="comment-list">
                      <li v-for="userReviews in reviews" :key="userReviews" class="comment">
                        <div>
                          <div class="comment_head">
                            <h6 @click="redirectToOffer(userReviews.offer.id)" class="clickable-item">
                              {{
                              userReviews.offer.title
                              }}
                            </h6>
                            <div @click="redirectToOffer(userReviews.offer.id)" class="img-thumb clickable-item"
                                 v-if="userReviews.offer.imageData != null">
                              <img :src="'data:' + userReviews.offer.imageMimeType + ';base64,' + userReviews.offer.imageData">
                            </div>
                            <div class="img-thumb" v-if="userReviews.offer.imageData == null">
                              <img :src="defaultProfileImgSrc">
                            </div>
                          </div>
                          <div class="comment-body">
                            <div class="comment-author vcard" v-if="userReviews.reviewer.profilePicture != null">
                              <img alt="" @click="redirectToProfile(userReviews.reviewer.user_Id)"
                                   :src="'data:' + userReviews.offer.imageMimeType + ';base64,' + userReviews.reviewer.profilePicture"
                                   class="avatar avatar-80 photo clickable-item" height="80" width="80" decoding="async">
                            </div>
                            <div class="comment-author vcard" v-if="userReviews.reviewer.profilePicture == null">
                              <img alt="" @click="redirectToProfile(userReviews.reviewer.user_Id)"
                                   :src="defaultProfileImgSrc" class="avatar avatar-80 photo clickable-item" height="80"
                                   width="80" decoding="async">
                            </div>
                            <div class="comment-content">
                              <div class="comment-head">
                                <div class="comment-user">
                                  <div @click="redirectToProfile(userReviews.reviewer.user_Id)"
                                       class="user clickable-item">
                                    {{ userReviews.reviewer.firstName }} {{
                                      userReviews.reviewer.lastName
                                    }}
                                  </div>
                                  <div class="comment-date">
                                    <time :datetime="userReviews.createdAt">
                                      {{
                                      formatDate(userReviews.createdAt)
                                      }}
                                    </time>
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

  <div class="modal profile_pic_modal_layout" v-if="showPicModal == true">
    <div class="modal-content profile_pic_modal">
      <div class="modal-header">
        <h4 class="card-title mb-0 fs-14">Change Profile Picture</h4>
        <span class="close" @click="showPicModal = false">&times;</span>
      </div>
      <div class="modal-body">
        <div class="upload_container">
          <div v-if="profilePic" class="profile_pic_preview">
            <img :src="profilePic" alt="Profile Picture Preview">
          </div>
          <div class="profile_pic_upload_layout">
            <input type="file" id="profilePicInput" accept="image/*" @change="previewProfilePic" style="display: none;">
            <label for="profilePicInput">
              <div class="profile_pic_upload_btn">
                <i class="ri-upload-2-line"></i>
                <span class="ml-1">Upload Profile Picture</span>
              </div>
            </label>
          </div>
        </div>
      </div>
      <div class="modal-footer justify-content-end">
        <button type="button" class="btn themeBtn btn-sm" @click="submitProfilePic">Submit</button>
      </div>
    </div>
  </div>

</template>

<script>
  import router from "@/router";
  import Swal from 'sweetalert2';
  import axiosInstance from "@/interceptor/interceptor"
  import Navbar from "@/components/navbar/Navbar.vue";
  import {GetUserRole} from "@/services/GetUserPrivileges";
  import {isActiveMembership} from "@/services/GetUserPrivileges";
  import Securitybot from "@/services/SecurityBot";
  import getLoggedUserId from "@/services/LoggedInUserId";
  import toast from "@/components/toaster/toast";

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
        userRole: GetUserRole(),
        isActiveMember: isActiveMembership(),
        showModal: false,
        showPicModal: false,
        profilePic: null,
        selectedFile: null,
        userId: getLoggedUserId(),
        reviews: [],
      };
    },
    mounted() {
      Securitybot();
      this.fetchUserData();
    },
    watch: {
      profileImgSrc(newVal) {
        if (!newVal) {
          this.profileImgSrc = this.defaultProfileImgSrc;
        }
      },
    },
    methods: {
      formatMembershipDate(dateString) {
        if (!dateString) return '';
        const date = new Date(dateString);
        const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        const month = months[date.getMonth()];
        const day = date.getDate();
        const year = date.getFullYear();
        return `${month} ${day}, ${year}`;
      },
      async redeemCoupon() {
        try {
          // Step 1: Open SweetAlert for user input
          const { value: redeemCode } = await Swal.fire({
            title: 'Coupon einlösen',
            input: 'text',
            inputLabel: 'Gib deinen Coupon-Code ein',
            inputPlaceholder: 'Coupon-Code...',
            showCancelButton: true,
            confirmButtonText: 'Einreichen',
            cancelButtonText: 'Abbrechen',
            inputValidator: (value) => {
              if (!value) {
                return 'Sie müssen einen Coupon-Code eingeben!';
              }
            }
          });

          // Step 2: Check if the user entered a redeem code
          if (redeemCode) {
            // Post the redeem code to your API
            const response = await axiosInstance.post('coupon/redeem', {
              couponCode: redeemCode,
            });
            // Step 3: Handle the API response
            if (response.data.isSuccess == true) {
              sessionStorage.clear();
              router.push({ name: 'Login' });
              Swal.fire({
                icon: 'success',
                title: 'Coupon erfolgreich eingelöst',
                text: 'Bitte loggen Sie sich erneut ein!',
              });

            } else {
              Swal.fire({
                icon: '',
                title: 'Unable To Redeem',
                text: response.data.error.message || 'Fehler beim Einlösen des Coupons. Bitte versuchen Sie es erneut.'
              });
            }
          }
        } catch (error) {
          Swal.fire({
            icon: '',
            title: 'Oops!',
            text: 'Etwas ist schief gelaufen. Bitte versuchen Sie es erneut.'
          });
          console.error(error);
        }
      },
      openReviewsModal() {
        this.showModal = true;
        this.showReviews(this.userId);
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
        router.push("/account");
      },
      formatDate(dateString) {
        const options = { year: 'numeric', month: 'long', day: '2-digit' };
        return new Date(dateString).toLocaleDateString(undefined, options);
      },
      async showReviews(userid) {
        try {
          const response = await axiosInstance.get(`review/get-user-reviews?userId=${userid}`);
          this.reviews = response.data.items;
        } catch (error) {
          console.error('Error fetching reviews:', error);
        }
      },
      // Preview selected profile picture
      previewProfilePic(event) {
        const file = event.target.files[0];
        if (file) {
          this.selectedFile = file;
          const reader = new FileReader();
          reader.onload = (e) => {
            this.profilePic = e.target.result;
          };
          reader.readAsDataURL(file);
        }
      },

      // Submit the profile picture as Base64 JSON to the API
      async submitProfilePic() {
        if (!this.selectedFile) {
          toast.info("Bitte wählen Sie ein Profilbild aus!");
          return;
        }

        const reader = new FileReader();
        reader.onloadend = async () => {
          const base64String = reader.result.split(',')[1];
          const requestBody = {
            ProfilePicture: base64String
          };

          try {
            const response = await axiosInstance.put(`profile/update-profile-picture`, requestBody, {
              headers: {
                'Content-Type': 'application/json'
              }
            });

            if (response.status === 200) {
              toast.success("Profilbild erfolgreich aktualisiert!");
              this.showPicModal = false;
              this.fetchUserData();
            } else {
              toast.info("Fehler beim Aktualisieren des Profilbilds!");
            }
          } catch (error) {
            console.error(error);
            toast.info("Ein Fehler ist aufgetreten, bitte melden Sie sich beim Support");
          }
        };

        reader.readAsDataURL(this.selectedFile);
      }
      ,
      onImageError(event) {
        event.target.src = this.defaultProfileImgSrc;
      },
      redirectToFacebook(fblink) {
        window.open(fblink);
      },
      editProfilePic() {
        this.showPicModal = true;
      },
      upload_id() {
        router.push("/upload-id").then(() => { });
      },
      // Method to delete skills and hobbies 
      async deleteSkillsAndHobbies() {
        Swal.fire({
          title: "Sind Sie sicher?",
          text: "Sie möchten Ihre Fertigkeiten und Hobbies löschen!",
          icon: "warning",
          showCancelButton: true,
          confirmButtonColor: "#3085d6",
          cancelButtonColor: "#d33",
          confirmButtonText: "Ja, löschen!",
        }).then(async (result) => {
          if (result.isConfirmed) {
            try {
              const response = await axiosInstance.get('profile/get-user-profile');
              const userData = response.data.profile;
              const updateData = {
                firstName: userData.firstName,
                lastName: userData.lastName,
                gender: userData.gender,
                dateOfBirth: userData.dateOfBirth,
                displayName: userData.address?.displayName || '',
                latitude: userData.address?.latitude || '',
                longitude: userData.address?.longitude || '',
                id: userData.address?.id || null,
                skills: '',
                hobbies: ''
              };
              // dateOfBirth ggf. ins richtige Format bringen
              if (updateData.dateOfBirth instanceof Date) {
                updateData.dateOfBirth = updateData.dateOfBirth.toISOString().slice(0, 10);
              } else if (typeof updateData.dateOfBirth === 'string' && updateData.dateOfBirth.length > 10) {
                // Falls String mit Zeitanteil
                updateData.dateOfBirth = updateData.dateOfBirth.slice(0, 10);
              }
              await axiosInstance.put('profile/update-user-data', updateData);
              toast.success("Fertigkeiten und Hobbies erfolgreich gelöscht!");
              this.fetchUserData();
            } catch (error) {
              toast.error("Fehler beim Löschen der Daten.");
            }
          }
        });
      },

      // User und S3-Bilder löschen
      async deleteUser() {
        Swal.fire({
          title: "Sind Sie sicher?",
          text: "Ihr Account und alle zugehörigen Bilder werden unwiderruflich gelöscht!",
          icon: "warning",
          showCancelButton: true,
          confirmButtonColor: "#3085d6",
          cancelButtonColor: "#d33",
          confirmButtonText: "Ja, Account löschen!",
        }).then(async (result) => {
          if (result.isConfirmed) {
            try {
              await axiosInstance.delete('profile/delete-user');
              toast.success("Account und Bilder erfolgreich gelöscht!");
              // Optional: Weiterleitung nach Logout
              window.location.href = '/logout';
            } catch (error) {
              toast.error("Fehler beim Löschen des Accounts.");
            }
          }
        });
      },

      // Function to fetch user data using API request
      async fetchUserData() {
        try {
          const response = await axiosInstance.get(`profile/get-user-profile`);
          this.user = response.data.profile;
          this.profileImgSrc = `data:image/jpeg;base64,${response.data.profile.profilePicture}`;
        } catch (error) {
          toast.info("Benutzerdaten konnten nicht abgerufen werden");
        }
      },
      editProfile() {
        router.push('/edit-profile');
      },
      editUserData() {
        router.push('/edit-user-data');
      },
      changePassword() {
        router.push('/change-password');
      },
      showAddressOnMap() {
        if (this.user.address && this.user.address.latitude && this.user.address.longitude) {
          // Open address in new window with OpenStreetMap
          const lat = this.user.address.latitude;
          const lng = this.user.address.longitude;
          const url = `https://www.openstreetmap.org/?mlat=${lat}&mlon=${lng}&zoom=15&layers=M`;
          window.open(url, '_blank');
        } else {
          toast.info('Keine Koordinaten für diese Adresse verfügbar');
        }
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
    margin-bottom: 10px;
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
    padding: 10px;
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
</style>
