<template>
  <v-app>
    <v-container>
      <header>
        <header-logo />
        <header-meta-nav />
      </header>
    </v-container>
    <default-view />
  </v-app>
</template>

<script lang="ts" setup>
  import DefaultView from './View.vue'
  import HeaderMetaNav from '@/components/HeaderMetaNav.vue'
  import HeaderLogo from '@/components/HeaderLogo.vue'
</script>

<style lang="scss" scoped>
header {
  display: flex;
  justify-content: space-between;
}
</style>
