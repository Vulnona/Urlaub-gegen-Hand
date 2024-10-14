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
            <i v-if="user.verificationState === 3" @click="editProfilePic" class="ri-pencil-line edit_icon"></i>
            <span class="avatar avatar-xxl avatar-rounded online mb-3">

              <img :src="profileImgSrc || defaultProfileImgSrc" @error="onImageError" class="profile-img"
                alt="User Profile Picture">
            </span>
            <h5 class="fw-semibold mb-1">{{ user.firstName }} {{ user.lastName }}</h5>
            <span class="d-block fw-medium text-muted mb-2">Host</span>
            <!-- <p class="fs-12 mb-0 text-muted"> <span class="me-3"><i class="ri-building-line me-1 align-middle"></i>Hamburg</span> 
                    <span><i class="ri-map-pin-line me-1 align-middle"></i>Germany</span> </p> -->
          </div>
        </div>

        <div v-if="userRole != 'Admin' && user.averageRating != 0"
          class="rating_block d-flex mb-0 flex-wrap gap-3 p-3 justify-content-center border-bottom border-block-end-dashed">
          <div class="">
            <p class="card-text text-center mb-0"><span class="star ri-star-fill gold"></span>
              <span class="star ri-star-fill gold"></span>
              <span class="star ri-star-fill gold"></span>
              <span class="star ri-star-half-s-line gold"></span>
              <span class="star ri-star-line"></span>
            </p>
            <p class="card-text text-center">
              <span class="average-rating">{{ user.averageRating }}</span> <span @click="showModal = true"
                class="action-link fs-13 font-normal">View All</span>
            </p>
          </div>
        </div>
        <div class="p-3 pb-1 d-flex flex-wrap justify-content-between">
          <div class="fw-medium fs-15 themeColor">
            Basic Info :
          </div>
        </div>
        <div class="card-body border-bottom border-block-end-dashed p-0">
          <ul class="list-group list-group-flush basic_info">
            <li class="list-group-item border-0">
              <div>
                <span class="fw-medium me-2">Name :</span><span class="text-muted">{{ user.firstName }} {{ user.lastName
                  }}</span>
              </div>
            </li>
            <li class="list-group-item border-0">
              <div><span class="fw-medium me-2">DOB :</span><span class="text-muted">{{ user.dateOfBirth }}</span>
              </div>
            </li>
            <li class="list-group-item border-0">
              <div><span class="fw-medium me-2">Gender:</span><span class="text-muted">{{ user.gender }}</span>
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
            <ul class="list-group list-group-flush border rounded-3">
              <li class="list-group-item p-3">
                <span class="fw-medium fs-15 d-block mb-3">General Info :</span>
                <div class="text-muted">
                  <p class="mb-3">
                    <span class="icon icon2">
                      <i class="ri-map-pin-line align-middle fs-15"></i>
                    </span>
                    <span class="fw-medium text-default">House Number : </span> {{ user.houseNumber }}
                  </p>
                  <p class="mb-3">
                    <span class="icon icon3">
                      <i class="ri-building-line align-middle fs-15"></i>
                    </span>
                    <span class="fw-medium text-default">Address : </span>{{ user.houseNumber }}, {{ user.street }},
                    {{ user.country }}
                  </p>
                  <p class="mb-0">
                    <span class="icon icon4">
                      <i class="ri-phone-line align-middle fs-15"></i>
                    </span>
                    <span class="fw-medium text-default">Postal code : </span> {{ user.postCode }}
                  </p>
                </div>
              </li>
              <li class="list-group-item p-3 hobbies_content">
                <span class="fw-medium fs-15 d-block mb-3">Hobbies:</span>
                <div class="w-75">
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Leadership</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Project Management</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Technical
                      Proficiency</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Communication</span>
                  </a>
                </div>
              </li>
              <li class="list-group-item p-3 skills_content">
                <span class="fw-medium fs-15 d-block mb-3">Skills:</span>
                <div class="w-75">
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Leadership</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Project Management</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Technical
                      Proficiency</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Communication</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Team Building</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Problem-Solving</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Stakeholder
                      Management</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Conflict
                      Resolution</span>
                  </a>
                  <a href="javascript:void(0);">
                    <span class="badge bg-light text-muted m-1 border">Continuous
                      Improvement</span>
                  </a>
                </div>
              </li>
              <li class="list-group-item p-3 social_link">
                <span class="fw-medium fs-15 d-block mb-3">Social Media :</span>
                <ul class="d-flex align-items-center flex-wrap">
                  <li class="d-flex align-items-center gap-3 me-2">
                    <button @click="redirectToFacebook(user.facebookLink)" type="button" class="btn social_btn">
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
                  <i class="fa fa-star" aria-hidden="true"></i>5<span>/5</span>
                </div>
                <div class="">
                  <label>Overall Review</label>
                  <div class="review-criteria">
                    <div v-for="star in 5" :key="star" class="review-item">
                      <span class="in-value">
                        <span>{{ 6 - star }} <i class="ri-star-line"></i></span>
                      </span>
                      <div class="review-progress">
                        <span class="progress-bar" :style="{ width: ((6 - star) * 20) + '%' }"></span>
                      </div>
                      <div class="review_range">{{ (6 - star) * 100 }}</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-sm-8">
              <div class="all_reviews">
                <div class="flex_header">
                  <h5>5 Reviews</h5>
                  <div class="filter_search">
                    <select class="form-control">
                      <option value="1">Most Recent</option>
                      <option value="4">Positive First</option>
                      <option value="3">Negative First</option>
                    </select>
                  </div>
                </div>
                <div class="comments-area">
                  <div class="comment-list-wrap">
                    <ol class="comment-list">
                      <li v-for="index in 2" :key="index" class="comment">
                        <div>
                          <div class="comment_head">
                            <h6>5 Star with an access to Golf Course</h6>
                            <div class="img-thumb"><img
                                src="https://a0.muscache.com/im/pictures/miso/Hosting-797359417161342415/original/9e4be089-334e-4309-939b-729b4b93b75e.jpeg?aki_policy=small">
                            </div>
                          </div>
                          <div class="comment-body">
                            <div class="comment-author vcard">
                              <img alt=""
                                src="https://secure.gravatar.com/avatar/8eb1b522f60d11fa897de1dc6351b7e8?s=80&amp;d=mm&amp;r=g"
                                srcset="https://secure.gravatar.com/avatar/8eb1b522f60d11fa897de1dc6351b7e8?s=160&amp;d=mm&amp;r=g 2x"
                                class="avatar avatar-80 photo" height="80" width="80" decoding="async">
                            </div>
                            <div class="comment-content">
                              <div class="comment-head">
                                <div class="comment-user">
                                  <div class="user">Joe Doe</div>
                                  <div class="comment-date">
                                    <time datetime="2024-08-02T09:54:50+00:00">August 2, 2024</time>
                                  </div>
                                  <div class="comment-rating-stars stars">
                                    <span v-for="star in 5" :key="star" class="star" :class="'star-' + star">
                                      <i class="ri-star-line"></i>
                                    </span>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div>
                            <div class="comment-text">
                              <p class="mb-0">Everything was seamless and well-organized. The guide was engaging, the
                                activities were fun, and the food was delicious.</p>
                            </div>
                          </div>
                        </div>
                        <ol class="comment_reply_layout">
                          <li class="comment_reply">
                            <div>
                              <div class="comment-body">
                                <div class="comment-author vcard">
                                  <img alt=""
                                    src="https://secure.gravatar.com/avatar/8eb1b522f60d11fa897de1dc6351b7e8?s=80&amp;d=mm&amp;r=g"
                                    srcset="https://secure.gravatar.com/avatar/8eb1b522f60d11fa897de1dc6351b7e8?s=160&amp;d=mm&amp;r=g 2x"
                                    class="avatar avatar-80 photo" height="80" width="80" decoding="async">
                                </div>
                                <div class="comment-content">
                                  <div class="comment-head">
                                    <div class="comment-user">
                                      <div class="user">David</div>
                                      <div class="comment-date">
                                        <time datetime="2024-08-02T09:54:50+00:00">August 2, 2024</time>
                                      </div>
                                    </div>
                                  </div>
                                </div>
                              </div>
                              <div class="comment-text">
                                <p class="mb-0">Everything was seamless and well-organized. The guide was engaging, the
                                  activities were fun, and the food was delicious.</p>
                              </div>
                            </div>
                          </li>
                        </ol>
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
import router from "@/router";
import axios from "axios";
import Swal from 'sweetalert2';
import CryptoJS from 'crypto-js';
import VueJwtDecode from 'vue-jwt-decode';
import {
  S3Client,
  DeleteObjectCommand
} from '@aws-sdk/client-s3';
import axiosInstance from "@/interceptor/interceptor"
import Navbar from "@/components/navbar/Navbar.vue";
import userRole from "@/services/CheckUserRole";
import isActiveMember from "@/services/CheckActiveMembership";
import Securitybot from "@/services/SecurityBot";
import toast from "@/components/toaster/toast";

