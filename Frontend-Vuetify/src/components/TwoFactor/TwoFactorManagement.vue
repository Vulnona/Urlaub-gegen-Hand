<template>
  <div class="two-factor-management">
    <div class="management-container">
      <div class="section-header">
        <h3>Zwei-Faktor-Authentifizierung</h3>
        <p>Erhöhen Sie die Sicherheit Ihres Kontos</p>
      </div>

      <!-- Current Status -->
      <div class="status-section">
        <div class="status-card" :class="{ 'enabled': is2FAEnabled, 'disabled': !is2FAEnabled }">
          <div class="status-icon">
            <i :class="is2FAEnabled ? 'ri-shield-check-fill' : 'ri-shield-line'"></i>
          </div>
          <div class="status-content">
            <h4>{{ is2FAEnabled ? '2FA aktiviert' : '2FA deaktiviert' }}</h4>
            <p>{{ statusDescription }}</p>
            <div v-if="is2FAEnabled && backupCodesRemaining !== null" class="backup-status">
              <small>{{ backupCodesRemaining }} Backup-Codes verbleibend</small>
            </div>
          </div>
        </div>
      </div>

      <!-- Actions -->
      <div class="actions-section">
        <!-- Enable 2FA -->
        <div v-if="!is2FAEnabled" class="action-card">
          <h4>2FA aktivieren</h4>
          <p>Schützen Sie Ihr Konto mit einer zusätzlichen Sicherheitsschicht</p>
          <button @click="startSetup" class="btn btn-primary" :disabled="loading">
            <i class="ri-add-line"></i>
            2FA einrichten
          </button>
        </div>

        <!-- Manage 2FA -->
        <div v-else class="action-cards">
          <div class="action-card">
            <h4>Backup-Codes anzeigen</h4>
            <p>Zeigen Sie Ihre verbleibenden Backup-Codes an</p>
            <button @click="showBackupCodes" class="btn btn-secondary">
              <i class="ri-file-list-line"></i>
              Codes anzeigen
            </button>
          </div>

          <div class="action-card">
            <h4>Neue Backup-Codes generieren</h4>
            <p>Erstellen Sie neue Backup-Codes (alte werden ungültig)</p>
            <button @click="regenerateBackupCodes" class="btn btn-secondary" :disabled="loading">
              <i class="ri-refresh-line"></i>
              Codes erneuern
            </button>
          </div>

          <div class="action-card danger">
            <h4>2FA deaktivieren</h4>
            <p>Deaktivieren Sie die Zwei-Faktor-Authentifizierung</p>
            <button @click="showDisableModal" class="btn btn-danger">
              <i class="ri-close-line"></i>
              2FA deaktivieren
            </button>
          </div>
        </div>
      </div>

      <!-- Setup Modal -->
      <div v-if="showSetup" class="modal-overlay" @click="closeSetup">
        <div class="modal-content" @click.stop>
          <div class="modal-header">
            <h3>2FA einrichten</h3>
            <button @click="closeSetup" class="close-btn">
              <i class="ri-close-line"></i>
            </button>
          </div>
          <div class="modal-body">
            <TwoFactorSetup @setup-complete="onSetupComplete" />
          </div>
        </div>
      </div>

      <!-- Backup Codes Modal -->
      <div v-if="showBackupModal" class="modal-overlay" @click="closeBackupModal">
        <div class="modal-content" @click.stop>
          <div class="modal-header">
            <h3>Backup-Codes</h3>
            <button @click="closeBackupModal" class="close-btn">
              <i class="ri-close-line"></i>
            </button>
          </div>
          <div class="modal-body">
            <div class="backup-codes-display">
              <p class="warning-text">
                <i class="ri-warning-line"></i>
                Diese Codes können nur einmal verwendet werden. Bewahren Sie sie sicher auf.
              </p>
              <div class="codes-grid">
                <div v-for="(code, index) in displayedBackupCodes" :key="index" class="backup-code">
                  {{ code }}
                </div>
              </div>
              <div class="backup-actions">
                <button @click="copyBackupCodes" class="btn btn-secondary">
                  <i class="ri-file-copy-line"></i> Codes kopieren
                </button>
                <button @click="downloadBackupCodes" class="btn btn-secondary">
                  <i class="ri-download-line"></i> Codes herunterladen
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Disable 2FA Modal -->
      <div v-if="showDisableConfirm" class="modal-overlay" @click="closeDisableModal">
        <div class="modal-content" @click.stop>
          <div class="modal-header">
            <h3>2FA deaktivieren</h3>
            <button @click="closeDisableModal" class="close-btn">
              <i class="ri-close-line"></i>
            </button>
          </div>
          <div class="modal-body">
            <div class="disable-form">
              <p class="warning-text">
                <i class="ri-warning-line"></i>
                <strong>Warnung:</strong> Das Deaktivieren der 2FA verringert die Sicherheit Ihres Kontos erheblich.
              </p>
              <div class="form-group">
                <label for="disablePassword">Passwort zur Bestätigung eingeben:</label>
                <input 
                  id="disablePassword"
                  type="password" 
                  v-model="disablePassword" 
                  placeholder="Ihr aktuelles Passwort"
                  class="form-control"
                  @keyup.enter="disable2FA"
                />
              </div>
              <div class="form-actions">
                <button @click="disable2FA" class="btn btn-danger" :disabled="!disablePassword || disabling">
                  <span v-if="disabling">Deaktiviere...</span>
                  <span v-else>2FA deaktivieren</span>
                </button>
                <button @click="closeDisableModal" class="btn btn-secondary">
                  Abbrechen
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Loading State -->
      <div v-if="loading" class="loading-overlay">
        <div class="loading-spinner">
          <i class="ri-loader-4-line"></i>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import TwoFactorSetup from './TwoFactorSetup.vue'
