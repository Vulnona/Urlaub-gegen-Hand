<template>
  <Navbar />
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2>Profil bearbeiten</h2>
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
            <div class="col-sm-12">
              <div class="form-group">
                <label>Fertigkeiten </label>
                <multiselect v-model="profile.skills" :options="availableSkills" placeholder="Fertigkeiten auswählen"
                  class="skills-multiselect" :multiple="true">
                </multiselect>
                <small v-if="errors.skills" style="color: red;">{{ errors.skills }}</small>
              </div>
            </div>
            <input type="hidden" v-model="profile.displayName" />
            <input type="hidden" v-model="profile.latitude" />
            <input type="hidden" v-model="profile.longitude" />
            <input type="hidden" v-model="profile.id" />
            <div class="col-sm-12">
              <div class="form-group">
                <label>Hobbys</label>
                <div class="hobbies_inputBox input-group mb-3">
                  <input type="text" v-model="newHobby" class="form-control" placeholder="Hobby eingeben"
                    @keyup.enter="addHobby">
                  <div class="input-group-append">
                    <button class="btn btn-outline-secondary" type="button" @click="addHobby">Hobby hinzufügen</button>
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
                <button type="button" @click="back()" class="btn btn-back rounded">Zurück</button>
                <button type="submit" class="btn btn-primary  rounded">Profil speichern</button>
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
        skills: [],
        hobbies: [],
      },
      validations: {
        skills: true,
      },
      skills: [],
      newHobby: '',
      availableSkills: [],
      errors: {},
      formIsValid: true,
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
        const response = await axiosInstance.get(`skills/get-all-skills`);
        this.availableSkills = response.data.map(skill => skill.skillDescrition);
      } catch (error) {
      }
    },
    async fetchUserProfile() {
      try {
        const response = await axiosInstance.get(`profile/get-user-profile`);
        if (response.data.profile) {
          this.profile = response.data.profile;
          this.profile.skills = Array.isArray(this.profile.skills) ? this.profile.skills : [];
          this.profile.hobbies = Array.isArray(this.profile.hobbies) ? this.profile.hobbies :
            (typeof this.profile.hobbies === 'string' ? this.profile.hobbies.split(', ') : []);
          await this.fetchSkills();
          this.profile.skills = this.profile.skills.map(userSkill =>
            this.skills.find(skill => skill.skill_ID === userSkill.skill_ID) || userSkill
          );
        } else {
          toast.info("Benutzerprofildaten nicht gefunden");
        }
      } catch (error) {
        toast.info("Fehler beim Laden des Benutzerprofils!");
      }
    },
    saveProfile() {
      if (this.validateForm()) {
        const skillsString = this.profile.skills.join(', ');
        const hobbiesString = this.profile.hobbies.join(', ');
        const updatedProfile = {
          ...this.profile,
          skills: skillsString,
          hobbies: hobbiesString
        };
        delete updatedProfile.countryName;
        delete updatedProfile.stateName;
        this.updateProfileAPI(updatedProfile);
      } else {
        toast.error("Bitte korrigieren Sie die Fehler im Formular vor dem Absenden.");
        console.warn('Profil speichern fehlgeschlagen:', this.errors, this.profile);
      }
    },
    async updateProfileAPI(updatedProfile) {
      try {
        const response = await axiosInstance.put(`profile/update-profile`, updatedProfile);
        if (response.status === 200) {
          toast.success("Profil erfolgreich gespeichert!");
          if (response.data && response.data.profile) {            
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
          toast.error("Fehler beim Speichern des Profils. Bitte versuchen Sie es erneut.");
        }
      } catch (error) {
        console.error("Error updating profile:", error);
        toast.error("Ein Fehler ist beim Speichern des Profils aufgetreten. Bitte versuchen Sie es später erneut.");
      }
    },
    validateForm() {
      this.errors = {};
      this.formIsValid = true;
      const requiredFields = [
        'firstName', 'lastName', 'gender', 'dateOfBirth',
        'displayName', 'latitude', 'longitude'
      ];
      requiredFields.forEach(field => {
        if (!this.profile[field]) {
          this.errors[field] = `${field.charAt(0).toUpperCase() + field.slice(1)} is required`;
          this.formIsValid = false;
        }
      });
      if ((!this.profile.skills || this.profile.skills.length === 0) &&
          (!this.profile.hobbies || this.profile.hobbies.length === 0)) {
        this.errors.skills = "Bitte geben Sie mindestens ein Skill oder ein Hobby an";
        this.errors.hobbies = "Bitte geben Sie mindestens ein Skill oder ein Hobby an";
        this.formIsValid = false;
      } else {
        delete this.errors.skills;
        delete this.errors.hobbies;
      }
      if (this.profile.facebookLink && !this.validateFacebookLink()) {
        this.formIsValid = false;
      }
      return this.formIsValid;
    }
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
