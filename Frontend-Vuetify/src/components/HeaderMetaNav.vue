<template>
  <ul>
    <template v-if="isLoggedIn">
      <li>
        <RouterLink to="account">
          <v-icon icon="mdi-account-circle-outline" />
          <span class="label"> Benutzerkonto </span>
        </RouterLink>
      </li>
      <li>
        <RouterLink to="offer">
          <v-icon icon="mdi-map-plus" />
          <span class="label"> Angebot erstellen </span>
        </RouterLink>
      </li>
      <li>
        <button @click.prevent="doLogout">
          <v-icon icon="mdi-logout-variant" />
          <span class="label"> Abmelden </span>
        </button>
      </li>
    </template>
    <li v-else>
      <button @click.prevent="doLogin">
        <v-icon icon="mdi-login-variant" />
        <span class="label"> Anmelden </span>
      </button>
    </li>
  </ul>
</template>

<script lang="ts" setup>
import { computed } from "vue";
import { useAppStore } from "@/store/app";
import { useRouter } from "vue-router";

const { push } = useRouter();
const store = useAppStore();
const isLoggedIn = computed(() => store.isLoggedIn);

function doLogout() {
  store.user = null;
}

function doLogin() {
  push({ name: "Account" });
}
</script>

<style lang="scss" scoped>
ul {
  list-style: none;
  display: flex;
  gap: 1rem;
}

.label {
  @media screen and (max-width: 959px) {
    visibility: hidden;
    display: block;
    width: 0;
    height: 0;
  }
}
</style>
