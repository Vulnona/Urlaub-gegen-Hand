<template>
  <PublicNav />
    <div class="loginmain-bg">
      <div class="loginmain-form">
        <div class="login-right">
          <div class="login-right-content-heading form-act">
            <div class="login-form-section" id="login-content">
              <div class="signin-tabs">
                <h2>Neue Bestätigungsmail anfordern</h2>
              </div>
              <div class="auth-card">
                <form class="form-border" @submit.prevent="VerificationEmail">
                  <div class="custom-form">
                    <label for="fname">E-Mail</label>
                    <input type="text" placeholder="E-Mail-Adresse eingeben" v-model="email" id="email" required>
                  </div>
                  <div>
                    <div class="login-buttons">
                      <!-- Disable button when loading -->
                      <button type="submit" class="btn-primary-ugh" :disabled="loading">
                        <!-- Show loader or text based on loading status -->
                        <span v-if="loading">Sende...</span>
                        <span v-else>Bestätigungs-E-Mail versenden</span>
                      </button>
                    </div>
                    <div class="back-login">
                      <a href="/login"><i class="ri-arrow-left-double-fill"></i> Zurück zum Login</a>
                    </div>
                  </div>
                  <div v-if="message" :class="{ 'error-message': isError, 'success-message': !isError }">
                    {{ message }}
                  </div>
                </form>
              </div>
            </div>

          </div>
        </div>
      </div>
    </div>
</template>

<script>
import axiosInstance from '@/interceptor/interceptor';
import toast from '@/components/toaster/toast';
import PublicNav from '@/components/navbar/PublicNav.vue';
export default {
    components: {
        PublicNav
    },
  data() {
    return {
      email: '',
      message: '',
      isError: false,
      loading: false // Add loading state
    };
  },

  methods: {
    async VerificationEmail() {
      this.loading = true; // Disable the button
      try {
        const response = await axiosInstance.post(`authenticate/resend-email-verification`, {
          email: this.email
        });
        this.message = response.data.value;
        this.isError = false;
      } catch (error) {
        console.error('Fehler beim Senden der Bestätigungs-E-Mail:', error);
        this.message = 'Fehler beim Senden der Bestätigungs-E-Mail. Bitte versuchen Sie es erneut.';
        this.isError = true;
      } finally {
        this.loading = false; // Enable the button
      }
    }
  }
};

</script>


