<template>
  <div v-if="userRole !== 'Admin'">
    <Errorpage />
  </div>
  <div v-else>
    <Navbar />
    <div class="inner_banner_layout">
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
  <div class="header-actions">
    <!-- Search Bar -->
    <input 
      type="text" 
      v-model="searchQuery" 
      class="search-field" 
      placeholder="Search email..." 
    />

    <!-- Sort Dropdown -->
    <select v-model="sortKey" class="sort-dropdown">
      <option value="" disabled selected>Sort By</option>
      <option value="FirstName">Name</option>
      <option value="Email_Address">Email</option>
      <option value="VerificationState">Status</option>
    </select>

    <!-- Ascending/Descending Toggle -->
    <button @click="toggleSortOrder" class="sort-order-btn">
      <i :class="sortOrder === 'asc' ? 'ri-arrow-up-line' : 'ri-arrow-down-line'"></i>
    </button>

    <!-- Generate Button -->
    <button v-if="!couponCode" class="btn btn-primary" @click="generateCode()">
      Generate Coupon Code
    </button>

    <!-- Display the Coupon Code if Available -->
    <p v-if="couponCode">
      <input type="text" :value="couponCode" class="coupon-input" disabled />&nbsp;
      <!-- Clear Button -->
      <button class="" title="Clear" @click="clearCode()">
        <i class="ri-close-circle-line"></i>
      </button>
    </p>
  </div>
