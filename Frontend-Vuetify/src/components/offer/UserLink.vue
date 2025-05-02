<template>
          <a style="display: flex; align-items: center; cursor: pointer;" @click="showUserDetails(hostId)">
          <span class="user_img" v-if="hostPic != null">
            <img loading="lazy" class="" alt="User Profile Picture" :src="'data:' + 'image/jpeg' + ';base64,' + hostPic">
          </span>
          <span class="user_img" v-if="hostPic == null">
            <img loading="lazy" class="" alt="User Profile Picture" :src="defaultProfileImgSrc">
          </span> {{hostName}} </a>
</template>

<script setup lang="ts">
import router from '@/router';
const props = defineProps({
    hostPic: String,
    hostId: String,
    hostName: String
})
const defaultProfileImgSrc = '/defaultprofile.jpg';
const showUserDetails = (userId:String) => {
      sessionStorage.setItem("UserId", userId);
      router.push("/account");
};
</script>    
