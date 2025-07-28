<template>
  <div class="admin-setup">
    <div class="setup-container">
      <!-- Step 1: Admin Account Creation -->
      <div v-if="currentStep === 1" class="setup-step">
        <div class="setup-header">
          <h1>🔐 Admin-Account Erstellen</h1>
          <p>Willkommen! Erstellen Sie Ihren sicheren Admin-Account für UGH.</p>
        </div>

        <div class="setup-form">
          <div class="form-group">
            <label for="firstName">Vorname *</label>
            <input 
              id="firstName" 
              v-model="adminData.firstName" 
              type="text" 
              required 
              placeholder="Ihr Vorname"
            />
          </div>

          <div class="form-group">
            <label for="lastName">Nachname *</label>
            <input 
              id="lastName" 
              v-model="adminData.lastName" 
              type="text" 
              required 
              placeholder="Ihr Nachname"
            />
          </div>

          <div class="form-group">
            <label for="email">E-Mail-Adresse *</label>
            <input 
              id="email" 
              v-model="adminData.email" 
              type="email" 
              required 
              placeholder="ihre.email@example.com"
            />
          </div>

          <div class="form-group">
            <label for="password">Passwort *</label>
            <input 
              id="password" 
              v-model="adminData.password" 
              type="password" 
              required 
              placeholder="Sicheres Passwort"
            />
            <small>Mindestens 8 Zeichen, Groß-/Kleinschreibung, Zahlen</small>
          </div>

          <div class="form-group">
            <label for="dateOfBirth">Geburtsdatum *</label>
            <input 
              id="dateOfBirth" 
              v-model="adminData.dateOfBirth" 
              type="date" 
              required 
            />
          </div>

          <div class="form-group">
            <label for="gender">Geschlecht *</label>
            <select id="gender" v-model="adminData.gender" required>
              <option value="">Bitte wählen</option>
              <option value="Male">Männlich</option>
              <option value="Female">Weiblich</option>
              <option value="Other">Divers</option>
            </select>
          </div>

          <div class="form-group">
            <label for="setupToken">Setup-Token *</label>
            <input 
              id="setupToken" 
              v-model="adminData.setupToken" 
              type="password" 
              required 
              placeholder="Ihr Setup-Token"
            />
            <small>Dieser Token wurde Ihnen vom Systemadministrator bereitgestellt.</small>
          </div>

          <button 
            @click="createAdminAccount" 
            :disabled="!isFormValid || loading"
            class="btn btn-primary"
          >
            <span v-if="loading">Erstelle Account...</span>
            <span v-else>Admin-Account erstellen</span>
          </button>
        </div>
      </div>

      <!-- Step 2: 2FA Setup -->
      <div v-if="currentStep === 2" class="setup-step">
        <div class="setup-header">
          <h1>📱 Zwei-Faktor-Authentifizierung einrichten</h1>
          <p>Schritt 2: Richten Sie die 2FA für maximale Sicherheit ein.</p>
        </div>

        <div v-if="loading" class="loading">
          <p>Lade 2FA-Einrichtung...</p>
        </div>

        <div v-else class="setup-2fa">
          <div class="qr-section">
            <h3>1. QR-Code scannen</h3>
            <div class="qr-container">
              <img :src="`data:image/png;base64,${qrCodeImage}`" alt="QR-Code" />
            </div>
            <p class="qr-instructions">
              Öffnen Sie Ihre Authenticator-App (Google Authenticator, Authy, etc.) 
              und scannen Sie den QR-Code.
            </p>
          </div>

          <div class="manual-section">
            <h3>2. Oder manuell eingeben</h3>
            <div class="secret-display">
              <code>{{ secret }}</code>
              <button @click="copySecret" class="btn-copy">Kopieren</button>
            </div>
          </div>

          <div class="verification-section">
            <h3>3. Code verifizieren</h3>
            <div class="form-group">
              <label for="verificationCode">6-stelliger Code aus Ihrer App</label>
              <input 
                id="verificationCode" 
                v-model="verificationCode" 
                type="text" 
                maxlength="6" 
                placeholder="123456"
                @keyup.enter="verify2FA"
              />
            </div>

            <button 
              @click="verify2FA" 
              :disabled="!verificationCode || verificationCode.length !== 6 || verifying"
              class="btn btn-primary"
            >
              <span v-if="verifying">Verifiziere...</span>
              <span v-else>2FA aktivieren</span>
            </button>
          </div>
        </div>
      </div>

      <!-- Step 3: Backup Codes -->
      <div v-if="currentStep === 3" class="setup-step">
        <div class="setup-header">
          <h1>🔑 Backup-Codes speichern</h1>
          <p>Schritt 3: Speichern Sie diese Backup-Codes an einem sicheren Ort.</p>
        </div>

        <div class="backup-codes-section">
          <div class="warning-box">
            <h3>⚠️ Wichtig!</h3>
            <p>
              Diese Backup-Codes sind Ihre letzte Möglichkeit, sich anzumelden, 
              falls Sie Ihr Handy verlieren oder die Authenticator-App nicht funktioniert.
            </p>
            <p><strong>Jeder Code kann nur einmal verwendet werden!</strong></p>
          </div>

          <div class="backup-codes">
            <h3>Ihre Backup-Codes:</h3>
            <div class="codes-grid">
              <div 
                v-for="(code, index) in backupCodes" 
                :key="index" 
                class="backup-code"
              >
                {{ code }}
              </div>
            </div>
          </div>

          <div class="actions">
            <button @click="downloadBackupCodes" class="btn btn-secondary">
              📄 Als PDF herunterladen
            </button>
            <button @click="copyBackupCodes" class="btn btn-secondary">
              📋 In Zwischenablage kopieren
            </button>
          </div>

          <div class="confirmation">
            <label class="checkbox-label">
              <input 
                type="checkbox" 
                v-model="backupCodesConfirmed" 
                required 
              />
              <span>Ich habe die Backup-Codes sicher gespeichert</span>
            </label>
          </div>

          <button 
            @click="finishSetup" 
            :disabled="!backupCodesConfirmed"
            class="btn btn-success"
          >
            ✅ Setup abschließen
          </button>
        </div>
      </div>

      <!-- Success Step -->
      <div v-if="currentStep === 4" class="setup-step">
        <div class="setup-header">
          <h1>🎉 Setup erfolgreich abgeschlossen!</h1>
          <p>Ihr Admin-Account ist jetzt sicher eingerichtet.</p>
        </div>

        <div class="success-content">
          <div class="success-icon">✅</div>
          <h3>Was Sie jetzt tun können:</h3>
          <ul>
            <li>✅ Sich mit Ihrer E-Mail und Passwort anmelden</li>
            <li>✅ 2FA-Code aus Ihrer Authenticator-App eingeben</li>
            <li>✅ Auf das Admin-Dashboard zugreifen</li>
          </ul>

          <div class="next-steps">
            <h4>Nächste Schritte:</h4>
            <ol>
              <li>Gehen Sie zur <a href="/login">Anmeldeseite</a></li>
              <li>Melden Sie sich mit Ihrer E-Mail an</li>
              <li>Geben Sie den 2FA-Code aus Ihrer App ein</li>
            </ol>
          </div>

          <button @click="goToLogin" class="btn btn-primary">
            Zur Anmeldung
          </button>
        </div>
      </div>

      <!-- Error Display -->
      <div v-if="error" class="error-message">
        <h3>❌ Fehler</h3>
        <p>{{ error }}</p>
        <button @click="error = ''" class="btn btn-secondary">Schließen</button>
      </div>
    </div>
  </div>
