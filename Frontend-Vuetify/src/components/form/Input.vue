<template>
  <div class="form-group">   
    <label v-if='req'>{{n}}<b style="color: red;">*</b></label>
    <label v-else>{{n}}</label>
  <textarea v-model="input" type="text" class="form-control desc-textarea" :placeholder="p" v-if="t" />
  <input v-model="input" type="text" class="form-control" :placeholder="p" v-else />
</div>
</template>

<script setup lang="ts">
  import { ref, defineExpose, onMounted, onUpdated } from "vue";
  const props = defineProps({n:String, p:String, t:Boolean, req:Boolean})
  const input = defineModel()
</script>

<style scoped>
.desc-textarea {height: 90px;}
</style>
