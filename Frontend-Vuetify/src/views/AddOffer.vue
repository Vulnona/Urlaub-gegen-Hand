<template>
  <div class="bg-ltgrey pt-40 pb-40">
    <form @submit.prevent="createOffer">
      <div class="container">
        <div class="row">
          <div class="col-sm-12">
            <div class="flexBox justify-between align-items-center top_headingBox">
              <h1 class="main-title">Erstelle Angebot</h1>
              <div class="top-rightbtns themeFlexBtn">
                <button type="button" class="btn themeCancelBtn">Cancel</button>
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
                          <i class="ri-tools-line"></i> Title for Offer<b style="color: red;">*</b>
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
                          <i class="ri-tools-line"></i> Description
                        </h5>
                      </div>
                      <textarea v-model="offer.description" class="form-control descriptiontextarea"
                        placeholder="Detailed description about the offer"></textarea>
                    </div>
                  </div>

                  <div>
                    <div class="headingSecondary">
                      <h5 class="d-flex gap-x-2 heading5">
                        <i class="ri-information-line fa-blue"></i> Country<b style="color: red;">*</b>
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
                        <i class="ri-information-line fa-blue"></i> City<b style="color: red;">*</b>
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
                      <input v-model="offer.location" type="text" class="form-control" placeholder="Stadt">
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
import router from '@/router'; // Importing router for navigation
import axios from 'axios'; // Importing axios for HTTP requests
import Swal from 'sweetalert2'; // Importing SweetAlert2 for notifications
import Multiselect from 'vue-multiselect'; // Importing Multiselect component
import 'vue-multiselect/dist/vue-multiselect.css'; // Importing Multiselect CSS
import CryptoJS from 'crypto-js'; // Importing CryptoJS for encryption
import { createToastInterface } from "vue-toastification";
import "vue-toastification/dist/index.css";

