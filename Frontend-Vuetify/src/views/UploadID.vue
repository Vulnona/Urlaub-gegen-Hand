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
                <input type="file" accept="image/x-png,image/gif,image/jpeg" @change="onFrontIdChange">
                <button type="button" class="btn themeBtn">
                  <i class="ri-upload-2-line"></i> Choose File
                </button>
              </div>
              <div class="mt-3">
                <div v-if="frontIdPreview">
                  <img :src="frontIdPreview" alt="Front ID Preview" class="card-img-top">
                </div>
                <div v-else class="no-image-selected">
                  No image selected
                </div>
              </div>
            </div>
          </div>
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Hintere Seite des Ausweises</h5>
              <div class="custom-upload">
                <input type="file" accept="image/x-png,image/gif,image/jpeg" @change="onBackIdChange">
                <button type="button" class="btn themeBtn">
                  <i class="ri-upload-2-line"></i> Choose File
                </button>
              </div>
              <div class="mt-3">
                <div v-if="backIdPreview">
                  <img :src="backIdPreview" alt="Back ID Preview" class="card-img-top">
                </div>
                <div v-else class="no-image-selected">
                  No image selected
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row mt-3">
      <div class="col-sm-12 bottom_btn text-center">
        <button @click="back" class="btn btn-primary rounded grey_btn">Cancel</button>&nbsp;&nbsp;
        <button @click="uploadImages" class="btn btn-primary rounded">Upload</button>
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>
import { onMounted, ref } from 'vue';
import { PutObjectCommand, S3Client } from '@aws-sdk/client-s3';
import Swal from 'sweetalert2';
import router from '@/router';
import CryptoJS from 'crypto-js';

// AWS S3 configuration
const secretAccessKey = process.env.SecretAccessKey;
const accessKeyId = process.env.AccessKeyId;
const bucket = process.env.S3_BUCKET_NAME;
const region = process.env.Aws_region;
const client = new S3Client({
  region,
  credentials: {
    secretAccessKey,
    accessKeyId,
  },
});

// Function to convert uploaded file to JPEG format
const convertToJpeg = (file: File): Promise<File> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = (event) => {
      const img = new Image();
      img.onload = () => {
        const canvas = document.createElement('canvas');
        canvas.width = img.width;
        canvas.height = img.height;
        const ctx = canvas.getContext('2d');
        ctx.drawImage(img, 0, 0);
        canvas.toBlob((blob) => {
          if (blob) {
            const jpegFile = new File([blob], file.name.replace(/\..+$/, ".jpg"), { type: "image/jpeg" });
            resolve(jpegFile);
          } else {
            reject(new Error('Conversion to JPEG failed'));
          }
        }, 'image/jpeg', 0.8);
      };
      img.src = event.target.result as string;
    };
    reader.onerror = reject;
    reader.readAsDataURL(file);
  });
};
// Security check function to ensure user authentication
const Securitybot = () => {
  if (!sessionStorage.getItem("logId")) {
    router.push('/login');
  }
};
// Function to upload image file to AWS S3 bucket
const uploadImage = async (file: File, fileName: string) => {
  try {
    const jpegFile = await convertToJpeg(file);
    const command = new PutObjectCommand({
      Bucket: bucket,
      Key: fileName,
      Body: jpegFile,
    });
    await client.send(command);
  } catch (err) {
    console.error(err);
    Swal.fire('Error', 'Das Bild konnte nicht hochgeladen werden', 'error');
    throw err;
  }
};
// Function to encrypt a link using AES encryption
const encryptLink = (link: string) => {
  return CryptoJS.AES.encrypt(link, process.env.SECRET_KEY).toString();
};
// Function to update user links with encrypted URLs
const updateUserLinks = async (userId: string, linkVS: string, linkRS: string) => {
  try {
    const encryptedLinkVS = encryptLink(linkVS);
    const encryptedLinkRS = encryptLink(linkRS);
    const apiUrl = `${process.env.baseURL}user/upload-id`;

    const response = await fetch(apiUrl, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        id: userId,
        link_VS: encryptedLinkVS,
        link_RS: encryptedLinkRS,
      }),
    });

    if (!response.ok) {
      throw new Error('Failed to update');
    }

    Swal.fire('Erfolgreich', 'Upload erfolgreich!', 'success').then(() => {
      sessionStorage.clear();
      router.push('/');
    });
  } catch (err) {
    console.error(err);
    Swal.fire('Error', 'Upload nicht erfolgreich', 'error');
  }
};

// Event handler for front ID image file change
const onFrontIdChange = (event: any) => {
  const file = event.target.files[0];
  frontIdFile.value = file;
  frontIdPreview.value = URL.createObjectURL(file);
};
// Event handler for back ID image file change
const onBackIdChange = (event: any) => {
  const file = event.target.files[0];
  backIdFile.value = file;
  backIdPreview.value = URL.createObjectURL(file);
};
const frontIdFile = ref<File | null>(null);
const backIdFile = ref<File | null>(null);
const frontIdPreview = ref<string | null>(null);
const backIdPreview = ref<string | null>(null);
// Function to initiate image upload process
const uploadImages = () => {
  if (!frontIdFile.value || !backIdFile.value) {
    Swal.fire('', 'Bitte lade beides hoch, Vorder- und Rückseite!', 'warning');
    return;
  }
  Swal.fire({
    title: 'Bist du sicher?',
    text: 'Möchtest du diese Bilder hochladen?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Ja, lade sie hoch!',
  }).then((result) => {
    if (result.isConfirmed) {
      const userId = decryptlogID(sessionStorage.getItem("logId"));
      const frontFileName = `${userId}VS.jpg`;
      const backFileName = `${userId}RS.jpg`;
      const uploadPromises = [
        uploadImage(frontIdFile.value, frontFileName),
        uploadImage(backIdFile.value, backFileName),
      ];
      Promise.all(uploadPromises)
        .then(() => {
          const linkVS = `https://${bucket}.s3.${region}.amazonaws.com/${frontFileName}`;
          const linkRS = `https://${bucket}.s3.${region}.amazonaws.com/${backFileName}`;
          updateUserLinks(userId, linkVS, linkRS);
        })
        .catch(err => {
          console.error(err);
          Swal.fire('Error', 'Die Bilder konnten nicht hochgeladen werden', 'error');
        });
    }
  });
};
const back = () => {
  window.history.back();
}
const decryptlogID = (encryptedItem: string | null) => {
  if (!encryptedItem) {
    return null;
  }
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY);
    const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
    return decryptedString;
  } catch (e) {
    console.error('Error decrypting item:', e);
    return null;
  }
};
onMounted(() => {
  Securitybot();
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
</style>