import axiosInstance from '@/interceptor/interceptor'
import toast from '@/components/toaster/toast'

export default {
  name: 'TwoFactorManagement',
  components: {
    TwoFactorSetup
  },
  data() {
    return {
      is2FAEnabled: false,
      backupCodesRemaining: null,
      loading: false,
      disabling: false,
      showSetup: false,
      showBackupModal: false,
      showDisableConfirm: false,
      displayedBackupCodes: [],
      disablePassword: ''
    }
  },
  computed: {
    statusDescription() {
      if (this.is2FAEnabled) {
        return 'Ihr Konto ist durch Zwei-Faktor-Authentifizierung geschützt'
      } else {
        return 'Ihr Konto ist nur durch ein Passwort geschützt'
      }
    }
  },
  async mounted() {
    await this.loadStatus()
  },
  methods: {
    async loadStatus() {
      try {
        const response = await axiosInstance.get(`authenticate/2fa-status`)
        this.is2FAEnabled = response.data.isEnabled
        this.backupCodesRemaining = response.data.backupCodesRemaining
      } catch (error) {
        console.error('Error loading 2FA status:', error)
        toast.error('Fehler beim Laden des 2FA-Status')
      }
    },

    startSetup() {
      this.showSetup = true
    },

    closeSetup() {
      this.showSetup = false
    },

    async onSetupComplete() {
      this.closeSetup()
      await this.loadStatus()
      toast.success('2FA wurde erfolgreich aktiviert!')
    },

    async showBackupCodes() {
      this.loading = true
      try {
        const response = await axiosInstance.get(`authenticate/backup-codes`)
        this.displayedBackupCodes = response.data.codes
        this.showBackupModal = true
      } catch (error) {
        toast.error('Fehler beim Laden der Backup-Codes')
      } finally {
        this.loading = false
      }
    },

    closeBackupModal() {
      this.showBackupModal = false
      this.displayedBackupCodes = []
    },

    async regenerateBackupCodes() {
      this.loading = true
      try {
        const response = await axiosInstance.post(`authenticate/regenerate-backup-codes`)
        this.displayedBackupCodes = response.data.codes
        this.showBackupModal = true
        await this.loadStatus()
        toast.success('Neue Backup-Codes wurden generiert')
      } catch (error) {
        toast.error('Fehler beim Generieren neuer Backup-Codes')
      } finally {
        this.loading = false
      }
    },

    showDisableModal() {
      this.showDisableConfirm = true
      this.disablePassword = ''
    },

    closeDisableModal() {
      this.showDisableConfirm = false
      this.disablePassword = ''
    },

    async disable2FA() {
      if (!this.disablePassword) return

      this.disabling = true
      try {
        await axiosInstance.post(`authenticate/disable-2fa`, {
          password: this.disablePassword
        })
        
        this.closeDisableModal()
        await this.loadStatus()
        toast.success('2FA wurde deaktiviert')
      } catch (error) {
        if (error.response?.status === 401) {
          toast.error('Falsches Passwort')
        } else {
          toast.error('Fehler beim Deaktivieren der 2FA')
        }
      } finally {
        this.disabling = false
      }
    },

    copyBackupCodes() {
      const codesText = this.displayedBackupCodes.join('\n')
      navigator.clipboard.writeText(codesText)
      toast.success('Backup-Codes in die Zwischenablage kopiert!')
    },

    downloadBackupCodes() {
      const codesText = this.displayedBackupCodes.join('\n')
      const blob = new Blob([codesText], { type: 'text/plain' })
      const url = window.URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = 'ugh-backup-codes-new.txt'
      document.body.appendChild(a)
      a.click()
      document.body.removeChild(a)
      window.URL.revokeObjectURL(url)
      toast.success('Backup-Codes heruntergeladen!')
    }
  }
}
</script>

