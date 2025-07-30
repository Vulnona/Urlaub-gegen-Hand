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
              <h2>Alle Bewertungen (Admin)</h2>
            </div>
          </div>
        </div>
      </div>
    </div>
    <section class="section_space reviews_list">
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <div class="sort-new-button justify-content-center">
              <div class="SearchBox">
                <i class="ri-search-line"></i>
                <input type="text" v-model="searchTerm" @input="debouncedSearch" placeholder="Suche Nutzer, Angebot, Kommentar..." class="form-control">
              </div>
            </div>
            <div v-if="loading" class="spinner-container text-center">
              <div class="spinner"></div>
            </div>
            <div v-else>
              <div v-if="reviews.length">
                <table class="table table-striped table-bordered reviews-table">
                  <thead>
                    <tr>
                      <th>Bewertender Nutzer</th>
                      <th>Bewerteter Nutzer</th>
                      <th>Angebot</th>
                      <th>Bewertung</th>
                      <th>Kommentar</th>
                      <th>Datum</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="review in filteredReviews" :key="review.id">
                      <td>{{ review.reviewer?.firstName }} {{ review.reviewer?.lastName }}</td>
                      <td>{{ review.reviewed?.firstName }} {{ review.reviewed?.lastName }}</td>
                      <td>
                        <router-link :to="{ name: 'OfferDetail', params: { id: review.offerId } }">
                          {{ review.offer?.title || 'Angebot #' + review.offerId }}
                        </router-link>
                      </td>
                      <td>
                        <span v-for="n in 5" :key="n">
                          <i v-if="n <= review.ratingValue" class="ri-star-fill text-primary-yellow"></i>
                          <i v-else class="ri-star-line text-primary-yellow"></i>
                        </span>
                      </td>
                      <td>{{ review.reviewComment }}</td>
                      <td>{{ formatDate(review.createdAt) }}</td>
                    </tr>
                  </tbody>
                </table>
                <div class="pagination">
                  <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1"><i class="ri-arrow-left-s-line"></i>Vorherige</button>
                  <span>Seite {{ currentPage }} von {{ totalPages }}</span>
                  <button class="action-link" @click="changePage(currentPage + 1)" :hidden="currentPage === totalPages">Nächste<i class="ri-arrow-right-s-line"></i></button>
                </div>
              </div>
              <div v-else>
                <h2 class="text-center">Keine Bewertungen gefunden!</h2>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from "@/interceptor/interceptor"
import Securitybot from '@/services/SecurityBot';
import {GetUserRole} from "@/services/GetUserPrivileges";
import Errorpage from "../Errorpage.vue";
import toast from "@/components/toaster/toast";

export default {
  components: {
    Navbar,
    Errorpage
  },
  data() {
    return {
      userRole: GetUserRole(),
      loading: true,
      reviews: [],
      searchTerm: '',
      searchTimeout: null,
      currentPage: 1,
      totalPages: 1,
      pageSize: 50,
    };
  },
  mounted() {
    this.fetchReviews();
    Securitybot();
  },
  methods: {
    async fetchReviews() {
      try {
        const response = await axiosInstance.get(`review/get-all-reviews-admin`, {
          params: {
            pageSize: this.pageSize,
            pageNumber: this.currentPage
          }
        });
        if (response.data && response.data.items) {
          this.reviews = response.data.items;
          this.totalPages = Math.ceil((response.data.totalCount || 0) / this.pageSize);
        } else {
          this.reviews = [];
          this.totalPages = 1;
        }
      } catch (error) {
        console.error('Fehler beim Abrufen der Reviews:', error);
        this.reviews = [];
        this.totalPages = 1;
        if (error.response) {
          if (error.response.status === 401) {
            toast.error("Session abgelaufen. Bitte melden Sie sich erneut an.");
          } else if (error.response.status === 404) {
            toast.error("Reviews-Endpoint nicht gefunden.");
          } else {
            toast.error(`Serverfehler: ${error.response.status}`);
          }
        } else if (error.request) {
          toast.error("Netzwerkfehler. Bitte überprüfen Sie Ihre Verbindung.");
        } else {
          toast.error("Unbekannter Fehler beim Laden der Reviews.");
        }
      } finally {
        this.loading = false;
      }
    },
    changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.currentPage = newPage;
        this.fetchReviews();
      }
    },
    debouncedSearch() {
      clearTimeout(this.searchTimeout);
      this.searchTimeout = setTimeout(() => {
        this.currentPage = 1;
        // Optional: Filter clientseitig, Backend-Search kann später ergänzt werden
      }, 500);
    },
    formatDate(dateStr) {
      const date = new Date(dateStr);
      return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
    }
  },
  computed: {
    filteredReviews() {
      if (!this.searchTerm) return this.reviews;
      const term = this.searchTerm.toLowerCase();
      return this.reviews.filter(r =>
        (r.reviewer?.firstName + ' ' + r.reviewer?.lastName).toLowerCase().includes(term) ||
        (r.reviewed?.firstName + ' ' + r.reviewed?.lastName).toLowerCase().includes(term) ||
        (r.offer?.title || '').toLowerCase().includes(term) ||
        (r.reviewComment || '').toLowerCase().includes(term)
      );
    }
  }
};
</script>

<style scoped>
.reviews-table {
  width: 100%;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.07);
  margin-top: 30px;
}
.reviews-table th, .reviews-table td {
  padding: 10px 8px;
  text-align: left;
  vertical-align: middle;
}
.reviews-table th {
  background: #f8f8f8;
  font-weight: 600;
}
.text-primary-yellow {
  color: #ffc107;
}
</style>
