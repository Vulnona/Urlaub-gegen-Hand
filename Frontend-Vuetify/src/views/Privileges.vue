<template>
  <PublicNav />
  <Navbar />
  <div class="container">          
    <h3>Du benötigst eine Mitgliedschaft um dieses Feature zu nutzen oder dein Account ist nicht verifiziert.</h3>
    <div v-if="this.verificationStatus==='ok'">
      <p style="color: green">Dein Account ist verifiziert.</p>
    </div>
    <div v-else>
      <p style="color: red"> Dein Account ist nicht verifiziert.</p>
      <p>Falls du deinen Personalausweis noch nicht zur Verifikation hochgeladen hast, kannst du das <a href="/upload-id">hier</a> tun.</p>
    </div>
    <div v-if="this.membership==='Active'">
      <p style="color: green">Du hast eine aktive Mitgliedschaft.</p>
    </div>
    <div v-else>
      <p style="color: red"> Du hast keine aktive Mitgliedschaft.</p>
      <p>Falls dein Account verifiziert ist und du einen Couponcode hast kannst du diesen im <a href="/profile">Profil</a> eingeben.</p>
    </div>    
  </div>
</template>

<script>
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from '@/interceptor/interceptor';
import PublicNav from '@/components/navbar/PublicNav.vue';
import getLoggedUserId from '@/services/LoggedInUserId';
import Swal from "sweetalert2";
import router from "@/router";
import toast from '@/components/toaster/toast';
import debounce from 'lodash/debounce';
import {GetUserPrivileges} from '@/services/GetUserPrivileges';

export default {
  components: {
    Navbar,
    PublicNav
  },

    data (){
        return{
            membership : '',
            verificationStatus : ''
        };
    },    
    beforeMount(){
        const privileges = GetUserPrivileges();
        this.membership=privileges.membershipStatus;
        this.verificationStatus=privileges.verification;
    }
};
</script>
