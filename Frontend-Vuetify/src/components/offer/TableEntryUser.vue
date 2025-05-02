<template>
<td><a class="view_user" style="cursor: pointer;" @click="showUserDetails(item.user.user_Id)">
    <span class="user_img" v-if="item.user.profilePicture != null">
      <img loading="lazy" class="" alt="User Profile Picture" :src="'data:' + 'image/jpeg' + ';base64,' + item.user.profilePicture">
    </span>
    <span class="user_img" v-if="item.user.profilePicture == null">
      <img loading="lazy" class="" alt="User Profile Picture" :src="defaultProfileImgSrc">
    </span>
    {{item.user.firstName}} {{item.user.lastName}} </a> </td>
</template>

<script setup lang="ts">
  import {ref, onMounted, computed, toRefs} from "vue";
  import router from "@/router";
const props = defineProps({item: Object})
const defaultProfileImgSrc = '/defaultprofile.jpg'
const showUserDetails = (userId:String) => {
      sessionStorage.setItem("UserId", props.item.user.user_Id);
      router.push("/account");
}
</script>
