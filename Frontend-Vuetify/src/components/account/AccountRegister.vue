<template>
  <v-form @submit.prevent>
    <v-text-field
      v-model="firstName"
      :rules="[rules.required, rules.min]"
      :error-messages="errorMessages"
      label="Vorname"
      placeholder="Maximilian"
      required
    />
    <v-text-field
      v-model="lastName"
      :rules="[() => !!lastName || 'This field is required']"
      :error-messages="errorMessages"
      label="Nachname"
      placeholder="Mustermann"
      required
    />
    <v-text-field
      v-model="address"
      :rules="[
        () => !!address || 'This field is required',
        () => !!address && address.length <= 5 || 'Address must be less than 25 characters']"
      label="Straße + Hausnummer"
      placeholder="Musterstraße 88"
      required
    />
    <v-text-field
      v-model="zip"
      :rules="[() => !!zip || 'This field is required']"
      label="Postleitzahl"
      required
      placeholder="79938"
    />
    <v-text-field
      v-model="city"
      :rules="[() => !!city || 'This field is required']"
      label="Stadt"
      placeholder="Berlin"
      required
    />
    <v-autocomplete
      v-model="country"
      :rules="[() => !!country || 'This field is required']"
      :items="countries"
      label="Land"
      placeholder="Bitte wählen Sie Ihr Land aus"
      required
    />
    <v-select
      v-model="region"
      :rules="[() => !!region || 'This field is required']"
      :items="regions"
      label="Bundesland"
      placeholder="Bitte wählen Sie Ihr Bundesland aus"
      required
    />
    <v-btn
      size="large"
      color="light-blue-darken-1"
      ripple
      type="submit"
      block
      class="mt-2"
    >
      Submit
    </v-btn>
  </v-form>
</template>

<script lang="ts" setup>
import { ref, watch } from 'vue'

const firstName = ref('')
const lastName = ref('')
const address = ref('')
const zip = ref('')
const city = ref('')
const region = ref('')
const country = ref('')

// https://vuetifyjs.com/en/components/text-fields/#custom-validation
const regions = ['Baden-Württemberg', 'Bayern', 'Berlin', 'Brandenburg', 'Bremen', 'Hamburg', 'Hessen', 'Mecklenburg-Vorpommern', 'Niedersachsen', 'Nordrhein-Westfalen', 'Rheinland-Pfalz', 'Saarland', 'Sachsen', 'Sachsen-Anhalt', 'Schleswig-Holstein', 'Thüringen']
const countries = ['Deutschland', 'Schweiz', 'Österreich', "Anderes"]
const errorMessages = ref('')

watch(firstName, (newValue: string)=> {
  console.log(newValue)
})

const rules = {
  required: (value: string) => !!value || 'Required.',
  min: (value: string) => value.length >= 8 || 'Min 8 characters'
}
</script>
