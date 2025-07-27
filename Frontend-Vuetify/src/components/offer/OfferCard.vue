<template>
<div @click="redirectToOfferDetail(offer.id)">
  <div class="all_items card-offer">
    <div class="item_img">
      <img loading="lazy" :src="'data:' + offer.imageMimeType + ';base64,' + offer.imageData"
           class="card-img-top" alt="Offer Image">
      <div class="rating"
           v-if="isActiveMember && offer.hostId != logId && offer.appliedStatus == 'Approved'"
           @click="showAddRatingModal(offer.id, offer.hostId, index)"><i class="ri-star-line"></i></div>
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
    <Apply :offer=offer :isActiveMember=isActiveMember :logId=logId />                    
  </div>
</div>
</template>
<script setup lang="ts">
import Apply from '@/components/Apply.vue';
import router from '@/router';
  const props= defineProps({offer:Object,logId:String,isActiveMember:Boolean});
  const redirectToOfferDetail = (offerId:String) => {
      router.push({
        name: 'OfferDetail',
        params: {id: props.offer.id}
      });
    };
</script>
