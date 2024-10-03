<template>
  <Navbar />
  <div class="create-offer-wrapper bg-ltgrey">
    <div class="inner_banner_layout">
      <!-- Banner Section -->
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <div class="inner_banner">
              <h2>Erstelle Angebot</h2>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="bg-ltgrey pt-40 pb-40">
      <form @submit.prevent="createOffer">
        <div class="container">
          <div class="row">
            <div class="col-sm-6">
              <div class="card general_info_box">
                <div class="general_infoContent">
                  <div class="card-title">
                    <h5>Allgemeine Informationen</h5>
                  </div>
                  <div class="card-body">
                    <div class="form-group">
                      <label>Titel für Angebot <b style="color: red;">*</b></label>
                      <input v-model="offer.title" type="text" class="form-control"
                        placeholder="Offer 1 for Urlaub Gegen Hand" />
                      <small v-if="!validations.title" style="color: red;">Title is required</small>
                    </div>
                    <div class="form-group">
                      <label>Beschreibung</label>
                      <textarea v-model="offer.description" class="form-control desc-textarea"
                        placeholder="Detailed description about the offer"></textarea>
                    </div>
                  </div>
                </div>
              </div>
              <div class="card general_info_box">
                <div class="general_infoContent">
                  <div class="card-title">
                    <h5>Address Information</h5>
                  </div>
                  <div class="card-body address-search-selection">
                    <div class="Address-form">
                      <div class="form-group">
                        <label>Land</label>
                        <select id="country" v-model="countryId" @change="loadStates" class="form-control"
                          :disabled="addressFieldsDisabled">
                          <option v-for="c in countries" :key="c.country_ID" :value="c.country_ID">{{ c.countryName }}
                          </option>
                        </select>
                        <small v-if="!validations.addressFields" style="color: black;">Either address or location must
                          be
                          provided</small>
                      </div>
                      <div class="form-group">
                        <label>Region</label>
                        <select id="state" v-model="stateId" @change="loadCities" class="form-control"
                          :disabled="addressFieldsDisabled">
                          <option v-for="s in states" :key="s.id" :value="s.id">{{ s.name }}</option>
                        </select>
                      </div>
                      <div class="form-group">
                        <label>Stadt</label>
                        <select id="city" v-model="cityId" @change="updateCityName" class="form-control"
                          :disabled="addressFieldsDisabled">
                          <option v-for="c in cities" :key="c.id" :value="c.id">{{ c.name }}</option>
                        </select>
                      </div>
                    </div>
                    <span class="or-divider"><span>or</span></span>
                    <div class="form-group">
                      <label>Ort</label>
                      <input v-model="offer.location" type="text" class="form-control"
                        placeholder="Geben Sie Ihren Standort ein" :disabled="locationFieldDisabled"
                        @input="handleLocationChange" />
                      <small v-if="!validations.location" style="color: black;">Location is required if address is not
                        provided</small>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-sm-6">
              <div class="card general_info_box">
                <div class="general_infoContent">
                  <div class="card-title">
                    <h5>Additional Information</h5>
                  </div>
                  <div class="card-body">
                    <div class="form-group">
                      <label>Fertigkeiten <b style="color: red;">*</b></label>
                      <multiselect v-model="offer.skills" :options="skills" placeholder="Select Fertigkeiten"
                        label="skillDescrition" track-by="skill_ID" multiple></multiselect>
                      <small v-if="!validations.skills" style="color: red;">At least one skill is required</small>
                    </div>
                    <div class="amenities-wrapper">
                      <div class="form-group">
                        <label>
                          Angebotene Annehmlichkeiten
                        </label>
                        <div class="contact_infoBox">
                          <ul>
                            <li v-for="accommodation in accommodations" :key="accommodation.id">
                              <div class="flexBox gap-x-2 image-withCheckbox">
                                <label class="checkbox_container">{{ accommodation.nameAccommodationType }}
                                  <input type="checkbox" :value="accommodation.nameAccommodationType"
                                    v-model="offer.accommodation">
                                  <span class="checkmark"></span>
                                </label>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                      <div class="form-group">
                        <label>
                          Passend für
                        </label>
                        <div class="contact_infoBox">
                          <ul>
                            <li v-for="suitable in suitableAccommodations" :key="suitable.id">
                              <div class="flexBox gap-x-2 image-withCheckbox">
                                <label class="checkbox_container">{{ suitable.name }}
                                  <input type="checkbox" :value="suitable.name" v-model="offer.accommodationSuitable">
                                  <span class="checkmark"></span>
                                </label>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                    <div class="form-group">
                      <label>Bild hochladen <b style="color: red;">*</b></label>
                      <input ref="imageInput" type="file" accept="image/x-png,image/gif,image/jpeg" multiple
                        @change="onFileChange" class="form-control" />
                      <small v-if="!validations.image" style="color: red;">Image is required</small>
                      <small v-if="validations.tooManyImages" style="color: red;">You can upload up to 6 images
                        only.</small>
                    </div>


                  </div>
                </div>
              </div>
              <div class="text-right">
                <button type="submit" class="btn themeBtn">Erstelle Angebot&nbsp;<i
                    class="ri-add-circle-line"></i></button>
              </div>
            </div>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import router from '@/router';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.css';
