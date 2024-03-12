<template>
  <v-table fixed-header>
    <thead>
      <tr>
        <th>
          Name
        </th>
        <th>
          E-Mail
        </th>
        <th>
          Code
        </th>
        <th>
          Link
        </th>
        <th>
          Status
        </th>
        <th class="text-right">
          Aktion
        </th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="entry in entries"
        :key="entry.coupon.code"
      >
        <td>{{ entry.user.firstName }} {{ entry.user.lastName }}</td>
        <td><a :href="`mailto:${entry.user.email}`">{{ entry.user.email }}</a></td>

        <td>
          {{ entry.coupon.code }}
          <v-icon
            :icon="entry.coupon.isValid ? 'mdi-check-circle-outline' : 'mdi-close-circle-outline'"
            :color="entry.coupon.isValid ? 'success' : 'error'"
          />
        </td>
        <td>
          <p>
            <a :href="`internal-url.test/uploads/perso/${entry.user.id}/front.jpg`">Vorderseite</a>
          </p>
          <p>
            <a :href="`internal-url.test/uploads/perso/${entry.user.id}/back.jpg`">Rückseite</a>
          </p>
        </td>
        <td>
          {{ entry.state }}
          <v-icon
            v-if="entry.state === VerificationState.Failed"
            icon="mdi-close-circle-outline"
            color="error"
          />
          <v-icon
            v-else-if="entry.state === VerificationState.Verified"
            icon="mdi-check-circle-outline"
            color="success"
          />
        </td>
        <td class="text-right">
          <template v-if="entry.state === VerificationState.Pending || entry.state === VerificationState.New">
            <v-btn
              color="primary"
            >
              Aktion
              <v-menu activator="parent">
                <v-list>
                  <v-list-item>
                    <v-btn
                      color="success"
                      @click="verifyUser"
                    >
                      Zulassen
                    </v-btn>
                  </v-list-item>
                  <v-list-item>
                    <v-btn
                      color="error"
                      @click="rejectUser"
                    >
                      Ablehnen
                    </v-btn>
                  </v-list-item>
                </v-list>
              </v-menu>
            </v-btn>
          </template>
          <v-btn
            v-else-if="entry.state === VerificationState.Failed"
            color="error"
            @click="deleteUser"
          >
            Löschen
          </v-btn>
        </td>
      </tr>
    </tbody>
    <v-dialog
      v-model="showDeleteModal"
      width="500"
    >
      <AdminDeleteModal @close-modal="showDeleteModal = false" />
    </v-dialog>
  </v-table>
</template>

<script lang="ts" setup>

import { UserVerification, VerificationState } from '@/types'
import { PropType, ref } from 'vue'
import AdminDeleteModal from '@/components/admin/AdminDeleteModal.vue'

const showDeleteModal = ref(false)

defineProps({
  entries: {
    type: Array as PropType<UserVerification[]>,
    required: true
  },
})

function verifyUser() {
// send user.state = verified
}

function rejectUser() {
// send user.state = failed
}

function deleteUser() {
// send DELETE user
  showDeleteModal.value = true
}

</script>
