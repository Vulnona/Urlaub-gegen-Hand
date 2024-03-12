<template>
  <div class="wrapper">
    <AdminTable :entries="users" />
  </div>
</template>

<script lang="ts" setup>

import AdminTable from '@/components/admin/AdminTable.vue'
import { UserVerification, VerificationState } from '@/types'

const users:UserVerification[] = [
  {
    user: {
      id: "7877fea7",
      firstName: "Candy",
      lastName: "Sugar",
      email: "info@gmail.com",
    },
    coupon: {
      code: "1075502c5aad",
      isValid: true
    },
    state: VerificationState.Pending,
  },
  {
    user: {
      id: "d5bcaf4d",
      firstName: "Ginger",
      lastName: "Bread",
      email: "info@gmail.com",
    },
    coupon: {
      code: "5362c71c9998",
      isValid: true
    },
    state: VerificationState.Pending,
  },
  {
    user: {
      id: "c8d3ba16",
      firstName: "Honey",
      lastName: "Melone",
      email: "info@gmail.com",
    },
    coupon: {
      code: "fef70ea12710",
      isValid: true
    },
    state: VerificationState.Failed,
  },
  {
    user: {
      id: "da227b90",
      firstName: "KittyKat",
      lastName: "Donut",
      email: "info@gmail.com",
    },
    coupon: {
      code: "c15c43627da7",
      isValid: false
    },
    state: VerificationState.Failed,
  },
  {
    user: {
      id: "c42fdade",
      firstName: "Sweet Cupcake",
      lastName: "Chocolate",
      email: "info@gmail.com",
    },
    coupon: {
      code: "6319be104ea3",
      isValid: true
    },
    state: VerificationState.Verified,
  }
]
</script>
<style lang="scss" scoped>
@use "@/styles/wrapper";
.wrapper {
  padding-inline: wrapper.$inline-space;
  max-inline-size: wrapper.$max-inline-size;
  margin-inline:  auto;
}
</style>
