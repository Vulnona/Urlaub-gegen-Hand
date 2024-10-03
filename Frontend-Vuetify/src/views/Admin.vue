<template>
  <Navbar />
  <div v-if="userRole !== 'Admin'">
    <div class="justify-content center">
      <section class="page_404">
        <div class="container">
          <div class="row">
            <div class="col-sm-12">
              <div class="col-sm-12 col-sm-offset-1 text-center">
                <div class="four_zero_four_bg">
                  <h1 class="text-center">404</h1>
                </div>
                <div class="contant_box_404">
                  <h3 class="h2">Look like you're lost</h3>
                  <p>The page you are looking for is not available!</p>
                  <button @click="goHome()" class="link_404">Go to Home</button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    </div>
  </div>
  <div v-else>
    <div class="Admin_inner_banner inner_banner_layout">
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <div class="inner_banner">
              <h2>Admin Panel</h2>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="section_space offers_request_layout admin-panel-grid">
      <div class="offers_request_content">
        <div class="card">
          <div class="card-header">
            <h1 class="main-title">Users List</h1>
            <div class="sort-select"><label>Filter by</label>
              <select class="form-control">
                <option value="1">New</option>
                <option value="4">Approved</option>
                <option value="3">Pending</option>
              </select>
            </div>
          </div>
          <div class="card-body">
            <table class="table theme_table">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>E-Mail</th>
                  <th>Code</th>
                  <th>VS</th>
                  <th>RS</th>
                  <th class="text-center">Status</th>
                  <th class="text-center">Action</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(user, index) in admin" :key="index">
                  <td @click="getProfile(user.user_Id)" class="clickable">{{ user.firstName }}</td>
                  <td @click="setModal(user.user_Id)" class="clickable">{{ user.email_Address }}</td>
                  <td>{{ user.code }}</td>
                  <td v-if="user.link_VS" class="vs_link">
                    <a class="" @click="showImageModal(decryptLink(user.link_VS))">
                      <i class="ri-eye-line"></i> View VS
                    </a>
                  </td>
                  <td v-else>No VS Data Available</td>
                  <td v-if="user.link_RS" class="vs_link">
                    <a class="" @click="showImageModal(decryptLink(user.link_RS))">
                      <i class="ri-eye-line"></i> View RS
                    </a>
                  </td>
                  <td v-else>No RS Data Available</td>
                  <td class="text-center">
                    <span class="newState badge badge-primary" v-if="user.verificationState === 0">New</span>
                    <span class="penState badge badge-warning" v-else-if="user.verificationState === 1">Pending</span>
                    <span class="failState badge badge-danger" v-else-if="user.verificationState === 2">Failed</span>
                    <span class="verState badge badge-success" v-else-if="user.verificationState === 3">Verified</span>
                  </td>
                  <td class="text-center">
                    <div class="action-icon-btns buttons_text">
                      <button title="Reactivate" class="icon_btn bg_ltblue" @click="reactivate_User(user.user_Id, 1)"
                        v-if="user.verificationState === 2 && user.verificationState !== 0">
                        Reactivate
                      </button>
                      <button title="Delete" class="icon_btn bg_ltred"
                        @click="deleteUser(user.user_Id, user.link_VS, user.link_RS)"
                        v-if="user.verificationState === 2 && user.verificationState !== 0">
                        Delete
                      </button>
                      <button title="Approve" class="icon_btn bg_ltgreen" @click="approveUser(user.user_Id, 3)"
                        v-if="user.verificationState === 1 || user.verificationState === 0">
                        Approve
                      </button>
                      <button title="Reject" class="icon_btn bg_ltred"
                        @click="rejectUser(user.user_Id, 2, user.link_VS, user.link_RS)"
                        v-if="user.verificationState === 1 || user.verificationState === 0">
                        Reject
                      </button>
                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
    <!-- Image Modal -->
    <div class="modal-container text-center" v-if="imageUrlToShow">
      <div class="modal-overlay" @click="closeImageModal"></div>
      <div class="modal-content">
        <span class="close" @click="closeImageModal">&times;</span>
        <img :src="imageUrlToShow" alt="Image" style="max-width: 100%; max-height: 80vh;">
      </div>
    </div>
    <!-- Email Modal -->
    <div class="modal-container text-center" v-if="selectedUser">
      <div class="modal-overlay" @click="closeModal"></div>
      <div class="modal-content">
        <span class="close" @click="closeModal">&times;</span>
        <h4>Send Email</h4>
        <form @submit.prevent="sendEmail">
          <div class="form-group">
            <label for="from" class="text-left">From:</label>
            <input id="from" type="text" value="info@alreco.de" disabled>
          </div>
          <div class="form-group">
            <label for="to" class="text-left">To:</label>
            <input id="to" type="text" :value="selectedUser.email_Address" disabled>
          </div>
          <div class="form-group">
            <label for="subject" class="text-left">Subject:</label>
            <input id="subject" v-model="emailSubject" type="text" required>
          </div>
          <div class="form-group">
            <label for="body" class="text-left">Body:</label>
            <textarea id="body" rows="5" v-model="emailBody" required></textarea>
          </div>
          <div class="modal-buttons">
            <button class="btn themeBtn common-btn" type="submit" :disabled="isSending">
              Send
            </button>
            <button class="btn-cancel common-btn" type="button" @click="closeModal" :disabled="isSending">
              Close
            </button>
          </div>
          <div v-if="isSending" class="loader"></div>
        </form>
      </div>
    </div>
  </div>
