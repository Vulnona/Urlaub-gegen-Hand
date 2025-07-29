<template>
  <div v-if="userRole !== 'Admin'">
    <Errorpage />
  </div>
  <div v-else>
    <Navbar />
    <div class="inner_banner_layout">
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <div class="inner_banner">
              <h2>Coupons</h2>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="section_space offers_request_layout admin-panel-grid">
      <div class="offers_request_content">
        <div class="card">
          <div class="card-header">
            <div class="d-flex flex-column align-items-center">
              <h1 class="main-title mb-3">Liste der Coupons</h1>
              <div class="admin-actions mb-3">
                <button class="btn btn-primary me-2 admin-action-btn" @click="showBulkEmailModal">
                  <i class="ri-mail-line"></i> Massen-E-Mail
                </button>
                <button class="btn btn-success me-2 admin-action-btn" @click="showNewCouponModal">
                  <i class="ri-add-line"></i> Neuer Coupon
                </button>
                <button class="btn btn-info admin-action-btn" @click="generateCouponCode">
                  <i class="ri-coupon-line"></i> Code generieren
                </button>
              </div>

            </div>
          </div>
          <div class="card-body">
            <!-- Add a responsive wrapper -->
            <div class="table-responsive">
              <table class="table theme_table">
                <thead>
                  <tr>
                    <th class="text-center">Coupon Code</th>
                    <th>Name</th>
                    <th>Ersteller des Coupons</th>
                    <th>Erstellungsdatum</th>
                    <th>Gültigkeit</th>
                    <th class="text-center">Email Status</th> 
                    <th class="text-center">Eingelöst</th>
                    <th class="text-center">Status</th>
                    <th class="text-center">Aktionen</th>
                  </tr>
                </thead>
                  <tbody>
                    <tr v-if="coupons.length === 0">
                      <td colspan="9" class="text-center">
                        <div class="empty-state">
                          <i class="ri-coupon-line" style="font-size: 3rem; color: #ccc; margin-bottom: 1rem;"></i>
                          <h4>Keine Coupons vorhanden</h4>
                          <p>Es wurden noch keine Coupons erstellt.</p>
                        </div>
                      </td>
                    </tr>
                    <tr v-for="(coupon, index) in coupons" :key="index" v-else>
                      <td class="text-center">
                        <span class="codeReveal" v-if="revealedCodes.includes(index)">
                          <b>{{ coupon.code }}</b>
                          <button @click="toggleReveal(index)" class="bg_ltred"><i class="ri-close-line"></i></button>
                        </span>
                        <span v-else>
                          <button @click="toggleReveal(index, coupon.code)" class="bg_ltgreen">Klicken zum Anzeigen</button>
                        </span>
                      </td>

                      <td>{{ coupon.name }}</td>
                      <td>{{ coupon.createdBy }}</td>
                      <td>{{ formatDate(coupon.createdDate) }}</td>
                     <td v-if="coupon.duration">
                     {{ Math.round(coupon.duration / 365) }} Jahr<span v-if="Math.round(coupon.duration / 365) !== 1">e</span></td>
                      <td class="text-center">
                        <span v-if="coupon.isEmailSent" class="emailState badge badge-info">
                          Sent {{ formatEmailDate(coupon.emailSentDate) }}
                        </span>
                        <span v-else class="emailState badge badge-warning">Not Sent</span>
                      </td>
                      <td class="text-center">{{ coupon.redeemedBy }}<span v-if="coupon.redeemedBy == ''">N/A</span></td>
                      <td class="text-center">
                        <span v-if="coupon.isRedeemed" class="newState badge badge-primary">Redeemed</span>
                        <span v-if="!coupon.isRedeemed" class="newState badge badge-success">Available</span>
                      </td>
                      <td class="text-center">
                        <div class="action-buttons">
                          <button 
                            v-if="!coupon.isEmailSent && !coupon.isRedeemed" 
                            @click="sendCouponEmail(coupon)" 
                            class="btn btn-sm btn-outline-primary me-1"
                            title="E-Mail senden"
                          >
                            <i class="ri-mail-line"></i>
                          </button>
                          <button 
                            @click="copyCouponCode(coupon.code)" 
                            class="btn btn-sm btn-outline-secondary me-1"
                            title="Code kopieren"
                          >
                            <i class="ri-file-copy-line"></i>
                          </button>
                          <button 
                            v-if="!coupon.isRedeemed" 
                            @click="console.log('Coupon object:', coupon); console.log('Coupon.id:', coupon.id); console.log('Coupon.Id:', coupon.Id); console.log('All coupon properties:', Object.keys(coupon)); deleteCoupon(coupon.id || coupon.Id)" 
                            class="btn btn-sm btn-outline-danger"
                            title="Coupon löschen"
                          >
                            <i class="ri-delete-bin-line"></i>
                          </button>
                        </div>
                      </td>
                    </tr>
                  </tbody>
                
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Pagination Section -->
    <div class="pagination">
      <button class="action-link" @click="changePage(currentPage - 1)" :hidden="currentPage === 1">
        <i class="ri-arrow-left-s-line"></i>Vorherige
      </button>
      <span>Page {{ currentPage }} of {{ totalPages }}</span>
      <button class="action-link" @click="changePage(currentPage + 1)" :hidden="currentPage === totalPages">
        Nächste<i class="ri-arrow-right-s-line"></i>
      </button>
    </div>

    <!-- Bulk Email Modal -->
    <div class="modal-container" v-if="showBulkEmail">
      <div class="modal-overlay" @click="closeBulkEmailModal"></div>
      <div class="modal-content">
        <span class="close" @click="closeBulkEmailModal">&times;</span>
        <h4>Bulk E-Mail - Coupons versenden</h4>
        <div class="bulk-email-tabs">
          <button 
            :class="['tab-btn', { active: bulkEmailTab === 'upload' }]" 
            @click="bulkEmailTab = 'upload'"
          >
            <i class="ri-upload-line"></i> E-Mail Liste hochladen
          </button>
          <button 
            :class="['tab-btn', { active: bulkEmailTab === 'manual' }]" 
            @click="bulkEmailTab = 'manual'"
          >
            <i class="ri-edit-line"></i> Manuell eingeben
          </button>
        </div>

        <!-- Upload Tab -->
        <div v-if="bulkEmailTab === 'upload'" class="tab-content">
          <div class="form-group">
            <label>CSV/Text Datei mit E-Mail Adressen:</label>
            <input 
              type="file" 
              @change="handleFileUpload" 
              accept=".csv,.txt"
              class="form-control"
            />
            <small class="form-text text-muted">
              Eine E-Mail-Adresse pro Zeile. Nur verifizierte Benutzer erhalten Coupons.
            </small>
          </div>
        </div>

        <!-- Manual Tab -->
        <div v-if="bulkEmailTab === 'manual'" class="tab-content">
          <div class="form-group">
            <label>E-Mail Adressen (eine pro Zeile):</label>
            <textarea 
              v-model="manualEmails" 
              rows="8" 
              class="form-control"
              placeholder="user1@example.com&#10;user2@example.com&#10;user3@example.com"
            ></textarea>
            <small class="form-text text-muted">
              Nur verifizierte Benutzer erhalten Coupons!
            </small>
          </div>
        </div>

        <!-- Membership Selection -->
        <div class="form-group">
          <label>Mitgliedschaft auswählen:</label>
          <select v-model="selectedMembershipId" class="form-control" required>
            <option value="">Bitte wählen...</option>
            <option v-for="membership in memberships" :key="membership.id" :value="membership.id">
              {{ membership.name }} ({{ membership.durationDays }} Tage)
            </option>
          </select>
        </div>

        <div class="modal-buttons">
          <button 
            class="btn themeBtn common-btn" 
            @click="sendBulkEmails" 
            :disabled="isSendingBulkEmails || !selectedMembershipId"
          >
            <span v-if="isSendingBulkEmails">
              <i class="ri-loader-4-line"></i> Sende...
            </span>
            <span v-else>
              <i class="ri-mail-send-line"></i> Coupons versenden
            </span>
          </button>
          <button class="btn-cancel common-btn" @click="closeBulkEmailModal" :disabled="isSendingBulkEmails">
            Abbrechen
          </button>
        </div>

        <!-- Results -->
        <div v-if="bulkEmailResults.length > 0" class="bulk-results">
          <h5>Ergebnisse:</h5>
          <div class="results-summary">
            <span class="badge badge-success">{{ bulkEmailResults.filter(r => r.success).length }} Erfolgreich</span>
            <span class="badge badge-warning">{{ bulkEmailResults.filter(r => !r.success).length }} Fehlgeschlagen</span>
          </div>
          <div class="results-list">
            <div v-for="result in bulkEmailResults" :key="result.email" :class="['result-item', { success: result.success, error: !result.success }]">
              <i :class="result.success ? 'ri-check-line' : 'ri-close-line'"></i>
              {{ result.email }}: {{ result.message }}
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- New Coupon Modal -->
    <div class="modal-container" v-if="showNewCoupon">
      <div class="modal-overlay" @click="closeNewCouponModal"></div>
      <div class="modal-content">
        <span class="close" @click="closeNewCouponModal">&times;</span>
        <h4>Neuen Coupon erstellen</h4>
        
        <div class="form-group">
          <label>E-Mail Adresse des Empfängers:</label>
          <input 
            v-model="newCouponEmail" 
            type="email" 
            class="form-control"
            placeholder="empfaenger@example.com"
            required
          />
          <small class="form-text text-muted">
            Der Benutzer muss bereits registriert und verifiziert sein.
          </small>
        </div>

        <div class="form-group">
          <label>Mitgliedschaft auswählen:</label>
          <select v-model="newCouponMembershipId" class="form-control" required>
            <option value="">Bitte wählen...</option>
            <option v-for="membership in memberships" :key="membership.id" :value="membership.id">
              {{ membership.name }} ({{ membership.durationDays }} Tage)
            </option>
          </select>
        </div>

        <div class="form-group">
          <label>Coupon Name (optional):</label>
          <input 
            v-model="newCouponName" 
            type="text" 
            class="form-control"
            placeholder="z.B. Willkommens-Coupon"
          />
        </div>

        <div class="modal-buttons">
          <button 
            class="btn themeBtn common-btn" 
            @click="createAndSendCoupon" 
            :disabled="isCreatingCoupon || !newCouponEmail || !newCouponMembershipId"
          >
            <span v-if="isCreatingCoupon">
              <i class="ri-loader-4-line"></i> Erstelle...
            </span>
            <span v-else>
              <i class="ri-add-line"></i> Coupon erstellen & senden
            </span>
          </button>
          <button class="btn-cancel common-btn" @click="closeNewCouponModal" :disabled="isCreatingCoupon">
            Abbrechen
          </button>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import Swal from "sweetalert2";
