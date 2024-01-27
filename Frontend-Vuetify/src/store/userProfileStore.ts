import axios from 'axios';
import { defineStore } from 'pinia';

interface UserProfile {
  Profile_ID: number;
  MembershipFirstActivation: string;
  UghUser: {
    User_Id: number;
    VisibleName: string;
    FirstName: string;
    LastName: string;
    DateOfBirth: Date;
    Gender: string;
    Street: string;
    HouseNumber: string;
    PostCode: string;
    City: string;
    Country: string;
    Email_Adress: string;
    IsEmailVerified: boolean;
    PasswordHash?: string;
    VerificationState: string; 
    CurrentMembership?: string; 
  };
  NickName: string | null;
}

export const useUserProfileStore = defineStore('userProfile', {
  state: () => ({
    userProfile: null as UserProfile | null,
  }),

  getters: {
    getUserProfile(): UserProfile | null {
      return this.userProfile;
    },
  },

  actions: {
    async fetchUserProfile(userId: number): Promise<void> {
      try {
        const response = await axios.get<UserProfile>(`/api/Profile/${userId}`);

        if (response.status === 200) {
          this.userProfile = response.data;
        } else {
          this.userProfile = null;
        }
      } catch (error) {
        console.error('Fehler beim Abrufen des Benutzerprofils:', error);
        this.userProfile = null;
      }
    },
  },
});