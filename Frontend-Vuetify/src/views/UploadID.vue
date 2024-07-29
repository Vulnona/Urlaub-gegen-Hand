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
import { ref } from 'vue'; 
import { PutObjectCommand, S3Client } from '@aws-sdk/client-s3'; // AWS SDK imports for S3 operations
import Swal from 'sweetalert2'; // SweetAlert library for alerts
import router from '@/router'; // Vue router instance
import CryptoJS from 'crypto-js'; // CryptoJS library for encryption

// AWS S3 credentials and configuration
const secretAccessKey = process.env.SecretAccessKey;
const accessKeyId = process.env.AccessKeyId;
const bucket = process.env.S3_BUCKET_NAME; // S3 bucket name
const region = process.env.Aws_region; // AWS region
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
            resolve(jpegFile); // Resolve with converted JPEG file
          } else {
            reject(new Error('Conversion to JPEG failed')); // Reject if conversion fails
          }
        }, 'image/jpeg', 0.8); // Convert canvas to JPEG blob with quality 0.8
      };
      img.src = event.target.result as string; // Set image source
    };
    reader.onerror = reject; // Handle error in case of FileReader error
    reader.readAsDataURL(file); // Read uploaded file as data URL
  });
};

// Function to upload image file to AWS S3 bucket
const uploadImage = async (file: File, fileName: string) => {
  try {
    const jpegFile = await convertToJpeg(file); // Convert uploaded file to JPEG format
    const command = new PutObjectCommand({
      Bucket: bucket,
      Key: fileName,
      Body: jpegFile,
    });
    await client.send(command); // Send PutObjectCommand to upload file to S3 bucket

  } catch (err) {
    console.error(err); // Log error if upload fails
    Swal.fire('Error', 'Failed to upload image', 'error'); // Show error alert using Swal
    throw err; // Throw error to handle further in calling function
  }
};

// Function to encrypt a link using AES encryption
const encryptLink = (link: string) => {
  return CryptoJS.AES.encrypt(link, process.env.SECRET_KEY).toString(); // Encrypt link using SECRET_KEY from environment variables
};

// Function to update user links with encrypted URLs
const updateUserLinks = async (userId: string, linkVS: string, linkRS: string) => {
  try {
    const encryptedLinkVS = encryptLink(linkVS); // Encrypt linkVS
    const encryptedLinkRS = encryptLink(linkRS); // Encrypt linkRS
    const apiUrl = `${process.env.baseURL}user/upload-id/${userId}?link_vs=${encodeURIComponent(encryptedLinkVS)}&link_rs=${encodeURIComponent(encryptedLinkRS)}`;
    // Construct API URL with encrypted links
    const response = await fetch(apiUrl, {
      method: 'PUT', // HTTP PUT method for updating user links
    });

    if (!response.ok) {
      throw new Error('Failed to update user links'); // Throw error if API request fails
    }

    // Show success message and redirect on successful update
    Swal.fire('Success', 'User links updated successfully', 'success').then(() => {
      localStorage.clear(); // Clear localStorage
      router.push('/'); // Redirect to home page using Vue router
    });
  } catch (err) {
    console.error(err); // Log error if update fails
    Swal.fire('Error', 'Failed to update user links', 'error'); // Show error alert using Swal
  }
};

// Event handler for front ID image file change
const onFrontIdChange = (event: any) => {
  const file = event.target.files[0];
  frontIdFile.value = file; // Update frontIdFile ref with selected file
  frontIdPreview.value = URL.createObjectURL(file); // Create preview URL for front ID image
};

// Event handler for back ID image file change
const onBackIdChange = (event: any) => {
  const file = event.target.files[0];
  backIdFile.value = file; // Update backIdFile ref with selected file
  backIdPreview.value = URL.createObjectURL(file); // Create preview URL for back ID image
};

// Reactive variables for front and back ID image files and previews
const frontIdFile = ref<File | null>(null);
const backIdFile = ref<File | null>(null);
const frontIdPreview = ref<string | null>(null);
const backIdPreview = ref<string | null>(null);

// Function to initiate image upload process
const uploadImages = () => {
  if (!frontIdFile.value || !backIdFile.value) {
    Swal.fire('Error', 'Please select both front and back ID card images', 'error'); // Show error if both files are not selected
    return;
  }

  // Confirm upload with user
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
      const userId = decryptlogID(localStorage.getItem("logId")); // Decrypt user log ID
      const frontFileName = `${userId}VS.jpg`; // Construct front ID file name
      const backFileName = `${userId}RS.jpg`; // Construct back ID file name

      const uploadPromises = [
        uploadImage(frontIdFile.value, frontFileName), // Upload front ID image
        uploadImage(backIdFile.value, backFileName), // Upload back ID image
      ];

      // Execute all upload promises
      Promise.all(uploadPromises)
        .then(() => {
          const linkVS = `https://${bucket}.s3.${region}.amazonaws.com/${frontFileName}`; // Construct S3 URL for front ID image
          const linkRS = `https://${bucket}.s3.${region}.amazonaws.com/${backFileName}`; // Construct S3 URL for back ID image
          updateUserLinks(userId, linkVS, linkRS); // Update user links with encrypted URLs
        })
        .catch(err => {
          console.error(err); // Log error if upload promises fail
          Swal.fire('Error', 'Failed to upload images', 'error'); // Show error alert using Swal
        });
    }
  });
};

// Function to navigate back using Vue router
const back = () => {
  router.push('/'); // Navigate back to home page
}

// Function to decrypt log ID stored in localStorage
const decryptlogID = (encryptedItem: string | null) => {
  if (!encryptedItem) {
    return null; // Return null if encrypted item is not present
  }
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedItem, process.env.SECRET_KEY); // Decrypt encrypted item
    const decryptedString = bytes.toString(CryptoJS.enc.Utf8); // Convert decrypted bytes to UTF-8 string
    return parseInt(decryptedString, 10).toString(); // Parse decrypted string as integer and return
  } catch (e) {
    console.error('Error decrypting item:', e); // Log error if decryption fails
    return null; // Return null if decryption fails
  }
};


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
