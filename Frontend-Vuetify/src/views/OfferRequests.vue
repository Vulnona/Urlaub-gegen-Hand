<template>
<Navbar />
<Banner n="Anfragen" />
<ListOfOffers :isHost="true" />
<ListOfOffers :isHost="false" />
</template>

<script setup lang="ts">
import Banner from '@/components/Banner.vue';
import Navbar from '@/components/navbar/Navbar.vue';
import ListOfOffers from '@/components/offer/ListOfOffers.vue';
import Securitybot from '@/services/SecurityBot';
</script>

