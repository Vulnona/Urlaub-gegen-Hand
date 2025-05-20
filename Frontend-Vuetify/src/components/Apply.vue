<template>
  <div v-if="offer != null && isActiveMember && offer.hostId != logId && offer.status == '0'">
    <button v-if="offer.appliedStatus === 'CanApply'" @click="sendRequest(offer.id)"
            class="btn btn-success OfferButtons">Apply</button>
    <button v-else-if="offer.appliedStatus === 'Applied'" class="btn btn-info OfferButtons"
            disabled>Applied</button>
    <button v-else-if="offer.appliedStatus === 'Rejected'" class="btn btn-danger OfferButtons"
            disabled>Rejected</button>
    <button v-else-if="offer.appliedStatus === 'Approved'" @click="showUserDetails(offer.hostId)"
            class="btn btn-primary OfferButtons">View Host Details</button>
  </div>
</template>

<script setup lang="ts">
const props = defineProps({
    offer: Object,
    isActiveMember: Boolean,
    logId: String
})

</script>
    
<script lang="ts">  
    import axiosInstance from '@/interceptor/interceptor';
  import {isActiveMembership,GetUserRole} from '@/services/GetUserPrivileges'; 
  import getLoggedUserId from '@/services/LoggedInUserId';
  import Swal from "sweetalert2";
  import toast from '@/components/toaster/toast';
import router from "@/router";

export default {
  methods: {
      // Method to send request for offer application
          async sendRequest(offerId) {
      const result = await Swal.fire({
        title: 'Bist du sicher?',
        text: 'Möchtest du diese Anfrage senden?',
        icon: '',
        showCancelButton: true,
        confirmButtonText: 'Apply',
        customClass: {
          popup: 'custom-apply-modal dialog_box',
          confirmButton: 'themeBtn',
          cancelButton: 'Cancel_btn',
        }
      });
      if (result.isConfirmed) {
        try {
          await axiosInstance.post(`${process.env.baseURL}offer/apply-offer?offerId=${offerId}`);
    toast.success("Deine Anfrage wurde gesendet.!");
    location.reload();
        } catch (error) {
            toast.info("Leider konnte deine Anfrage nicht versendet werden!");            
        }
      }
     },
      showUserDetails(userId) {
      sessionStorage.setItem("UserId", userId);
      router.push("/account")
    },
  }
};
</script>
