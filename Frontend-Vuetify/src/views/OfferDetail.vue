<template>  
<Navbar />
<PublicNav />
  <section class="offer-detail-container offer_detail_layout section_space" v-if="!loading">
    <div class="container">
      <div class="row">
        <div class="col-xs-12 col-sm-6">
          <!-- Galerie-Start -->
          <div v-if="offer.images && offer.images.length > 0" class="gallery-container">
            <div class="main-image" @click="openLightbox = true">
              <img :src="offer.images[activeImage].src" alt="Angebotsbild" style="width:100%; max-height:350px; object-fit:contain; border-radius:8px; cursor: pointer;" />
            </div>
            <div v-if="offer.images.length > 1" class="thumbnails d-flex mt-2 gap-2">
              <img v-for="(img, idx) in offer.images" :key="idx" :src="img.src" @click="activeImage = idx" :class="['thumbnail', {active: idx === activeImage}]" style="width:60px; height:60px; object-fit:cover; border-radius:6px; border:2px solid #eee; cursor:pointer;" />
            </div>
            <!-- Lightbox -->
            <div v-if="openLightbox" class="lightbox" @click.self="openLightbox = false">
              <span class="close-btn" @click="openLightbox = false">&times;</span>
              <img :src="offer.images[activeImage].src" class="lightbox-img" />
              <div class="lightbox-thumbs">
                <img v-for="(img, idx) in offer.images" :key="idx" :src="img.src" @click.stop="activeImage = idx" :class="['lightbox-thumb', {active: idx === activeImage}]" />
              </div>
              <button v-if="activeImage > 0" class="lightbox-prev" @click.stop="activeImage--">&#8592;</button>
              <button v-if="activeImage < offer.images.length-1" class="lightbox-next" @click.stop="activeImage++">&#8594;</button>
            </div>
          </div>
          <div v-else>
            <img src="/defaultprofile.jpg" alt="Kein Bild verfügbar" style="width:100%; max-height:350px; object-fit:contain; border-radius:8px;" />
          </div>
          <!-- Galerie-Ende -->
        </div>
        <div class="col-xs-12 col-sm-6">
          <div class="offer-content">
            <h1 class="offer-title">{{ offer.title }}</h1>
            <table class="item_description">
              <tbody v-if="offer.hostId != null">
                <tr>
                  <td>
                    <div style="display: flex; align-items: center;"> <a class="b">Gastgeber:&nbsp;</a>
                      <UserLink :hostPic=offer.hostPicture :hostId=offer.hostId :hostName=offer.hostName />
                  </div> </td>
                </tr>
              </tbody>
              <tbody>
                <tr><a class="b">Gesuchte Fähigkeiten:</a> {{ offer.skills }}</tr>
                <tr><a class="b">Unterbringung:</a> {{ offer.accomodation || 'Nicht angegeben' }}</tr>
                <tr><a class="b">Geeignet für:</a> {{ offer.accomodationsuitable || 'Nicht angegeben' }}</tr>
                <tr><a class="b">Ort/Region:</a> {{ offer.location || 'Nicht angegeben' }}</tr>
                <tr><a class="b">Angebotszeitraum:</a> {{offer.fromDate}} - {{offer.toDate}}</tr>
               </tbody>
              </table>
            <Apply :offer=offer :isActiveMember=isActiveMember :logId=logId :userRole=userRole />
            <div class="offer_btn">
              <button @click="backtooffers()" class="btn-arrow-ugh"><i class="ri-arrow-go-back-line" aria-hidden="true"></i></button>
              <span class="ms-2"><span class="text-primary-blue">Zurück</span></span>
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-sm-12">
          <div class="offer_decription mt-4">
            <VueMarkdown :source="offer.description" class="offer-description mb-0" />
          </div>
        </div>
      </div>
      <div v-if="logId == offer.hostId">
        <div v-if="offer.status == '0'">
          <button class="btn btn-danger" @click="closeOffer(offer)">
            Angebot schließen
          </button>
          <a v-if="!offer.applicationsExist">
           <button class="btn-primary-ugh ms-2" @click="modifyOffer()">
            Angebot modifizieren
           </button>
          </a>
          <a v-else>
            <button class="btn btn-blocked">
              Angebot schreibgeschützt.
             </button>
          </a>
        </div>
        <div v-else>
          <button class="btn btn-blocked">Angebot geschlossen</button>
          <button
            class="btn btn-success ms-2"
            @click="reactivateOffer(offer.id)"
            :disabled="!offer.canReactivate"
            :title="!offer.canReactivate ? 'Angebotszeitraum abgelaufen. Reaktivierung nicht möglich.' : 'Angebot reaktivieren'"
          >
            <i class="ri-refresh-line"></i> Reaktivieren
          </button>
        </div>        
      </div>
    </div>
  </section>
  <div class="loading" v-else>
    Wird geladen...
  </div>
</template>

