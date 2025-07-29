<template>
  <Navbar />
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2>Edit Profile</h2>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="container mt-4">
    <div class="col-md-7 mx-auto">
      <div class="card">
        <form @submit.prevent="saveProfile" class="profile-form">
          <div class="row">
            <div class="col-md-4 col-sm-6">
              <!-- First Name -->
              <div class="form-group">
                <label for="firstName">Vorname</label>
                <input type="text" v-model="profile.firstName" class="form-control" id="firstName" />
                <span v-if="errors.firstName" class="text-danger">{{ errors.firstName }}</span>
              </div>
            </div>
            <div class="col-md-4 col-sm-6">
              <!-- Last Name -->
              <div class="form-group">
                <label for="lastName">Nachname</label>
                <input type="text" v-model="profile.lastName" class="form-control" id="lastName" />
                <span v-if="errors.lastName" class="text-danger">{{ errors.lastName }}</span>
              </div>
            </div>
            <div class="col-md-4 col-sm-6">
              <div class="form-group">
                <label for="gender">Geschlecht</label>
                <select v-model="profile.gender" class="form-control" id="gender">
                  <option value="">Geschlecht auswählen</option>
                  <option value="Male">Männlich</option>
                  <option value="Female">Weiblich</option>
                  <option value="Other">Möchte ich nicht bekannt geben</option>
                </select>
                <span v-if="errors.gender" class="text-danger">{{ errors.gender }}</span>
              </div>
            </div>
            <div class="col-md-4 col-sm-6">
              <!-- Date of Birth -->
              <div class="form-group">
                <label for="dateOfBirth">Geburtsdatum</label>
                <input type="date" v-model="profile.dateOfBirth" class="form-control" id="dateOfBirth"
                  @change="validateDateOfBirth" />
                <span v-if="errors.dateOfBirth" class="text-danger">{{ errors.dateOfBirth }}</span>
              </div>
            </div>
            
            <!-- Address Section with Map -->
            <div class="col-md-12">
              <div class="form-group address-section">
                <label>Adresse</label>
                <AddressMapPicker 
                  @address-selected="onAddressSelected"
                  :initial-address="profile.address"
                  :required="true"
                />
                <span v-if="errors.address" class="text-danger">{{ errors.address }}</span>
              </div>
            </div>
            <div v-if="profile.facebookLink" class="col-md-8 col-sm-12">
              <!-- Facebook Link -->
              <div class="form-group">
                <label for="facebookLink">Facebook-Profillink</label>
                <input type="url" v-model="profile.facebookLink" class="form-control" id="facebookLink"
                  @blur="validateFacebookLink" disabled/>
                <span v-if="errors.facebookLink" class="text-danger">{{ errors.facebookLink }}</span>
              </div>
            </div>
          </div>

          <div class="row">
            <div class="col-sm-12">
              <div class="profile_btn">
                <!-- Submit Button -->
                <button type="button" @click="back()" class="btn btn-back rounded">Zurück</button>
                <button type="submit" class="btn-primary-ugh">Profil speichern</button>
              </div>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import Navbar from '@/components/navbar/Navbar.vue';
