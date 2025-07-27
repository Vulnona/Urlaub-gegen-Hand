<template>
  <div class="two-factor-login">
    <div class="login-container">
      <div class="login-header">
        <h2>Zwei-Faktor-Authentifizierung</h2>
        <p>Geben Sie den 6-stelligen Code aus Ihrer Authenticator App ein</p>
      </div>

      <div class="login-form">
        <div class="code-input-section">
          <label for="twoFactorCode">Authentifizierungscode</label>
          <input 
            id="twoFactorCode"
            type="text" 
            v-model="twoFactorCode" 
            placeholder="000000"
            maxlength="6"
            class="code-input"
            :class="{ 'error': hasError }"
            @input="formatCode"
            @keyup.enter="submitCode"
            autofocus
          />
        </div>

        <div class="form-actions">
          <button @click="submitCode" class="btn btn-primary" :disabled="!isCodeValid || submitting">
            <span v-if="submitting">Verifiziere...</span>
            <span v-else>Anmelden</span>
          </button>
        </div>

        <!-- Backup Code Option -->
        <div class="backup-section">
          <div v-if="!showBackupInput" class="backup-link">
            <a @click="showBackupInput = true" href="#">
              Backup-Code verwenden
            </a>
          </div>

          <div v-else class="backup-input-section">
            <label for="backupCode">Backup-Code</label>
            <input 
              id="backupCode"
              type="text" 
              v-model="backupCode" 
              placeholder="xxxxx-xxxxx"
              class="backup-input"
              :class="{ 'error': hasError }"
              @keyup.enter="submitBackupCode"
            />
            <div class="backup-actions">
              <button @click="submitBackupCode" class="btn btn-secondary" :disabled="!backupCode.trim() || submitting">
                <span v-if="submitting">Verifiziere...</span>
                <span v-else>Mit Backup-Code anmelden</span>
              </button>
              <button @click="showBackupInput = false; backupCode = ''" class="btn btn-link">
                Abbrechen
              </button>
            </div>
          </div>
        </div>

        <!-- Error Message -->
        <div v-if="errorMessage" class="error-message">
          <i class="ri-error-warning-line"></i>
          {{ errorMessage }}
        </div>

        <!-- Help Section -->
        <div class="help-section">
          <h4>Probleme beim Anmelden?</h4>
          <ul>
            <li>Stellen Sie sicher, dass die Zeit auf Ihrem Gerät korrekt ist</li>
            <li>Verwenden Sie einen aktuellen Code aus Ihrer Authenticator App</li>
            <li>Falls Sie keinen Zugang zu Ihrer App haben, verwenden Sie einen Backup-Code</li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axiosInstance from '@/interceptor/interceptor'
import toast from '@/components/toaster/toast'

export default {
  name: 'TwoFactorLogin',
  props: {
    email: {
      type: String,
      required: true
    },
    password: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      twoFactorCode: '',
      backupCode: '',
      showBackupInput: false,
      submitting: false,
      hasError: false,
      errorMessage: ''
    }
  },
  computed: {
    isCodeValid() {
      return this.twoFactorCode.length === 6 && /^\d{6}$/.test(this.twoFactorCode)
    }
  },
  methods: {
    formatCode() {
      // Remove any non-numeric characters and limit to 6 digits
      this.twoFactorCode = this.twoFactorCode.replace(/\D/g, '').slice(0, 6)
      this.clearError()
    },

    async submitCode() {
      if (!this.isCodeValid) return
      
      await this.performLogin(this.twoFactorCode, false)
    },

    async submitBackupCode() {
      if (!this.backupCode.trim()) return
      
      await this.performLogin(this.backupCode.trim(), true)
    },

    async performLogin(code, isBackupCode) {
      this.submitting = true
      this.clearError()

      try {
        const response = await axiosInstance.post(`authenticate/login-with-2fa`, {
          email: this.email,
          password: this.password,
          twoFactorCode: code,
          isBackupCode: isBackupCode
        })

        // Store JWT token
        if (response.data.token) {
          localStorage.setItem('auth_token', response.data.token)
          localStorage.setItem('userRole', response.data.role)
          localStorage.setItem('user', JSON.stringify(response.data.user))
        }

        toast.success('Erfolgreich angemeldet!')
        
        // Emit success event with user data
        this.$emit('login-success', response.data)

        // Redirect based on role
        const redirectPath = response.data.role === 'Admin' ? '/admin' : '/home'
        this.$router.push(redirectPath)

      } catch (error) {
        this.hasError = true
        console.error('2FA Login Fehler:', error);
        if (error.response?.status === 401) {
          this.errorMessage = 'Ungültiger Authentifizierungscode. Bitte versuchen Sie es erneut.'
        } else if (error.response?.status === 400) {
          this.errorMessage = 'Ungültige Eingabe. Bitte überprüfen Sie Ihre Daten.'
        } else {
          this.errorMessage = 'Anmeldung nicht möglich. Bitte versuchen Sie es später erneut.'
        }
        toast.error(this.errorMessage)
        // Clear input on error
        if (isBackupCode) {
          this.backupCode = ''
        } else {
          this.twoFactorCode = ''
        }
      } finally {
        this.submitting = false
      }
    },

    clearError() {
      this.hasError = false
      this.errorMessage = ''
    }
  }
}
</script>

