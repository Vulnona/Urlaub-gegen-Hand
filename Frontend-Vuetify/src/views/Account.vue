<template>
  <AccountDetail v-if="isLoggedIn" />
  <v-row
    v-else
    class="justify-center"
  >
    <v-card
      width="500"
    >
      <v-tabs
        v-model="accountMode"
        color="primary"
        align-tabs="center"
      >
        <v-tab :value="AccountModes.login">
          Anmelden
        </v-tab>
        <v-tab :value="AccountModes.register">
          Registrieren
        </v-tab>
      </v-tabs>

      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <AccountLogin v-if="accountMode === AccountModes.login" />
              <AccountRegister v-else-if="accountMode === AccountModes.register" />
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
    </v-card>
  </v-row>
</template>

<script lang="ts" setup>
import { computed, ref} from 'vue';
import { useAppStore} from '@/store/app';
import AccountLogin from '@/components/account/AccountLogin.vue'
import AccountRegister from '@/components/account/AccountRegister.vue'
import AccountDetail from '@/components/account/AccountDetail.vue';

enum AccountModes {
  login,
  register
}
const store = useAppStore()
const isLoggedIn = computed(() => store.isLoggedIn)
const accountMode = ref(AccountModes.login)
</script>