</template>

<script>
import axiosInstance from '@/interceptor/interceptor';
import toast from '@/components/toaster/toast';

export default {
  name: 'AdminSetup',
  data() {
    return {
      currentStep: 1,
      loading: false,
      verifying: false,
      error: '',
      adminData: {
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        dateOfBirth: '',
        gender: '',
        setupToken: ''
      },
      setupToken: '',
      secret: '',
      qrCodeImage: '',
      backupCodes: [],
      verificationCode: '',
      backupCodesConfirmed: false
    };
  },
  computed: {
    isFormValid() {
      return this.adminData.firstName && 
             this.adminData.lastName && 
             this.adminData.email && 
             this.adminData.password && 
             this.adminData.password.length >= 8 &&
             this.adminData.dateOfBirth && 
             this.adminData.gender && 
             this.adminData.setupToken;
    }
  },
  methods: {
    async createAdminAccount() {
      this.loading = true;
      this.error = '';

      try {
        const response = await axiosInstance.post('admin-setup/secure-setup', {
          setupToken: this.adminData.setupToken,
          firstName: this.adminData.firstName,
          lastName: this.adminData.lastName,
          email: this.adminData.email,
          password: this.adminData.password,
          dateOfBirth: this.adminData.dateOfBirth,
          gender: this.adminData.gender
        });

        toast.success('Admin-Account erfolgreich erstellt! Prüfen Sie Ihre E-Mails.');
        
        // Extrahiere Setup-Token aus URL oder verwende einen Standard
        const urlParams = new URLSearchParams(window.location.search);
        this.setupToken = urlParams.get('token') || 'default-token';
        
        this.currentStep = 2;
        await this.load2FASetup();
      } catch (error) {
        console.error('Error creating admin account:', error);
        this.error = error.response?.data || 'Fehler beim Erstellen des Admin-Accounts';
        toast.error(this.error);
      } finally {
        this.loading = false;
      }
    },

    async load2FASetup() {
      this.loading = true;
      this.error = '';

      try {
        const response = await axiosInstance.post('admin-setup/setup-2fa', {
          setupToken: this.setupToken
        });

        this.secret = response.data.secret;
        this.qrCodeImage = response.data.qrCodeImage;
        this.backupCodes = response.data.backupCodes;
      } catch (error) {
        console.error('Error loading 2FA setup:', error);
        this.error = error.response?.data || 'Fehler beim Laden der 2FA-Einrichtung';
        toast.error(this.error);
      } finally {
        this.loading = false;
      }
    },

    async verify2FA() {
      this.verifying = true;
      this.error = '';

      try {
        const response = await axiosInstance.post('admin-setup/verify-2fa', {
          setupToken: this.setupToken,
          secret: this.secret,
          code: this.verificationCode,
          backupCodes: this.backupCodes
        });

        toast.success('2FA erfolgreich aktiviert!');
        this.currentStep = 3;
      } catch (error) {
        console.error('Error verifying 2FA:', error);
        this.error = error.response?.data || 'Fehler bei der 2FA-Verifizierung';
        toast.error(this.error);
        this.verificationCode = '';
      } finally {
        this.verifying = false;
      }
    },

    copySecret() {
      navigator.clipboard.writeText(this.secret);
      toast.success('Secret in Zwischenablage kopiert');
    },

    downloadBackupCodes() {
      const content = `UGH Admin Backup-Codes

WICHTIG: Diese Codes können nur einmal verwendet werden!

${this.backupCodes.map((code, index) => `${index + 1}. ${code}`).join('\n')}

Erstellt am: ${new Date().toLocaleString('de-DE')}
`;
      
      const blob = new Blob([content], { type: 'text/plain' });
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'ugh-admin-backup-codes.txt';
      a.click();
      window.URL.revokeObjectURL(url);
      
      toast.success('Backup-Codes heruntergeladen');
    },

    copyBackupCodes() {
      const codesText = this.backupCodes.join('\n');
      navigator.clipboard.writeText(codesText);
      toast.success('Backup-Codes in Zwischenablage kopiert');
    },

    finishSetup() {
      this.currentStep = 4;
      toast.success('Setup erfolgreich abgeschlossen!');
    },

    goToLogin() {
      this.$router.push('/login');
    }
  }
};
</script>

