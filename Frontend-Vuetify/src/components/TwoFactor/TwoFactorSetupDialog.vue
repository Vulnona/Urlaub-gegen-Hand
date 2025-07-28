<template>
  <div class="twofa-setup-dialog">
    <h3>2-Faktor-Authentifizierung einrichten</h3>
    <div v-if="loading">Lade QR-Code...</div>
    <div v-else>
      <div class="qr-section">
        <img :src="qrCodeImage" alt="QR-Code für Authenticator" v-if="qrCodeImage" style="max-width:200px;" />
        <div v-if="qrCodeUri && !qrCodeImage">
          <a :href="qrCodeUri" target="_blank">QR-Code öffnen</a>
        </div>
        <div class="secret-section">
          <strong>Secret (manuell):</strong> <span>{{ secret }}</span>
        </div>
      </div>
      <div class="totp-section">
        <label for="totp">Code aus Ihrer Authenticator-App:</label>
        <input id="totp" v-model="totpCode" maxlength="6" placeholder="123456" />
        <button class="btn" @click="verify2FA" :disabled="verifying">2FA aktivieren</button>
      </div>
      <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
    </div>
  </div>
</template>
<script>
import axiosInstance from '@/interceptor/interceptor';
export default {
  name: 'TwoFactorSetupDialog',
  props: {
    email: { type: String, required: true }
  },
  data() {
    return {
      loading: true,
      verifying: false,
      qrCodeImage: '',
      qrCodeUri: '',
      secret: '',
      totpCode: '',
      errorMessage: ''
    };
  },
  mounted() {
    this.fetch2FASetup();
  },
  methods: {
    async fetch2FASetup() {
      this.loading = true;
      this.errorMessage = '';
      try {
        const response = await axiosInstance.post('authenticate/2fa/setup', { email: this.email });
        this.qrCodeImage = response.data.qrCodeImage || '';
        this.qrCodeUri = response.data.qrCodeUri || '';
        this.secret = response.data.secret || '';
      } catch (err) {
        this.errorMessage = 'Fehler beim Laden des QR-Codes.';
      } finally {
        this.loading = false;
      }
    },
    async verify2FA() {
      if (!this.totpCode.trim()) {
        this.errorMessage = 'Bitte geben Sie den Code aus Ihrer App ein!';
        return;
      }
      this.verifying = true;
      this.errorMessage = '';
      try {
        const response = await axiosInstance.post('authenticate/2fa/verify-setup', {
          email: this.email,
          secret: this.secret,
          code: this.totpCode
        });
        this.$emit('setup-success');
      } catch (err) {
        this.errorMessage = 'Code ungültig oder Serverfehler. Bitte erneut versuchen.';
      } finally {
        this.verifying = false;
      }
    }
  }
};
</script>
<style scoped>
.twofa-setup-dialog {
  max-width: 350px;
  margin: 0 auto;
  background: #fff;
  border-radius: 8px;
  padding: 24px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
}
.qr-section {
  text-align: center;
  margin-bottom: 16px;
}
.secret-section {
  margin-top: 8px;
  font-size: 0.95em;
  color: #333;
}
.totp-section {
  margin-top: 16px;
}
.error-message {
  color: #c00;
  margin-top: 12px;
}
</style> 