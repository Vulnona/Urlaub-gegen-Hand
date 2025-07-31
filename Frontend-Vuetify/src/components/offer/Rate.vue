<template>
<div v-if="active" class="overlay"></div>
<div id="rating-modal" class="rating-modal" v-if="active && offer != null && user != null" >
      <div class="modal-content rating-modal-content">
        <div class="review_rating_layout">
          <div class="review_header">            
            <div class="photo" v-if="!loadingImage">
              <img loading="lazy"
                   :src="'data:' + 'image/jpeg'  + ';base64,' + image"
                   alt="Offer Image" />
            </div>
              <div class="loading" v-else>
    Wird geladen...
              </div>
            <div class="rightSideBox">
              <h5>{{ offer.title }}</h5>
              <p class="hostName">{{user.firstName }} {{user.lastName }}</p>
            </div>
          </div>
          <div class="rating_flexBox">
            <p>Bewertung</p>
            <div id="rating-stars">
              <span class="star ri-star-fill" v-for="n in 5" :key="n" :data-value="n" @click="selectedRating=n"
                    :class="{ 'selected': n <= selectedRating }"></span>
            </div>
          </div>
          <div class="review_box">
            <p>Schreibe eine Bewertung</p>
            <textarea class="textarea form-control" style="height: 120px;" v-model="reviewText">
              Bitte gebt die gegenseitige Bewertung erst ab, nachdem diese terminlich abgeschlossen ist.
            </textarea>
          </div>
        </div>
      </div>
      <div class="modal-footer">
        <div class="rating-buttons">
          <button @click="submitRating" class="btn common-btn themeBtn">Bewertung abschicken</button>
          <button @click="cancelRating" class="btn common-btn btn-cancel">Abbrechen</button>
        </div>
      </div>
    </div>    
</template>

<script setup lang="ts">
import {ref,onUpdated, getCurrentInstance, nextTick, watch} from "vue";
import toast from '@/components/toaster/toast';
import axiosInstance from '@/interceptor/interceptor';
import TableEntryUser from '@/components/offer/TableEntryUser.vue';
const props = defineProps({active: Boolean, offer: Object, user: Object})
const emit = defineEmits(['update:active', "update:refresh"]);
let image = ref(null);
let selectedRating = ref(0)
let reviewText = ""
const loadingImage = ref(true);
watch (() => props.active, async () => {
    if (props.active == false)
        return;
    loadingImage.value = true;
    try {
        const response = await axiosInstance.get(`offer/get-offer-by-id/${props.offer.offerId}`);
        // Check if imageData exists and is not undefined
        if (response.data && response.data.imageData) {
            image.value = response.data.imageData;
        } else {
            image.value = null; // Set to null if no image data
        }
        loadingImage.value = false;
  } catch (error) {
      console.error("failed to load image:", error);
      image.value = null; // Set to null on error
      loadingImage.value = false;
  } 
});
const cancelRating = () => {reviewText = ''; selectedRating=ref(0); emit('update:active', false);}
const submitRating = async () => {
    const text = "";
    if (selectedRating.value > 0) {
        await nextTick();
        try {
              const response = await axiosInstance.post(`review/add-review`, {
                  offerId: props.offer.offerId,
                  ratingValue: selectedRating.value,
                  reviewComment: reviewText,
                  reviewedUserId: props.user.user_Id,
              });
        if (response.status === 200) {
            toast.success("Dein Rating wurde erfolgreich hinzugefügt.");
            emit('update:refresh');
        }
      } catch (error) {
          toast.error("Fehler beim Absenden der Bewertungen!");
      }
          cancelRating();
      } else {
        toast.info("Bitte wählen Sie eine Bewertung");
      }
}
</script>
<style scoped>
.overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.5);
  /* Semi-transparent black */
  z-index: 100;
  /* Behind the modal */
}
.rating-star {
  font-family: var(--fa-style-family, "Font Awesome 6 Free");
  font-weight: var(--fa-style, 900);
}

.rating-star:before {
  content: "\f005";
}

.rating-modal-content {
  padding: 0;
  border: none;
}

#rating-modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  max-width: 500px;
  width: 100%;
}

.modal-content {
  text-align: center;
}

.rating-stars {
  display: flex;
  justify-content: center;
  margin: 20px 0;
}

.star {
  font-size: 2rem;
  cursor: pointer;
  color: gray;
}

.star.selected {
  color: gold;
}
.loading {
  font-size: 1.5rem;
  color: #888;
  text-align: center;
  padding: 40px;
}
</style>
