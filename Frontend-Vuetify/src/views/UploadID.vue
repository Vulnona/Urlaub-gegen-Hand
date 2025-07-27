<template>
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2>Lade deinen Ausweis hoch</h2>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="container">
    <div class="row">
      <div class="col-md-12">
        <div class="upload-cards">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Vordere Seite des Ausweises</h5>
              <div class="custom-upload">
                <input type="file" accept="image/x-png,image/gif,image/jpeg" @change="onFrontIdChange" />
                <button type="button" class="btn themeBtn">
                  <i class="ri-upload-2-line"></i> Choose File
                </button>
              </div>
              <div class="mt-3">
                <div v-if="frontIdPreview">
                  <img :src="frontIdPreview" alt="Front ID Preview" class="card-img-top" />
                </div>
                <div v-else class="no-image-selected">
                  Kein Bild ausgewählt
                </div>
              </div>
            </div>
          </div>

          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Hintere Seite des Ausweises</h5>
              <div class="custom-upload">
                <input type="file" accept="image/x-png,image/gif,image/jpeg" @change="onBackIdChange" />
                <button type="button" class="btn themeBtn">
                  <i class="ri-upload-2-line"></i> Datei auswählen
                </button>
              </div>
              <div class="mt-3">
                <div v-if="backIdPreview">
                  <img :src="backIdPreview" alt="Back ID Preview" class="card-img-top" />
                </div>
                <div v-else class="no-image-selected">
                  Kein Bild ausgewählt
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="row mt-3">
      <div class="col-sm-12 bottom_btn text-center">
        <button @click="back" class="btn btn-primary rounded grey_btn" :disabled="isLoading">Abbrechen</button>
        &nbsp;&nbsp;
        <button @click="uploadImages" class="btn btn-primary rounded" :disabled="isLoading">
          Upload
        </button>
      </div>
    </div>

    <!-- Loader -->
    <div v-if="isLoading" class="loader-overlay">
      <div class="loader"></div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import Swal from 'sweetalert2';
import router from '@/router';
import CryptoJS from 'crypto-js';
import axiosInstance from '@/interceptor/interceptor';
import toast from '@/components/toaster/toast';

const frontIdFile = ref<File | null>(null);
const backIdFile = ref<File | null>(null);
const frontIdPreview = ref<string | null>(null);
const backIdPreview = ref<string | null>(null);
const isLoading = ref(false); 
const Securitybot = () => {
  if (!sessionStorage.getItem('logId')) {
    router.push('/');
  }
};

const onFrontIdChange = (event: any) => {
  const file = event.target.files[0];
  if (file) {
    frontIdFile.value = file;
    frontIdPreview.value = URL.createObjectURL(file);
  }
};

const onBackIdChange = (event: any) => {
  const file = event.target.files[0];
  if (file) {
    backIdFile.value = file;
    backIdPreview.value = URL.createObjectURL(file);
  }
};

const uploadImages = async () => {
  if (!frontIdFile.value || !backIdFile.value) {
    toast.info("Bitte lade beides hoch, Vorder- und Rückseite!");
    return;
  }

  Swal.fire({
    title: 'Bist du sicher?',
    text: 'Möchtest du diese Bilder hochladen?',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Ja, lade sie hoch!',
  }).then(async (result) => {
    if (result.isConfirmed) {
      isLoading.value = true; 
      const userId = decryptlogID(sessionStorage.getItem('logId'));
      const frontFileName = `${userId}VS`;
      const backFileName = `${userId}RS`;

      try {
        await updateUserLinks(userId, frontFileName, backFileName);
        toast.success("Upload erfolgreich!");
        sessionStorage.clear();
        router.push({ name: 'Login' }); // Use named route instead of path
      } catch (error) {
        console.error('Fehler beim Upload:', error);
        if (typeof error === 'object' && error !== null && 'isAxiosError' in error && (error as any).isAxiosError && (error as any).response && (error as any).response.status === 413) {
            toast.error("Bilddateien zu groß.");
        } else {
            toast.error("Upload nicht erfolgreich. Bitte versuchen Sie es erneut.");
        }
      } finally {
        isLoading.value = false; 
      }
    }
  });
};

const updateUserLinks = async (userId: string, linkVS: string, linkRS: string) => {
  const userData = new FormData();
  userData.append('fileNameVS', linkVS);
  userData.append('fileNameRS', linkRS);
  userData.append('fileVS', frontIdFile.value!);
  userData.append('fileRS', backIdFile.value!);

  await axiosInstance.post(
    `authenticate/upload-id?userId=${userId}`,
    userData,
    {
      headers: { 'Content-Type': 'multipart/form-data' },
    }
  );
};

const back = () => {
  window.history.back();
};

const decryptlogID = (encryptedItem: string | null) => {
  if (!encryptedItem) return null;
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (e) {
    console.error('Error decrypting item:', e);
    return null;
  }
};

onMounted(() => {
  Securitybot();
  //membershipPopup();
});
</script>

<style scoped>
.card {
  margin-bottom: 20px;
}

.no-image-selected {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 200px;
  width: 100%;
  background-color: #f8f9fa;
  border: 1px dashed #ccc;
  color: #999;
  text-align: center;
}

.custom-upload {
  position: relative;
  width: 200px;
  margin: auto;
  cursor: pointer;
}

.custom-upload button {
  width: 100%;
}

.custom-upload input {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  width: 100%;
  z-index: 999;
  opacity: 0;
}

.upload-cards {
  text-align: center;
}
.loader-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.loader {
  display: inline-block;
  width: 80px;
  height: 80px;
  border: 8px solid #f3f3f3;
  border-radius: 50%;
  border-top-color: #3498db;
  animation: spin 1s ease-in-out infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

/* Optional: Smooth fade-in effect for the loader */
.loader-overlay {
  animation: fadeIn 0.3s ease-in-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

</style>
