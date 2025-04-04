import { defineStore } from "pinia";

export const useAuthStore = defineStore("userAuth", {
  state: () => {
    return {
      isAuthenticated: false,
      basicToken: null,
    };
  },
  actions: {
    auth(token) {
      this.isAuthenticated = true;
      this.basicToken = token;
    },
  },
});
