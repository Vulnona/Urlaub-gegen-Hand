<template>
<div v-if="!loading">
<EditOffer banner="Modifiziere Angebot" :offer="offer" />
</div>
</template>

<script setup lang="ts">
import {onMounted, ref} from "vue";
import EditOffer from '@/components/offer/EditOffer.vue';
import axiosInstance from '@/interceptor/interceptor';
import {useRoute} from 'vue-router';
const {params} = useRoute();
const offer = ref(null);
const loading = ref(true);
const fetchOfferDetail = async () => {
    try {
        const response = await axiosInstance.get(`${process.env.baseURL}offer/get-offer-by-id/${params.id}`);
        offer.value = response.data;
    } catch (error) {
        console.error('Error fetching offer detail:', error);
    } finally {
        loading.value = false;
    }
};
onMounted(fetchOfferDetail);
</script>