</div>


          <div class="card-body">
            <!-- Add a responsive wrapper -->
            <div class="table-responsive">
              <table class="table theme_table">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>E-Mail</th>
                    <!-- <th>Code</th> -->
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
                    <!-- <td>{{ user.code }}</td> -->
                    <td v-if="user.link_VS" class="vs_link">
                      <a class="" @click="showImageModal(user.link_VS, user)">
                        <i class="ri-eye-line"></i> View VS
                      </a>
                    </td>
                    <td v-else>No VS Data Available</td>
                    <td v-if="user.link_RS" class="vs_link">
                      <a class="" @click="showImageModal(user.link_RS, user)">
                        <i class="ri-eye-line"></i> View RS
                      </a>
                    </td>
                    <td v-else>No RS Data Available</td>
                    <td class="text-center">
                      <span class="newState badge badge-primary" v-if="user.verificationState === 0">New</span>
                      <span class="penState badge badge-warning" v-else-if="user.verificationState === 1">Pending</span>
                      <span class="failState badge badge-danger" v-else-if="user.verificationState === 2">Failed</span>
                      <span class="verState badge badge-success"
                        v-else-if="user.verificationState === 3">Verified</span>
                    </td>
                    <td class="text-center">
                      <div class="action-icon-btns buttons_text">
                        <button title="Reactivate" class="icon_btn bg_ltblue" @click="reactivate_User(user.user_Id, 1)"
                          v-if="user.verificationState === 2 && user.verificationState !== 0">
                          Reactivate
                        </button>
                        <button title="Delete" class="icon_btn bg_ltred" @click="deleteUser(user.user_Id)"
                          v-if="user.verificationState === 2 && user.verificationState !== 0">
                          Delete
                        </button>
                        <button title="Approve" class="icon_btn bg_ltgreen" @click="approveUser(user.user_Id, 3)"
                          v-if="user.verificationState === 1 || user.verificationState === 0">
                          Approve
                        </button>
                        <button title="Reject" class="icon_btn bg_ltred" @click="rejectUser(user.user_Id, 2)"
                          v-if="user.verificationState === 1 || user.verificationState === 0">
                          Reject
                        </button>
                        <button title="Coupon" class="icon_btn bg_ltgreen" @click="handleSendCode(user)"
                          v-if="user.verificationState === 3 && user.membershipId == null">
                          Send Code
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
    </div>
    <!-- Image Modal -->
    <div class="modal-container-2 text-center" v-if="imageUrlToShow">
      <div class="modal-overlay-2" @click="closeImageModal"></div>
      <div class="modal-content-2">
        <span class="close" @click="closeImageModal">&times;</span>
        <div class="modal-body-2">
          <div class="user-data">
            <div class="card">
              <h4>Benutzerdaten</h4>
              <p><strong>Vollständiger Name:</strong> {{ userdata.firstName }} {{ userdata.lastName }}</p>
              <p><strong>Geburtsdatum:</strong> {{ userdata.dateOfBirth }}</p>
              <p><strong>Geschlecht:</strong> {{ userdata.gender }}</p>
              <p><strong>Land:</strong> {{ userdata.country }}</p>
              <p><strong>Bundesland:</strong> {{ userdata.state }}</p>
              <p><strong>Stadt:</strong> {{ userdata.city }}</p>
              <p><strong>Postleitzahl:</strong> {{ userdata.postCode }}</p>
              <p><strong>Hausnummer:</strong> {{ userdata.houseNumber }}</p>
              <p><strong>Straßenadresse:</strong> {{ userdata.street }}</p>
            </div>
          </div>
          <div class="image-container">
            <img :src="imageUrlToShow" alt="Image" class="modal-image">
          </div>
        </div>
      </div>
    </div>
    <!-- Pagination Section -->
    <div class="pagination">
      <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1">
        <i class="ri-arrow-left-s-line"></i>Previous
      </button>
      <span>Page {{ currentPage }} of {{ totalPages }}</span>
      <button class="action-link" @click="changePage(currentPage + 1)" :hidden="currentPage === totalPages">
        Next<i class="ri-arrow-right-s-line"></i>
      </button>
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
import axiosInstance from "@/interceptor/interceptor"
import Navbar from "@/components/navbar/Navbar.vue";
import Securitybot from "@/services/SecurityBot";
import {GetUserRole} from "@/services/GetUserPrivileges";
import Errorpage from "../Errorpage.vue";
import toast from "@/components/toaster/toast";
export default {
  components: {
    Navbar,
    Errorpage
  },
  name: "admin",
  data() {
    return {
      currentIndex: 0,
      currentPage: 1,
      totalPages: 1,
      pageSize: 10,
      admin: [],
      customMessage: "",
      selectedUser: null,
      emailBody: "",
      emailSubject: "",
      userRole: GetUserRole(),
      imageUrlToShow: null,
      isSending: false,
      userdata: null,
      couponCode: "",
      searchQuery: '',
      sortKey: '',
      sortOrder: 'asc',
    };
  },
  watch: {
    // Watch for changes in searchQuery, sortKey, or currentPage to refetch data
    searchQuery: 'getdata',
    sortKey: 'getdata',
    currentPage: 'getdata',
  },
  mounted() {
    this.getdata();
    Securitybot();
  },
  methods: {
    changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.currentPage = newPage;
        this.getdata(); // fetch new data for the selected page
      }
    },
    // Method to show an image modal
    showImageModal(imageUrl, userdata) {
      axiosInstance
        .get(imageUrl, { responseType: 'arraybuffer' })
        .then(response => {
          const blob = new Blob([response.data], { type: 'image/jpeg' });
          this.imageUrlToShow = URL.createObjectURL(blob);
          this.userdata = userdata;
        })
        .catch(error => {
          console.error('Error fetching image:', error);
        });
    },
    // Method to close the image modal
    closeImageModal() {
      this.imageUrlToShow = null;
    },
    // Method to fetch data from the server
    async getdata() {
      try {
        const res = await axiosInstance.get(`${process.env.baseURL}admin/get-all-users`, {
          params: {
            pageSize: this.pageSize,
            pageNumber: this.currentPage,
            searchTerm: this.searchQuery, // Add search query to the request
            sortBy: this.sortKey, // Add sort key to the request
            sortDirection: this.sortOrder, // Add sort order to the request
          },
        });
        this.admin = res.data.items;
        this.totalPages = Math.ceil(res.data.totalCount / this.pageSize);
      } catch (error) {
        this.handleAxiosError(error);
      }
    },
    // Toggle sort order between ascending and descending
    toggleSortOrder() {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
      this.getdata(); // Refetch data with updated sort order
    },
    // Method to navigate to the home page
    goHome() {
      router.push('/home');
    },
    async generateCode() {
      try {
        const res = await axiosInstance.post(`${process.env.baseURL}coupon/add-coupon`);
        this.couponCode = res.data.value; // Assign the generated code to `couponCode`
        toast.success("Coupon Code generated successfully!");
      } catch (error) {
        this.handleAxiosError(error); // Handle errors with a helper method
      }
    },
    copyCode() {
      // Copy the coupon code to the clipboard
      navigator.clipboard
        .writeText(this.couponCode)
        .then(() => {
          toast.success("Coupon Code copied to clipboard!");
        })
        .catch(() => {
          toast.error("Failed to copy the code.");
        });
    },
    clearCode() {
      // Clear the coupon code
      this.couponCode = "";
    },
    async sendCouponCode(uid, code) {
      try {
        const res = await axiosInstance.post(
          `${process.env.baseURL}coupon/send-coupon`,
          {
            userId: uid,
            couponCode: code,
          }
        );
        if (res.data.isSuccess == true) {
          toast.success(res.data.value);
        }
        else {
          toast.error(res.data.error.message);
        }
      } catch (error) {
        toast.error("Something went wrong!");
      }
    },

    async handleSendCode(user) {
      const { value: code } = await Swal.fire({
        title: "Enter Coupon Code",
        input: "text",
        inputPlaceholder: "Enter the coupon code here",
        showCancelButton: true,
        confirmButtonText: "Send",
        cancelButtonText: "Cancel",
        inputValidator: (value) => {
          if (!value) {
            return 'You need to enter a coupon code!';
          }
        }
      });

      if (code) {
        this.sendCouponCode(user.user_Id, code);
      }
    },

    // Method to update the verification status of a user
    async statusUpdate(uid, staid) {
      await axiosInstance.post(`${process.env.baseURL}admin/update-verification-state/${uid}/${staid}`).then((res) => {
      }).catch((error) => {
        this.handleAxiosError(error);
      });
    },
    // Method to fetch a user's profile
    getProfile(uid) {
      axiosInstance.get(`${process.env.baseURL}admin/get-user-profile/${uid}`).then((res) => {
        sessionStorage.setItem("UserId", res.data.user_Id);
        router.push("/account");

      }).catch((error) => {
        this.handleAxiosError(error);
      });
    },
    // Method to delete a user and associated images from S3
    deleteUser(userid) {
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
          axiosInstance.delete(`${process.env.baseURL}admin/delete-admin-user/${userid}`).then(() => {
            toast.success("User deleted successfully!");
            this.getdata();
          }).catch((error) => {
            this.handleAxiosError(error);
          });
        }
      });
    },
    // Method to approve a user
    async approveUser(userid, statusid) {
      await this.statusUpdate(userid, statusid);
      await this.getdata();
      toast.success("Status Updated successfully!");
    },

    // Method to reject a user and delete associated images from S3
    async rejectUser(userid, statusid) {
      await this.statusUpdate(userid, statusid);
      await this.getdata();
      toast.success("Status Updated successfully!");
    },

    // Method to reactivate a user
    async reactivate_User(userid, statusid) {
      await this.statusUpdate(userid, statusid);
      await this.getdata();
      toast.success("Status Updated successfully!");
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
          toast.success("Email sent successfully!");
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
          toast.info("Your session has expired. Please log in again.")
            .then(() => {
              sessionStorage.clear();
              router.push('/');
            });
        } else {
          // toast.info("An error occurred!");
        }
      } else if (error.request) {
        toast.info("A network error occurred. Please check your connection and try again.")
          .then(() => {
            router.push('/');
          });
      } else {
        //  toast.success("An error occurred");
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

.modal-container-2 {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}

.modal-overlay-2 {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
}

.modal-content-2 {
  position: relative;
  background: white;
  padding: 20px;
  border-radius: 10px;
  max-width: 90%;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.modal-body-2 {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  width: 100%;
  max-width: 900px;
}

.user-data {
  flex: 1;
  padding-right: 20px;
  text-align: left;
}

.image-container {
  flex: 1;
}

.modal-image {
  max-width: 100%;
  max-height: 80vh;
  border-radius: 8px;
}

.close {
  position: absolute;
  top: 10px;
  right: 20px;
  font-size: 24px;
  cursor: pointer;
}

input.coupon-input {
  border: solid #4b4e4d 1px;
  padding: 5px;
  border-radius: 17px;
  width: 270px;
}

.table-responsive {
  width: 100%;
  overflow-x: auto;
  /* Allow horizontal scrolling on small screens */
  -webkit-overflow-scrolling: touch;
  /* Smooth scrolling for mobile devices */
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.theme_table th,
.theme_table td {
  text-align: left;
  padding: 8px;
  border: 1px solid #ddd;
  white-space: nowrap;
  /* Prevent text wrapping in table cells */
}

.theme_table th {
  background-color: #f8f9fa;
}

@media (max-width: 768px) {

  .theme_table th,
  .theme_table td {
    font-size: 14px;
    /* Adjust font size for smaller screens */
  }
}

.search-field {
    padding: 3px;
    border: solid 2px;
    border-radius: 8px;
}

.sort-dropdown {
    padding: 6px;
    margin: 6px;
}
</style>
