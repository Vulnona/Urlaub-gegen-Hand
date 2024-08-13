<template>
  <div class="bg-ltgrey pt-40 pb-40">
    <form @submit.prevent="createOffer">
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <div class="flexBox justify-between align-items-center top_headingBox">
              <h1 class="main-title">Erstelle Angebot</h1>
              <div class="top-rightbtns themeFlexBtn">
                <button type="submit" class="btn themeBtn">Erstelle Angebot</button>
              </div>
            </div>
            <div class="flexBox justify-between createOfferContent">
              <div class="card general_info_box">
                <div class="general_infoContent">
                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5">
                        <i class="ri-information-line fa-blue"></i> Allgemeine Informationen
                      </h5>
                    </div>
                    <div class="form-group">
                      <div class="headingSecondary">
                        <h5 class="d-flex gap-x-2 heading5 align-items-center">
                          <i class="ri-tools-line"></i> Titel für Angebot<b style="color: red;">*</b>
                        </h5>
                      </div>
                      <div class="titleBox">
                        <input v-model="offer.title" type="text" class="form-control"
                          placeholder="Offer 1 for Urlaub Gegen Hand">
                      </div>
                    </div>
                    <div class="form-group">
                      <div class="headingSecondary">
                        <h5 class="d-flex gap-x-2 heading5 align-items-center">
                          <i class="ri-tools-line"></i> Beschreibung
                        </h5>
                      </div>
                      <textarea v-model="offer.description" class="form-control descriptiontextarea"
                        placeholder="Detailed description about the offer"></textarea>
                    </div>
                  </div>

                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5">
                        <i class="ri-information-line fa-blue"></i> Land<b style="color: red;">*</b>
                      </h5>
                    </div>
                    <div class="form-group">
                      <select id="country" v-model="countryId" @change="loadStates" class="form-control">
                        <option v-for="c in countries" :key="c.country_ID" :value="c.country_ID">
                          {{ c.countryName }}
                        </option>
                      </select>
                    </div>
                  </div>
                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5">
                        <i class="ri-information-line fa-blue"></i> Region<b style="color: red;">*</b>
                      </h5>
                    </div>
                    <div class="form-group">
                      <select id="state" v-model="stateId" @change="loadCities" class="form-control">
                        <option v-for="s in states" :key="s.id" :value="s.id">
                          {{ s.name }}
                        </option>
                      </select>
                    </div>
                  </div>
                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5">
                        <i class="ri-information-line fa-blue"></i> Stadt<b style="color: red;">*</b>
                      </h5>
                    </div>
                    <div class="form-group">
                      <select id="city" v-model="cityId" @change="updateCityName" class="form-control">
                        <option v-for="c in cities" :key="c.id" :value="c.id">
                          {{ c.name }}
                        </option>
                      </select>
                    </div>
                  </div>
                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5">
                        <i class="ri-map-pin-2-line"></i> Ort
                      </h5>
                    </div>
                    <div class="form-group">
                      <input v-model="offer.location" type="text" class="form-control"
                        placeholder="Geben Sie Ihren Standort ein">
                    </div>
                  </div>
                </div>
              </div>
              <div class="card general_info_box rightSide">
                <div class="general_infoContent">
                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5 align-items-center">
                        <i class="ri-tools-line"></i> Fertigkeiten<b style="color: red;">*</b>
                      </h5>
                    </div>
                    <div class="form-group">
                      <multiselect v-model="offer.skills" :options="skills" placeholder="Select Fertigkeiten"
                        label="skillDescrition" track-by="skill_ID" multiple></multiselect>
                    </div>
                  </div>
                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5 align-items-center">
                        <i class="ri-building-line"></i> Angebotene Annehmlichkeiten
                      </h5>
                    </div>
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
                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5 align-items-center">
                        <i class="ri-building-line"></i> Passend für
                      </h5>
                    </div>
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
                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5 align-items-center">
                        <i class="ri-image-line"></i> Bild hochladen<b style="color: red;">*</b>
                      </h5>
                    </div>
                    <div class="form-group uploadimgBox">
                      <input type="file" accept="image/x-png,image/gif,image/jpeg" @change="onFileChange"
                        class="form-control" />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>