</template>
<script>
import Swal from "sweetalert2";
import router from '@/router';
import 'bootstrap/dist/css/bootstrap.min.css';
import "bootstrap/dist/js/bootstrap.min.js";
import { S3Client, DeleteObjectCommand } from '@aws-sdk/client-s3';
import CryptoJS from 'crypto-js';
import { createToastInterface } from "vue-toastification";
import "vue-toastification/dist/index.css"
import axiosInstance from "@/interceptor/interceptor"
import Navbar from "@/components/navbar/Navbar.vue";
import UserRole from "@/services/CheckUserRole";
import Securitybot from "@/services/SecurityBot";

export default {
  components: {
    Navbar,
  },
  name: "admin",
  data() {
    return {
      admin: [],
      customMessage: "",
      selectedUser: null,
      emailBody: "",
      emailSubject: "",
      userRole: UserRole(),
      imageUrlToShow: null,
      isSending: false
    };
  },
  mounted() {
    this.getdata();
    Securitybot();
  },
  created() {
    this.toast = createToastInterface({
      position: "top-right",
      timeout: 3000,
      closeOnClick: true,
      pauseOnFocusLoss: true,
      pauseOnHover: true,
      draggable: true,
      draggablePercent: 0.6,
      showCloseButtonOnHover: false,
      hideProgressBar: false,
      closeButton: "button",
      icon: true,
      rtl: false
    });
  },
  methods: {
    // Method to show an image modal
    showImageModal(imageUrl) {
      this.imageUrlToShow = imageUrl;
    },
    // Method to close the image modal
    closeImageModal() {
      this.imageUrlToShow = null;
    },
    // Method to decrypt a link using CryptoJS
    decryptLink(encryptedLink) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedLink, process.env.SECRET_KEY);
        return bytes.toString(CryptoJS.enc.Utf8);
      } catch (e) {
        //  console.error('Error decrypting link:', e);
        return null;
      }
    },
    // Method to fetch data from the server
    async getdata() {
      try {
        const res = await axiosInstance.get(`${process.env.baseURL}admin/get-all-users`);
        this.admin = res.data;
      } catch (error) {
        this.handleAxiosError(error);
      }
    },
    // Method to navigate to the home page
    goHome() {
      router.push('/home');
    },
    // Method to update the verification status of a user
    async statusUpdate(uid, staid) {
      await axiosInstance.post(`${process.env.baseURL}admin/update-verification-state/${uid}/${staid}`).then((res) => {
        // this.admin = res.data;
      }).catch((error) => {
        this.handleAxiosError(error);
      });
    },
    // Method to fetch a user's profile
    getProfile(uid) {
      axiosInstance.get(`${process.env.baseURL}admin/get-user-profile/${uid}`).then((res) => {
        //   console.log(res.data.profile.user_Id)
        sessionStorage.setItem("UserId", res.data.user_Id);
        router.push("/account");

      }).catch((error) => {
        this.handleAxiosError(error);
      });
    },
    // Method to delete a user and associated images from S3
    deleteUser(userid, link_VS, link_RS) {
      Swal.fire({
        title: "Are you sure?",
        text: "You want to delete This User!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
      }).then((result) => {
        if (result.isConfirmed) {
          axiosInstance.delete(`${process.env.baseURL}user/delete-user/${userid}`).then(() => {
            this.toast.success("User deleted successfully!");
            this.deleteImagesFromS3(link_VS, link_RS);
            this.getdata();
          }).catch((error) => {
            this.handleAxiosError(error);
          });
        }
      });
    },
    // Method to delete images from S3
    async deleteImagesFromS3(linkVS, linkRS) {
      if (linkVS) {
        await this.deleteImageFromS3(linkVS);
      }
      if (linkRS) {
        await this.deleteImageFromS3(linkRS);
      }
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

        const command = new DeleteObjectCommand({
          Bucket: process.env.S3_BUCKET_NAME,
          Key: decryptedLink.split(`${Aws_Url}`)[1],
        });

        await s3Client.send(command);
        //  console.log(`Deleted ${decryptedLink} from S3.`);
      } catch (error) {
        //console.error('Error deleting image from S3:', error);
      }
    },
    // Method to approve a user
    async approveUser(userid, statusid) {
      await this.statusUpdate(userid, statusid);
      await this.getdata(); // Ensure it fetches data after updating the status
      this.toast.success("Status Updated successfully!");
    },

    // Method to reject a user and delete associated images from S3
    async rejectUser(userid, statusid, link_VS, link_RS) {
      await this.statusUpdate(userid, statusid);
      await this.getdata(); // Ensure data is fetched after images are deleted
      this.toast.success("Status Updated successfully!");
      await this.deleteImagesFromS3(link_VS, link_RS);

    },

    // Method to reactivate a user
    async reactivate_User(userid, statusid) {
      await this.statusUpdate(userid, statusid);
      await this.getdata(); // Ensure data fetch after reactivation
      this.toast.success("Status Updated successfully!");
    },
    // Method to set the selected user for email modal
    setModal(userId) {
      this.selectedUser = this.admin.find(user => user.user_Id === userId);
      this.emailBody = "";
      this.emailSubject = "";
    },
    closeModal() {
      this.selectedUser = null;
    },
    // Method to send an email to the selected user
    sendEmail() {
      this.isSending = true;
      axiosInstance.post(`${process.env.baseURL}custom-mail/send`, {
        to: this.selectedUser.email_Address,
        subject: this.emailSubject,
        body: this.emailBody
      })
        .then(response => {
          this.toast.success("Email sent successfully!");
          this.closeModal();
        })
        .catch(error => {
          this.handleAxiosError(error);
        })
        .finally(() => {
          this.isSending = false;
        });
    },
    // Method to handle Axios errors
    handleAxiosError(error) {
      if (error.response) {
        if (error.response.status === 401) {
          this.toast.info("Your session has expired. Please log in again.")
            .then(() => {
              sessionStorage.clear();
              router.push('/');
            });
        } else {
          this.toast.info("An error occurred!");
        }
      } else if (error.request) {
        this.toast.info("A network error occurred. Please check your connection and try again.")
          .then(() => {
            router.push('/');
          });
      } else {
        this.toast.success("An error occurred");
      }
    }
  }
};
</script>
<style scoped>
.user-list {
  width: 100%;

  table {
    width: 100%;
    border-collapse: collapse;
  }

  th,
  td {
    padding: 8px;
    border: 1px solid #ddd;
  }

  th {
    text-align: left;
  }
}

