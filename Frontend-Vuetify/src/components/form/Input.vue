<template>
  <div class="form-group">   
    <label v-if='req'>{{n}}<b style="color: red;">*</b></label>
    <label v-else>{{n}}</label>
  <textarea v-model="input" type="text" class="form-control desc-textarea" :placeholder="p" v-if="t" />
  <input v-model="input" type="text" class="form-control" :placeholder="p" v-else />
</div>
</template>

<script setup lang="ts">
  const props = defineProps({n:String, p:String, t:Boolean, req:Boolean})
  const input = defineModel()
</script>

<style scoped>
.desc-textarea {height: 90px;}

/* Placeholder Styling für hellgraue Schrift */
.desc-textarea::placeholder {
  color: #9ca3af !important; /* Hellgrau */
  opacity: 1; /* Firefox */
}

.desc-textarea::-webkit-input-placeholder {
  color: #9ca3af !important; /* Hellgrau für Webkit-Browser */
}

.desc-textarea::-moz-placeholder {
  color: #9ca3af !important; /* Hellgrau für Firefox */
  opacity: 1;
}

.desc-textarea:-ms-input-placeholder {
  color: #9ca3af !important; /* Hellgrau für IE */
}
</style>
