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
                    <label for="fname">Email</label>
                    <input type="text" placeholder="Emailadresse eingeben" v-model="email" id="email" required>
                  </div>
                  <div>
                    <div class="login-buttons">
                      <!-- Disable button when loading -->
                      <button type="submit" class="btn" :disabled="loading">
                        <!-- Show loader or text based on loading status -->
                        <span v-if="loading">Sending...</span>
                        <span v-else>Versende Bestätigungs-Email</span>
                      </button>
                    </div>
                    <div class="back-login">
                      <a href="/login"><i class="ri-arrow-left-double-fill"></i> Back to Login</a>
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
        if (error.response) {
          this.message = error.response.data.value;
          this.isError = true;
        } else {
          toast.info("Ein Fehler ist aufgetreten. Versuche es noch einmal");
          this.isError = true;
        }
      } finally {
        this.loading = false; // Enable the button
      }
    }
  }
};

</script>


