// Composables
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    component: () => import('@/layouts/default/Default.vue'),
    children: [
      {
        path: '',
        name: 'Home',
        component: () => import('@/views/Home.vue'),
      },
      {
        path: 'account',
        name: 'Account',
        component: () => import('@/views/Account.vue'),
      },
      {
        path: 'offer',
        name: 'AddOffer',
        component: () => import('@/views/AddOffer.vue'),
      },
      {
        path: 'offer/:id',
        name: 'OfferDetail',
        component: () => import('@/views/OfferDetail.vue'),
      },
    ],
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
})

export default router
