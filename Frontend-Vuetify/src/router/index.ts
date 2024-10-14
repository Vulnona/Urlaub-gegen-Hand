import { createRouter, createWebHistory } from 'vue-router';

const routes = [
  {
    path: '/',
    redirect: '/home',
  },
  {
    path: '/home',
    name: 'Home',
    component: () => import('@/views/Home.vue'),
  },
  {
    path: '/my-offers',
    name: 'MyOffers',
    component: () => import('@/views/MyOffers.vue'),
  },
  {
    path: '/account',
    name: 'Account',
    component: () => import('@/views/Account.vue'),
  },
  {
    path: '/add-offer',
    name: 'AddOffer',
    component: () => import('@/views/AddOffer.vue'),
  },
  {
    path: '/offer/:id',
    name: 'OfferDetail',
    component: () => import('@/views/OfferDetail.vue'),
  },
  {
    path: '/admin',
    name: 'Admin',
    component: () => import('@/views/Admin.vue'),
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/components/account/AccountLogin.vue'),
    beforeEnter: (to, from, next) => {
      const token = sessionStorage.getItem('token');
      if (token) {
        next({ name: 'Home' });
      } else {
        next();
      }
    },
  },
  {
    path: '/register',
    name: 'AccountRegister',
    component: () => import('@/components/account/AccountRegister.vue'),
  },
  {
    path: '/offer-request',
    name: 'OfferRequest',
    component: () => import('@/views/OfferRequests.vue'),
  },
  {
    path: '/profile',
    name: 'Profile',
    component: () => import('@/views/Profile.vue'),
  },
  {
    path: '/edit-profile',
    name: 'editprofile',
    component: () => import('@/views/EditProfile.vue'),
  },
  {
    path: '/upload-id',
    name: 'UploadID',
    component: () => import('@/views/UploadID.vue'),
  },
  {
    path: '/verify-email',
    name: 'verifyemail',
    component: () => import('@/views/VerifyEmail.vue'),
  },
  {
    path: '/:catchAll(.*)',
    name: 'error',
    component: () => import('@/views/Errorpage.vue'),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
