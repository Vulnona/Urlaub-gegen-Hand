import CheckUserRole from '@/services/CheckUserRole';
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
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
    meta: { roles: ['User'] },
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
    meta: { roles: ['User'] },
  },
  {
    path: '/show-more',
    name: 'ShowMore',
    component: () => import('@/views/ShowMore.vue'),
  },
  {
    path: '/offer/:id',
    name: 'OfferDetail',
    component: () => import('@/views/OfferDetail.vue'),
  },
  {
    path: '/offer-reviews/:id',
    name: 'OfferReviews',
    component: () => import('@/views/Admin/OfferReviews.vue'),
    meta: { roles: ['Admin'] },
  },
  {
    path: '/admin',
    name: 'Admin',
    component: () => import('@/views/Admin/Admin.vue'),
    meta: { roles: ['Admin'] },
  },
  {
    path: '/reviews',
    name: 'Reviews',
    component: () => import('@/views/Admin/Reviews.vue'),
    meta: { roles: ['Admin'] },
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
    meta: { roles: ['User'] },
  },
  {
    path: '/profile',
    name: 'Profile',
    component: () => import('@/views/Profile.vue'),
    meta: { roles: ['User'] },
  },
  {
    path: '/edit-profile',
    name: 'EditProfile',
    component: () => import('@/views/EditProfile.vue'),
    meta: { roles: ['User'] },
  },
  {
    path: '/upload-id',
    name: 'UploadID',
    component: () => import('@/views/UploadID.vue'),
    // meta: { roles: ['User'] },
  },
  {
    path: '/verify-email',
    name: 'VerifyEmail',
    component: () => import('@/views/VerifyEmail.vue'),
    // meta: { roles: ['User'] },
  },
  {
    path: '/store',
    name: 'Store',
    component: () => import('@/views/Store.vue'),
    meta: { roles: ['User'] },
  },
  {
    path: '/coupons',
    name: 'Coupons',
    component: () => import('@/views/Admin/Coupons.vue'),
    meta: { roles: ['Admin'] },
  },
  // {
  //   path: '/payment-confirmation',
  //   name: 'PaymentConfirmation',
  //   component: () => import('@/views/PaymentConfirmation.vue'),
  //   meta: { roles: ['User'] },
  // },
  {
    path: '/purchase-history',
    name: 'PurchaseHistory',
    component: () => import('@/views/PurchaseHistory.vue'),
    meta: { roles: ['User'] },
  },
  {
    path: '/:catchAll(.*)',
    name: 'Error',
    component: () => import('@/views/Errorpage.vue'),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

// Global role-based navigation guard
router.beforeEach((to, from, next) => {
  const userRole = CheckUserRole(); 
  const requiredRoles = to.meta.roles as string[];

  if (requiredRoles && !requiredRoles.includes(userRole || '')) {
    next({ name: 'Error' }); // Redirect to error page if role is not authorized
  } else {
    next(); // Proceed to the route
  }
});

export default router;
