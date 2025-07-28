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
                    <input type="text" placeholder="Benutzernamen eingeben" id="username" v-model="email" />
                  </div>
                  <div>
                    <div class="custom-form">
                      <div class="d-flex justify-content-between">
                        <label>Passwort</label>
                      </div>
                      <div class="password-container" style="position: relative;">
                        <input :type="showPassword ? 'text' : 'password'" placeholder="Passwort eingeben" id="password" v-model="password" />
                        <i @click="togglePasswordVisibility" :class="showPassword ? 'ri-eye-off-fill' : 'ri-eye-fill'" style="position: absolute; right: 10px; top: 10px; cursor: pointer;"></i>
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
                <!-- 2FA input and backup codes for test environment -->
                <div v-if="show2FA" class="twofa-section">
                  <label for="twoFactorCode">2FA Code eingeben:</label>
                  <input 
                    type="text" 
                    id="twoFactorCode" 
                    v-model="twoFactorCode" 
                    placeholder="2FA Code (6 Ziffern) oder Backup Code (8 Zeichen)" 
                    @keyup.enter="submit2FA"
                    maxlength="8"
                    autofocus
                  />
                  <div v-if="twoFactorCode" class="code-type-indicator">
                    <small v-if="isBackupCode(twoFactorCode)" style="color: #28a745;">
                      <i class="ri-shield-keyhole-line"></i> Backup-Code erkannt
                    </small>
                    <small v-else-if="twoFactorCode.length === 6" style="color: #007bff;">
                      <i class="ri-time-line"></i> 2FA-Code erkannt
                    </small>
                  </div>
                  <button class="btn" @click="submit2FA">2FA bestätigen</button>
                  <!-- Show current TOTP code for admin in dev/test -->
                  <div v-if="email.toLowerCase() === 'adminuser@example.com' && process.env.NODE_ENV !== 'production'" class="totp-dev-code" style="margin-top:10px;">
                    <strong>Entwickler-Testcode:</strong>
                    <span v-if="totpCode">{{ totpCode }}</span>
                    <span v-else>Lade Code...</span>
                    <span style="color:#888; font-size:0.9em; margin-left:10px;">(automatisch aktualisiert)</span>
                  </div>
                  <div v-if="showBackupCodes && backupCodes.length && process.env.NODE_ENV !== 'production'" class="backup-codes-test">
                    <p><strong>Test-Backup-Codes (nur Testumgebung):</strong></p>
                    <ul>
                      <li v-for="code in backupCodes" :key="code">{{ code }}</li>
                    </ul>
                    <p style="color: #888; font-size: 0.9em;">In Produktion werden diese Codes nicht angezeigt.</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <TwoFactorSetupDialog v-if="show2FASetup" :email="email" @setup-success="on2FASetupSuccess" />
  </div>
</template>
<script>
import PublicNav from '@/components/navbar/PublicNav.vue';
import jsSHA from 'jssha';
import router from '@/router';
import AES from 'crypto-js/aes';
import {GetUserRole} from "@/services/GetUserPrivileges";
import axiosInstance from '@/interceptor/interceptor';
import toast from '../toaster/toast';
import TwoFactorSetupDialog from '@/components/TwoFactor/TwoFactorSetupDialog.vue';
// Minimalistische TOTP-Berechnung für den Browser (SHA-1, 6 digits)
function base32ToHex(base32) {
  const base32chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
  let bits = "";
  let hex = "";
  for (let i = 0; i < base32.length; i++) {
    const val = base32chars.indexOf(base32.charAt(i).toUpperCase());
    bits += ("00000" + val.toString(2)).slice(-5);
  }
  for (let i = 0; i + 4 <= bits.length; i += 4) {
    hex += parseInt(bits.substr(i, 4), 2).toString(16);
  }
  // Padding: HEX-String muss gerade sein (volle Bytes)
  if (hex.length % 2 !== 0) {
    hex += "0";
  }
  return hex;
}

function leftpad(str, len, pad) {
  return str.length >= len ? str : Array(len - str.length + 1).join(pad) + str;
}

