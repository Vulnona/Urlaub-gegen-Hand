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
                             :auto-apply="true" placeholder="dd.mm.yyyy" :typeable="true" input-class-name="datepicker-input" startingView='month'  inputFormat="dd.MM.yyyy" @update:model-value="validateDateRange"/>
                      </div>
                      <div class="col">
                        <label for="offer.UntilDate">Möglich bis<b style="color: red;">*</b></label>
                        <Datepicker date v-model="offer.UntilDate" :locale="de" :enable-time-picker="false"
                                    :auto-apply="true" placeholder="dd.mm.yyyy" :typeable="true" input-class-name="datepicker-input" startingView='month'  inputFormat="dd.MM.yyyy" @update:model-value="validateDateRange"/>
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
                    <multiselect v-model="offer.skills" :options="skillOptions" placeholder="Fertigkeiten auswählen"
                                 label="name" track-by="id" :group-label="'name'" :group-values="'children'" multiple>
  <template #group="{ group, index, disabled, children, select, deselect }">
    <div @click.stop="expandState[group.id] = !expandState[group.id]" style="font-weight:bold; cursor:pointer; padding:4px 8px; background:#f8f8f8;">
      <span>{{ group.name }}</span>
      <span v-if="expandState[group.id]">▼</span><span v-else>▶</span>
    </div>
    <div v-show="expandState[group.id] ?? false">
      <slot name="options" :options="children" />
    </div>
  </template>
</multiselect>
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
                    <div>Bilder hochladen (max. 8)<b style="color: red;"><a v-if="!modify">*</a></b> </div>
                    <input ref="imageInput" type="file" accept="image/x-png,image/jpeg" @change="onFileChange" class="form-control" multiple :disabled="images.length >= 8" />
                    <div class="image-preview-list mt-2 d-flex flex-wrap gap-2">
                      <draggable v-model="images" class="d-flex flex-wrap gap-2" :options="{animation:150, handle:'.drag-handle'}">
                        <template #item="{element, index}">
                          <div class="image-preview-box position-relative">
                            <span class="drag-handle" style="cursor:grab; position:absolute; left:4px; top:4px; z-index:2; font-size:18px;">☰</span>
                            <img :src="getImageUrl(element)" alt="Bild" style="max-width: 120px; max-height: 120px; border-radius: 6px; border: 1px solid #ccc;" />
                            <button type="button" class="remove-image-btn position-absolute top-0 end-0" @click="removeImage(index)" style="background:rgba(255,255,255,0.8); border:none; border-radius:50%; width:24px; height:24px; font-size:18px; cursor:pointer;">✖</button>
                          </div>
                        </template>
                      </draggable>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="text-right">
              <button type="button" class="btn btn-secondary me-2" @click="goBack">Zurück</button>
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
import draggable from 'vuedraggable';
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
let skillOptions = [];
let accommodations = [];
let suitableAccommodations = [];
let modify = false;
let expandState = ref<Record<number, boolean>>({});
let images = ref<(File|string|{id:number,src:string})[]>([]); // Array für neue und bestehende Bilder
let removedImageIds = ref<number[]>([]); // IDs der zu löschenden Bilder

const onAddressSelected = (address) => {
    offer.address = address;
};

const validateDateRange = () => {
    if (offer.FromDate && offer.UntilDate && offer.FromDate > offer.UntilDate) {
        toast.warning("Das 'Möglich ab' Datum darf nicht nach dem 'Möglich bis' Datum liegen.");
        return false;
    }
    return true;
};

const createOffer = async() => {
        loading.value = true;
      if (images.value.length > 8) {
          toast.warning("Maximal 8 Bilder erlaubt.");
          loading.value = false;
          return;
      }
      if (!offer.title || !offer.skills.length || (!images.value.length && !modify) || !offer.description || !offer.address || !offer.UntilDate || !offer.FromDate) {
          toast.info("Bitte alle mit * markierten Felder ausfüllen.");
          loading.value = false;
          return;
      }
      
      // Validate date range
      if (offer.FromDate && offer.UntilDate && offer.FromDate > offer.UntilDate) {
          toast.warning("Das 'Möglich ab' Datum darf nicht nach dem 'Möglich bis' Datum liegen.");
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
            offerData.append('latitude', offer.address.latitude?.toString() || '');
            offerData.append('longitude', offer.address.longitude?.toString() || '');
        // Try different possible property names for displayName
        const displayName = offer.address.displayName || offer.address.DisplayName || offer.address.display_name || '';
        offerData.append('DisplayName', displayName);
            offerData.append('id', offer.address.id?.toString() || '');
        
        // Debug: Log address data being sent
        console.log('DEBUG: Address data being sent:', {
            latitude: offer.address.latitude,
            longitude: offer.address.longitude,
            displayName: displayName,
            id: offer.address.id
        });
    } else {
        console.warn('DEBUG: No address data available!');
    }
        
        offerData.append('accommodation', offer.accommodation.join(', '));
        offerData.append('accommodationSuitable', offer.accommodationSuitable.join(', '));
        offerData.append('ToDate', offer.UntilDate.toISOString().split('+')[0]);
        offerData.append('FromDate', offer.FromDate.toISOString().split('+')[0]);
        offerData.append('skills', offer.skills.map(skill => skill.name).join(', '));
        offerData.append('OfferId', offer.id.toString());
      // Bilder anhängen
      images.value.forEach((img, idx) => {
        if (typeof img === 'string') {
          // Bestehendes Bild (Base64-String oder URL) – Backend muss das unterstützen
          offerData.append('existingImages', img);
        } else {
          offerData.append('images', img);
        }
      });
      try {
        const response = await axiosInstance.put(
          `offer/put-offer`,
          offerData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        }
        );
        
            if(modify)
                toast.success("Das Angebot wurde bearbeitet.");
            else
                toast.success("Das Angebot wurde erstellt.");
          router.push('/my-offers');
        
      } catch (error: any) {
          if (error.request?.status == 412)
              toast.info("Es existieren bereits Bewerbungen, das Angebot ist schreibgeschützt.");
          else if (error.request?.status == 403)
              toast.info("Du hast nicht die Autorisierung das Angebot zu erstellen/bearbeiten.");
          else
              toast.info("Das Angebot kann nicht bearbeitet/erstellt werden");
      }
       loading.value = false;
}
const onFileChange = (event) => {
  const files = Array.from(event.target.files);
  // Füge nur bis zu 8 Bilder hinzu
  images.value = images.value.concat(files).slice(0, 8);
};
const removeImage = async (idx) => {
  const img = images.value[idx];
  // Wenn es sich um ein bestehendes Bild-Objekt handelt, API-Call zum Löschen
  if (typeof img === 'object' && img.id) {
    try {
      await axiosInstance.delete(`/offer/delete-picture/${img.id}`);
    } catch (e) { toast.error('Bild konnte nicht gelöscht werden.'); }
  }
  images.value.splice(idx, 1);
};
const calcDate = (dateString) => {
    var dateArray = dateString.split('.');
    return new Date(dateArray[2]+'-'+dateArray[1]+'-'+dateArray[0]);
    }
