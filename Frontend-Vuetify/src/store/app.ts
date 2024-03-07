import { defineStore } from 'pinia'
import { User } from '@/types';

type AppStoreState = {
  user: User | null
}

export const useAppStore = defineStore("app", {
  state: () => ({
    user: null,
  } as AppStoreState),
  getters: {
    isLoggedIn: (state) => {
      return !!state.user;
    },
  },
});