import router from '@/router';
import 'bootstrap/dist/css/bootstrap.min.css';
import "bootstrap/dist/js/bootstrap.min.js";
import axiosInstance from "@/interceptor/interceptor"
import Navbar from "@/components/navbar/Navbar.vue";
import Securitybot from "@/services/SecurityBot";
import {GetUserRole} from "@/services/GetUserPrivileges";
import Errorpage from "../Errorpage.vue";
import toast from "@/components/toaster/toast";
import dayjs from "dayjs";

export default {
  components: {
    Navbar,
    Errorpage
  },
  name: "admin",
  data() {
    return {
      currentIndex: 0,
      currentPage: 1,
      totalPages: 1,
      pageSize: 10,
      coupons: [],
      userRole: GetUserRole(),
      userdata: null,
      revealedCodes: [],
      showBulkEmail: false,
      bulkEmailTab: 'upload',
      manualEmails: '',
      selectedMembershipId: '',
      isSendingBulkEmails: false,
      bulkEmailResults: [],
      showNewCoupon: false,
      newCouponEmail: '',
      newCouponMembershipId: '',
      newCouponName: '',
      isCreatingCoupon: false,
      memberships: [], // Added for new coupon modal

    };
  },
  mounted() {
    this.getdata();
    Securitybot();
    this.getMemberships(); // Fetch memberships on mount
  },
  methods: {
    changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.currentPage = newPage;
        this.getdata();
      }
    },



    formatDate(date) {
      return dayjs(date).format("MMMM D, YYYY h:mm A"); // Example: January 21, 2025 1:23 PM
    },
    formatEmailDate(date) {
      if (!date) return '';
      return dayjs(date).format("MMM D, HH:mm"); // Example: Jan 21, 13:45
    },
    copyToClipboard(text) {
  if (navigator.clipboard && navigator.clipboard.writeText) {
    navigator.clipboard.writeText(text)
      .then(() => {
        toast.success("Coupon-Code in die Zwischenablage kopiert!");
      })
      .catch(err => {
        toast.error("Fehler beim Kopieren des Coupon-Codes.");
        console.error("Error copying text: ", err);
      });
  } else {
    // Fallback for insecure contexts
    const textarea = document.createElement("textarea");
    textarea.value = text;
    textarea.style.position = "fixed"; // Prevent scrolling to bottom
    document.body.appendChild(textarea);
    textarea.select();
    try {
      document.execCommand("copy");
      toast.success("Coupon code copied to clipboard!");
    } catch (err) {
      toast.error("Failed to copy coupon code.");
      console.error("Fallback copy failed: ", err);
    }
    document.body.removeChild(textarea);
  }
},

    toggleReveal(index, code) {
      const codeIndex = this.revealedCodes.indexOf(index);
      if (codeIndex === -1) {
        this.revealedCodes.push(index); // Reveal the code
        this.copyToClipboard(code);
      } else {
        this.revealedCodes.splice(codeIndex, 1); // Hide the code
      }
    },
    // Method to fetch data from the server
    async getdata() {
      try {
        console.log('=== DEBUG: getdata called ===');
        console.log('Request URL:', `coupon/get-all-coupon`);
        console.log('Request params:', {
          pageSize: this.pageSize,
          pageNumber: this.currentPage
        });
        console.log('Axios baseURL:', axiosInstance.defaults.baseURL);
        console.log('Full URL:', `${axiosInstance.defaults.baseURL}coupon/get-all-coupon`);
        
        const res = await axiosInstance.get(`coupon/get-all-coupon`, {
          params: {
            pageSize: this.pageSize,
            pageNumber: this.currentPage
          }
        }
        );
        
        console.log('Response received:', res);
        
        // Handle empty response gracefully
        if (res.data && res.data.items) {
          this.coupons = res.data.items;
          this.totalPages = Math.ceil((res.data.totalCount || 0) / this.pageSize);
        } else {
          this.coupons = [];
          this.totalPages = 1;
        }
      } catch (error) {
        console.error('=== DEBUG: getdata error ===', error);
        console.error('Error config:', error.config);
        console.error('Error response:', error.response);
        // Set default values on error
        this.coupons = [];
        this.totalPages = 1;
        this.handleAxiosError(error);
      }
    },
    // Method to navigate to the home page
    goHome() {
      router.push('/home');
    },

    // Method to handle Axios errors
    handleAxiosError(error) {
      if (error.response) {
        if (error.response.status === 401) {
          toast.info("Die Session ist abgelaufen. Erneuter Login notwendig.")
            .then(() => {
              sessionStorage.clear();
              router.push('/');
            });
        } else if (error.response.status === 404) {
          console.error('Admin/Coupons 404 error:', error);
          toast.error("API-Endpunkt nicht gefunden. Bitte kontaktieren Sie den Administrator.");
        } else {
          console.error('Admin/Coupons error:', error);
          toast.error(`Fehler beim Laden der Daten: ${error.response.status}`);
        }
      } else if (error.request) {
        toast.info("Ein Netzwerkfehler ist aufgetreten. Bitte überprüfen Sie Ihre Verbindung und versuchen Sie es erneut.")
          .then(() => {
            router.push('/');
          });
      } else {
        //  toast.success("An error occurred");
          console.error('Admin/Coupons error:', error);
      }
    },

    // Bulk Email Modal Methods
    showBulkEmailModal() {
      this.bulkEmailTab = 'upload';
      this.manualEmails = '';
      this.selectedMembershipId = '';
      this.bulkEmailResults = [];
      this.showBulkEmail = true;
    },

    closeBulkEmailModal() {
      this.showBulkEmail = false;
    },

    handleFileUpload(event) {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          this.manualEmails = e.target.result;
        };
        reader.readAsText(file);
      }
    },



    isValidEmail(email) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return emailRegex.test(email);
    },

    // New Coupon Modal Methods
    showNewCouponModal() {
      this.newCouponEmail = '';
      this.newCouponMembershipId = '';
      this.newCouponName = '';
      this.showNewCoupon = true;
    },

    closeNewCouponModal() {
      this.showNewCoupon = false;
    },

    // Bulk Email Modal Methods
    showBulkEmailModal() {
      this.bulkEmailTab = 'upload';
      this.manualEmails = '';
      this.selectedMembershipId = '';
      this.bulkEmailResults = [];
      this.showBulkEmail = true;
    },

    closeBulkEmailModal() {
      this.showBulkEmail = false;
    },

    handleFileUpload(event) {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          this.manualEmails = e.target.result;
        };
        reader.readAsText(file);
      }
    },

    async sendBulkEmails() {
      if (!this.selectedMembershipId) {
        toast.error("Bitte wählen Sie eine Mitgliedschaft aus.");
        return;
      }

      const emails = this.manualEmails.split('\n')
        .map(email => email.trim())
        .filter(email => email && this.isValidEmail(email));

      if (emails.length === 0) {
        toast.error("Keine gültigen E-Mail-Adressen gefunden.");
        return;
      }

      this.isSendingBulkEmails = true;
      this.bulkEmailResults = [];

      for (const email of emails) {
        try {
          // First create coupon for this user
          const createResponse = await axiosInstance.post('coupon/create-and-send', {
            email: email,
            membershipId: parseInt(this.selectedMembershipId),
            name: `Bulk Coupon - ${new Date().toLocaleDateString()}`
          });

          if (createResponse.data.isSuccess) {
            this.bulkEmailResults.push({
              email: email,
              success: true,
              message: 'Coupon erstellt und E-Mail gesendet'
            });
          } else {
            this.bulkEmailResults.push({
              email: email,
              success: false,
              message: createResponse.data.error || 'Unbekannter Fehler'
            });
          }
        } catch (error) {
          let errorMessage = 'Unbekannter Fehler';
          if (error.response?.data?.error) {
            errorMessage = error.response.data.error;
          } else if (error.response?.status === 404) {
            errorMessage = 'Benutzer nicht gefunden oder nicht verifiziert';
          } else if (error.response?.status === 400) {
            errorMessage = 'Ungültige Anfrage';
          }
          
          this.bulkEmailResults.push({
            email: email,
            success: false,
            message: errorMessage
          });
        }
      }

      this.isSendingBulkEmails = false;
      
      const successCount = this.bulkEmailResults.filter(r => r.success).length;
      const errorCount = this.bulkEmailResults.filter(r => !r.success).length;
      
      toast.success(`Bulk-Versand abgeschlossen: ${successCount} erfolgreich, ${errorCount} fehlgeschlagen`);
      
      // Refresh coupon list to show new coupons
      await this.getdata();
    },





    // Generate Coupon Code (moved from Admin.vue)
    async generateCouponCode() {
      console.log('=== DEBUG: SweetAlert2 called ===');
      console.log('this.memberships:', this.memberships);
      console.log('this.memberships.length:', this.memberships.length);
      
      const { value: membershipId } = await Swal.fire({
        title: 'Mitgliedschaft auswählen',
        input: 'select',
        inputOptions: this.memberships.reduce((options, membership) => {
          console.log('Membership:', membership);
          console.log('membership.membershipID:', membership.membershipID);
          console.log('membership.name:', membership.name);
          console.log('membership.durationDays:', membership.durationDays);
          
          const key = membership.membershipID;
          const value = `${membership.name} (${membership.durationDays} Tage)`;
          options[key] = value;
          console.log(`Added option: ${key} = ${value}`);
          
          return options;
        }, {}),
        inputPlaceholder: 'Mitgliedschaft auswählen',
        showCancelButton: true,
        confirmButtonText: 'Generieren',
        cancelButtonText: 'Abbrechen',
        inputValidator: (value) => {
          if (!value) {
            return 'Sie müssen eine Mitgliedschaft auswählen!';
          }
        }
      });
      
      console.log('=== DEBUG: SweetAlert2 result ===');
      console.log('membershipId from SweetAlert2:', membershipId);
      console.log('membershipId type:', typeof membershipId);

      if (membershipId) {
        try {
          console.log('=== DEBUG: generateCouponCode called ===');
          console.log('membershipId:', membershipId);
          console.log('membershipId type:', typeof membershipId);
          console.log('membershipId value:', membershipId);
          
          // Ensure membershipId is a valid number
          const membershipIdNum = parseInt(membershipId);
          if (isNaN(membershipIdNum)) {
            toast.error("Ungültige Mitgliedschaft-ID");
            return;
          }
          
          console.log('Request data:', { membershipId: membershipIdNum });
          
          const res = await axiosInstance.post(
            `add-coupon`, 
            { membershipId: membershipIdNum },
            {
              headers: {
                'Content-Type': 'application/json'
              }
            }
          );
          
          console.log('Response:', res);
          
          if (res.data.isSuccess) {
            const couponCode = res.data.value;
            
            // Show the generated code in a nice modal
            await Swal.fire({
              title: 'Coupon-Code generiert!',
              html: `
                <div style="text-align: center;">
                  <p><strong>Ihr Coupon-Code:</strong></p>
                  <div style="background: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0; font-family: monospace; font-size: 18px; font-weight: bold; color: #007bff;">
                    ${couponCode}
                  </div>
                  <p style="font-size: 14px; color: #666;">Klicken Sie auf "Kopieren" um den Code in die Zwischenablage zu kopieren.</p>
                </div>
              `,
              icon: 'success',
              showCancelButton: true,
              confirmButtonText: 'Kopieren',
              cancelButtonText: 'Schließen',
              reverseButtons: true
            }).then((result) => {
              if (result.isConfirmed) {
                // Copy to clipboard
                navigator.clipboard.writeText(couponCode).then(() => {
                  toast.success("Coupon-Code in die Zwischenablage kopiert!");
                }).catch(() => {
                  toast.error("Kopieren fehlgeschlagen");
                });
              }
            });
            
            // Refresh the coupon list to show the new coupon
            await this.getdata();
          } else {
            toast.error("Fehler beim Generieren des Coupon-Codes");
          }
        } catch (error) {
          console.error('=== DEBUG: generateCouponCode error ===', error);
          console.error('Error config:', error.config);
          console.error('Error response:', error.response);
          toast.error("Fehler beim Generieren des Coupon-Codes");
        }
      }
    },


    async createAndSendCoupon() {
      if (!this.newCouponEmail || !this.newCouponMembershipId) {
        toast.error("Bitte füllen Sie alle Felder aus.");
        return;
      }
      this.isCreatingCoupon = true;
      try {
        console.log('=== DEBUG: createAndSendCoupon called ===');
        console.log('Request data:', {
          email: this.newCouponEmail,
          membershipId: this.newCouponMembershipId,
          name: this.newCouponName || undefined
        });
        
        const res = await axiosInstance.post(`coupon/create-and-send`, {
          email: this.newCouponEmail,
          membershipId: this.newCouponMembershipId,
          name: this.newCouponName || undefined
        });
        
        console.log('Response:', res);
        toast.success("Neuer Coupon erstellt und an Benutzer gesendet!");
        this.closeNewCouponModal();
        this.getdata(); // Refresh list to show new coupon
      } catch (error) {
        this.handleAxiosError(error);
      } finally {
        this.isCreatingCoupon = false;
      }
    },

    // Individual Coupon Email Methods
    async sendCouponEmail(coupon) {
      try {
        console.log('=== DEBUG: sendCouponEmail called ===');
        console.log('Coupon object:', coupon);
        console.log('Coupon ID:', coupon.id);
        
        const res = await axiosInstance.post(`coupon/send-existing`, {
          couponId: coupon.id
        });
        
        console.log('Response:', res);
        
        if (res.data.isSuccess) {
          toast.success("E-Mail erfolgreich gesendet!");
          await this.getdata(); // Refresh list to show updated email status
        } else {
          toast.error(res.data.error || "Fehler beim Senden der E-Mail");
        }
      } catch (error) {
        console.error('=== DEBUG: sendCouponEmail error ===', error);
        console.error('Error response:', error.response);
        
        let errorMessage = "Fehler beim Senden der E-Mail";
        if (error.response?.data?.error) {
          errorMessage = error.response.data.error;
        } else if (error.response?.data?.message) {
          errorMessage = error.response.data.message;
        } else if (error.message) {
          errorMessage = error.message;
        }
        
        toast.error(errorMessage);
      }
    },
    copyCouponCode(code) {
      this.copyToClipboard(code);
    },
    async deleteCoupon(couponId) {
      Swal.fire({
        title: 'Bist du sicher?',
        text: "Dieser Coupon wird unwiderruflich gelöscht!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Ja, lösche!',
        cancelButtonText: 'Abbrechen'
      }).then(async (result) => {
        if (result.isConfirmed) {
          try {
            console.log('=== DEBUG: deleteCoupon called ===');
            console.log('couponId:', couponId);
            
            await axiosInstance.delete(`coupon/delete-coupon/${couponId}`);
            console.log('Delete successful');
            toast.success("Coupon erfolgreich gelöscht!");
            this.getdata(); // Refresh list
          } catch (error) {
            console.error('=== DEBUG: deleteCoupon error ===', error);
            console.error('Error config:', error.config);
            console.error('Error response:', error.response);
            this.handleAxiosError(error);
          }
        }
      });
    },

    // Fetch memberships for new coupon modal
    async getMemberships() {
      try {
        const res = await axiosInstance.get(`membership/get-all-memberships`);
        this.memberships = res.data;
      } catch (error) {
        this.handleAxiosError(error);
      }
    }
  }
};
</script>
<style scoped>
.codeReveal {
  display: flex;
  justify-content: space-evenly;
}

