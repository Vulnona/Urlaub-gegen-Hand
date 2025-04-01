<template>
<Navbar />
<div class="create-offer-wrapper bg-ltgrey">
  <Banner n="Erstelle Angebot" />
  <div class="bg-ltgrey pt-40 pb-40">
    <form @submit.prevent="createOffer">
      <div class="container">
        <div class="row">
          <div class="col-sm-6">
            <div class="card general_info_box">
              <div class="general_infoContent">
                <div class="card-title"> <h5>Allgemeine Informationen</h5> </div>
                <div class="card-body">
                  <Input n="Titel für Angebot" v-model="offer.title" p="" req="true" />
                  <Input n="Beschreibung" v-model="offer.description" p="" :t="true" :req="true" />
                </div>
              </div>
            </div>
            <div class="card general_info_box">
              <div class="general_infoContent">
                <div class="card-title"> <h5>Address Information</h5> </div>
                <div class="card-body address-search-selection">
                  <Input n="Ort" v-model="offer.location" p="Geben Sie den Ort des Angebots ein" :req="true" />
                </div>
              </div>
            </div>
          </div>
          <div class="col-sm-6">
            <div class="card general_info_box">
              <div class="general_infoContent">
                <div class="card-title"> <h5>Additional Information</h5> </div>
                <div class="card-body">
                  <div class="form-group">
                    <label>Fertigkeiten <b style="color: red;">*</b></label>
                    <multiselect v-model="offer.skills" :options="skills" placeholder="Select Fertigkeiten"
                                 label="skillDescrition" track-by="skill_ID" multiple></multiselect>
                  </div>
                  <div class="amenities-wrapper">
                    <div class="form-group"> <label> Angebotene Annehmlichkeiten </label> <div class="contact_infoBox">
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
                    <req n="Bild hochladen" />
                    <input ref="imageInput" type="file" accept="image/x-png,image/jpeg" @change="onFileChange"
                           class="form-control" />
                  </div>
                </div>
              </div>
            </div>
            <div class="text-right">
              <button type="submit" class="btn themeBtn">Erstelle Angebot&nbsp;<i class="ri-add-circle-line"></i></button>
              </div>
            </div>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, defineExpose, onMounted, onUpdated } from "vue";
  import Banner from '@/components/Banner.vue';
  import Input from '@/components/form/Input.vue';
  import Navbar from '@/components/navbar/Navbar.vue';
</script>

<script lang="ts">
import router from '@/router';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.css';
import Securitybot from '@/services/SecurityBot';
import axiosInstance from '@/interceptor/interceptor';
import toast from '@/components/toaster/toast';

export default {
  components: {
    Multiselect,
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
          image: null,
      },
      skills: [],
      accommodations: [],
      suitableAccommodations: [],
    };
  },

  mounted() {
    Securitybot();
    this.fetchSkills();
    this.fetchAccommodations();
      this.fetchSuitableAccommodations();
  },
  methods: {
    // Method to fetch accommodations data
    async fetchAccommodations() {
      try {
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
    async createOffer() {
        // Method to create a new offer
      if (this.offer.image && this.offer.image.size > 3.5 * 1024 * 1024) {
        toast.warning("Image size cannot be greater than 3.5 MB.");
        return;
      }
      if (!this.offer.title || !this.offer.skills.length || !this.offer.image || !this.offer.description || !this.offer.location) {
        toast.info("Please fill all the required fields marked with *");
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
      offerData.append('country', "");
      offerData.append('state', "");
      offerData.append('city', "");
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
          toast.success("Your offer has been created!");
          router.push('/my-offers');
        });
      } catch (error) {
        toast.info("Unable To Create Offer!");
      }
    },
    onFileChange(event) {
      this.offer.image = event.target.files[0];
    }
  }
};
</script>
<style scoped>
.desc-textarea {height: 90px;}
</style>
