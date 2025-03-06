import {GetUserPrivileges} from '@/services/GetUserPrivileges';
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import toast from '@/components/toaster/toast';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/login',
  },
  {
    path: '/home',
    name: 'Home',
    component: () => import('@/views/Home.vue'),
  },
  {
    path: '/privileges',
    name: 'Privileges',
    component: () => import('@/views/Privileges.vue'),
  },
  {
    path: '/datenschutz',
    name: 'Datenschutzerklärung',
    component: () => import('@/views/Datenschutz.vue'),
  }, 
  {
    path: '/my-offers',
    name: 'MyOffers',
    component: () => import('@/views/MyOffers.vue'),
    meta: { roles: ['User'], membershipStatus: ['Active'], verification: ['ok'] },
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
    meta: { roles: ['User'], membershipStatus: ['Active'], verification: ['ok'] },
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
    meta: { roles: ['User'], membershipStatus: ['Active'], verification: ['ok'] },      
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
    path: '/edit-user-data',
    name: 'EditUserData',
    component: () => import('@/views/EditUserData.vue'),
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
  const privileges = GetUserPrivileges();
  console.log(privileges);
  const requiredRoles = to.meta.roles as string[];
  const requiredMembership = to.meta.membershipStatus as string[];
  const requiredVerification = to.meta.verification as string[];
  if (requiredRoles) {
    const userRole = privileges.userRole;
    const membership = privileges.membershipStatus;
    const verification = privileges.verification;
    if (!userRole) {
      // No user role means user is not logged in, redirect to login
      next({ name: 'Login' });
    } else if (!requiredRoles.includes(userRole)) {
      // User is logged in but lacks permissions
      next({ name: 'Error' });
    } else {
      // Check for verification and Memberships
      if(requiredMembership || requiredVerification){
        if(!requiredMembership.includes(membership) || !requiredVerification.includes(verification)){
            console.log(requiredVerification);
            console.log(verification);
        toast.error('Mitgliedschaft oder Verifizierung fehlt.');
        next({ name: 'Privileges' });
        return;
        }
    } 
      next(); // User is authorized, proceed
    }
  } else {
    next(); // No role required, allow access
  }

});


export default router;