.bg_ltgreen {
  padding: 3px;
  padding-left: 5px;
  padding-right: 5px;
  color: #2c8326;
  border-radius: 14px;
  border: solid .2px rgb(25, 134, 25);
}

.bg_ltred {
  padding-right: 3px;
  padding-left: 3px;
  border-radius: 8px;
}

.btn {
  padding: 4px 12px;
  font-size: 14px;
  border: 1px solid;
  border-radius: 5px;

  box-shadow: 0px 0px 2px rgb(0 0 0 / 36%);
}

.btn-danger {
  background: #b0061d;
  border-color: #b0061d;
}

.btn-success {
  background: #4daf4c;
  border-color: #4daf4c;
}

.cross_icon,
.tick_icon {
  position: relative;
  top: 2px;
}

.close {
  position: absolute;
  top: 10px;
  right: 10px;
  font-size: 24px;
  font-weight: bold;
  cursor: pointer;
}

h2 {
  margin-top: 0;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 8px;
}

.empty-state {
  padding: 2rem;
  text-align: center;
}

.empty-state h4 {
  color: #666;
  margin-bottom: 0.5rem;
}

.empty-state p {
  color: #999;
  margin: 0;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.btn-primary {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-primary:hover {
  background-color: #0056b3;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-secondary:hover {
  background-color: #817d7d;
}

.clickable {
  cursor: pointer;
  color: rgba(var(--bs-link-color-rgb));
  transition: color 0.3s ease;
}

.clickable:hover {
  text-decoration: underline;
}

.page_404 {
  padding: 40px 0;
  background: #fff;
  font-family: 'Arvo', serif;
}

.page_404 img {
  width: 100%;
}

.four_zero_four_bg {
  background-image: url(https://cdn.dribbble.com/users/285475/screenshots/2083086/dribbble_1.gif);
  height: 400px;
  background-position: center;
}

.four_zero_four_bg h1 {
  font-size: 80px;
}

.four_zero_four_bg h3 {
  font-size: 80px;
}

.link_404 {
  color: #fff !important;
  padding: 10px 20px;
  background: #39ac31;
  margin: 20px 0;
  display: inline-block;
}

.contant_box_404 {
  margin-top: -50px;
}

.table-responsive {
  width: 100%;
  overflow-x: auto;
  /* Allow horizontal scrolling on small screens */
  -webkit-overflow-scrolling: touch;
  /* Smooth scrolling for mobile devices */
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.theme_table th,
.theme_table td {
  text-align: left;
  padding: 8px;
  border: 1px solid #ddd;
  white-space: nowrap;
  /* Prevent text wrapping in table cells */
}

.theme_table th {
  background-color: #f8f9fa;
}

.action-buttons {
  display: flex;
  gap: 5px;
}

.bulk-email-tabs {
  display: flex;
  margin-bottom: 20px;
  border-bottom: 1px solid #eee;
  padding-bottom: 10px;
}

.bulk-email-tabs .tab-btn {
  padding: 8px 15px;
  border: none;
  border-bottom: 2px solid transparent;
  cursor: pointer;
  font-weight: bold;
  transition: border-bottom-color 0.3s ease;
  background-color: #f0f0f0;
  color: #333;
  margin-right: 10px;
}

.bulk-email-tabs .tab-btn.active {
  border-bottom-color: #007bff;
  color: #007bff;
}

.bulk-email-tabs .tab-btn:hover {
  background-color: #e0e0e0;
}

.tab-content {
  margin-top: 20px;
}

.bulk-results {
  margin-top: 20px;
  padding: 15px;
  background-color: #f9f9f9;
  border: 1px solid #eee;
  border-radius: 5px;
}

.bulk-results h5 {
  margin-top: 0;
  margin-bottom: 10px;
}

.results-summary {
  margin-bottom: 10px;
}

.results-list .result-item {
  margin-bottom: 5px;
  padding: 8px;
  border-radius: 4px;
  font-size: 0.9em;
}

.results-list .result-item.success {
  background-color: #e8f5e9;
  color: #2e7d32;
  border: 1px solid #a5d6a7;
}

.results-list .result-item.error {
  background-color: #ffebee;
  color: #c62828;
  border: 1px solid #ef9a9a;
}

.modal-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: -1;
}

.modal-content {
  background-color: #fff;
  padding: 25px;
  border-radius: 8px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
  position: relative;
  width: 90%;
  max-width: 600px;
  max-height: 90vh;
  overflow-y: auto;
  z-index: 1001;
}

.modal-content h4 {
  margin-top: 0;
  margin-bottom: 20px;
  text-align: center;
}

.modal-buttons {
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
}

.btn-cancel {
  background-color: #6c757d;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-cancel:hover {
  background-color: #5a6268;
}

.btn-cancel:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.themeBtn {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.themeBtn:hover {
  background-color: #0056b3;
}

.themeBtn:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.common-btn {
  padding: 10px 20px;
  font-size: 14px;
  border-radius: 4px;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.common-btn:hover {
  opacity: 0.9;
}

.common-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

@media (max-width: 768px) {

  .theme_table th,
  .theme_table td {
    font-size: 14px;
    /* Adjust font size for smaller screens */
  }
}

.admin-action-btn {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 12px 20px;
  font-weight: 500;
  border-radius: 6px;
  transition: all 0.3s ease;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: none;
  font-size: 14px;
  text-transform: none;
  letter-spacing: 0.5px;
}

.admin-action-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
}

.admin-action-btn i {
  font-size: 16px;
}

.admin-action-btn.btn-primary {
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
}

.admin-action-btn.btn-primary:hover {
  background: linear-gradient(135deg, #0056b3, #004085);
}

.admin-action-btn.btn-success {
  background: linear-gradient(135deg, #28a745, #1e7e34);
  color: white;
}

.admin-action-btn.btn-success:hover {
  background: linear-gradient(135deg, #1e7e34, #155724);
}

.admin-action-btn.btn-info {
  background: linear-gradient(135deg, #17a2b8, #138496);
  color: white;
}

.admin-action-btn.btn-info:hover {
  background: linear-gradient(135deg, #138496, #0f6674);
}

/* Responsive Design für Admin Buttons */
@media (max-width: 768px) {
  .admin-actions {
    flex-direction: column;
    gap: 10px;
  }
  
  .admin-action-btn {
    width: 100%;
    justify-content: center;
  }
}


</style>
