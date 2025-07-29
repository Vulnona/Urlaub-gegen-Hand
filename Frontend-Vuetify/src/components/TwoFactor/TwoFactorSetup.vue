<template>
  <div class="two-factor-setup-page">
    <PublicNav />
    <div class="setup-main">
      <div class="two-factor-setup">
        <div class="setup-container">
      <div class="setup-header">
        <h2>Zwei-Faktor-Authentifizierung einrichten</h2>
        <p>Erhöhen Sie die Sicherheit Ihres Kontos mit 2FA</p>
      </div>

      <!-- Step 1: Generate QR Code -->
      <div v-if="currentStep === 1" class="setup-step">
        <h3>Schritt 1: Authenticator App einrichten</h3>
        <p>Scannen Sie den QR-Code mit Ihrer Authenticator App (Google Authenticator, Authy, etc.)</p>
        
        <div class="qr-code-container" v-if="qrCodeData">
          <QRCodeVue3 
            :value="qrCodeData" 
            :size="200"
            :margin="10"
            :level="'M'"
            :background="'#ffffff'"
            :foreground="'#000000'"
          />
        </div>

        <div class="manual-entry" v-if="secret">
          <h4>Manuelle Eingabe</h4>
          <p>Falls Sie den QR-Code nicht scannen können, geben Sie diesen Code manuell ein:</p>
          <div class="secret-code">
            <code>{{ secret }}</code>
            <button @click="copySecret" class="copy-btn" title="Kopieren">
              <i class="ri-file-copy-line"></i>
            </button>
          </div>
        </div>

        <div class="step-actions">
          <button @click="generateQRCode" class="btn btn-primary" :disabled="loading">
            <span v-if="loading">Laden...</span>
            <span v-else>QR-Code generieren</span>
          </button>
          <button @click="currentStep = 2" class="btn btn-secondary" :disabled="!qrCodeData">
            Weiter zu Schritt 2
          </button>
        </div>
      </div>

      <!-- Step 2: Verify Setup -->
      <div v-if="currentStep === 2" class="setup-step">
        <h3>Schritt 2: Setup verifizieren</h3>
        <p>Geben Sie den 6-stelligen Code aus Ihrer Authenticator App ein:</p>
        
        <div class="verification-form">
          <input 
            type="text" 
            v-model="verificationCode" 
            placeholder="000000"
            maxlength="6"
            class="verification-input"
            @input="formatCode"
          />
          <button @click="verifySetup" class="btn btn-primary" :disabled="verificationCode.length !== 6 || verifying">
            <span v-if="verifying">Verifiziere...</span>
            <span v-else>Verifizieren</span>
          </button>
        </div>

        <div class="step-actions">
          <button @click="currentStep = 1" class="btn btn-secondary">
            Zurück zu Schritt 1
          </button>
        </div>
      </div>

      <!-- Step 3: Backup Codes -->
      <div v-if="currentStep === 3" class="setup-step">
        <h3>Schritt 3: Backup-Codes speichern</h3>
        <p class="warning-text">
          <i class="ri-warning-line"></i>
          Wichtig: Speichern Sie diese Backup-Codes an einem sicheren Ort. 
          Sie können diese verwenden, wenn Sie keinen Zugang zu Ihrer Authenticator App haben.
        </p>
        
        <div class="backup-codes" v-if="backupCodes.length > 0">
          <div class="codes-grid">
            <div v-for="(code, index) in backupCodes" :key="index" class="backup-code">
              {{ code }}
            </div>
          </div>
          
          <div class="backup-actions">
            <button @click="downloadBackupCodes" class="btn btn-secondary">
              <i class="ri-download-line"></i> Codes herunterladen
            </button>
            <button @click="copyBackupCodes" class="btn btn-secondary">
              <i class="ri-file-copy-line"></i> Codes kopieren
            </button>
          </div>
        </div>

        <div class="completion-checkbox">
          <label>
            <input type="checkbox" v-model="codesStored" />
            Ich habe die Backup-Codes sicher gespeichert
          </label>
        </div>

        <div class="step-actions">
          <button @click="finishSetup" class="btn btn-success" :disabled="!codesStored">
            2FA-Setup abschließen
          </button>
        </div>
      </div>

      <!-- Error Display -->
      <div v-if="errorMessage" class="error-message">
        {{ errorMessage }}
      </div>
    </div>
  </div>
</div>
</div>
</template>

<script>
import QRCodeVue3 from 'qrcode-vue3'
import axiosInstance from '@/interceptor/interceptor'
import toast from '@/components/toaster/toast'
import PublicNav from '@/components/navbar/PublicNav.vue'
import router from '@/router'
import CryptoJS from 'crypto-js'

