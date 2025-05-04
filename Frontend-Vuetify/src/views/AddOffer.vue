<template>
<Navbar />
<div v-if="loading"  class="spinner-container text-center">
  <div class="spinner"></div>
</div>
<div v-else>
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
                  <Input n="Titel für Angebot" v-model="offer.title" p="" :req=true />
                  <Input n="Beschreibung" v-model="offer.description" p="" :t="true" :req=true />
                </div>
              </div>
            </div>
            <div class="card general_info_box">
              <div class="general_infoContent">
                <div class="card-title"> <h5>Zeit und Ort</h5> </div>
                <div class="card-body">
                  <Input n="Ort" v-model="offer.location" p="Geben Sie den Ort des Angebots ein" :req=true />
                  <div class="form-group">


                    
                    <div class="row">
                      <div class="col">
                        <label for="offer.FromDate">Möglich ab<b style="color: red;">*</b></label>
                 <Datepicker date v-model="offer.FromDate" :locale="de" :enable-time-picker="false"
                             :auto-apply="true" placeholder="dd.mm.yyyy" :typeable="true" input-class-name="datepicker-input" startingView='month'  inputFormat="dd.MM.yyyy"/>
                      </div>
                      <div class="col">
                        <label for="offer.UntilDate">Möglich bis<b style="color: red;">*</b></label>
                        <Datepicker date v-model="offer.UntilDate" :locale="de" :enable-time-picker="false"
                                    :auto-apply="true" placeholder="dd.mm.yyyy" :typeable="true" input-class-name="datepicker-input" startingView='month'  inputFormat="dd.MM.yyyy"/>
                      </div>
                      </div>

                   </div>
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
                    <div class="form-group"> <label> Unterbringung </label> <div class="contact_infoBox">                        
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
                    <div>Bild hochladen<b style="color: red;">*</b> </div>
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
</div>
</template>

<script setup lang="ts">
import { ref, defineExpose, onMounted, onUpdated, reactive } from "vue";
import Banner from '@/components/Banner.vue';
import Input from '@/components/form/Input.vue';
import Navbar from '@/components/navbar/Navbar.vue';
import { de } from 'date-fns/locale';
import Datepicker from 'vue3-datepicker';
import router from '@/router';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.css';
import Securitybot from '@/services/SecurityBot';
import axiosInstance from '@/interceptor/interceptor';
import toast from '@/components/toaster/toast';


let loading = ref(true);
let offer = reactive ({
    title: '',
    description: '',
    location: '',
    accommodation: [],
    accommodationSuitable: [],
    skills: [],
    image: '',
    UntilDate: null,
    FromDate: null
});
let skills = [];
let accommodations = [];
let suitableAccommodations = [];

const createOffer = async() => {
        loading.value = true;
      if (offer.image && offer.image.size > 3.5 * 1024 * 1024) {
        toast.warning("Image size cannot be greater than 3.5 MB.");
        return;
      }
      if (!offer.title || !offer.skills.length || !offer.image || !offer.description || !offer.location) {
        toast.info("Please fill all the required fields marked with *");
        return;
      }
    const offerData = new FormData();
    offer.description = offer.description.replaceAll("\n","\\\n");
        offerData.append('title', offer.title);
        offerData.append('description', offer.description);
        offerData.append('location', offer.location);
        offerData.append('contact', offer.contact);
        offerData.append('accommodation', offer.accommodation.join(', '));
        offerData.append('accommodationSuitable', offer.accommodationSuitable.join(', '));
        offerData.append('ToDate', offer.UntilDate.toISOString().split('+')[0]);
        offerData.append('FromDate', offer.FromDate.toISOString().split('+')[0]);
        offerData.append('skills', offer.skills.map(skill => skill.skillDescrition).join(', '));
        offerData.append('country', "");
        offerData.append('state', "");
        offerData.append('city', "");        
      if (offer.image) {
        offerData.append('image', offer.image);
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
       loading.value = false;
}
const onFileChange = (event) => {
      offer.image = event.target.files[0];
    }

onMounted(async () => {
    Securitybot();
    loading.value = true;
    accommodations = (await axiosInstance.get(`${process.env.baseURL}accommodation/get-all-accommodations`)).data;
    suitableAccommodations = (await axiosInstance.get(`${process.env.baseURL}accommodation-suitability/get-all-suitable-accommodations`)).data;
    skills = (await axiosInstance.get(`${process.env.baseURL}skills/get-all-skills`)).data;
    loading.value = false;
})
</script>


<style scoped>
.desc-textarea {height: 90px;}
</style>
