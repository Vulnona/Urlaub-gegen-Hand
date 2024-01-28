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
    async fetchUserProfile(Profile_ID: number) {  
      console.log("fetchUserProfile called with:", Profile_ID);
      try {
        const response = await axios.get(`/api/Profile/${Profile_ID}`); 
        console.log(response.data);
        if (response.status === 200) {
          const userProfile = response.data;
          userProfile.MembershipFirstActivation = new Date(userProfile.MembershipFirstActivation);
          userProfile.UghUser.DateOfBirth = new Date(userProfile.UghUser.DateOfBirth);
          this.userProfile = userProfile;
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