<template>
<div class="inner_banner_layout">
  <!-- Banner Section -->
  <div class="container">
    <div class="row">
      <div class="col-sm-12">
        <div class="inner_banner">
          <h2>{{n}}</h2>
        </div>
      </div>
    </div>
  </div>
</div>
</template>
<script setup lang="ts">
  const props = defineProps({n:String})
</script>