// Profile options bitmask enumeration
const ProfileOptions = {
  None: 0,
  Smoker: 1 << 0,
  PetOwner: 1 << 1,
  HaveLiabilityInsurance: 1 << 2
};
// Global variable for storing log ID
let globalLogId = '';
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
      userRole: userRole(),
      isActiveMember: isActiveMember(),
      globalEmail: '',
      showModal: false,
      showPicModal: false,
      profilePic: null,
      selectedFile: null
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
    // Preview selected profile picture
    previewProfilePic(event) {
      const file = event.target.files[0];
      if (file) {
        this.selectedFile = file;
        const reader = new FileReader();
        reader.onload = (e) => {
          this.profilePic = e.target.result;
        };
        reader.readAsDataURL(file); // Convert image to base64 for preview
      }
    },

    // Submit the profile picture as Base64 JSON to the API
    async submitProfilePic() {
      if (!this.selectedFile) {
        toast.info("Please Select a profile picture!");
        return;
      }

      const reader = new FileReader();
      reader.onloadend = async () => {
        const base64String = reader.result.split(',')[1]; // Get the base64 content only
        const requestBody = {
          ProfilePic: base64String // Backend expects this field
        };

        try {
          const response = await axiosInstance.put(`${process.env.baseURL}profile/update-profile-picture`, requestBody, {
            headers: {
              'Content-Type': 'application/json'
            }
          });

          if (response.status === 200) {
            toast.success("Profile picture updated successfully!");
            this.showPicModal = false;  // Close modal on success
            this.fetchUserData();
          } else {
            toast.info("Failed to update proile picture!");
          }
        } catch (error) {
          console.error(error);
          toast.info("An Error Occoured At Our End!");
        }
      };

      reader.readAsDataURL(this.selectedFile); // Convert image to Base64
    }
    ,
    onImageError(event) {
      event.target.src = this.defaultProfileImgSrc;
    },
    redirectToFacebook(fblink) {
      window.open(fblink);
    },

    // Function to check user login status and retrieve user role from JWT token
    checkLoginStatus() {
      const token = sessionStorage.getItem("token");
      const decryptToken = this.decryptToken(token);
      globalToken = decryptToken;
      const testlogid = this.decryptlogID(sessionStorage.getItem("logId"));
      globalLogId = testlogid;
      if (token) {
        const decryptedToken = this.decryptToken(token);
        if (decryptedToken) {
          const decodedToken = VueJwtDecode.decode(decryptedToken);
          this.userRole = decodedToken[`${process.env.claims_Url}`] || '';
        } else {
          sessionStorage.removeItem('token');
        }
      }
    },
    editProfilePic() {
      this.showPicModal = true;
    },
    upload_id() {
      router.push("/upload-id").then(() => { });
    },
    // Function to decrypt encrypted token using AES decryption
    decryptToken(encryptedToken) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
        return bytes.toString(CryptoJS.enc.Utf8);
      } catch (e) {
        //    console.error('Error decrypting token:', e);
        return null;
      }
    },
    // Function to decrypt encrypted log ID and parse it as an integer
    decryptlogID(encryptedItem) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY);
        const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
        return parseInt(decryptedString, 10).toString();
      } catch (e) {
        // console.error('Error decrypting item:', e);
        return null;
      }
    },
    // Method to delete a user and associated images from S3
    deleteUser(userid, link_VS, link_RS) {
      Swal.fire({
        title: "Are you sure?",
        text: "You want to delete your Data!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
      }).then((result) => {
        if (result.isConfirmed) {
          axiosInstance.delete(`${process.env.baseURL}user/delete-user/${userid}`).then(() => {
            toast.success("User data deleted successfully!");
            this.deleteImagesFromS3(link_VS, link_RS);
            sessionStorage.clear();
            router.push("/");
          }).catch((error) => {
            //    console.log(error);
          });
        }
      });
    },
    // Method to delete an image from S3
    async deleteImageFromS3(encryptedLink) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedLink, process.env.SECRET_KEY);
        const decryptedLink = bytes.toString(CryptoJS.enc.Utf8);
        const s3Client = new S3Client({
          region: process.env.Aws_region,
          credentials: {
            accessKeyId: process.env.AccessKeyId,
            secretAccessKey: process.env.SecretAccessKey,
          },
        });
        const imageKey = decryptedLink.replace(process.env.Aws_Url, '');
        const command = new DeleteObjectCommand({
          Bucket: process.env.S3_BUCKET_NAME,
          Key: imageKey,
        });
        await s3Client.send(command);
        //   console.log(`Deleted ${decryptedLink} from S3.`);
      } catch (error) {
     
      }
    },
    // Function to fetch user data using API request
    async fetchUserData() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}profile/get-user-profile`);
        this.user = response.data.profile;
        this.profileImgSrc = `data:image/jpeg;base64,${response.data.profile.profilePicture}`;
        this.options = this.processOptions(response.data.profile.options);
        this.hobbies = response.data.profile.hobbies;
      } catch (error) {
        if (error.response && error.response.status === 500) {
          toast.info("Deine Sitzung ist abgelaufen. Bitte logge dich erneut ein");
          sessionStorage.clear();
          router.push('/');
        } else {
          toast.info("Benutzerdaten konnten nicht abgerufen werden");
        }
      }
    },
    // Function to process user profile options bitmask and return corresponding array
    processOptions(options) {
      const result = [];
      if (options & ProfileOptions.Smoker) result.push('raucht');
      if (options & ProfileOptions.PetOwner) result.push('besitzt Tier(e)');
      if (options & ProfileOptions.HaveLiabilityInsurance) result.push('ist haftpflichtversichert');
      return result;
    },
    editProfile() {
      router.push('/edit-profile');
    },
    // Function to dynamically set star class based on rating
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
    stars() {
      const stars = [];
      const integerPart = Math.floor(this.user.averageRating);
      const decimalPart = this.user.averageRating - integerPart;
      for (let i = 0; i < 5; i++) {
        if (i < integerPart) {
          stars.push('full');
        } else if (decimalPart >= 0.5 && i === integerPart) {
          stars.push('half');
        } else {
          stars.push('empty');
        }
      }
      return stars;
    }
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

/*
    .btn-primary {
      margin-top: 20px;
      display: block;
      width: 100%;
      text-align: center;
    } */
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
</style>
<style scoped>
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