import Securitybot from '@/services/SecurityBot';
import axiosInstance from '@/interceptor/interceptor';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.css';
import toast from '@/components/toaster/toast';
import Swal from 'sweetalert2';
import AddressMapPicker from '@/components/common/AddressMapPicker.vue';
export default {
  components: {
    Navbar,
    Multiselect,
    AddressMapPicker,
  },
  data() {
    return {
      profile: {
        firstName: "",
        lastName: "",
        gender: "",
        dateOfBirth: "",
        address: null,
        facebookLink: "",
      },
      errors: {},
      formIsValid: true,
    };
  },
  mounted() {
    Securitybot();
    this.fetchUserProfile();
  },
  methods: {
    onAddressSelected(address) {
      this.profile.address = address;
      if (address) {
        this.errors.address = '';
      }
    },
    back() {
      window.history.back();
    },    
    getLocationNames() {
      // This method is no longer needed with the new address structure
      return {};
    },
    async fetchUserProfile() {
      try {
        const response = await axiosInstance.get(`profile/get-user-profile`);
        if (response.data.profile) {
          this.profile = response.data.profile;
        } else {
          toast.info("Benutzerdatenprofil nicht gefunden");
        }
      } catch (error) {
        console.error('Fehler beim Abrufen des Benutzerprofils:', error);
        toast.info('Benutzerdaten konnten nicht abgerufen werden. Bitte versuchen Sie es erneut.');
      }
    },
    saveProfile() {
      if (this.validateForm()) {
        const updatedProfile = {
          ...this.profile,
          latitude: this.profile.address?.latitude,
          longitude: this.profile.address?.longitude,
          displayName: this.profile.address?.displayName,
          id: this.profile.address?.id
        };
        this.updateProfileAPI(updatedProfile);
      } else {
        toast.error("Bitte korrigieren Sie die Fehler im Formular, bevor Sie es absenden.");
      }
    },
async updateProfileAPI(updatedProfile) {
    Swal.fire({
        title: "Bist du sicher?",
        text: "Eine Änderung deiner Userdaten erfordert eine erneute Verifikation durch den Admin. Du kannst deinen Account erst wieder regulär benutzen, wenn diese erfolgt ist.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Ja, möchte ich!",
        }).then( async(result) => {
            if (result.isConfirmed) {
      try {
          const response = await axiosInstance.put(`profile/update-user-data`, updatedProfile);
          if (response.status === 200) {
              toast.success("Profile saved successfully!");
              if (response.data && response.data.profile) {
                  this.profile = response.data.profile;
                  this.profile = {
                      ...response.data.profile
                  };
              }
              this.$router.push('/profile');
          } else {
              toast.error("Fehler beim Speichern des Profils. Bitte versuchen Sie es erneut.");
          }
      } catch (error) {
          console.error('Fehler beim Speichern des Profils:', error);
          toast.error('Profil konnte nicht gespeichert werden. Bitte versuchen Sie es erneut.');
      }
            }
        })
},
    validateForm() {
      this.errors = {};
      this.formIsValid = true;
      
      const requiredFields = ['firstName', 'lastName', 'gender', 'dateOfBirth'];
      
      requiredFields.forEach(field => {
        if (!this.profile[field]) {
          this.errors[field] = `${field.charAt(0).toUpperCase() + field.slice(1)} ist erforderlich`;
          this.formIsValid = false;
        }
      });

      // Address validation
      if (!this.profile.address) {
        this.errors.address = "Adresse ist erforderlich";
        this.formIsValid = false;
      }

      if (!this.validateDateOfBirth()) {
        this.formIsValid = false;
      }      
      
      if (this.profile.facebookLink && !this.validateFacebookLink()) {
        this.formIsValid = false;
      }      
      
      return this.formIsValid;
    },
    validateDateOfBirth() {
      const today = new Date();
      const dob = new Date(this.profile.dateOfBirth);
      let age = today.getFullYear() - dob.getFullYear();
      const monthDiff = today.getMonth() - dob.getMonth();
      if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
        age--;
      }
      if (age < 14 || age > 120) {
        this.errors.dateOfBirth = "Das Alter muss zwischen 14 und 120 liegen.";
        return false;
      }
      return true;
    },
    validateFacebookLink() {
      if(this.profile.facebookLink!=''){

        const facebookRegex = /^https:\/\/www\.facebook\.com(\/.*)?$/;
        if (!facebookRegex.test(this.profile.facebookLink)) {
          this.errors.facebookLink = "Bitte geben Sie einen gültigen Facebook-Link ein (z. B. https://www.facebook.com/name)";
          return false;
        }
      }
      return true;
    },
  },
};
</script>

<style scoped>
.v-container {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
}

.account-page {
  background-color: #f8f9fa;
  padding: 30px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  max-width: 600px;
  width: 100%;
}

.card {
  background-color: #fff;
  border: none;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
}

.card-body {
  padding: 20px;
}

.card-title {
  margin-bottom: 20px;
  font-size: 24px;
  font-weight: bold;
  color: #333;
  text-align: center;
}

.card-text-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.card-text {
  font-size: 16px;
  color: #555;
}

.card-text strong {
  color: #333;
}

.card-text a {
  color: #007bff;
  text-decoration: none;
}

.card-text a:hover {
  text-decoration: underline;
}

.btn-primary {
  color: #fff !important;
}

/* .btn-primary {
  margin-top: 20px;
  display: block;
  width: 100%;
  text-align: center;
} */

.profile-form-inner {
  display: flex;
  flex-wrap: wrap;
  gap: 1%;
}

.profile-form-inner .form-group {
  width: 19.2%;
}

.form-group {
  margin-bottom: 15px;
}

.text-danger {
  color: red;
}

.badge {
  font-size: 0.9em;
  padding: 0.5em 0.7em;
}

.badge .close {
  font-size: 1em;
  line-height: 0.8;
}

.btn.btn-back {
  background: #4e4e4e;
  background-color: #585757;
  color: #fff;
}

.btn.btn-back:hover {
  background: #4e4e4e;
  color: #fff;
}



.badge.badge-primary {
  background: #f0f6fd !important;
  font-size: 11px;
  font-weight: 400;
  color: #000;
  border: 1px solid rgb(0 0 0 / 12%) !important;
  padding: 5px 7px 5px 7px;
  border-radius: 5px;
}
</style>
