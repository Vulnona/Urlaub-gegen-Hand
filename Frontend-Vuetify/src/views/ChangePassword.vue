<template>
<PublicNav />
<div class="loginmain-bg">
  <div class="loginmain-form">
<div class="login-form-section auth-card auth-register" id="login-content">
  <form class="form-border" @submit.prevent="change">
  <div class="register-form-fields" :class="{ 'register-form-fields-mobile': isMobile }">
    <div class="custom-form" :class="{ 'has-error': !password && showError || !isPasswordValid }">
      <label for="password">Passwort</label>
      <div class="password-container" style="position: relative;">
        <input :type="showPassword ? 'text' : 'password'" placeholder="Bitte geben Sie das neue Passwort ein."
               id="password" v-model="password" @input="validatePassword">
        <i @click="togglePasswordVisibility" :class="showPassword ? 'ri-eye-off-fill' : 'ri-eye-fill'"
           style="position: absolute; right: 10px; top: 10px; cursor: pointer;">
        </i>
      </div>
      <span v-if="(!password && showError) || (showError && !isPasswordValid)" class="error-message">
        Passwort muss mindestens einen Großbuchstaben, eine Zahl, ein Sonderzeichen <a style="font-weight: bold;"> !@#$%^&* </a> enthalten und zwischen 8 und 20 Zeichen lang sein.
      </span>
      <label for="confirmPassword">Passwort bestätigen</label>
      <div class="password-container" style="position: relative;">
        <input :type="showPassword ? 'text' : 'password'" placeholder="Bestätigen Sie Ihr Passwort"
               id="confirmPassword" v-model="confirmPassword">
        <i @click="togglePasswordVisibility"
           :class="showPassword ? 'ri-eye-off-fill' : 'ri-eye-fill'"
           style="position: absolute; right: 10px; top: 10px; cursor: pointer;">
        </i>
      </div>
      <span v-if="!confirmPassword && showError || (password !== confirmPassword && showError)"
            class="error-message">
        Passwortbestätigung ist erforderlich und muss mit dem Passwort übereinstimmen.
      </span>
      <div class="login-buttons">
      <button type="submit" class="btn">Passwort ändern.</button>
    </div>
    </div>    
  </div>
  </form>
</div>
  </div>
</div>
</template>

<script>
import PublicNav from '@/components/navbar/PublicNav.vue';
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from '@/interceptor/interceptor';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.css';
import toast from '@/components/toaster/toast';
import Swal from 'sweetalert2';
export default {
  components: {
      Navbar,
      PublicNav,
  }, 
  mounted() {
  },
    methods: {
        togglePasswordVisibility() {
            this.showPassword = !this.showPassword;
        },
        validatePassword() {
            const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,20}$/;
            this.isPasswordValid = passwordRegex.test(this.password);
        },
        change() {
            this.showError = true;
            
            if (!this.isPasswordValid || this.password !== this.confirmPassword)
                return;
            const passwordData = {
                NewPassword: this.password,
                Token: this.$route.query.token
            }
            axiosInstance.put(`${process.env.baseURL}authenticate/change-password`, passwordData)
                .then(response => {
                    Swal.close();
                    toast.success('Passwort erfolgreich geändert.');
                    this.$router.push('/login');
                })
                .catch(error => {
                    Swal.close();
                    if(this.$route.query.token)
                        toast.error('Änderung fehlgeschlagen. So der Änderungslink älter als 2 Stunden ist, bitte einen neuen anfordern.')
                    else
                        toast.error('Änderung des Passworts fehlgeschlagen.');
        });
        },
    },
    computed: {
        isMobile () {
            return screen.width <= 760;
        },
    },
    data() {
        return {
            showPassword: false,              
            password: '',
            confirmPassword: '',
            isPasswordValid: false,
            showError: false,
        };
    }
}
</script>
