<template>
<Navbar />
<Banner n="Anfragen" />
<RequestsToUser />
<RequestsFromUser />
</template>

<script setup lang="ts">
import Banner from '@/components/Banner.vue';
import Navbar from '@/components/navbar/Navbar.vue';
import RequestsToUser from '@/components/offer/RequestsToUser.vue';
import RequestsFromUser from '@/components/offer/RequestsFromUser.vue';
import Securitybot from '@/services/SecurityBot';
</script>

