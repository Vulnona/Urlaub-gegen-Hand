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
import Navbar from '@/components/navbar/Navbar.vue';
import toast from '@/components/toaster/toast';
import axiosInstance from "@/interceptor/interceptor"
import Securitybot from '@/services/SecurityBot';
import { ref, onMounted, computed } from 'vue';

declare global {
  interface Window {
    FontAwesomeConfig?: any;
  }
}

window.FontAwesomeConfig = {
  autoReplaceSvg: false
};

const loading = ref(true);
const offers = ref([]);
const searchTerm = ref('');
const searchTimeout = ref(null);
const currentIndex = ref(0);
const currentPage = ref(1);
const totalPages = ref(1);
const pageSize = ref(8);
const showInactive = ref(false);
const sortBy = ref('newest');

const filteredOffers = computed(() => {
  return offers.value;
});

const fetchOffers = async () => {
  try {
    const response = await axiosInstance.get(`offer/get-offer-by-user`, {
      params: {
        searchTerm: searchTerm.value,
        pageSize: pageSize.value,
        pageNumber: currentPage.value,
        sortBy: sortBy.value,
        includeInactive: showInactive.value ? "true" : "false"
      }
    });
    offers.value = response.data.items || response.data.Items || [];
    totalPages.value = Math.ceil((response.data.totalCount || response.data.TotalCount || 0) / pageSize.value);
  } catch (error) {
    console.error('Fehler beim Laden der eigenen Angebote:', error);
    toast.error('Eigene Angebote konnten nicht geladen werden. Bitte versuchen Sie es erneut.');
  } finally {
    loading.value = false;
  }
};

const changePage = (newPage: number) => {
  if (newPage >= 1 && newPage <= totalPages.value) {
    currentPage.value = newPage;
    fetchOffers();
  }
};

const searchOffers = () => {
  loading.value = true;
  currentPage.value = 1;
  fetchOffers();
};

const debouncedSearch = () => {
  if (searchTimeout.value) {
    clearTimeout(searchTimeout.value);
  }
  searchTimeout.value = setTimeout(searchOffers, 500);
};

const onSortChange = () => {
  loading.value = true;
  currentPage.value = 1;
  fetchOffers();
};

const onShowInactiveChange = () => {
  loading.value = true;
  currentPage.value = 1;
  fetchOffers();
};

const reactivateOffer = async (offerId: number) => {
  try {
    await axiosInstance.put(`offer/reactivate/${offerId}`);
    toast.success('Angebot erfolgreich reaktiviert!');
    fetchOffers();
  } catch (error) {
    console.error('Fehler beim Reaktivieren des Angebots:', error);
    toast.error('Angebot konnte nicht reaktiviert werden.');
  }
};

const deactivateAllOffers = async () => {
  const result = await Swal.fire({
    title: 'Alle Angebote deaktivieren?',
    text: 'Möchten Sie wirklich alle Ihre aktiven Angebote deaktivieren?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Ja, alle deaktivieren',
    cancelButtonText: 'Abbrechen'
  });

  if (result.isConfirmed) {
    try {
      await axiosInstance.post('offer/deactivate-all-by-user');
      toast.success('Alle Angebote wurden deaktiviert!');
      fetchOffers();
    } catch (error) {
      console.error('Fehler beim Deaktivieren aller Angebote:', error);
      toast.error('Angebote konnten nicht deaktiviert werden.');
    }
  }
};

onMounted(() => {
  fetchOffers();
  Securitybot();
});
</script>

<style>
.offers_list_page {
  margin-top: 30px;
}
</style>
