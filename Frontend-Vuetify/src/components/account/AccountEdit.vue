<template>
  <v-card>
    <v-card-title>
      <span class="text-h5">Profil bearbeiten</span>
    </v-card-title>
    <v-card-text class="pa-0">
      <v-container>
        <h3 class="pb-2">
          Kontaktdaten bearbeiten
        </h3>
        <v-row>
          <v-col cols="12">
            <v-text-field
              :model-value="user.address.address"
              label="Straße Haus Nr."
            />
            <v-text-field
              label="Postleitzahl"
              :model-value="user.address.postalCode"
            />
            <v-text-field
              label="Stadt"
              :model-value="user.address.city"
            />
          </v-col>
        </v-row>

        <v-select
          label="Land"
          :items="countries"
          :model-value="user.address.country"
        />
        <v-autocomplete
          :items="regions"
          :model-value="user.address.state"
          label="Bundesland"
        />
        <v-text-field
          :model-value="user.phone"
          label="Telefonnummer"
        />
        <h3 class="pb-2">
          Konto bearbeiten
        </h3>
        <v-row>
          <v-col cols="12">
            <v-text-field
              label="Email"
              :model-value="user.email"
            />
            <v-text-field
              label="Passwort"
              :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
              :type="showPassword ? 'text' : 'password'"
              :model-value="user.password"
              @click:append="showPassword = !showPassword"
            />
          </v-col>
        </v-row>
      </v-container>
    </v-card-text>
    <v-card-actions>
      <v-spacer />
      <v-btn-group>
        <v-btn
          prepend-icon="mdi-delete-alert-outline"
          @click="cancelEdit()"
        >
          Verwerfen
        </v-btn>
        <v-btn
          color="primary"
          prepend-icon="mdi-content-save-check"
          @click="saveData()"
        >
          Speichern
        </v-btn>
      </v-btn-group>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts" setup>
import { defineEmits, ref } from 'vue'

const user = ref({
  id: 1,
  firstName: 'Katharina',
  lastName: 'Tan',
  email: 'atuny0@sohu.com',
  phone: '+63 791 675 8914',
  username: 'atuny0',
  password: '9uQFF1Lh',
  birthDate: '1988-02-05',
  image: 'https://robohash.org/Terry.png?set=set4',
  ip: '117.29.86.254',
  address: {
    address: '1745 T Street Southeast',
    city: 'Washington',
    postalCode: '20020',
    state: 'Bremen',
    country: 'Deutschland'
  }
})

const regions = ['Baden-Württemberg', 'Bayern', 'Berlin', 'Brandenburg', 'Bremen', 'Hamburg', 'Hessen', 'Mecklenburg-Vorpommern', 'Niedersachsen', 'Nordrhein-Westfalen', 'Rheinland-Pfalz', 'Saarland', 'Sachsen', 'Sachsen-Anhalt', 'Schleswig-Holstein', 'Thüringen']
const countries = ['Deutschland', 'Schweiz', 'Österreich', 'Anderes']
const showPassword = ref(false)

const emit = defineEmits(['closeModal'])

function closeModal() {
  emit('closeModal')
}

function cancelEdit() {
  closeModal()
}

function saveData() {
  // patch data
  closeModal()
}

</script>
