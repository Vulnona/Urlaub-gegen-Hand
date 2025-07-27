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
              <h2>Coupons</h2>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="section_space offers_request_layout admin-panel-grid">
      <div class="offers_request_content">
        <div class="card">
          <div class="card-header">
            <h1 class="main-title">Coupons List</h1>
          </div>
          <div class="card-body">
            <!-- Add a responsive wrapper -->
            <div class="table-responsive">
              <table class="table theme_table">
                <thead>
                  <tr>
                    <th class="text-center">Coupon Code</th>
                    <th>Name</th>
                    <th>Created By</th>
                    <th>Created Date</th>
                    <th>Duration</th>
                    <th class="text-center">Email Status</th>
                    <th class="text-center">Redeemed by</th>
                    <th class="text-center">Status</th>
                    <!-- <th class="text-center">Actions</th> -->
                  </tr>
                </thead>
                  <tbody>
                    <tr v-for="(coupon, index) in coupons" :key="index">
                      <td class="text-center">
                        <span class="codeReveal" v-if="revealedCodes.includes(index)">
                          <b>{{ coupon.code }}</b>
                          <button @click="toggleReveal(index)" class="bg_ltred"><i class="ri-close-line"></i></button>
                        </span>
                        <span v-else>
                          <button @click="toggleReveal(index, coupon.code)" class="bg_ltgreen">Klicken zum Anzeigen</button>
                        </span>
                      </td>

                      <td>{{ coupon.name }}</td>
                      <td>{{ coupon.createdBy }}</td>
                      <td>{{ formatDate(coupon.createdDate) }}</td>
                     <td v-if="coupon.duration">
                     {{ Math.round(coupon.duration / 365) }} Jahr<span v-if="Math.round(coupon.duration / 365) !== 1">e</span></td>
                      <td class="text-center">
                        <span v-if="coupon.isEmailSent" class="emailState badge badge-info">
                          Sent {{ formatEmailDate(coupon.emailSentDate) }}
                        </span>
                        <span v-else class="emailState badge badge-warning">Not Sent</span>
                      </td>
                      <td class="text-center">{{ coupon.redeemedBy }}<span v-if="coupon.redeemedBy == ''">N/A</span></td>
                      <td class="text-center">
                        <span v-if="coupon.isRedeemed" class="newState badge badge-primary">Redeemed</span>
                        <span v-if="!coupon.isRedeemed" class="newState badge badge-success">Available</span>
                      </td>
                      <!-- <td v-if="coupon.status=='Redeemed'" class="verState badge badge-success text-center">{{ coupon.status }}</td> -->
                      <!-- <td class="text-center"></td> -->
                    </tr>
                  </tbody>
                
              </table>
            </div>
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
import dayjs from "dayjs";

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
      coupons: [],
      userRole: GetUserRole(),
      userdata: null,
      revealedCodes: []
    };
  },
  mounted() {
    this.getdata();
    Securitybot();
  },
  methods: {
    changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.currentPage = newPage;
        this.getdata();
      }
    },

    formatDate(date) {
      return dayjs(date).format("MMMM D, YYYY h:mm A"); // Example: January 21, 2025 1:23 PM
    },
    formatEmailDate(date) {
      if (!date) return '';
      return dayjs(date).format("MMM D, HH:mm"); // Example: Jan 21, 13:45
    },
    copyToClipboard(text) {
  if (navigator.clipboard && navigator.clipboard.writeText) {
    navigator.clipboard.writeText(text)
      .then(() => {
        toast.success("Coupon code copied to clipboard!");
      })
      .catch(err => {
        toast.error("Failed to copy coupon code.");
        console.error("Error copying text: ", err);
      });
  } else {
    // Fallback for insecure contexts
    const textarea = document.createElement("textarea");
    textarea.value = text;
    textarea.style.position = "fixed"; // Prevent scrolling to bottom
    document.body.appendChild(textarea);
    textarea.select();
    try {
      document.execCommand("copy");
      toast.success("Coupon code copied to clipboard!");
    } catch (err) {
      toast.error("Failed to copy coupon code.");
      console.error("Fallback copy failed: ", err);
    }
    document.body.removeChild(textarea);
  }
},

    toggleReveal(index, code) {
      const codeIndex = this.revealedCodes.indexOf(index);
      if (codeIndex === -1) {
        this.revealedCodes.push(index); // Reveal the code
        this.copyToClipboard(code);
      } else {
        this.revealedCodes.splice(codeIndex, 1); // Hide the code
      }
    },
    // Method to fetch data from the server
    async getdata() {
      try {
        const res = await axiosInstance.get(`coupon/get-all-coupon`, {
          params: {
            pageSize: this.pageSize,
            pageNumber: this.currentPage
          }
        }
        );
        this.coupons = res.data.items;
        this.totalPages = Math.ceil(res.data.totalCount / this.pageSize);
      } catch (error) {
        console.error('Fehler beim Abrufen der Coupons:', error);
        this.handleAxiosError(error);
      }
    },
    // Method to navigate to the home page
    goHome() {
      router.push('/home');
    },

    // Method to handle Axios errors
    handleAxiosError(error) {
      if (error.response) {
        if (error.response.status === 401) {
          toast.info("Die Session ist abgelaufen. Erneuter Login notwendig.")
            .then(() => {
              sessionStorage.clear();
              router.push('/');
            });
        } else {
            console.error('Admin/Coupons error:', error);
        }
      } else if (error.request) {
        toast.info("Ein Netzwerkfehler ist aufgetreten. Bitte überprüfen Sie Ihre Verbindung und versuchen Sie es erneut.")
          .then(() => {
            router.push('/');
          });
      } else {
        //  toast.success("An error occurred");
          console.error('Admin/Coupons error:', error);
      }
    }
  }
};
</script>
<style scoped>
.codeReveal {
  display: flex;
  justify-content: space-evenly;
}

.bg_ltgreen {
  padding: 3px;
  padding-left: 5px;
  padding-right: 5px;
  color: #2c8326;
  border-radius: 14px;
  border: solid .2px rgb(25, 134, 25);
}

.bg_ltred {
  padding-right: 3px;
  padding-left: 3px;
  border-radius: 8px;
}

.btn {
  padding: 4px 12px;
  font-size: 14px;
  border: 1px solid;
  border-radius: 5px;

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
</style>