import CryptoJS from 'crypto-js';
import {
  createToastInterface
} from "vue-toastification";
import "vue-toastification/dist/index.css";
import Navbar from '@/components/navbar/Navbar.vue';
import Securitybot from '@/services/SecurityBot';
import axiosInstance from '@/interceptor/interceptor';
let globalLogid = '';
export default {
  components: {
    Multiselect,
    Navbar,
  },
  data() {
    return {
      addressFieldsDisabled: false,
      locationFieldDisabled: false,
      activeStep: 0,
      steps: [{
        label: "Allgemeine Informationen"
      },
      {
        label: "Address Information"
      },
      {
        label: "Additional Information"
      }
      ],
      toast: null,
      offer: {
        title: '',
        description: '',
        location: '',
        accommodation: [],
        accommodationSuitable: [],
        skills: [],
        user_Id: globalLogid,
        image: null,
        city: '',
        state: '',
        country: '',
        activeTab: 'gen_info',
      },
      validations: {
        title: true,
        country: true,
        state: true,
        city: true,
        skills: true,
        image: true,
      },
      skills: [],
      accommodations: [],
      suitableAccommodations: [],
      countries: [],
      states: [],
      cities: [],
      countryId: '',
      stateId: '',
      cityId: '',
      countryName: '',
      stateName: '',
      cityName: '',
    };
  },
  created() {
    this.toast = createToastInterface({
      position: "top-right",
      timeout: 3000,
      closeOnClick: true,
      pauseOnFocusLoss: true,
      pauseOnHover: true,
      draggable: true,
      draggablePercent: 0.6,
      showCloseButtonOnHover: false,
      hideProgressBar: false,
      closeButton: "button",
      icon: true,
      rtl: false
    });
  },
  mounted() {
    Securitybot();
    this.fetchSkills();
    this.fetchAccommodations();
    this.fetchSuitableAccommodations();
    this.loadCountries();
  },
  watch: {
    offer: {
      handler() {
        this.handleLocationChange();
      },
      deep: true
    },
    countryId() {
      this.handleAddressChange();
    },
    stateId() {
      this.handleAddressChange();
    },
    cityId() {
      this.handleAddressChange();
    }
  },
  methods: {
    nextStep() {
      if (this.validateStep()) {
        this.activeStep += 1;
      }
    },
    goToStep(index) {
      if (this.validateStep() || index < this.activeStep) {
        this.activeStep = index;
      }
    },
    prevStep() {
      this.activeStep -= 1;
    },
    handleLocationChange() {
      if (this.offer.location.trim() !== '') {
        this.addressFieldsDisabled = true;
        this.validations.addressFields = true;
        this.validations.location = true;
      } else {
        this.addressFieldsDisabled = false;
      }
    },
    handleAddressChange() {
      if (this.countryId && this.stateId && this.cityId) {
        this.locationFieldDisabled = true;
        this.validations.location = true;
      } else {
        this.locationFieldDisabled = false;
      }
    },
    resetAddressAndLocation() {
      // Reset both address and location states
      this.addressFieldsDisabled = false;
      this.locationFieldDisabled = false;
    },
    validateStep() {
      // Reset validations
      this.resetValidations();
      if (this.activeStep === 0) {
        // Step 1 validation
        if (!this.offer.title) {
          this.validations.title = false;
          return false;
        }
      } else if (this.activeStep === 1) {
        // Step 2 validation
        const isLocationFilled = this.offer.location.trim() !== '';
        const isAddressFilled = this.countryId && this.stateId && this.cityId;
        if (!isLocationFilled && !isAddressFilled) {
          this.validations.address = false;
          return false;
        }
        if (!isLocationFilled) {
          this.validations.location = false;
        }
        if (!isAddressFilled) {
          this.validations.addressFields = false;
        }
      } else if (this.activeStep === 2) {
        // Step 3 validation
        if (this.offer.skills.length === 0) {
          this.validations.skills = false;
          return false;
        }
        if (!this.offer.image) {
          this.validations.image = false;
          return false;
        }
      }
      return true;
    },
    resetValidations() {
      this.validations = {
        title: true,
        country: true,
        state: true,
        city: true,
        skills: true,
        image: true,
      };
    },
    // Method to decrypt user ID from sessionStorage
    decryptlogID(encryptedItem) {
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY);
        const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
        return decryptedString;
      } catch (e) {
        return null;
      }
    },
    // Method to fetch accommodations data
    async fetchAccommodations() {
      try {
        const decryptlogid = this.decryptlogID(sessionStorage.getItem("logId"));
        globalLogid = decryptlogid;
        const response = await axiosInstance.get(`${process.env.baseURL}accommodation/get-all-accommodations`);
        this.accommodations = response.data;
      } catch (error) {
        // console.error('Error fetching accommodations:', error);
      }
    },
    // Method to fetch suitable accommodations data
    async fetchSuitableAccommodations() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}accommodation-suitability/get-all-suitable-accommodations`);
        this.suitableAccommodations = response.data;
      } catch (error) {
        //  console.error('Error fetching suitable accommodations:', error);
      }
    },
    // Method to fetch skills data
    async fetchSkills() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}skills/get-all-skills`);
        this.skills = response.data;
      } catch (error) {
        // console.error('Error fetching skills:', error);
      }
    },
    loadCountries() {
      axiosInstance.get(`${process.env.baseURL}region/get-all-countries`)
        .then(response => {
          this.countries = response.data;
        })
        .catch(error => {
          //   console.error('Error loading countries:', error);
        });
    },
    loadStates() {
      if (this.countryId) {
        this.countryName = this.countries.find(c => c.country_ID === this.countryId).countryName;
        axiosInstance.get(`${process.env.baseURL}region/get-state-by-countryId/${this.countryId}`)
          .then(response => {
            this.states = response.data;
            this.stateId = '';
            this.stateName = '';
            this.cityId = '';
            this.cityName = '';
            this.cities = [];
          })
          .catch(error => {
            //  console.error('Error loading states:', error);
          });
      } else {
        this.countryName = '';
        this.states = [];
        this.stateId = '';
        this.stateName = '';
        this.cityId = '';
        this.cityName = '';
        this.cities = [];
      }
    },
    loadCities() {
      if (this.stateId) {
        this.stateName = this.states.find(s => s.id === this.stateId).name;
        axiosInstance.get(`${process.env.baseURL}region/get-city-by-stateId/${this.stateId}`)
          .then(response => {
            this.cities = response.data;
            this.cityId = '';
            this.cityName = '';
          })
          .catch(error => {
            //   console.error('Error loading cities:', error);
          });
      } else {
        this.stateName = '';
        this.cities = [];
        this.cityId = '';
        this.cityName = '';
      }
    },
    updateCityName() {
      if (this.cityId) {
        this.cityName = this.cities.find(c => c.id === this.cityId).name;
      } else {
        this.cityName = '';
      }
    },
    async createOffer() {
      // Method to create a new offer
      if (this.offer.image && this.offer.image.size > 3.5 * 1024 * 1024) {
        this.toast.warning("Image size cannot be greater than 3.5 MB.");
        return;
      }
      if (!this.offer.title || !this.offer.skills.length || !this.offer.image) {
        this.toast.info("Please fill all the required fields marked with *");
        return;
      }
      if (!this.offer.location && !this.cityName) {
        this.toast.info("Please select land, region, and stadt or You can enter Ort.");
        return;
      }
      const offerData = new FormData();
      offerData.append('title', this.offer.title);
      offerData.append('description', this.offer.description);
      offerData.append('location', this.offer.location);
      offerData.append('contact', this.offer.contact);
      offerData.append('accommodation', this.offer.accommodation.join(', '));
      offerData.append('accommodationSuitable', this.offer.accommodationSuitable.join(', '));
      offerData.append('skills', this.offer.skills.map(skill => skill.skillDescrition).join(', '));
      offerData.append('user_Id', globalLogid);
      offerData.append('country', this.countryName);
      offerData.append('state', this.stateName);
      offerData.append('city', this.cityName);
      if (this.offer.image) {
        offerData.append('image', this.offer.image);
      }
      try {
        const response = await axiosInstance.post(
          `${process.env.baseURL}offer/add-new-offer`,
          offerData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        }
        ).then(() => {
          this.toast.success("Your offer has been created!");
          router.push('/my-offers');
        });
        this.resetForm();
      } catch (error) {
        this.toast.info("Unable To Create Offer!");
        this.resetForm();
      }
    },
    decryptEmail(encryptedToken) {
      // Method to decrypt email
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedToken, process.env.SECRET_KEY);
        return bytes.toString(CryptoJS.enc.Utf8);
      } catch (e) {
        // console.error('Error decrypting Email:', e);
        return null;
      }
    },
    resetForm() {
      this.offer.title = '';
      this.offer.description = '';
      this.offer.location = '';
      this.offer.accommodation = [];
      this.offer.accommodationSuitable = [];
      this.offer.skills = [];
      this.offer.image = null;
      this.countryId = '';
      this.stateId = '';
      this.cityId = '';
      this.countryName = '';
      this.stateName = '';
      this.cityName = '';
      this.offer.image = null;
      if (this.$refs.imageInput) {
        this.$refs.imageInput.value = '';
      }
    },
    onFileChange(event) {
      // Method to handle file change (image selection)
      this.offer.image = event.target.files[0]; // Updating selected image
    }
  }
};
</script>
<style scoped>
/* Zoom effect */
.zoom-enter-active,
.zoom-leave-active {
  transition: transform 0.5s ease, opacity 0.5s ease;
}

.zoom-enter,
.zoom-leave-to {
  transform: scale(0.8);
  opacity: 0;
}

.zoom-leave,
.zoom-enter-to {
  transform: scale(1);
  opacity: 1;
}

.desc-textarea {
  height: 90px;
}
</style>