<style scoped>
.admin-setup {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
}

.setup-container {
  background: white;
  border-radius: 12px;
  box-shadow: 0 20px 40px rgba(0,0,0,0.1);
  max-width: 600px;
  width: 100%;
  padding: 40px;
}

.setup-header {
  text-align: center;
  margin-bottom: 30px;
}

.setup-header h1 {
  color: #333;
  margin-bottom: 10px;
  font-size: 2em;
}

.setup-header p {
  color: #666;
  font-size: 1.1em;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 600;
  color: #333;
}

.form-group input,
.form-group select {
  width: 100%;
  padding: 12px;
  border: 2px solid #e1e5e9;
  border-radius: 8px;
  font-size: 16px;
  transition: border-color 0.3s;
}

.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #667eea;
}

.form-group small {
  display: block;
  margin-top: 5px;
  color: #666;
  font-size: 0.9em;
}

.btn {
  width: 100%;
  padding: 14px;
  border: none;
  border-radius: 8px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s;
  margin-top: 10px;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(102, 126, 234, 0.3);
}

.btn-secondary {
  background: #f8f9fa;
  color: #333;
  border: 2px solid #e1e5e9;
}

.btn-success {
  background: #28a745;
  color: white;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.qr-section,
.manual-section,
.verification-section {
  margin-bottom: 30px;
}

.qr-container {
  text-align: center;
  margin: 20px 0;
}

.qr-container img {
  max-width: 200px;
  border: 2px solid #e1e5e9;
  border-radius: 8px;
}

.qr-instructions {
  color: #666;
  text-align: center;
  font-size: 0.9em;
}

.secret-display {
  background: #f8f9fa;
  padding: 15px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin: 10px 0;
}

.secret-display code {
  font-family: monospace;
  font-size: 1.1em;
  color: #333;
}

.btn-copy {
  background: #667eea;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9em;
}

.warning-box {
  background: #fff3cd;
  border: 1px solid #ffeaa7;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 20px;
}

.warning-box h3 {
  color: #856404;
  margin-bottom: 10px;
}

.codes-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
  gap: 10px;
  margin: 20px 0;
}

.backup-code {
  background: #f8f9fa;
  border: 2px solid #e1e5e9;
  border-radius: 6px;
  padding: 12px;
  text-align: center;
  font-family: monospace;
  font-weight: 600;
  color: #333;
}

.actions {
  display: flex;
  gap: 10px;
  margin: 20px 0;
}

.actions .btn {
  flex: 1;
  margin-top: 0;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 10px;
  margin: 20px 0;
  font-weight: 600;
}

.checkbox-label input[type="checkbox"] {
  width: auto;
  margin: 0;
}

.success-content {
  text-align: center;
}

.success-icon {
  font-size: 4em;
  margin-bottom: 20px;
}

.success-content ul {
  text-align: left;
  margin: 20px 0;
}

.success-content li {
  margin: 10px 0;
  color: #666;
}

.next-steps {
  background: #f8f9fa;
  padding: 20px;
  border-radius: 8px;
  margin: 20px 0;
  text-align: left;
}

.next-steps ol {
  margin: 10px 0;
  padding-left: 20px;
}

.next-steps a {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
}

.error-message {
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  border-radius: 8px;
  padding: 20px;
  margin-top: 20px;
  text-align: center;
}

.error-message h3 {
  color: #721c24;
  margin-bottom: 10px;
}

.loading {
  text-align: center;
  padding: 40px;
  color: #666;
}
</style> 