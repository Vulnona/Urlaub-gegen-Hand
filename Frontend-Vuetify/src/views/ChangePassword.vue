<template>
<PublicNav />
<div class="loginmain-bg">
  <div class="loginmain-form">
<div class="login-form-section auth-card auth-register" id="login-content">
  <!-- Special message for backup code users -->
  <div v-if="isBackupCodePasswordChange" class="backup-code-warning" style="background: #fff3cd; border: 1px solid #ffeaa7; border-radius: 8px; padding: 15px; margin-bottom: 20px;">
    <h4 style="color: #856404; margin: 0 0 10px 0;">🔒 Sicherheitshinweis</h4>
    <p style="color: #856404; margin: 0;">Sie haben sich mit einem Backup-Code angemeldet. Aus Sicherheitsgründen müssen Sie Ihr Passwort ändern, bevor Sie fortfahren können.</p>
  </div>
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
        <button type="button" class="btn btn-secondary me-2" @click="goBack">
          <i class="ri-arrow-left-line"></i> Zurück
        </button>
        <button type="submit" class="btn">Passwort ändern</button>
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
import CryptoJS from 'crypto-js';
export default {
  components: {
      Navbar,
      PublicNav,
  }, 
  mounted() {
    // Check if this is a backup code password change
    const requirePasswordChange = sessionStorage.getItem('requirePasswordChange');
    if (requirePasswordChange === 'true') {
      // Show special message for backup code users
      this.isBackupCodePasswordChange = true;
      // Clear the flag
      sessionStorage.removeItem('requirePasswordChange');
    }
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
            
            // Ensure token is set in headers
            const token = sessionStorage.getItem('token');
            if (token) {
                try {
                    const bytes = CryptoJS.AES.decrypt(token, import.meta.env.VITE_SECRET_KEY || 'thisismytestsecretkey');
                    const decryptedToken = bytes.toString(CryptoJS.enc.Utf8);
                    console.log('Decrypted token:', decryptedToken ? 'Token found' : 'No token');
                    axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${decryptedToken}`;
                } catch (e) {
                    console.error('Error decrypting token:', e);
                }
            } else {
                console.error('No token found in session storage');
            }
            
            const passwordData = {
                NewPassword: this.password,
                Token: this.isBackupCodePasswordChange ? null : this.$route.query.token
            }
            
            console.log('Sending password change request with data:', passwordData);
            console.log('Authorization header:', axiosInstance.defaults.headers.common['Authorization']);
            
            axiosInstance.put(`authenticate/change-password`, passwordData)
                .then(response => {
                    Swal.close();
                    toast.success('Passwort erfolgreich geändert.');
                    
                    // If this was a backup code password change, redirect to 2FA setup
                    if (this.isBackupCodePasswordChange) {
                        this.$router.push('/setup-2fa');
                    } else {
                        this.$router.push('/login');
                    }
                })
                .catch(error => {
                    Swal.close();
                    console.error('Fehler beim Ändern des Passworts:', error);
                    if(this.$route.query.token)
                        toast.error('Änderung fehlgeschlagen. So der Änderungslink älter als 2 Stunden ist, bitte einen neuen anfordern.')
                    else
                        toast.error('Änderung des Passworts fehlgeschlagen.');
        });
        },
        goBack() {
            this.$router.go(-1);
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
            isBackupCodePasswordChange: false,
        };
    }
}
</script>