function generateTotp(secret, digits = 6) {
  // secret muss base32 sein
  const key = base32ToHex(secret.replace(/\s+/g, ""));
  const epoch = Math.floor(Date.now() / 1000);
  const time = leftpad(Math.floor(epoch / 30).toString(16), 16, "0");
  // HMAC-SHA1
  const hmacObj = new jsSHA("SHA-1", "HEX");
  hmacObj.setHMACKey(key, "HEX");
  hmacObj.update(time);
  const hmac = hmacObj.getHMAC("HEX");
  const offset = parseInt(hmac.substr(hmac.length - 1), 16) * 2;
  const code = (parseInt(hmac.substr(offset, 8), 16) & 0x7fffffff) + "";
  return leftpad((parseInt(code) % Math.pow(10, digits)).toString(), digits, "0");
}
export default {
  components: {
    PublicNav,
    TwoFactorSetupDialog
  },
  data() {
    return {
      email: '',
      password: '',
      errorMessage: '',
      showPassword: false,
      backupCodes: [],
      showBackupCodes: false,
      show2FA: false,
      twoFactorCode: '',
      totpCode: '',
      totpTimer: null,
      twoFactorToken: '', // temporäres 2FA-Token
      show2FASetup: false,
    };
  },
  methods: {
    togglePasswordVisibility() {
      this.showPassword = !this.showPassword;
    },
    async fetchBackupCodes(userId) {
      // Backup-Codes nur für Admins über die Admin-Route, sonst keine Abfrage
      if (this.email && this.email.toLowerCase().includes('admin')) {
        try {
          const response = await axiosInstance.get(`/admin/get-user-by-id/${userId}`);
          if (response.data.backupCodes) {
            this.backupCodes = JSON.parse(response.data.backupCodes);
            this.showBackupCodes = true;
          } else {
            this.backupCodes = [];
            this.showBackupCodes = false;
          }
        } catch (error) {
          this.backupCodes = [];
          this.showBackupCodes = false;
        }
      } else {
        this.backupCodes = [];
        this.showBackupCodes = false;
      }
    },
    async login() {
      try {
        if (this.email.trim() == '' || this.password.trim() == '') {
          toast.info("Bitte geben Sie sowohl E-Mail als auch Passwort ein!");
          return;
        }
        const response = await axiosInstance.post('authenticate/login', {
          email: this.email,
          password: this.password
        });
        if (response.data.requiresTwoFactor) {
          this.show2FA = true;
          this.twoFactorToken = response.data.twoFactorToken || '';
          toast.info("Bitte geben Sie Ihren 2FA-Code ein.");
          this.$nextTick(() => {
            if (this.email.toLowerCase() === 'adminuser@example.com' && process.env.NODE_ENV !== 'production') {
              this.startTotpUpdater();
            }
          });
          return;
        }
        await this.handleLoginSuccess(response.data);
      } catch (error) {
        console.error('Fehler beim Login:', error);
        // Prüfe auf Admin-2FA-Setup-Fehler
        if (error.response && error.response.data && typeof error.response.data === 'object' && error.response.data.Message && error.response.data.Message.includes('Admin accounts must have 2FA enabled')) {
          this.show2FASetup = true;
          toast.info('Bitte richten Sie jetzt die Zwei-Faktor-Authentifizierung ein.');
        } else if (error.response && error.response.status === 401) {
          toast.info("Ungültige E-Mail oder Passwort oder bestätigen Sie zuerst Ihre E-Mail");
        } else {
          toast.info("Login nicht möglich. Bitte versuchen Sie es erneut.");
        }
      }
    },
    // Helper function to detect if input is a backup code
    isBackupCode(code) {
      // Backup codes are typically 8 characters long and contain only alphanumeric characters
      // They might also have a specific format like "XXXX-XXXX" or just "XXXXXXXX"
      const cleanCode = code.replace(/[^A-Za-z0-9]/g, ''); // Remove non-alphanumeric chars
      return cleanCode.length === 8; // Backup codes are 8 characters
    },

    async submit2FA() {
      if (!this.twoFactorCode.trim()) {
        toast.info("Bitte geben Sie Ihren 2FA-Code ein!");
        return;
      }
      this.stopTotpUpdater();
      
      // Automatically detect if it's a backup code
      const isBackup = this.isBackupCode(this.twoFactorCode);
      
      try {
        const response = await axiosInstance.post('authenticate/login-2fa', {
          email: this.email,
          twoFactorCode: this.twoFactorCode,
          isBackupCode: isBackup,
          twoFactorToken: this.twoFactorToken
        });
        // Check if backup code was used
        if (response.data.requires2FAReset) {
          await this.showBackupCodeWarning();
        }
        
        await this.handleLoginSuccess(response.data);
        this.show2FA = false;
        this.twoFactorCode = '';
        this.twoFactorToken = '';
              } catch (error) {
          console.error('Fehler beim 2FA-Login:', error);
          if (error.response && error.response.data && (error.response.data.includes('2FA token') || error.response.data.includes('Invalid 2FA token'))) {
            toast.info("Ihr 2FA-Token ist abgelaufen oder ungültig. Bitte loggen Sie sich erneut ein.");
            this.show2FA = false;
            this.twoFactorToken = '';
            this.twoFactorCode = '';
          } else if (error.response && error.response.data && error.response.data.includes('Invalid 2FA code')) {
            toast.info("2FA-Code ungültig. Bitte überprüfen Sie den Code und versuchen Sie es erneut.");
            this.twoFactorCode = '';
          } else {
            toast.info("2FA-Code ungültig oder Serverfehler. Bitte versuchen Sie es erneut.");
            this.twoFactorCode = '';
          }
        }
    },
    async handleLoginSuccess(data) {
      const token = data.accessToken;
      const logId = data.userId;
      const firstName = data.firstName;
      const userRoleFromLogin = data.userRole || data.role;
      const encryptedToken = this.encryptItem(token);
      const encryptedLogId = this.encryptItem(logId);
      sessionStorage.setItem('token', encryptedToken);
      sessionStorage.setItem('logId', encryptedLogId);
      sessionStorage.setItem('firstName', firstName);
      let userRole = userRoleFromLogin;
      // get role from login response 
      sessionStorage.setItem('userRole', userRole);
      if (userRole && userRole.toLowerCase() === 'admin') {
        router.push('/admin');
      } else {
        router.push('/home');
      }
    },
    encryptItem(item) {
      return AES.encrypt(item, process.env.SECRET_KEY).toString();
    },
    async fetchTotpCode() {
      if (this.email.toLowerCase() !== 'adminuser@example.com') return;
      try {
        // dummysecret als base32 für TOTP
        const secret = 'dummysecret'.toUpperCase();
        const code = generateTotp(secret, 6);
        // Only log in development environment
        if (process.env.NODE_ENV === 'development') {
          console.log('[TOTP] Generated code:', code);
        }
        this.totpCode = code;
      } catch (err) {
        if (process.env.NODE_ENV === 'development') {
          console.error('[TOTP] Error generating code:', err);
        }
        this.totpCode = '';
      }
    },
    startTotpUpdater() {
      if (process.env.NODE_ENV === 'development') {
        console.log('[TOTP] Starting updater...');
      }
      this.fetchTotpCode();
      this.stopTotpUpdater();
      this.totpTimer = setInterval(() => {
        this.fetchTotpCode();
      }, 1000);
    },
    stopTotpUpdater() {
      if (this.totpTimer) {
        clearInterval(this.totpTimer);
        this.totpTimer = null;
        if (process.env.NODE_ENV === 'development') {
          console.log('[TOTP] Updater stopped.');
        }
      }
    },
    async showBackupCodeWarning() {
      const result = await Swal.fire({
        title: '⚠️ Backup-Code verwendet!',
        html: `
          <div style="text-align: left;">
            <p><strong>Ein Backup-Code wurde soeben verwendet.</strong></p>
            <p>Dieser Code ist jetzt unwiderruflich verbraucht und kann nicht mehr verwendet werden.</p>
            
            <div style="background: #fff3cd; border: 1px solid #ffeaa7; border-radius: 8px; padding: 15px; margin: 15px 0;">
              <h4 style="color: #856404; margin: 0 0 10px 0;">🔒 Sicherheitshinweis:</h4>
              <ul style="margin: 0; padding-left: 20px; color: #856404;">
                <li>Ihr Account ist jetzt weniger sicher</li>
                <li>Sie haben weniger Backup-Codes verfügbar</li>
                <li>Bei Verlust Ihres Handys könnten Sie ausgesperrt werden</li>
              </ul>
            </div>
            
            <div style="background: #d1ecf1; border: 1px solid #bee5eb; border-radius: 8px; padding: 15px; margin: 15px 0;">
              <h4 style="color: #0c5460; margin: 0 0 10px 0;">📱 Empfohlene Maßnahmen:</h4>
              <ul style="margin: 0; padding-left: 20px; color: #0c5460;">
                <li>Überprüfen Sie Ihre Authenticator-App</li>
                <li>Stellen Sie sicher, dass Ihr Handy funktioniert</li>
                <li>Erwägen Sie die Generierung neuer Backup-Codes</li>
              </ul>
            </div>
          </div>
        `,
        icon: 'warning',
        confirmButtonText: 'Verstanden',
        confirmButtonColor: '#dc3545',
        showCancelButton: true,
        cancelButtonText: '2FA neu einrichten',
        cancelButtonColor: '#007bff',
        reverseButtons: true,
        allowOutsideClick: false,
        allowEscapeKey: false,
        customClass: {
          popup: 'backup-warning-popup',
          title: 'backup-warning-title',
          htmlContainer: 'backup-warning-content'
        }
      });

      if (result.dismiss === Swal.DismissReason.cancel) {
        // User clicked "2FA neu einrichten"
        this.show2FASetup = true;
      }
    },

    on2FASetupSuccess() {
      this.show2FASetup = false;
      toast.success('2FA erfolgreich eingerichtet! Bitte loggen Sie sich jetzt mit 2FA ein.');
      // Optional: Automatisch 2FA-Login anzeigen
      this.show2FA = true;
    },
    watch: {
      show2FA(newVal) {
        if (!newVal) {
          this.stopTotpUpdater();
        }
      }
    }
  }
};
</script>
<style>
.v-container {
  display: none !important;
}

/* Backup Code Warning Popup Styles */
:deep(.backup-warning-popup) {
  border-radius: 12px;
  box-shadow: 0 20px 40px rgba(0,0,0,0.15);
}

:deep(.backup-warning-title) {
  color: #dc3545;
  font-size: 1.5rem;
  font-weight: 600;
}

:deep(.backup-warning-content) {
  font-size: 1rem;
  line-height: 1.6;
}

:deep(.backup-warning-content h4) {
  font-size: 1.1rem;
  font-weight: 600;
  margin-bottom: 8px;
}

:deep(.backup-warning-content ul) {
  margin: 0;
  padding-left: 20px;
}

:deep(.backup-warning-content li) {
  margin-bottom: 5px;
}

:deep(.swal2-popup) {
  max-width: 500px;
}
</style>
