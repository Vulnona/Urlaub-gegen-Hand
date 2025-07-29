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
              <i class="ri-search-line text-primary-blue"></i>
              <input type="text" v-model="searchTerm" @input="debouncedSearch"
                placeholder="Suche Angebote / Regionen / Skills" class="form-control">
            </div>
            <div class="add-new-offer">
              <router-link class="btn-primary-ugh" to="/add-offer"><i class="ri-add-circle-line"></i> Neues Angebot hinzufügen
              </router-link>
              <button class="btn-deactivate-all ms-2" @click="deactivateAllOffers"><i class="ri-close-circle-line"></i> Alle Angebote deaktivieren</button>
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
                    <span class="fw-bold">{{ offer.title }}</span>
                  </div>
                  <OfferCard :offer="offer" :showStatus="true">
                    <template #actions>
                      <div class="d-flex flex-wrap gap-2 justify-content-center">
                        <button class="btn-primary-ugh btn-sm" @click.stop="shareOnFacebook(offer)">
                          <i class="ri-facebook-fill"></i> Auf Facebook teilen
                        </button>
                      </div>
                      <div class="d-flex flex-wrap gap-2 justify-content-center mt-1">
                        <button v-if="offer.status === 1"
                          class="btn btn-outline-success btn-sm"
                          @click.stop="reactivateOffer(offer.id)"
                          :disabled="!offer.canReactivate"
                          :title="!offer.canReactivate ? 'Angebotszeitraum abgelaufen. Reaktivierung nicht möglich.' : 'Angebot reaktivieren'"
                        >
                          <i class="ri-refresh-line"></i> Reaktivieren
                        </button>
                        <!-- Hier können weitere Buttons ergänzt werden -->
                      </div>
                    </template>
                  </OfferCard>
                </div>
              </div>
            </div>
             <!-- Pagination Section -->
             <div class="pagination">
              <button class="btn-arrow-ugh me-2" @click="changePage(currentPage - 1)" :disabled="currentPage === 1"><i class="ri-arrow-left-s-line"></i></button>
              <span>Seite {{ Math.max(currentPage, 1) }} von {{ Math.max(totalPages, 1) }}</span>
              <button class="btn-arrow-ugh ms-2" @click="changePage(currentPage + 1)" :disabled="currentPage === totalPages"><i class="ri-arrow-right-s-line"></i></button>
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

const shareOnFacebook = (offer: any) => {
  const url = encodeURIComponent(`${window.location.origin}/offer/${offer.id}`);
  const fbUrl = `https://www.facebook.com/sharer/sharer.php?u=${url}`;
  window.open(fbUrl, '_blank', 'width=600,height=400');
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

/* Deaktivieren-Button Styling */
.btn-deactivate-all {
  background-color: #9ca3af !important; /* Mittel/hellgrau */
  color: #000000 !important; /* Schwarze Schrift */
  border: none;
  padding: 12px 25px;
  border-radius: 8px;
  font-weight: bold;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  transition: all 0.3s ease;
}

.btn-deactivate-all:hover {
  background-color: #6b7280 !important; /* Dunkleres Grau beim Hover */
  color: #000000 !important; /* Schwarze Schrift bleibt */
  transform: translateY(-2px);
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
}

.btn-deactivate-all:focus {
  background-color: #9ca3af !important;
  color: #000000 !important;
  outline: none;
  box-shadow: 0 0 0 3px rgba(156, 163, 175, 0.3);
}
</style>