let globalLogid = ''; // Variable to store decrypted user ID

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
        accommodation: [], // Array to store selected accommodations
        accommodationSuitable: [], // Array to store selected suitable accommodations
        skills: [], // Array to store selected skills
        user_Id: globalLogid, // User ID fetched after decryption
        // region_ID: null,
        image: null, // Variable to store selected image file
        city: '',
        state: '',
        country: '',
      },
      skills: [], // Array to store fetched skills
      // regions: [], // Array to store fetched regions
      accommodations: [], // Array to store fetched accommodations
      suitableAccommodations: [], // Array to store fetched suitable accommodations
      countries: [],
      states: [],
      cities: [],
      countryId: '', // For selecting the country in the dropdown
      stateId: '', // For selecting the state in the dropdown
      cityId: '', // For selecting the city in the dropdown
      countryName: '', // To store the country name
      stateName: '', // To store the state name
      cityName: '', // To store the city name
    };
  },
  created() {
    this.toast = createToastInterface({
      // You can add options here if needed
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
    this.Securitybot(); // Checking user authentication on component mount
    this.fetchSkills(); // Fetching skills data
    this.fetchRegions(); // Fetching regions data
    this.fetchAccommodations(); // Fetching accommodations data
    this.fetchSuitableAccommodations(); // Fetching suitable accommodations data
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
        router.push('/login'); // Redirecting to login page if not authenticated
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
        const testlogid = this.decryptlogID(sessionStorage.getItem("logId")); // Decrypting user ID
        globalLogid = testlogid; // Storing decrypted user ID globally
        const response = await axios.get(`${process.env.baseURL}accommodation/get-all-accommodation`); // Fetching accommodations from API
        this.accommodations = response.data; // Assigning fetched accommodations to data property
      } catch (error) {
        console.error('Error fetching accommodations:', error);
      }
    },
    async fetchSuitableAccommodations() {
      // Method to fetch suitable accommodations data
      try {
        const response = await axios.get(`${process.env.baseURL}accommodation-suitability/get-all-suitable-accommodations`); // Fetching suitable accommodations from API
        this.suitableAccommodations = response.data; // Assigning fetched suitable accommodations to data property
      } catch (error) {
        console.error('Error fetching suitable accommodations:', error);
      }
    },
    async fetchSkills() {
      // Method to fetch skills data
      try {
        const response = await axios.get(`${process.env.baseURL}get-all-skills`); // Fetching skills from API
        this.skills = response.data; // Assigning fetched skills to data property
      } catch (error) {
        console.error('Error fetching skills:', error);
      }
    },
    async fetchRegions() {
      // Method to fetch regions data
      try {
        const response = await axios.get(`${process.env.baseURL}region/get-all-region`); // Fetching regions from API
        this.regions = response.data; // Assigning fetched regions to data property
      } catch (error) {
        console.error('Error fetching regions:', error);
      }
    },
    loadCountries() {
      axios.get(`${process.env.baseURL}region/get-all-country`)
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
            this.stateId = ''; // Reset state selection
            this.stateName = ''; // Reset state name
            this.cityId = ''; // Reset city selection
            this.cityName = ''; // Reset city name
            this.cities = []; // Clear cities list
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
            this.cityId = ''; // Reset city selection
            this.cityName = ''; // Reset city name
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
        // Checking image size limit
        // Swal.fire(
        //   'File Too Big!',
        //   'Image size cannot be greater than 3.5 MB.',
        //   'warning'
        // );
        this.toast.warning("Image size cannot be greater than 3.5 MB.");
        return;
      }
      if (!this.offer.title || !this.offer.skills.length || !this.offer.image) {
        // Validating required fields
        // Swal.fire(
        //   '',
        //   'Please fill all the required fields marked with *',
        //   'warning'
        // );
        this.toast.info("Please fill all the required fields marked with *");
        return;
      }
      if (!this.countryName || !this.stateName || !this.cityName) {
        this.toast.info("Please select country, state, and city.");
        return;
      }
      const offerData = new FormData(); // Creating FormData object to send form data
      offerData.append('title', this.offer.title); // Appending offer title
      offerData.append('description', this.offer.description); // Appending offer description
      offerData.append('location', this.offer.location); // Appending offer location
      offerData.append('contact', this.offer.contact); // Appending offer contact
      offerData.append('accommodation', this.offer.accommodation.join(', ')); // Appending selected accommodations
      offerData.append('accommodationSuitable', this.offer.accommodationSuitable.join(', ')); // Appending selected suitable accommodations
      offerData.append('skills', this.offer.skills.map(skill => skill.skillDescrition).join(', ')); // Appending selected skills
      offerData.append('user_Id', globalLogid); // Appending decrypted user ID
      // offerData.append('region_ID', this.offer.region_ID); // Appending selected region ID
      offerData.append('country', this.countryName); // Appending selected country
      offerData.append('state', this.stateName);
      offerData.append('city', this.cityName);
      // offerData.append('skill_ID', this.offer.skill_ID); // Appending selected skill ID
     
      if (this.offer.image) {
        offerData.append('image', this.offer.image); // Appending selected image
      }
      
     
      try {
        let email = sessionStorage.getItem('logEmail'); // Fetching encrypted email from sessionStorage
        const decEmail = this.decryptEmail(email); // Decrypting email
        email = decEmail; // Storing decrypted email

        const response = await axios.post(
          `${process.env.baseURL}offer/add-new-offer?email=${email}`, // Sending POST request to create new offer
          offerData,
          {
            headers: {
              'Content-Type': 'multipart/form-data'
            }
          }
        ).then(() => {
          // Swal.fire(
          //   'Success!',
          //   'Your offer has been created.',
          //   'success'
          // );
          this.toast.success("Your offer has been created.");
          router.push('/home'); // Redirecting to home page on success
        });
        this.resetForm(); // Resetting form fields
      } catch (error) {
        // Swal.fire(
        //   'Subscription Required!',
        //   "You Don't have active Membership subscription!",
        //   'warning'
        // );
        this.toast.info("Unable To Create Offer!");
        this.resetForm(); // Resetting form fields
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
      this.offer.accomodation = [];
      this.offer.accomodationSuitable = [];
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
