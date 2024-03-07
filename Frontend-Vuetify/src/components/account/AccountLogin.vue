<template>
  <v-form
    v-model="isValid"
    :fast-fail="true"
    @submit.prevent="onSubmit"
  >
    <v-text-field
      v-model="userName"
      label="Username"
      :rules="[ruleRequired]"
    />

    <v-text-field
      v-model="password"
      :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
      :rules="[ruleRequired]"
      :type="showPassword ? 'text' : 'password'"
      label="Password"
      @click:append="showPassword = !showPassword"
    />

    <v-btn
      :disabled="!isValid"
      size="large"
      color="light-blue-darken-1"
      ripple
      type="submit"
      :block="true"
      class="mt-2"
    >
      Anmelden
    </v-btn>
    <v-alert
      v-if="errorMessage"
      :text="errorMessage"
      type="error"
    />
  </v-form>
</template>

<script lang="ts" setup>
import { Ref, ref } from "vue";
import { useAppStore } from "@/store/app";
import { isApiError, isUser } from "@/typeGuards";

const store = useAppStore();

const userName = ref("kminchelle");
const password = ref("0lelplR");
const showPassword = ref(false);
const isValid = ref(false);
const errorMessage: Ref<string | null> = ref(null);

const ruleRequired = (value: string) =>
  !!value || "Dieses Feld ist ein Pflichtfeld";

/*
 * username: kminchelle
 * password: 0lelplR
 */

function onSubmit() {
  if (!isValid.value) return;
  errorMessage.value = null;

  fetch("https://dummyjson.com/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      username: userName.value,
      password: password.value,
    }),
  })
    .then((response) => response.json())
    .then((jsonResponse) => {
      if (isUser(jsonResponse)) {
        store.user = jsonResponse;
        return;
      }
      if (isApiError(jsonResponse)) {
        errorMessage.value = jsonResponse.message;
        return;
      }
      console.error(jsonResponse);
    });
}
</script>
