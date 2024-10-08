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
                  <option value="">Select Gender</option>
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
            <div class="col-md-4 col-sm-6">
              <!-- Street -->
              <div class="form-group">
                <label for="street">Straße</label>
                <input type="text" v-model="profile.street" class="form-control" id="street" />
                <span v-if="errors.street" class="text-danger">{{ errors.street }}</span>
              </div>
            </div>
            <div class="col-md-4 col-sm-6">
              <!-- House Number -->
              <div class="form-group">
                <label for="houseNumber">Hausnummer</label>
                <input type="text" v-model="profile.houseNumber" class="form-control" id="houseNumber" />
                <span v-if="errors.houseNumber" class="text-danger">{{ errors.houseNumber }}</span>
              </div>
            </div>
            <div class="col-md-4 col-sm-6">
              <!-- Country -->
              <div class="form-group">
                <label for="country">Land</label>
                <!-- <div>Current: {{ profile.countryName }}</div> -->
                <select v-model="selectedCountry" class="form-control" id="country" @change="onCountryChange">
                  <option value="">Select Country</option>
                  <option v-for="country in countries" :key="country.country_ID" :value="country">
                    {{ country.countryName }}
                  </option>
                </select>
                <span v-if="errors.country" class="text-danger">{{ errors.country }}</span>
              </div>
            </div>
            <div class="col-md-4 col-sm-6">
              <!-- State -->
              <div class="form-group">
                <label for="state">Region/Bundesland</label>
                <!-- <div>Current: {{ profile.stateName }}</div> -->
                <select v-model="selectedState" class="form-control" id="state" @change="onStateChange">
                  <option value="">Select State</option>
                  <option v-for="state in states" :key="state.id" :value="state">{{ state.name }}</option>
                </select>
                <span v-if="errors.state" class="text-danger">{{ errors.state }}</span>
              </div>
            </div>
            <div class="col-md-4 col-sm-6">
              <!-- City -->
              <div class="form-group">
                <label for="city">Stadt</label>
                <!-- <div>Current: {{ profile.cityName }}</div> -->
                <select v-model="selectedCity" class="form-control" id="city">
                  <option value="">Select City</option>
                  <option v-for="city in cities" :key="city.id" :value="city">{{ city.name }}</option>
                </select>
                <span v-if="errors.city" class="text-danger">{{ errors.city }}</span>
              </div>
            </div>
            <div class="col-md-4 col-sm-6">
              <!-- Post Code -->
              <div class="form-group">
                <label for="postCode">Postleitzahl</label>
                <input type="text" v-model="profile.postCode" class="form-control" id="postCode" />
                <span v-if="errors.postCode" class="text-danger">{{ errors.postCode }}</span>
              </div>
            </div>
            <div class="col-md-8 col-sm-12">
              <!-- Facebook Link -->
              <div class="form-group">
                <label for="facebookLink">Facebook-Profillink</label>
                <input type="url" v-model="profile.facebookLink" class="form-control" id="facebookLink"
                  @blur="validateFacebookLink" />
                <span v-if="errors.facebookLink" class="text-danger">{{ errors.facebookLink }}</span>
              </div>

            </div>
            <div class="col-sm-12">
              <div class="form-group">
                <label>Fertigkeiten </label>
                <multiselect v-model="profile.skills" :options="availableSkills" placeholder="Select Fertigkeiten"
                  class="skills-multiselect" :multiple="true">
                </multiselect>
                <small v-if="errors.skills" style="color: red;">{{ errors.skills }}</small>
              </div>
            </div>
            <div class="col-sm-12">
              <div class="form-group">
                <label>Hobbies</label>
                <div class="hobbies_inputBox input-group mb-3">
                  <input type="text" v-model="newHobby" class="form-control" placeholder="Enter a hobby"
                    @keyup.enter="addHobby">
                  <div class="input-group-append">
                    <button class="btn btn-outline-secondary" type="button" @click="addHobby">Add Hobby</button>
                  </div>
                </div>
                <div v-if="profile.hobbies.length > 0" class="mt-2">
                  <span v-for="(hobby, index) in profile.hobbies" :key="index" class="badge badge-primary mr-2 mb-2">
                    {{ hobby }}
                    <button type="button" class="close ml-1" @click="removeHobby(index)">&times;</button>
                  </span>
                </div>
                <span v-if="errors.hobbies" class="text-danger">{{ errors.hobbies }}</span>
              </div>
            </div>
          </div>

          <div class="row">
            <div class="col-sm-12">
              <div class="profile_btn">
                <!-- Submit Button -->
                <button type="button" @click="back()" class="btn btn-back rounded">Back</button>
                <button type="submit" class="btn btn-primary  rounded">Save Profile</button>
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
export default {
  components: {
    Navbar,
    Multiselect,
  },
  data() {
    return {
      profile: {
        firstName: "",
        lastName: "",
        gender: "",
        dateOfBirth: "",
        street: "",
        houseNumber: "",
        postCode: "",
        country: "",
        state: "",
        city: "",
        facebookLink: "",
        countryName: "",
        stateName: "",
        cityName: "",
        skills: [],
        hobbies: [],
      },
      validations: {
        skills: true,
      },
      skills: [],
      newHobby: '',
      availableSkills: [],
      selectedCountry: null,
      selectedState: null,
      selectedCity: null,
      errors: {},
      formIsValid: true,
      countries: [],
      states: [],
      cities: [],
    };
  },
  mounted() {
    Securitybot();
    this.fetchUserProfile();
  },
  methods: {
    back() {
      window.history.back();
    },
    addHobby() {
      if (this.newHobby.trim()) {
        this.profile.hobbies.push(this.newHobby.trim());
        this.newHobby = '';
      }
    },

    removeHobby(index) {
      this.profile.hobbies.splice(index, 1);
    },
    async fetchSkills() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}skills/get-all-skills`);
        this.availableSkills = response.data.map(skill => skill.skillDescrition);
      } catch (error) {
      }
    },
    async fetchCountries() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}region/get-all-countries`);
        this.countries = response.data;
        this.selectedCountry = this.countries.find(country => country.countryName === this.profile.countryName) || null;
        if (this.selectedCountry) {
          this.fetchStates(this.selectedCountry.country_ID);
        }
      } catch (error) {
        toast.error("Failed to fetch countries");
      }
    },
    getLocationNames() {
      return {
        country: this.selectedCountry ? this.selectedCountry.countryName : this.profile.countryName,
        state: this.selectedState ? this.selectedState.name : this.profile.stateName,
        city: this.selectedCity ? this.selectedCity.name : this.profile.cityName
      };
    },
    async fetchUserProfile() {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}profile/get-user-profile`);
        if (response.data.profile) {
          this.profile = response.data.profile;
          this.profile.countryName = this.profile.country;
          this.profile.stateName = this.profile.state;
          this.profile.cityName = this.profile.city;

          this.fetchCountries();
          this.profile.skills = Array.isArray(this.profile.skills) ? this.profile.skills : [];
          this.profile.hobbies = Array.isArray(this.profile.hobbies) ? this.profile.hobbies :
            (typeof this.profile.hobbies === 'string' ? this.profile.hobbies.split(', ') : []);
          await this.fetchSkills();
          this.profile.skills = this.profile.skills.map(userSkill =>
            this.skills.find(skill => skill.skill_ID === userSkill.skill_ID) || userSkill
          );
        } else {
          toast.info("User profile data not found");
        }
      } catch (error) {
        toast.info("Failed to fetch user profile!");
      }
    },
    async fetchStates(countryId) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}region/get-state-by-countryId/${countryId}`);
        this.states = response.data;
        this.selectedState = this.states.find(state => state.name === this.profile.stateName) || null;
        if (this.selectedState) {
          this.fetchCities(this.selectedState.id);
        }
      } catch (error) {
        toast.error("Failed to fetch states");
      }
    },
    async fetchCities(stateId) {
      try {
        const response = await axiosInstance.get(`${process.env.baseURL}region/get-city-by-stateId/${stateId}`);
        this.cities = response.data;
        this.selectedCity = this.cities.find(city => city.name === this.profile.cityName) || null;
      } catch (error) {
        toast.error("Failed to fetch cities");
      }
    },
    onCountryChange() {
      this.selectedState = null;
      this.selectedCity = null;
      this.states = [];
      this.cities = [];
      if (this.selectedCountry) {
        this.fetchStates(this.selectedCountry.country_ID);
      }
    },
    onStateChange() {
      this.selectedCity = null;
      this.cities = [];
      if (this.selectedState) {
        this.fetchCities(this.selectedState.id);
      }
    },
    saveProfile() {
      if (this.validateForm()) {
        const locationNames = this.getLocationNames();
        const skillsString = this.profile.skills.join(', ');
        const hobbiesString = this.profile.hobbies.join(', ');
        const updatedProfile = {
          ...this.profile,
          country: locationNames.country,
          state: locationNames.state,
          city: locationNames.city,
          skills: skillsString,
          hobbies: hobbiesString
        };
        delete updatedProfile.countryName;
        delete updatedProfile.stateName;
        delete updatedProfile.cityName;
        this.updateProfileAPI(updatedProfile);
      } else {
        toast.error("Please correct the errors in the form before submitting.");
      }
    },
    async updateProfileAPI(updatedProfile) {
      try {
        const response = await axiosInstance.put(`${process.env.baseURL}profile/update-profile`, updatedProfile);
        if (response.status === 200) {
          toast.success("Profile saved successfully!");
          if (response.data && response.data.profile) {
            this.profile = response.data.profile;
            this.profile.countryName = this.profile.country;
            this.profile.stateName = this.profile.state;
            this.profile.cityName = this.profile.city;
            this.selectedCountry = this.countries.find(country => country.countryName === this.profile.country) || null;
            this.selectedState = this.states.find(state => state.name === this.profile.state) || null;
            this.selectedCity = this.cities.find(city => city.name === this.profile.city) || null;

            this.profile.skills = Array.isArray(this.profile.skills) ? this.profile.skills :
              (typeof this.profile.skills === 'string' ? this.profile.skills.split(', ') : []);
            this.profile = {
              ...response.data.profile,
              skills: skillsArray
            };
            this.profile.skills = this.profile.skills.map(userSkill =>
              this.skills.find(skill => skill.skill_ID === userSkill.skill_ID) || userSkill
            );
          }
          this.$router.push('/profile');
        } else {
          toast.error("Failed to save profile. Please try again.");
        }
      } catch (error) {
        console.error("Error updating profile:", error);
        toast.error("An error occurred while saving the profile. Please try again later.");
      }
    },
    validateForm() {
      this.errors = {};
      this.formIsValid = true;
      const requiredFields = [
        'firstName', 'lastName', 'gender', 'dateOfBirth',
        'street', 'houseNumber', 'postCode'
      ];
      requiredFields.forEach(field => {
        if (!this.profile[field]) {
          this.errors[field] = `${field.charAt(0).toUpperCase() + field.slice(1)} is required`;
          this.formIsValid = false;
        }
      });
      const locationNames = this.getLocationNames();
      if (!locationNames.country) {
        this.errors.country = "Country is required";
        this.formIsValid = false;
      }
      if (!locationNames.state) {
        this.errors.state = "State is required";
        this.formIsValid = false;
      }
      if (!locationNames.city) {
        this.errors.city = "City is required";
        this.formIsValid = false;
      }
      if (!this.validateDateOfBirth()) {
        this.formIsValid = false;
      }
      if (!this.profile.skills || this.profile.skills.length === 0) {
        this.errors.skills = "At least one skill is required";
        this.formIsValid = false;
      } else {
        delete this.errors.skills;
      }
      if (this.profile.facebookLink && !this.validateFacebookLink()) {
        this.formIsValid = false;
      }
      if (this.profile.hobbies.length === 0) {
        this.errors.hobbies = "Please add at least one hobby";
        this.formIsValid = false;
      } else {
        delete this.errors.hobbies;
      }
      return this.formIsValid;
    },
    validateDateOfBirth() {
      const today = new Date();
      const dob = new Date(this.profile.dateOfBirth);
      const age = today.getFullYear() - dob.getFullYear();
      const monthDiff = today.getMonth() - dob.getMonth();
      if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
        age--;
      }
      if (age < 14 || age > 120) {
        this.errors.dateOfBirth = "Date of birth must indicate age between 14 and 120.";
        return false;
      }
      return true;
    },
    validateFacebookLink() {
      const facebookRegex = /^https:\/\/www\.facebook\.com(\/.*)?$/;
      if (!facebookRegex.test(this.profile.facebookLink)) {
        this.errors.facebookLink = "Please enter a valid Facebook link (e.g., https://www.facebook.com/anything)";
        return false;
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

.hobbies_inputBox .input-group-append .btn {
  font-size: 14px;
  border: 1px solid #c4cbdd;
  background: #d9d9d9;
  border-left: 2px solid #c4cbdd;
  color: #333;
  font-weight: 500;
}
</style>