.btn {
  padding: 4px 12px;
  font-size: 14px;
  border: 1px solid;
  border-radius: 5px;
  color: #fff;
  box-shadow: 0px 0px 2px rgb(0 0 0 / 36%);
}

.btn-danger {
  background: #b0061d;
  border-color: #b0061d;
}

.btn-success {
  background: #4daf4c;
  border-color: #4daf4c;
}

.cross_icon,
.tick_icon {
  position: relative;
  top: 2px;
}

.modal-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
}

.modal-content {
  position: relative;
  background: white;
  padding: 20px;
  width: 80%;
  max-width: 100%;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  z-index: 1001;
}

.modal-content[data-v-3ca9c9bd] {
  background: white;
  padding: 30px;
  border-radius: 5px;
  position: relative;
  z-index: 1000;
  width: 500px;
}

.close {
  position: absolute;
  top: 10px;
  right: 10px;
  font-size: 24px;
  font-weight: bold;
  cursor: pointer;
}

h2 {
  margin-top: 0;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.modal-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.btn-primary {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-primary:hover {
  background-color: #0056b3;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-secondary:hover {
  background-color: #817d7d;
}

.clickable {
  cursor: pointer;
  color: rgba(var(--bs-link-color-rgb));
  transition: color 0.3s ease;
}

.clickable:hover {
  text-decoration: underline;
}

.modal-container {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 5px;
  position: relative;
  z-index: 1000;
  width: 400px;
}

.modal-content .close {
  position: absolute;
  top: 10px;
  right: 10px;
  cursor: pointer;
}

.modal-buttons {
  margin-top: 20px;
}

.modal-buttons .btn-primary {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 10px 20px;
  cursor: pointer;
  border-radius: 5px;
}

.modal-buttons .btn-secondary {
  background-color: #6c757d;
  color: white;
  border: none;
  padding: 10px 20px;
  cursor: pointer;
  border-radius: 5px;
  margin-left: 10px;
}

.page_404 {
  padding: 40px 0;
  background: #fff;
  font-family: 'Arvo', serif;
}

.page_404 img {
  width: 100%;
}

.four_zero_four_bg {

  background-image: url(https://cdn.dribbble.com/users/285475/screenshots/2083086/dribbble_1.gif);
  height: 400px;
  background-position: center;
}


.four_zero_four_bg h1 {
  font-size: 80px;
}

.four_zero_four_bg h3 {
  font-size: 80px;
}

.link_404 {
  color: #fff !important;
  padding: 10px 20px;
  background: #39ac31;
  margin: 20px 0;
  display: inline-block;
}

.contant_box_404 {
  margin-top: -50px;
}

.loader {
  border: 4px solid #f3f3f3;
  border-radius: 50%;
  border-top: 4px solid #3498db;
  width: 24px;
  height: 24px;
  animation: spin 2s linear infinite;
  margin: 20px auto;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }

  100% {
    transform: rotate(360deg);
  }
}
</style>