<style scoped>
.two-factor-login {
  max-width: 450px;
  margin: 0 auto;
  padding: 20px;
}

.login-container {
  background: white;
  border-radius: 8px;
  padding: 30px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.login-header {
  text-align: center;
  margin-bottom: 30px;
}

.login-header h2 {
  color: #333;
  margin-bottom: 10px;
}

.login-header p {
  color: #666;
  font-size: 14px;
}

.code-input-section {
  margin-bottom: 20px;
}

.code-input-section label {
  display: block;
  margin-bottom: 8px;
  font-weight: bold;
  color: #555;
}

.code-input {
  width: 100%;
  padding: 15px;
  border: 2px solid #ddd;
  border-radius: 6px;
  font-size: 24px;
  text-align: center;
  letter-spacing: 4px;
  font-family: monospace;
  transition: border-color 0.3s;
}

.code-input:focus {
  outline: none;
  border-color: #007bff;
}

.code-input.error {
  border-color: #dc3545;
}

.form-actions {
  margin-bottom: 30px;
}

.btn {
  width: 100%;
  padding: 12px;
  border: none;
  border-radius: 6px;
  font-size: 16px;
  cursor: pointer;
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
  margin-bottom: 10px;
}

.btn-secondary:hover:not(:disabled) {
  background: #545b62;
}

.btn-link {
  background: none;
  color: #007bff;
  text-decoration: underline;
  padding: 5px 0;
}

.btn-link:hover {
  color: #0056b3;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.backup-section {
  margin-bottom: 20px;
  padding-top: 20px;
  border-top: 1px solid #eee;
}

.backup-link {
  text-align: center;
}

.backup-link a {
  color: #007bff;
  text-decoration: none;
  font-size: 14px;
  cursor: pointer;
}

.backup-link a:hover {
  text-decoration: underline;
}

.backup-input-section label {
  display: block;
  margin-bottom: 8px;
  font-weight: bold;
  color: #555;
  font-size: 14px;
}

.backup-input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  margin-bottom: 15px;
  font-family: monospace;
}

.backup-input:focus {
  outline: none;
  border-color: #007bff;
}

.backup-input.error {
  border-color: #dc3545;
}

.backup-actions {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.error-message {
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  color: #721c24;
  padding: 12px;
  border-radius: 4px;
  margin-bottom: 20px;
  display: flex;
  align-items: center;
  gap: 8px;
}

.help-section {
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #eee;
}

.help-section h4 {
  color: #666;
  font-size: 16px;
  margin-bottom: 10px;
}

.help-section ul {
  color: #777;
  font-size: 14px;
  line-height: 1.5;
  margin: 0;
  padding-left: 20px;
}

.help-section li {
  margin-bottom: 5px;
}
</style>
