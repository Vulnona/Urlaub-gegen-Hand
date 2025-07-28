<template>
<div @click="redirectToOfferDetail(offer.id)">
  <div class="all_items card-offer" :class="{ 'inactive-offer': !isActive }">
    <!-- Status Badge -->
    <div class="status-badge" v-if="showStatus">
      <span v-if="offer.status === 0" class="badge badge-success">Aktiv</span>
      <span v-else-if="offer.status === 1" class="badge badge-warning">Geschlossen</span>
      <span v-else-if="offer.status === 2" class="badge badge-danger">Versteckt</span>
    </div>
    <span v-if="offer.isExpiringSoon" class="expiring-soon-badge" :title="`Achtung: Der Angebotszeitraum endet am ${offer.toDate}. Das Angebot wird danach automatisch deaktiviert.`">
      <i class="ri-error-warning-fill"></i>
    </span>
    
    <div class="item_img">
      <img loading="lazy" :src="offer.images && offer.images.length > 0 ? offer.images[0].src : '/defaultprofile.jpg'"
           class="card-img-top" alt="Offer Image">
      <div v-if="offer.images && offer.images.length > 1" class="gallery-indicator">
        +{{ offer.images.length }}
      </div>
      <div class="rating"
           v-if="isActiveMember && offer.hostId != logId && offer.appliedStatus == 'Approved'">
           <i class="ri-star-line"></i>
      </div>
    </div>
    <div class="item_text">
      
      <h3 class="card-title">{{ offer.title }}</h3>
      <div class="item_description">
        <p class="card-text">{{ offer.fromDate }} - {{ offer.toDate }}</p>
        <p class="card-text"><strong>Fähigkeiten:</strong> {{ offer.skills }}</p>
        <p class="card-text"><strong>Unterbringung:</strong> {{ offer.accomodation || 'Nicht angegeben' }}</p>
        <p class="card-text"><strong>Geeignet für:</strong> {{ offer.accomodationsuitable || 'Nicht angegeben' }}</p>
        <p class="card-text"><strong>Region/Ort:</strong> {{ offer.location || 'Nicht angegeben' }}</p>
      </div>
    </div>
    <Apply :offer=offer :isActiveMember=isActiveMember :logId=logId :userRole=userRole />                    
  </div>
</div>
</template>
<script setup lang="ts">
import Apply from '@/components/Apply.vue';
import router from '@/router';
import { computed } from 'vue';

const props = defineProps({
  offer: Object,
  logId: String,
  isActiveMember: Boolean,
  userRole: String,
  showStatus: {
    type: Boolean,
    default: false
  },
  index: {
    type: Number,
    default: 0
  }
});

const isActive = computed(() => props.offer?.status === 0);

const redirectToOfferDetail = (offerId: String) => {
  router.push({
    name: 'OfferDetail',
    params: { id: props.offer.id }
  });
};
</script>

<style scoped>
.inactive-offer {
  opacity: 0.6;
  filter: grayscale(50%);
}

.status-badge {
  position: absolute;
  top: 10px;
  right: 10px;
  z-index: 10;
}

.badge {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: bold;
  color: white;
}

.badge-success {
  background-color: #28a745;
}

.badge-warning {
  background-color: #ffc107;
  color: #212529;
}

.badge-danger {
  background-color: #dc3545;
}

.all_items {
  position: relative;
}

.expiring-soon-badge {
  position: absolute;
  top: 10px;
  right: 48px;
  z-index: 11;
  color: #dc3545;
  font-size: 1.5rem;
  cursor: pointer;
}

.gallery-indicator {
  position: absolute;
  bottom: 10px;
  right: 10px;
  background: rgba(0,0,0,0.7);
  color: #fff;
  border-radius: 12px;
  padding: 2px 10px;
  font-size: 1rem;
  font-weight: bold;
  z-index: 12;
}
</style>