<style scoped>
.two-factor-management {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.management-container {
  background: white;
  border-radius: 8px;
  padding: 30px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.section-header {
  margin-bottom: 30px;
}

.section-header h3 {
  color: #333;
  margin-bottom: 8px;
}

.section-header p {
  color: #666;
  margin: 0;
}

.status-section {
  margin-bottom: 30px;
}

.status-card {
  display: flex;
  align-items: center;
  padding: 20px;
  border-radius: 8px;
  border: 2px solid;
}

.status-card.enabled {
  background: #d4edda;
  border-color: #28a745;
  color: #155724;
}

.status-card.disabled {
  background: #f8d7da;
  border-color: #dc3545;
  color: #721c24;
}

.status-icon {
  font-size: 32px;
  margin-right: 15px;
}

.status-content h4 {
  margin: 0 0 5px 0;
  font-size: 18px;
}

.status-content p {
  margin: 0;
  font-size: 14px;
  opacity: 0.8;
}

.backup-status {
  margin-top: 5px;
}

.backup-status small {
  font-size: 12px;
  opacity: 0.7;
}

.actions-section {
  margin-bottom: 20px;
}

.action-card {
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  margin-bottom: 15px;
}

.action-card.danger {
  border-color: #dc3545;
  background: #fff5f5;
}

.action-card h4 {
  margin: 0 0 8px 0;
  color: #333;
}

.action-card p {
  margin: 0 0 15px 0;
  color: #666;
  font-size: 14px;
}

.action-cards {
  display: grid;
  gap: 15px;
}

.btn {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  transition: background-color 0.3s;
  display: inline-flex;
  align-items: center;
  gap: 8px;
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

.btn-danger {
  background: #dc3545;
  color: white;
}

.btn-danger:hover:not(:disabled) {
  background: #c82333;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  border-radius: 8px;
  max-width: 600px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 20px 0 20px;
  border-bottom: 1px solid #eee;
}

.modal-header h3 {
  margin: 0;
  color: #333;
}

.close-btn {
  background: none;
  border: none;
  font-size: 20px;
  cursor: pointer;
  color: #666;
  padding: 5px;
}

.close-btn:hover {
  color: #333;
}

.modal-body {
  padding: 20px;
}

.backup-codes-display {
  text-align: center;
}

.codes-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 10px;
  margin: 20px 0;
}

.backup-code {
  background: #f8f9fa;
  padding: 12px;
  border: 1px solid #dee2e6;
  border-radius: 4px;
  font-family: monospace;
  font-weight: bold;
  font-size: 14px;
}

.backup-actions {
  display: flex;
  gap: 10px;
  justify-content: center;
}

.disable-form {
  max-width: 400px;
  margin: 0 auto;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  margin-bottom: 8px;
  font-weight: bold;
  color: #555;
}

.form-control {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
}

.form-control:focus {
  outline: none;
  border-color: #007bff;
}

.form-actions {
  display: flex;
  gap: 10px;
  justify-content: center;
}

.warning-text {
  background: #fff3cd;
  border: 1px solid #ffeaa7;
  color: #856404;
  padding: 15px;
  border-radius: 4px;
  margin-bottom: 20px;
  display: flex;
  align-items: flex-start;
  gap: 8px;
}

.loading-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(255, 255, 255, 0.8);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 999;
}

.loading-spinner {
  font-size: 32px;
  color: #007bff;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>
