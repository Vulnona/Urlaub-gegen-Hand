<template>
<Navbar />
<div v-if="loading"  class="spinner-container text-center">
  <div class="spinner"></div>
</div>
<div v-else>
<div class="create-offer-wrapper bg-ltgrey">
  <Banner :n="props.banner" />
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
                  <!-- Address Selection via Map -->
                  <div class="form-group">
                    <label>Adresse <b style="color: red;">*</b></label>
                    <AddressMapPicker 
                      @address-selected="onAddressSelected"
                      :initial-address="offer.address"
                      :required="true"
                    />
                  </div>
                  
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
                    <div>Bild hochladen<b style="color: red;"><a v-if="!modify">*</a></b> </div>
                    <input ref="imageInput" type="file" accept="image/x-png,image/jpeg" @change="onFileChange"
                           class="form-control" />
                  </div>
                </div>
              </div>
            </div>
            <div class="text-right">
              <button type="submit" class="btn themeBtn"><a v-if="!modify">Erstelle Angebot</a><a v-else>Modifiziere Angebot</a>&nbsp;<i class="ri-add-circle-line"></i></button>
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
import AddressMapPicker from '@/components/common/AddressMapPicker.vue';
const props = defineProps({banner: String, offer:{type: Object, default: {id: -1}, required: false} })

let loading = ref(true);
let offer = reactive ({
    title: '',
    description: '',
    location: '',
    address: null,
    accommodation: [],
    accommodationSuitable: [],
    skills: [],
    image: '',
    UntilDate: null,
    FromDate: null,
    id: -1
});
let skills = [];
let accommodations = [];
let suitableAccommodations = [];
let modify = false;

const onAddressSelected = (address) => {
    offer.address = address;
};

const createOffer = async() => {
        loading.value = true;
      if (offer.image && offer.image.size > 17 * 1024 * 1024) {
          toast.warning("Das Bild darf nicht größer als 17 MB sein.");
          loading.value = false;
          return;
      }
      if (!offer.title || !offer.skills.length || !offer.image && !modify || !offer.description || !offer.address || !offer.UntilDate || !offer.FromDate) {
          toast.info("Bitte alle mit * markierten Felder ausfüllen.");
          loading.value = false;
          return;
      }
    const offerData = new FormData();
    offer.description = offer.description.replaceAll("\n","\ \ \n");
    offerData.append('title', offer.title);
        offerData.append('description', offer.description);
        offerData.append('location', offer.address ? offer.address.displayName : '');
        // Add address data
        if (offer.address) {
            offerData.append('addressLatitude', offer.address.latitude.toString());
            offerData.append('addressLongitude', offer.address.longitude.toString());
            offerData.append('addressDisplayName', offer.address.displayName);
            offerData.append('addressHouseNumber', offer.address.houseNumber || '');
            offerData.append('addressRoad', offer.address.road || '');
            offerData.append('addressCity', offer.address.city || '');
            offerData.append('addressPostcode', offer.address.postcode || '');
            offerData.append('addressCountry', offer.address.country || '');
        }
        offerData.append('accommodation', offer.accommodation.join(', '));
        offerData.append('accommodationSuitable', offer.accommodationSuitable.join(', '));
        offerData.append('ToDate', offer.UntilDate.toISOString().split('+')[0]);
        offerData.append('FromDate', offer.FromDate.toISOString().split('+')[0]);
        offerData.append('skills', offer.skills.map(skill => skill.skillDescrition).join(', '));
        offerData.append('country', '');
        offerData.append('state', '');
        offerData.append('city', '');
        offerData.append('OfferId', offer.id.toString());
      if (offer.image) {
        offerData.append('image', offer.image);
      }
      try {
        const response = await axiosInstance.put(
          `offer/put-offer`,
          offerData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        }
        ).then(() => {
            if(modify)
                toast.success("Das Angebot wurde bearbeitet.");
            else
                toast.success("Das Angebot wurde erstellt.");
          router.push('/my-offers');
        });
      } catch (error) {
          if (error.request.status == 412)
              toast.info("Es existieren bereits Bewerbungen, das Angebot ist schreibgeschützt.");
          else if (error.request.status == 403)
              toast.info("Du hast nicht die Autorisierung das Angebot zu erstellen/bearbeiten.");
          else
              toast.info("Das Angebot kann nicht bearbeitet/erstellt werden");
      }
       loading.value = false;
}
const onFileChange = (event) => {
      offer.image = event.target.files[0];
    }
const calcDate = (dateString) => {
    var dateArray = dateString.split('.');
    return new Date(dateArray[2]+'-'+dateArray[1]+'-'+dateArray[0]);
    }

onMounted(async () => {    
    Securitybot();
    loading.value = true;
    accommodations = (await axiosInstance.get(`accommodation/get-all-accommodations`)).data;
    suitableAccommodations = (await axiosInstance.get(`accommodation-suitability/get-all-suitable-accommodations`)).data;
    skills = (await axiosInstance.get(`skills/get-all-skills`)).data;
    if (props.offer.id != -1){        
        offer.title = props.offer.title;
        offer.description = props.offer.description;
        offer.location = props.offer.location;
        offer.id = props.offer.id;
        offer.skills = [];
        offer.FromDate = calcDate(props.offer.fromDate);
        offer.UntilDate = calcDate(props.offer.toDate);
        modify = true;
    }
    loading.value = false;
})



</script>


<style scoped>
.desc-textarea {height: 90px;}
</style>