<script setup lang="ts">
import {
  ref,
  onMounted,
  computed,
  watch
} from 'vue';
import Navbar from '@/components/navbar/Navbar.vue';
import PublicNav from '@/components/navbar/PublicNav.vue';
import axiosInstance from '@/interceptor/interceptor';
import router from '@/router';
import {isActiveMembership, GetUserRole} from '@/services/GetUserPrivileges';
import Apply from '@/components/Apply.vue';
import getLoggedUserId from '@/services/LoggedInUserId';
import UserLink from '@/components/offer/UserLink.vue';
import VueMarkdown from "vue-markdown-render";
import {useRoute} from 'vue-router';
import { useToast } from 'vue-toastification';
const toast = useToast();
const {params} = useRoute();

const pictureLink = `offer/get-preview-picture/${params.id}`;
const offer = ref(null);
const loading = ref(true);
const defaultProfileImgSrc = '/defaultprofile.jpg';
const activeImage = ref(0);
const openLightbox = ref(false);
const redirectToProfile = (userId) => {
  sessionStorage.setItem("UserId", userId);
  router.push("/account");
}

const fetchOfferDetail = async () => {
  try {
    const response = await axiosInstance.get(`offer/get-offer-by-id/${params.id}`);
    offer.value = response.data;
  } catch (error) {
    console.error('Fehler beim Laden der Angebotsdetails:', error);
  } finally {
    loading.value = false;
  }
};
const formatDate = (dateString) => {
  const options: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: 'long',
    day: '2-digit',
  };
  return new Date(dateString).toLocaleDateString(undefined, options);
};
const backtooffers = () => {
  window.history.back();
};
const closeOffer = async (offer) => {
  try {
    await axiosInstance.put(`/offer/close-offer/${offer.id}`);
    toast.success('Angebot wurde geschlossen.');
    await fetchOfferDetail(); // Angebot neu laden, damit canReactivate stimmt
  } catch (error) {
    toast.error('Fehler beim Schließen des Angebots.');
  }
};
const modifyOffer = () => {
      router.push({
        name: 'ModifyOffer', params: {id: params.id}
      });
};
const reactivateOffer = async (offerId) => {
  try {
    await axiosInstance.put(`/offer/reactivate/${offerId}`);
    toast.success('Angebot wurde reaktiviert.');
    window.location.reload();
  } catch (error) {
    toast.error((error && typeof error === 'object' && 'response' in error && (error as any).response?.data) ? (error as any).response.data : 'Fehler beim Reaktivieren des Angebots.');
  }
};

watch(() => offer.value?.images, (imgs) => { if (imgs && imgs.length > 0) activeImage.value = 0; });

onMounted(fetchOfferDetail);
</script>

<script lang="ts">    
export default {
    data() {
        return{
            isActiveMember: isActiveMembership(),
            logId: getLoggedUserId(),
            userRole: GetUserRole(),
        };
}
}
</script>

<style scoped lang="scss">
.loading {
  font-size: 1.5rem;
  color: #888;
  text-align: center;
  padding: 40px;
}

.modal {
  display: block;
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  background-color: rgb(0, 0, 0);
  background-color: rgba(0, 0, 0, 0.4);
  padding: 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.modal-content {
  background-color: #fefefe;
  width: 100%;
  max-width: 800px;
}

.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}

.review-item {
  // padding: 10px;
  // border: 1px solid #ddd;
  margin-bottom: 10px;
}

.outline_Greybtn {
  /* Example button styling */
  border: 1px solid grey;
  background-color: transparent;
  color: grey;
  padding: 8px 12px;
  cursor: pointer;
}

.reviews {
  margin-top: 20px;
}

.b {
    font-weight:bold;
    color:black;
    border-color: darkgrey;
}
.gallery-container { position: relative; }
.main-image { position: relative; }
.thumbnails .thumbnail { border: 2px solid #eee; margin-right: 4px; transition: border 0.2s; }
.thumbnails .thumbnail.active { border: 2px solid #007bff; }
.lightbox { position: fixed; top:0; left:0; width:100vw; height:100vh; background:rgba(0,0,0,0.85); display:flex; flex-direction:column; align-items:center; justify-content:center; z-index:9999; }
.lightbox-img { max-width:80vw; max-height:70vh; border-radius:8px; margin-bottom:16px; }
.lightbox-thumbs { display:flex; gap:8px; margin-bottom:16px; }
.lightbox-thumb { width:60px; height:60px; object-fit:cover; border-radius:6px; border:2px solid #eee; cursor:pointer; }
.lightbox-thumb.active { border:2px solid #007bff; }
.close-btn { position:absolute; top:24px; right:32px; font-size:2.5rem; color:#fff; cursor:pointer; z-index:10001; }
.lightbox-prev, .lightbox-next { position:absolute; top:50%; transform:translateY(-50%); background:none; border:none; color:#fff; font-size:2.5rem; cursor:pointer; z-index:10001; }
.lightbox-prev { left:32px; }
.lightbox-next { right:32px; }
</style>