const goBack = () => {
    router.go(-1);
    }

const getImageUrl = (img: File | string | {id:number,src:string}) => {
  if (typeof img === 'string') return img;
  if (typeof img === 'object' && img.src) return img.src;
  return window.URL.createObjectURL(img as File);
};

onMounted(async () => {    
    Securitybot();
    loading.value = true;
    accommodations = (await axiosInstance.get(`accommodation/get-all-accommodations`)).data;
    suitableAccommodations = (await axiosInstance.get(`accommodation-suitability/get-all-suitable-accommodations`)).data;
    // Lade hierarchische Skills
    skillOptions = (await axiosInstance.get(`skills/hierarchical`)).data;
    if (props.offer.id != -1){        
        offer.title = props.offer.title;
        offer.description = props.offer.description;
        offer.location = props.offer.location;
        offer.id = props.offer.id;
        // Adresse explizit mappen
        offer.address = props.offer.address && typeof props.offer.address === 'object' ? {
          latitude: props.offer.address.latitude || props.offer.latitude,
          longitude: props.offer.address.longitude || props.offer.longitude,
          displayName: props.offer.address.displayName || props.offer.displayName || props.offer.location,
          id: props.offer.address.id || props.offer.addressId || undefined
        } : null;
        offer.accommodation = Array.isArray(props.offer.accommodation) ? props.offer.accommodation : (typeof props.offer.accommodation === 'string' ? props.offer.accommodation.split(',').map(s => s.trim()) : []);
        offer.accommodationSuitable = Array.isArray(props.offer.accommodationSuitable) ? props.offer.accommodationSuitable : (typeof props.offer.accommodationSuitable === 'string' ? props.offer.accommodationSuitable.split(',').map(s => s.trim()) : []);
        // Skills: Mappe gespeicherte Skill-IDs/Strings auf die Objekte aus skillOptions
        let flatSkills = [];
        skillOptions.forEach(group => {
          if (group.children && group.children.length) flatSkills.push(...group.children);
        });
        if (Array.isArray(props.offer.skills)) {
          offer.skills = props.offer.skills.map(s => typeof s === 'object' ? flatSkills.find(fs => fs.id === s.id || fs.name === s.name) : flatSkills.find(fs => fs.name === s || fs.id === s)).filter(Boolean);
        } else if (typeof props.offer.skills === 'string') {
          offer.skills = props.offer.skills.split(',').map(s => flatSkills.find(fs => fs.name === s.trim())).filter(Boolean);
        } else {
          offer.skills = [];
        }
        offer.FromDate = calcDate(props.offer.fromDate);
        offer.UntilDate = calcDate(props.offer.toDate);
        // Bild: Zeige bestehendes Bild, solange kein neues hochgeladen wird
        offer.image = props.offer.image || '';
        // Mapping für bestehende Bilder
        if (Array.isArray(props.offer.images) && props.offer.images.length > 0) {
            images.value = props.offer.images.map((img, idx) => {
                if (typeof img === 'string') return img;
                if (img && typeof img === 'object' && img.src) return img;
                return img;
            });
        } else if (props.offer.image) {
            images.value = [props.offer.image];
        }
        modify = true;
    }
    loading.value = false;
})



</script>


<style scoped>
.desc-textarea {height: 90px;}

/* Multiselect Parent (Group) fett, nicht ausgegraut */
.multiselect__group {
  font-weight: bold;
  color: #222;
  background: #f8f8f8;
  cursor: default;
  font-size: 1.05em;
  opacity: 1 !important;
}
/* Child Skills normal, etwas kleiner */
.multiselect__option {
  font-size: 0.97em;
  font-weight: normal;
}
.image-preview-list { gap: 8px; }
.image-preview-box { position: relative; display: inline-block; }
.remove-image-btn { right: 2px; top: 2px; }
.drag-handle { cursor: grab; color: #888; }
</style>
