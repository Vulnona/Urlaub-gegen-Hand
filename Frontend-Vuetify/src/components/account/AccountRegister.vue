<template>
   <PublicNav />
    <div class="login-page">
    <div class="loginmain-bg">
      <div class="loginmain-form">
        <div class="login-left">
          <div class="login-left-content">
            <div class="login-main-text">
              <h2><span>Willkommen bei Urlaub gegen Hand</span></h2>

    <p>Durch Ihre Registrierung öffnen Sie die Tür zu einzigartigen Reiseerfahrungen und kulturellem Austausch. Bevor Sie Teil unserer Community werden, benötigen wir Ihre Zustimmung:</p>
    <ul style="display: inline-block; text-align: left;">
<li>Wir erfassen einige persönliche Daten, um Ihr Konto einzurichten und Ihnen die besten Möglichkeiten für Ihren Urlaub gegen Hand zu bieten.</li>    
<li>Ihre Daten werden vertraulich behandelt und nur für plattformbezogene Zwecke verwendet.</li>    
<li> Mit der Registrierung stimmen Sie unseren Nutzungsbedingungen und der Datenschutzerklärung zu.</li>
<li> Sie erklären sich bereit, respektvoll mit Gastgebern und anderen Reisenden zu interagieren.</li>
</ul>
<p>
Durch Klicken auf "Registrieren" bestätigen Sie, dass Sie diese Bedingungen akzeptieren.
</p> 
            </div>
            <div class="login-bottom-text">
              Entdecken Sie die Welt, teilen Sie Fähigkeiten, schaffen Sie unvergessliche Erlebnisse!
            </div>
          </div>
        </div>
        <div class="login-right">
          <div class="login-right-content-heading form-act">
            <div class="register-wrapper">
              <div class="login-form-section auth-card auth-register" id="login-content">
                
                <form class="form-border" @submit.prevent="register">

                  <div class="register-form-fields" :class="{ 'register-form-fields-mobile': isMobile }">
                    

                    <div class="custom-form" :class="{ 'has-error': !firstName && showError }">
                      <label for="firstName">Vorname</label>
                      <input type="text" placeholder="Geben Sie Ihren Vornamen ein" id="firstName"
                        v-model="firstName" />
                      <span v-if="!firstName && showError" class="error-message">Vorname ist erforderlich</span>
                    </div>
                    <div class="custom-form" :class="{ 'has-error': !lastName && showError }">
                      <label for="lastName">Nachname</label>
                      <input type="text" placeholder="Geben Sie Ihren Nachnamen ein" id="lastName" v-model="lastName" />
                      <span v-if="!lastName && showError" class="error-message">Nachname ist erforderlich</span>
                    </div>
                    <div class="custom-form" :class="{ 'has-error': !email_Address && showError }">
                      <label for="email_Address">E-Mail-Adresse</label>
                      <input type="email" placeholder="Geben Sie Ihre E-Mail-Adresse ein" id="email"
                        v-model="email_Address" />
                      <span v-if="!email_Address && showError" class="error-message">E-Mail-Adresse ist
                        erforderlich</span>
                    </div>
                    <div class="custom-form" :class="{ 'has-error': !gender && showError }">
                      <label>Geschlecht</label>&nbsp;&nbsp;
                      <select class="form-group p-2" style="width: 100%;" v-model="gender">
                        <option disabled value="">Bitte wählen Sie Ihr Geschlecht</option>
                        <option value="Male">Männlich</option>
                        <option value="Female">Weiblich</option>
                        <option value="Other">Möchte ich nicht bekannt geben</option>
                      </select>
                      <span v-if="!gender && showError" class="error-message">Geschlecht ist
                        erforderlich</span>
                    </div>
                    <div class="custom-form" :class="{ 'has-error': !dateOfBirth && showError }">
                      <label for="dateOfBirth">Geburtsdatum</label>
                      <Datepicker date v-model="dateOfBirth" :enable-time-picker="false"
                        :auto-apply="true" placeholder="dd.mm.yyyy" :typeable="true" input-class-name="datepicker-input" startingView='year'  inputFormat="dd.MM.yyyy"/>
                      <span v-if="!isValidDateOfBirth && showError" class="error-message">
                        Alter muss zwischen 14 und 120 Jahren liegen
                      </span>
                    </div>

                    <!-- Address Selection via Map -->
                    <div class="custom-form address-map-section" :class="{ 'has-error': !selectedAddress && showError }">
                      <label>Adresse</label>
                      <AddressMapPicker 
                        @address-selected="onAddressSelected"
                        :required="true"
                      />
                      <span v-if="!selectedAddress && showError" class="error-message">
                        Bitte wählen Sie eine Adresse aus
                      </span>
                    </div>
                    <div class="custom-form" :class="{ 'has-error': !isValidFacebookLink && showError }">
                      <label for="facebook_link">Facebook-Profillink</label>
                      <input type="text" placeholder="Geben Sie Ihren Facebook-Profillink ein" id="facebook_link"
                        v-model="facebook_link">
                      <span v-if="!isFacebookLinkValid && showError" class="error-message">
                        Ungültiger Facebook-Link. Muss mit https://www.facebook.com/ beginnen
                      </span>
                    </div>
                    <div class="custom-form" :class="{ 'has-error': !password && showError || !isPasswordValid }">
                      <label for="password">Passwort</label>
                      <div class="password-container" style="position: relative;">
                        <input :type="showPassword ? 'text' : 'password'" placeholder="Geben Sie Ihr Passwort ein"
                          id="password" v-model="password" @input="validatePassword">
                        <i @click="togglePasswordVisibility" :class="showPassword ? 'ri-eye-off-fill' : 'ri-eye-fill'"
                          style="position: absolute; right: 10px; top: 10px; cursor: pointer;">
                        </i>
                      </div>
                      <span v-if="(!password && showError) || !isPasswordValid" class="error-message">
                        Passwort muss mindestens einen Großbuchstaben, eine Zahl, ein
                        Sonderzeichen <a style="font-weight: bold;"> !@#$%^&* </a> enthalten und zwischen 8 und 20 Zeichen lang sein.
                      </span>
                    </div>
                    <div class="custom-form" :class="{ 'has-error': !confirmPassword && showError }">
                      <label for="confirmPassword">Passwort bestätigen</label>
                      <div class="password-container" style="position: relative;">
                        <input :type="showPassword ? 'text' : 'password'" placeholder="Bestätigen Sie Ihr Passwort"
                          id="confirmPassword" v-model="confirmPassword">
                        <i @click="toggleConfirmPasswordVisibility"
                          :class="showConfirmPassword ? 'ri-eye-off-fill' : 'ri-eye-fill'"
                          style="position: absolute; right: 10px; top: 10px; cursor: pointer;">
                        </i>
                      </div>
                      <span v-if="!confirmPassword && showError || (password !== confirmPassword && showError)"
                        class="error-message">
                        Passwortbestätigung ist erforderlich und muss mit dem Passwort
                        übereinstimmen.
                      </span>
                    </div>
                  </div>
                  <div class="login-buttons">
                    <button type="submit" class="btn">Registrieren</button>
                  </div>
                  <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import router from '@/router';
