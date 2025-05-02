<template>
  <Navbar />
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2>Purchases</h2>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="section_space offers_request_layout admin-panel-grid">
    <div class="offers_request_content">
      <div class="card">
        <div class="card-header">
          <h1 class="main-title">Purchase History</h1>
        </div>
        <div class="card-body">
          <!-- Add a wrapper with overflow-x: auto -->
          <div v-if="coupons.length" class="table-responsive">
            <table class="table theme_table">
              <thead>
                <tr>
                  <th class="text-center" style="width: 18%;">Date</th>
                  <th class="text-center" style="width: 18%;">Item Name</th>
                  <th class="text-center">Amount</th>
                  <th class="text-center" style="width: 30%;">Coupon Code</th>
                  <th class="text-center" style="width: 15%;">Purchase Status</th>
                  <th class="text-center" style="width: 15%;">Coupon Status</th>
                  <!-- <th class="text-center">Action</th> -->
                </tr>
              </thead>
              <tbody>
                <tr v-for="(coupon, index) in coupons" :key="index">
                  <td class="text-center">{{ formatDate(coupon.transactionDate) }}</td>
                  <td class="text-center">{{ coupon.shopItemName }}</td>
                  <td class="text-center">{{ coupon.amount.amount }} {{ coupon.amount.currency }}</td>
                  <td  class="copyCoupon text-center" >
                    <span v-if="coupon.couponCode"><b>{{ coupon.couponCode }}</b></span>
                    <span v-if="!coupon.couponCode">No Coupon</span>
                    <button v-if="coupon.couponCode" class="bg_ltgreen"
                      @click="copyToClipboard(coupon.couponCode)">
                      <i class="ri-file-copy-line"></i>
                    </button>
                  </td>

                  <td class="text-center">
                    <span v-if="coupon.status == 0" class="newState badge badge-primary">Pending</span>
                    <span v-if="coupon.status == 1" class="newState badge badge-success">Complete</span>
                    <span v-if="coupon.status == 2" class="newState badge badge-danger">Failed</span>
                  </td>
                  <td class="text-center">
                    <span v-if="coupon.couponStatus == 'Redeemed'" class="newState badge badge-primary">Redeemed</span>
                    <span v-if="coupon.couponStatus == 'Available'" class="newState badge badge-success">Available</span>
                    <span v-if="coupon.couponStatus == 'Failed'" class="newState badge badge-danger">Failed</span>
                  </td>
                  <!-- <td class="text-center"></td> -->
                </tr>
              </tbody>
            </table>
          </div>
          <div v-else>
            <h2 class="text-center">No Transactions</h2>
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
import toast from "@/components/toaster/toast";
import dayjs from "dayjs";
export default {
  components: {
    Navbar,
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
    };
  },
  mounted() {
    this.getdata();
    Securitybot();
    const urlParams = new URLSearchParams(window.location.search);
  if (urlParams.get('success') === 'true') {
    toast.success("Payment completed successfully!");
    setTimeout(() => {
      this.getdata();
    }, 2000);
  } else if (urlParams.get('canceled') === 'true') {
    toast.info("Payment was canceled.");
  }
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
    // Method to fetch data from the server
    async getdata() {
      try {
        const res = await axiosInstance.get(`transaction/get-user-transactions`, {
          params: {
            pageSize: this.pageSize,
            pageNumber: this.currentPage
          }
        }
        );
        this.coupons = res.data.items;
        this.totalPages = Math.ceil(res.data.totalCount / this.pageSize);
      } catch (error) {
        this.handleAxiosError(error);
      }
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

    // Method to navigate to the home page
    goHome() {
      router.push('/home');
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
.bg_ltgreen {
  background-color:#efefef;
  padding: 2px;
  padding-right: 4px;
  padding-left: 4px;
  color: #272525;
  border-radius: 7px;
}

.copyCoupon{
    display: flex;
    justify-content: space-evenly;
}

h2 {
  margin-top: 0;
  margin-bottom: 20px;
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
