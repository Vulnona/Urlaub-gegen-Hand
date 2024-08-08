<template>
  <div class="container" style="margin: auto;">
    <h2 class="text-center" style="padding: 10px;">Upload ID Card</h2>
    <div class="row">
      <div class="col-md-6">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Front ID Card</h5>
            <input type="file" accept="image/*" @change="onFrontIdChange">
            <div class="mt-3">
              <div v-if="frontIdPreview">
                <img :src="frontIdPreview" alt="Front ID Preview" class="card-img-top"
                  style="width: 300px; height: 200px;">
              </div>
              <div v-else class="no-image-selected">
                No image selected
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Back ID Card</h5>
            <input type="file" accept="image/*" @change="onBackIdChange">
            <div class="mt-3">
              <div v-if="backIdPreview">
                <img :src="backIdPreview" alt="Back ID Preview" class="card-img-top"
                  style="width: 300px; height: 200px;">
              </div>
              <div v-else class="no-image-selected">
                No image selected
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row mt-3">
      <div class="text-center">
        <button @click="back" class="btn-primary rounded col-2" style="padding: 5px;">Back</button>&nbsp;&nbsp;
        <button @click="uploadImages" class="btn-primary rounded col-2" style="padding: 5px;">Upload</button>
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

// AWS S3 credentials and configuration
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
    Swal.fire({
      title: 'You are not logged In!',
      text: 'Login First to continue.',
      icon: 'info',
      confirmButtonText: 'OK'
    });
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
    Swal.fire('Error', 'Failed to upload image', 'error');
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
    const apiUrl = `${process.env.baseURL}user/upload-id/${userId}?link_vs=${encodeURIComponent(encryptedLinkVS)}&link_rs=${encodeURIComponent(encryptedLinkRS)}`;
    const response = await fetch(apiUrl, {
      method: 'PUT',
    });

    if (!response.ok) {
      throw new Error('Failed to update');
    }

    Swal.fire('Success', 'Uploaded Successfully!', 'success').then(() => {
      sessionStorage.clear();
      router.push('/');
    });
  } catch (err) {
    console.error(err);
    Swal.fire('Error', 'Failed to upload', 'error');
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

// Reactive variables for front and back ID image files and previews
const frontIdFile = ref<File | null>(null);
const backIdFile = ref<File | null>(null);
const frontIdPreview = ref<string | null>(null);
const backIdPreview = ref<string | null>(null);

// Function to initiate image upload process
const uploadImages = () => {
  if (!frontIdFile.value || !backIdFile.value) {
    Swal.fire('Error', 'Please select both front and back ID card images', 'error');
    return;
  }

  Swal.fire({
    title: 'Are you sure?',
    text: 'Do you want to upload these images?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Yes, upload it!',
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
          Swal.fire('Error', 'Failed to upload images', 'error');
        });
    }
  });
};

// Function to navigate back using Vue router
const back = () => {
  router.push('/');
}

// Function to decrypt log ID stored in sessionStorage
const decryptlogID = (encryptedItem: string | null) => {
  if (!encryptedItem) {
    return null;
  }
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY);
    const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
    return parseInt(decryptedString, 10).toString();
  } catch (e) {
    console.error('Error decrypting item:', e);
    return null;
  }
};

// Ensure security check on mount
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
  width: 300px;
  background-color: #f8f9fa;
  border: 1px dashed #ccc;
  color: #999;
  text-align: center;
}
</style>
