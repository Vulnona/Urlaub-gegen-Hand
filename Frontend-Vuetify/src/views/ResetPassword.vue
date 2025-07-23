<template>
  <PublicNav />
    <div class="loginmain-bg">
      <div class="loginmain-form">
        <div class="login-right">
          <div class="login-right-content-heading form-act">
            <div class="login-form-section" id="login-content">
              <div class="signin-tabs">
                <h2>Neue Passwort anfordern</h2>
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
                        <span v-if="loading">Wird gesendet...</span>
                        <span v-else>Passwort-Link anfordern</span>
                      </button>
                    </div>
                    <div class="back-login">
                      <a href="/login"><i class="ri-arrow-left-double-fill"></i> Zurück zur Anmeldung</a>
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
        const response = await axiosInstance.post(`${process.env.baseURL}authenticate/reset-password`, {
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

<style scoped>
.resend-email-verification {
  max-width: 600px;
  margin: auto;
  padding: 30px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

h2 {
  text-align: center;
  color: #333;
  margin-bottom: 20px;
}

.form-container {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

label {
  font-weight: bold;
  margin-bottom: 5px;
  color: #555;
}

input[type="email"] {
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 16px;
}

input[type="email"]:focus {
  border-color: #007BFF;
  outline: none;
}

.submit-button {
  padding: 10px;
  background-color: #007BFF;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.submit-button:hover {
  background-color: #0056b3;
}

.error-message {
  color: #ff4d4d;
  text-align: center;
  margin-top: 20px;
}

.success-message {
  color: #28a745;
  text-align: center;
  margin-top: 20px;
}
</style>
