<template>
   <PublicNav />
  <div class="login-page">
    <div class="loginmain-bg">
      <div class="loginmain-form">
        <div class="login-left">
          <div class="login-left-content">            
            <div class="login-main-text">
              <h2><span>Entdecke neue Möglichkeiten des Reisens</span></h2>
              <p>Finde dein perfektes "Urlaub gegen Hand" Abenteuer auf unserer innovativen Plattform. Bei uns profitierst du von der direkten Verbindung zur größten deutschsprachigen Community mit über 200.000 aktiven Mitgliedern auf Facebook. Jedes Angebot und Gesuch wird automatisch in beiden Netzwerken geteilt - das maximiert deine Chancen auf den idealen Match.</p>

<p>Egal ob du eine Unterkunft suchst oder anbietest, bei uns findest du vielfältige Möglichkeiten: von der Mithilfe auf Bauernhöfen über Haustiersitting bis hin zu handwerklichen Projekten. Registriere dich kostenlos und werde Teil einer wachsenden Community von Gleichgesinnten, die Reisen neu definiert. </p>
            </div>
          </div>
        </div>
        <div class="login-right">
          <div class="login-right-content-heading form-act">

            <div class="login-form-section" id="login-content">


              <div class="auth-card">
                  <form class="form-border" @submit.prevent="login">
                  <div class="custom-form">
                    <label for="fname">E-Mail</label>
                    <input type="text" placeholder="Benutzernamen eingeben" id="username" v-model="email">
                  </div>
                  <div>
                    <div class="custom-form">
                      <div class="d-flex justify-content-between">
                        <label>Passwort</label>
                      </div>
                      <div class="password-container" style="position: relative;">
                        <input :type="showPassword ? 'text' : 'password'" placeholder="Passwort eingeben" id="password"
                          v-model="password">
                        <i @click="togglePasswordVisibility" :class="showPassword ? 'ri-eye-off-fill' : 'ri-eye-fill'"
                          style="position: absolute; right: 10px; top: 10px; cursor: pointer;">
                        </i>
                      </div>
                   </div>

                    <div class="login-buttons">
                      <button type="submit" class="btn"> Anmelden</button>
                    </div>
                  </div>
                  <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
                  <div class="back-login flexBox_btn">
                    <a href="/home"><i class="ri-arrow-left-double-fill"></i> Back to home</a>
                    <a href="/reset-password" previewlistener="true"><i class="ri-shield-check-fill"></i>Passwort zurücksetzen</a>
                    <a href="/verify-email" previewlistener="true"><i class="ri-shield-check-fill"></i> E-Mail bestätigen</a>
                  </div>

                </form>


              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import PublicNav from '@/components/navbar/PublicNav.vue';
    
import router from '@/router';
import AES from 'crypto-js/aes';
import {GetUserRole} from "@/services/GetUserPrivileges";
import axiosInstance from '@/interceptor/interceptor';
import toast from '../toaster/toast';
export default {
  components: {
  PublicNav
  },
  data() {
    return {
      email: '',
      password: '',
      errorMessage: '',
      showPassword: false,
    };
  },

  methods: {
    togglePasswordVisibility() {
      this.showPassword = !this.showPassword;
    },
    // Method to handle the login process
    async login() {
      try {
        if (this.email.trim() == '' || this.password.trim() == '') {
          toast.info("Bitte geben Sie sowohl E-Mail als auch Passwort ein!");
          return;
        }

        const response = await axiosInstance.post(`${process.env.baseURL}authenticate/login`, {
          email: this.email,
          password: this.password
        });
        const token = response.data.accessToken;
        const logId = response.data.userId;
        const firstName = response.data.firstName;

        const encryptedToken = this.encryptItem(token);
        const encryptedLogId = this.encryptItem(logId);

        sessionStorage.setItem('token', encryptedToken);
        sessionStorage.setItem('logId', encryptedLogId);
        sessionStorage.setItem('firstName', firstName);

        if (GetUserRole() == 'Admin') {
          router.push('/admin');
        } else {
          router.push('/home');
        }
      } catch (error) {

        if (error.response && error.response.status === 401) {
          toast.info("Ungültige E-Mail oder Passwort oder bestätigen Sie zuerst Ihre E-Mail");
        }
        else {
          toast.info("Wir haben ein Problem auf dem Server. Bitte versuchen Sie es erneut!");
        }
      }
    },

    encryptItem(item) {
      return AES.encrypt(item, process.env.SECRET_KEY).toString();
    }

  }
};
</script>
<style>
.v-container {
  display: none !important;
} 
</style>
