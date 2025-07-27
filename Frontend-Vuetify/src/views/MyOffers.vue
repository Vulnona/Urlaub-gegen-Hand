<template>
  <Navbar />
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2>Meine Angebote</h2>
          </div>
        </div>
      </div>
    </div>
  </div>
  <section class="section_space offers_list">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="sort-new-button ">
            <div class="sort-select"><label>Sortieren nach</label>
              <select class="form-control" v-model="sortBy" @change="onSortChange">
                <option value="newest">Neueste zuerst</option>
                <option value="oldest">Älteste zuerst</option>
                <option value="best">Beliebteste zuerst</option>
              </select>
            </div>
            <div class="SearchBox">
              <i class="ri-search-line"></i>
              <input type="text" v-model="searchTerm" @input="debouncedSearch"
                placeholder="Suche Angebote / Regionen / Skills" class="form-control ">
            </div>
            <div class="add-new-offer">
              <router-link class="btn themeBtn" to="/add-offer"><i class="ri-add-circle-line"></i> Neues Angebot hinzufügen
              </router-link>
              <button class="btn btn-outline-secondary ms-2" @click="deactivateAllOffers"><i class="ri-close-circle-line"></i> Alle Angebote deaktivieren</button>
            </div>
          </div>
          
          <!-- Status Filter -->
          <div class="status-filter mt-3 mb-3">
            <div class="form-check">
              <input class="form-check-input" type="checkbox" v-model="showInactive" id="showInactiveCheck" @change="onShowInactiveChange">
              <label class="form-check-label" for="showInactiveCheck">
                Deaktivierte Angebote einblenden
              </label>
            </div>
          </div>
          <div v-if="offers && offers.length > 0" class="offers_group">
            <div v-if="loading" class="spinner-container text-center">
              <div class="spinner"></div>
            </div>
            <div v-else class="row">
              <div v-for="offer in filteredOffers" :key="offer.id" class="col-md-3 mb-4">
                <div class="all_items card-offer">
                  <div class="d-flex align-items-center mb-1">
                    <span v-if="offer.isExpiringSoon" class="text-danger me-2" style="cursor:pointer;" :title="`Achtung: Der Angebotszeitraum endet am ${offer.toDate}. Das Angebot wird danach automatisch deaktiviert.`">
                      <i class="ri-error-warning-fill"></i>
                    </span>
                    <span class="fw-bold">{{ offer.title }}</span>
                  </div>
                  <OfferCard :offer=offer :showStatus=true />
                  <div v-if="offer.status === 1" class="mt-1 d-flex justify-content-center">
                    <button
                      class="btn btn-outline-success btn-sm"
                      @click="reactivateOffer(offer.id)"
                      :disabled="!offer.canReactivate"
                      :title="!offer.canReactivate ? 'Angebotszeitraum abgelaufen. Reaktivierung nicht möglich.' : 'Angebot reaktivieren'"
                    >
                      <i class="ri-refresh-line"></i> Reaktivieren
                    </button>
                  </div>
                </div>
              </div>
            </div>
             <!-- Pagination Section -->
             <div class="pagination">
              <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1"><i
                  class="ri-arrow-left-s-line"></i>Zurück</button>
              <span>Seite {{ currentPage }} von {{ totalPages }}</span>
              <button class="action-link" @click="changePage(currentPage + 1)"
                :hidden="currentPage === totalPages">Weiter<i class="ri-arrow-right-s-line"></i></button>
            </div>
          </div>
          <div v-else-if="!loading">
            <h2 class="text-center">Keine Angebote gefunden!</h2>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
<script setup lang="ts">
  import OfferCard from '@/components/offer/OfferCard.vue';
  import Swal from 'sweetalert2';
</script>
<script lang="ts">

import Navbar from '@/components/navbar/Navbar.vue';
import toast from '@/components/toaster/toast';
import axiosInstance from "@/interceptor/interceptor"
import Securitybot from '@/services/SecurityBot';


declare global {
  interface Window {
    FontAwesomeConfig?: any;
  }
}

window.FontAwesomeConfig = {
  autoReplaceSvg: false
};

export default {
  components: {
    Navbar,
  },
  data() {
    return {
      loading: true,
      offers: [],
      searchTerm: '',
      searchTimeout: null,
      currentIndex: 0,
      currentPage: 1,
      totalPages: 1,
      pageSize: 8,
      showInactive: false,
      sortBy: 'newest',
    };
  },

  mounted() {
    this.fetchOffers();
    Securitybot();
  },
  computed: {
    filteredOffers() {
      return this.offers;
    }
  },
  methods: {  
    async fetchOffers() {
      try {
        const response = await axiosInstance.get(`offer/get-offer-by-user`, {
          params: {
            searchTerm: this.searchTerm,
            pageSize: this.pageSize,
            pageNumber: this.currentPage,
            sortBy: this.sortBy,
            includeInactive: this.showInactive ? "true" : "false"
          }
        });
        this.offers = response.data.items || response.data.Items || [];
        this.totalPages = Math.ceil((response.data.totalCount || response.data.TotalCount || 0) / this.pageSize);
      } catch (error) {
        console.error('Fehler beim Laden der eigenen Angebote:', error);
        toast.error('Eigene Angebote konnten nicht geladen werden. Bitte versuchen Sie es erneut.');
      } finally {
        this.loading = false;
      }
    },
    changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.currentPage = newPage;
        this.fetchOffers(); // fetch new data for the selected page
      }
    },
    searchOffers() {
      this.loading = true;
      this.fetchOffers();
    },
    debouncedSearch() {
      clearTimeout(this.searchTimeout);
      this.searchTimeout = setTimeout(() => {
        this.currentPage = 1;
        this.searchOffers();
      }, 1000);
    },
    onShowInactiveChange() {
      this.currentPage = 1;
      this.fetchOffers();
    },
    onSortChange() {
      this.currentPage = 1;
      this.fetchOffers();
    },
    getAverageRating(offer) {
      if (!offer.reviews || offer.reviews.length === 0) return 0;
      const sum = offer.reviews.reduce((acc, r) => acc + Number(r.stars || r.rating || 0), 0);
      return sum / offer.reviews.length;
    },
    async deactivateAllOffers() {
      const result = await Swal.fire({
        title: 'Alle Angebote deaktivieren?',
        text: 'Möchtest du wirklich alle aktiven Angebote deaktivieren? Diese Aktion kann nicht rückgängig gemacht werden.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Ja, deaktivieren',
        cancelButtonText: 'Abbrechen',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
      });
      if (!result.isConfirmed) return;
      try {
        const response = await axiosInstance.post('/offer/deactivate-all-by-user');
        toast.success(`${response.data.deactivated} Angebote wurden deaktiviert.`);
        this.fetchOffers();
      } catch (error) {
        toast.error('Fehler beim Deaktivieren der Angebote.');
      }
    },
    async reactivateOffer(offerId) {
      try {
        const response = await axiosInstance.put(`/offer/reactivate/${offerId}`);
        toast.success('Angebot wurde reaktiviert.');
        this.fetchOffers();
      } catch (error) {
        const msg = ((error && typeof error === 'object' && 'response' in error) ? (error as any).response?.data : null) || 'Fehler beim Reaktivieren des Angebots.';
        toast.error(msg);
      }
    },
  }
};
</script>

<style>
.offers_list_page {
  margin-top: 30px;
}
</style>
