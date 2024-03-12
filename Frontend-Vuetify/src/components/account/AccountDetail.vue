<template>
  <v-row
    v-if="user"
    class="justify-center"
  >
    <v-card
      width="500"
    >
      <v-card-title>
        <h1 class="pt-4">
          Hallo {{ user.firstName }} {{ user.lastName }}
        </h1>
      </v-card-title>
      <v-card-text>
        <section>
          <h2>Persönliche Daten</h2>
          <p>
            <strong class="key">Vorname Nachname</strong>
            <span class="value">{{ user.firstName }} {{ user.lastName }}</span>
          </p>
          <p>
            <strong class="key">Geburtstag</strong>
            <span class="value">{{ user.birthDate }}</span>
          </p>
          <p>
            <strong class="key">Mail</strong>
            <span class="value">{{ user.email }}</span>
          </p>
          <p>
            <strong class="key">Telefon</strong>
            <span class="value">{{ user.phone }}</span>
          </p>
        </section>
        <section>
          <h2>Adresse</h2>
          <p>
            <strong class="key">Straße Hausnummer</strong>
            <span class="value">{{ user.address.address }}</span>
          </p>
          <p>
            <strong class="key">PLZ Ort</strong>
            <span class="value">{{ user.address.postalCode }} {{ user.address.city }}</span>
          </p>
          <p>
            <strong class="key">Bundesland / Land</strong>
            <span class="value">{{ user.address.country }} {{ user.address.state }}</span>
          </p>
        </section>
        <section />
        <h2>Konto und Sicherheit</h2>
        <p>
          <strong class="key">Benutzername</strong>
          <span class="value">{{ user.username }}</span>
        </p>
        <p>
          <v-text-field
            v-model="user.password"
            style="grid-column: span 2;"
            :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
            :type="showPassword ? 'text' : 'password'"
            label="Passwort"
            @click:append="showPassword = !showPassword"
          />
        </p>
        <v-btn-group>
          <v-btn
            color="primary"
            prepend-icon="mdi-account-edit-outline"
            @click="showEditForm=true"
          >
            Profil bearbeiten
          </v-btn>
          <v-btn
            color="error"
            type="submit"
            prepend-icon="mdi-account-remove-outline"
          >
            Profil löschen
          </v-btn>
        </v-btn-group>
      </v-card-text>
    </v-card>
    <v-dialog
      v-model="showEditForm"
      width="500"
    >
      <AccountEdit @close-modal="showEditForm = false" />
    </v-dialog>
  </v-row>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import { useAppStore } from '@/store/app'
import AccountEdit from '@/components/account/AccountEdit.vue'

const showEditForm = ref(false)
const showPassword = ref(false)
const store = useAppStore()

// const user = computed(() => store.user);
const user = {
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
}
</script>
<style lang="scss" scoped>
h1, h2 {
  margin-bottom: 1rem;
}

section + section {
  margin-top: 1rem;
}

p {
  display: grid;
  justify-content: space-between;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 0 32px;
  margin-bottom: 0.5rem;
}
</style>