export default {
  name: 'TwoFactorSetup',
  components: {
    QRCodeVue3,
    PublicNav
  },
  data() {
    return {
      currentStep: 1,
      loading: false,
      verifying: false,
      qrCodeData: '',
      secret: '',
      verificationCode: '',
      backupCodes: [],
      codesStored: false,
      errorMessage: ''
    }
  },
  methods: {
    async generateQRCode() {
      this.loading = true
      this.errorMessage = ''
      
      try {
        // For 2FA setup in recovery mode, we don't need a token
        // Get email from session storage or use a default for admin recovery
        const userEmail = sessionStorage.getItem('setup2fa_email') || 'admin@example.com';
        
        console.log('Generating QR code for email:', userEmail);
        
        // For 2FA setup, we don't need authorization header (recovery scenario)
        console.log('2FA setup - no authorization required for recovery');
        
        const response = await axiosInstance.post(`authenticate/2fa/setup`, {
          email: userEmail
        })
        
        this.qrCodeData = response.data.qrCodeUri
        this.secret = response.data.secret
        this.backupCodes = response.data.backupCodes
        
        toast.success('QR-Code generiert!')
      } catch (error) {
        console.error('QR-Code Generierung Fehler:', error);
        this.errorMessage = 'Fehler beim Generieren des QR-Codes. Bitte versuchen Sie es erneut.';
        toast.error(this.errorMessage)
      } finally {
        this.loading = false
      }
    },

    async verifySetup() {
      this.verifying = true
      this.errorMessage = ''
      
      try {
        const userEmail = 'admin@example.com'; // For now, hardcode admin email
        const response = await axiosInstance.post(`authenticate/2fa/verify-setup`, {
          email: userEmail,
          secret: this.secret,
          code: this.verificationCode
        })
        
        toast.success('2FA erfolgreich eingerichtet!')
        this.currentStep = 3
      } catch (error) {
        console.error('2FA Verifizierung Fehler:', error);
        if (error.response?.status === 400 || error.response?.status === 401) {
          this.errorMessage = 'Ungültiger Verifizierungscode. Bitte überprüfen Sie Ihre Eingabe.';
        } else {
          this.errorMessage = 'Fehler bei der Verifizierung. Bitte versuchen Sie es erneut.';
        }
        toast.error(this.errorMessage)
      } finally {
        this.verifying = false
      }
    },

    formatCode() {
      // Remove any non-numeric characters
      this.verificationCode = this.verificationCode.replace(/\D/g, '')
    },

    copySecret() {
      navigator.clipboard.writeText(this.secret)
      toast.success('Secret in die Zwischenablage kopiert!')
    },

    copyBackupCodes() {
      const codesText = this.backupCodes.join('\n')
      navigator.clipboard.writeText(codesText)
      toast.success('Backup-Codes in die Zwischenablage kopiert!')
    },

    downloadBackupCodes() {
      const codesText = this.backupCodes.join('\n')
      const blob = new Blob([codesText], { type: 'text/plain' })
      const url = window.URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = 'ugh-backup-codes.txt'
      document.body.appendChild(a)
      a.click()
      document.body.removeChild(a)
      window.URL.revokeObjectURL(url)
      toast.success('Backup-Codes heruntergeladen!')
    },

    decryptToken(encryptedToken) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedToken, import.meta.env.VITE_SECRET_KEY || 'thisismytestsecretkey');
        return bytes.toString(CryptoJS.enc.Utf8);
      } catch (e) {
        console.error('Error decrypting token:', e);
        return null;
      }
    },

    finishSetup() {
      toast.success('2FA-Setup erfolgreich abgeschlossen!')
      // Redirect to admin panel after successful setup
      this.$router.push('/admin')
    }
  }
}
</script>

<style scoped>
.two-factor-setup-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.setup-main {
  padding: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: calc(100vh - 80px);
}

.two-factor-setup {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
}

.setup-container {
  background: white;
  border-radius: 8px;
  padding: 30px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.setup-header {
  text-align: center;
  margin-bottom: 30px;
}

.setup-header h2 {
  color: #333;
  margin-bottom: 10px;
}

.setup-step {
  margin-bottom: 20px;
}

.setup-step h3 {
  color: #444;
  margin-bottom: 15px;
}

.qr-code-container {
  text-align: center;
  margin: 20px 0;
  padding: 20px;
  background: #f9f9f9;
  border-radius: 8px;
}

.manual-entry {
  margin-top: 20px;
  padding: 15px;
  background: #f5f5f5;
  border-radius: 6px;
}

.secret-code {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-top: 10px;
}

.secret-code code {
  background: white;
  padding: 8px 12px;
  border-radius: 4px;
  font-family: monospace;
  border: 1px solid #ddd;
  flex-grow: 1;
}

.copy-btn {
  background: #007bff;
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.verification-form {
  display: flex;
  gap: 15px;
  align-items: center;
  margin: 20px 0;
}

.verification-input {
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 18px;
  text-align: center;
  letter-spacing: 2px;
  font-family: monospace;
  width: 150px;
}

.backup-codes {
  margin: 20px 0;
}

.codes-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 10px;
  margin-bottom: 20px;
}

.backup-code {
  background: #f8f9fa;
  padding: 10px;
  border: 1px solid #dee2e6;
  border-radius: 4px;
  font-family: monospace;
  text-align: center;
  font-weight: bold;
}

.backup-actions {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
}

.completion-checkbox {
  margin: 20px 0;
}

.completion-checkbox label {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
}

.step-actions {
  display: flex;
  gap: 15px;
  justify-content: center;
  margin-top: 30px;
}

.btn {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #0056b3;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background: #545b62;
}

.btn-success {
  background: #28a745;
  color: white;
}

.btn-success:hover:not(:disabled) {
  background: #1e7e34;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.warning-text {
  background: #fff3cd;
  border: 1px solid #ffeaa7;
  color: #856404;
  padding: 15px;
  border-radius: 4px;
  margin-bottom: 20px;
}

.warning-text i {
  margin-right: 8px;
}

.error-message {
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  color: #721c24;
  padding: 15px;
  border-radius: 4px;
  margin-top: 20px;
}
</style>