<script>
import router from '@/router'; 
import axios from 'axios'; 
import Swal from 'sweetalert2'; 
import Multiselect from 'vue-multiselect'; 
import 'vue-multiselect/dist/vue-multiselect.css';
import CryptoJS from 'crypto-js'; 
import { createToastInterface } from "vue-toastification";
import "vue-toastification/dist/index.css";

let globalLogid = '';
export default {
  components: {
    Multiselect // Registering Multiselect component
  },
  data() {
    return {
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
    this.Securitybot(); 
    this.fetchSkills(); 
    this.fetchAccommodations(); 
    this.fetchSuitableAccommodations(); 
    this.loadCountries();
  },
  methods: {
    Securitybot() {
      // Method to check user authentication
      if (!sessionStorage.getItem("token")) {
        Swal.fire({
          title: 'You are not logged In !',
          text: 'Login First to continue.',
          icon: 'info',
          confirmButtonText: 'OK'
        });
        router.push('/login'); 
      }
    },
    decryptlogID(encryptedItem) {
      // Method to decrypt user ID from sessionStorage
      try {
        const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY);
        const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
        return parseInt(decryptedString, 10).toString();
      } catch (e) {
        console.error('Error decrypting item:', e);
        return null;
      }
    },
    async fetchAccommodations() {
      // Method to fetch accommodations data
      try {
        const decryptlogid = this.decryptlogID(sessionStorage.getItem("logId"));
        globalLogid = decryptlogid; 
        const response = await axios.get(`${process.env.baseURL}accommodation/get-all-accommodations`); 
        this.accommodations = response.data;
      } catch (error) {
        console.error('Error fetching accommodations:', error);
      }
    },
    async fetchSuitableAccommodations() {
      // Method to fetch suitable accommodations data
      try {
        const response = await axios.get(`${process.env.baseURL}accommodation-suitability/get-all-suitable-accommodations`); 
        this.suitableAccommodations = response.data;
      } catch (error) {
        console.error('Error fetching suitable accommodations:', error);
      }
    },
    async fetchSkills() {
      // Method to fetch skills data
      try {
        const response = await axios.get(`${process.env.baseURL}skills/get-all-skills`); 
        this.skills = response.data;
      } catch (error) {
        console.error('Error fetching skills:', error);
      }
    },
    loadCountries() {
      axios.get(`${process.env.baseURL}region/get-all-countries`)
        .then(response => {
          this.countries = response.data;
        })
        .catch(error => {
          console.error('Error loading countries:', error);
        });
    },
    loadStates() {
      if (this.countryId) {
        this.countryName = this.countries.find(c => c.country_ID === this.countryId).countryName;
        axios.get(`${process.env.baseURL}region/get-state-by-countryId/${this.countryId}`)
          .then(response => {
            this.states = response.data;
            this.stateId = '';
            this.stateName = ''; 
            this.cityId = ''; 
            this.cityName = '';
            this.cities = [];
          })
          .catch(error => {
            console.error('Error loading states:', error);
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
        axios.get(`${process.env.baseURL}region/get-city-by-stateId/${this.stateId}`)
          .then(response => {
            this.cities = response.data;
            this.cityId = '';
            this.cityName = ''; 
          })
          .catch(error => {
            console.error('Error loading cities:', error);
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
      if (!this.countryName || !this.stateName || !this.cityName) {
        this.toast.info("Please select country, state, and city.");
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
        const response = await axios.post(
          `${process.env.baseURL}offer/add-new-offer`, 
          offerData,
          {
            headers: {
              'Content-Type': 'multipart/form-data'
            }
          }
        ).then(() => {
          this.toast.success("Your offer has been created.");
          router.push('/home');
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
        console.error('Error decrypting Email:', e);
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
    },
    onFileChange(event) {
      // Method to handle file change (image selection)
      this.offer.image = event.target.files[0]; // Updating selected image
    }
  }
};
</script>