import Swal from 'sweetalert2';
import AES from 'crypto-js/aes';
import Datepicker from 'vue3-datepicker';
import axiosInstance from '@/interceptor/interceptor';
import toast from '../toaster/toast';
import AddressMapPicker from '@/components/common/AddressMapPicker.vue';
import PublicNav from '@/components/navbar/PublicNav.vue';
            
export default {
  components: {
    Datepicker,
    PublicNav,
    AddressMapPicker
  },
  data() {
    return {
      showPassword: false,
      showConfirmPassword: false,
      email_Address: '',
      password: '',
      firstName: '',
      lastName: '',
      gender: '',
      dateOfBirth: null,
      selectedAddress: null,
      facebook_link: '',
      confirmPassword: '',
      errorMessage: '',
      link_RS: '',
      link_VS: '',
      showError: false,
      isPasswordValid: true,
      isFacebookLinkValid: true,
    };
  },

  computed: {
    isMobile () {
      return screen.width <= 760;
    },
    isValidFacebookLink() {
      const facebookPattern = /^https:\/\/www\.facebook\.com\/.+$/;
      this.isFacebookLinkValid = facebookPattern.test(this.facebook_link);
      return this.isFacebookLinkValid;
    },
    isValidDateOfBirth() {
      if (!this.dateOfBirth) return false;
      const today = new Date();
      const birthDate = new Date(this.dateOfBirth);
      let age = today.getFullYear() - birthDate.getFullYear();
      const monthDifference = today.getMonth() - birthDate.getMonth();
      const dayDifference = today.getDate() - birthDate.getDate();

      if (monthDifference < 0 || (monthDifference === 0 && dayDifference < 0)) {
        age--;
      }

      return age >= 14 && age <= 120;
    }
  },
  methods: {
    onAddressSelected(address) {
      this.selectedAddress = address;
      if (address) {
        this.showError = false;
      }
    },
    
    togglePasswordVisibility() {
      this.showPassword = !this.showPassword;
    },
    toggleConfirmPasswordVisibility() {
      this.showPassword = !this.showPassword;
    },
    validatePassword() {
      const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,20}$/;
      this.isPasswordValid = passwordRegex.test(this.password);
    },
    // Method to handle the registration process
    register() {
      this.showError = true;

      if (!this.isPasswordValid) {
        this.errorMessage = "Passwort erfüllt nicht die Anforderungen.";
        return;
      }

      // Check if passwords match
      if (this.password !== this.confirmPassword) {
        this.errorMessage = "Passwörter stimmen nicht überein.";
        return;
      }
      if(this.facebook_link!= ''){
        if (!this.isValidFacebookLink) {
          this.errorMessage = "Ungültiger Facebook-Link. Bitte geben Sie einen gültigen Link ein.";
          return;
        }
      }
      // Check if Facebook link is valid

      // Check if date of birth is valid
      if (!this.isValidDateOfBirth) {
        this.errorMessage = "Das Geburtsdatum ist ungültig. Das Mindestalter beträgt 14 Jahre.";
        return;
      }

      // Check if address is selected
      if (!this.selectedAddress) {
        this.errorMessage = "Bitte wählen Sie eine Adresse aus.";
        return;
      }

      // Show loader
      Swal.fire({
        title: 'Registrieren...',
        text: 'Bitte warten',
        allowOutsideClick: false,
        showConfirmButton: false,
        willOpen: () => {
          Swal.showLoading();
        }
      });

      // Creating registration data object with new address structure
      const registrationData = {
        email_Address: this.email_Address,
        password: this.password,
        firstName: this.firstName,
        lastName: this.lastName,
        gender: this.gender,
        dateOfBirth: this.dateOfBirth,
        // Adressdaten jetzt FLACH auf Root-Level
        latitude: this.selectedAddress.latitude,
        longitude: this.selectedAddress.longitude,
        displayName: this.selectedAddress.displayName,
        houseNumber: this.selectedAddress.houseNumber,
        road: this.selectedAddress.road,
        city: this.selectedAddress.city,
        postcode: this.selectedAddress.postcode,
        country: this.selectedAddress.country,
        countryCode: this.selectedAddress.countryCode,
        facebook_link: this.facebook_link,
        link_RS: this.link_RS,
        link_VS: this.link_VS,
      };

      // Sending a POST request to the registration endpoint with the registration data
      axiosInstance.post(`authenticate/register`, registrationData)
        .then(response => {
          Swal.close();
          toast.success('Registrierung erfolgreich!', 'Überprüfen Sie Ihre E-Mail und klicken Sie auf „Bestätigen“.');

          const reg_Id = response.data.value.userId;
          const encryptedLogId = this.encryptItem(reg_Id);
          sessionStorage.setItem('logId', encryptedLogId);
          router.push('/upload-id');
        })
        .catch(error => {
          Swal.close();
          if (error.response && error.response.data.error.message === "E-Mail Adresse existiert bereits") {
            toast.warning('Benutzer mit dieser E-Mail existiert bereits!', 'Versuchen Sie es mit einer anderen E-Mail-Adresse oder versuchen Sie sich anzumelden');
          } else {
            console.error('Registrierungsfehler:', error);
            this.errorMessage = 'Registrierung fehlgeschlagen. Bitte versuchen Sie es erneut.';
            toast.error('Registrierung fehlgeschlagen!', 'Es ist ein Fehler aufgetreten!');
          }
        });
    },
    // Method to encrypt a given item using AES encryption
    // encryptItem(item) {
    //   return CryptoJS.AES.encrypt(item, process.env.SECRET_KEY).toString();
    // },
    encryptItem(item) {
      return AES.encrypt(item, import.meta.env.VITE_SECRET_KEY || 'thisismytestsecretkey').toString();
    }
  },
  mounted() {
    Swal.fire({
      title: 'Zustimmung erforderlich',
      html: `
         <p>Ihre Privatsphäre ist uns wichtig. Um Ihr Konto zu erstellen, müssen wir einige Ihrer Daten erfassen. Mit dem Fortfahren stimmen Sie zu:    
    <ul style="display: inline-block; text-align: left; padding-left: 50px;">
    <li>Der Erfassung und Nutzung Ihrer Daten für die Kontoerstellung</li>
    <li>Den wesentlichen Funktionen unserer Plattform</li>
    <li>Unseren Nutzungsbedingungen und <a href="https://alreco.de/datenschutz" target="_blank"> Datenschutzrichtlinien</a></li>
    </ul>   
         <input type="checkbox" id="gdpr-consent-checkbox">
         <label for="gdpr-consent-checkbox">Ich akzeptiere</label>`,
      showCancelButton: true,
      confirmButtonText: 'akzeptieren',
      cancelButtonText: 'abbrechen',
      allowOutsideClick: false,
      customClass: {
        popup: 'custom-consent-modal',
      },
      didOpen: () => {
        const confirmButton = Swal.getConfirmButton();
        confirmButton.disabled = true;
        confirmButton.classList.add('disabled-confirm-button');
        const checkbox = document.getElementById('gdpr-consent-checkbox');
        checkbox.addEventListener('change', (event) => {
          confirmButton.disabled = !event.target.checked;
          if (confirmButton.disabled) {
            confirmButton.classList.add('disabled-confirm-button');
          } else {
            confirmButton.classList.remove('disabled-confirm-button');
          }
        });
      }
    }).then((result) => {
      if (result.isConfirmed) {
      } else {
        //   console.log('User did not consent to cookies. Redirecting...');
        router.push({ name: 'Login' });
      }
    });
  }
}
</script>

<style>
.v-container {
  display: none !important;
}
</style>
