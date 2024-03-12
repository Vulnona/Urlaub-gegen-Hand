<template>
  <v-card title="User löschen">
    <v-card-text>
      <!--TODO: pass userName and userId to delete right user-->
      Möchtest du den User {Vorname} {Nachname} wirklich löschen?
    </v-card-text>

    <v-card-actions>
      <v-spacer />
      <v-btn-group>
        <v-btn
          text="Nein"
          color="primary"
          @click="cancelDelete()"
        />
        <v-btn
          text="Ja, User löschen"
          color="error"
          :loading="loading"
          :disabled="loading"
          @click="confirmDelete"
        />
      </v-btn-group>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts" setup>
import { defineEmits, ref } from 'vue'

const emit = defineEmits(['closeModal'])
const loading = ref(false)

function closeModal() {
  emit('closeModal')
}

function cancelDelete() {
  closeModal()
}

function confirmDelete() {
  /*
  * pass userName and userId to delete right user
  * loading.value = true
  * Send DELETE, wait for response 200
  * if response 200 {
  * loading.value = false
  * closeModal()
  * } else ??
  * TBD, Vorschlag: "Der Benutzer {firstName} {lastName} konnte nicht gelöscht werden."
  */
  loading.value = true
  setTimeout(() => (loading.value = false), 3000)
}

</script>
