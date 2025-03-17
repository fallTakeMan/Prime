import axios from "axios";
import config from "./config";
import { storeToRefs } from "pinia";
import { useAuthStore } from "@/stores/User";

const userStore = useAuthStore();
const { isAuthenticated, basicToken } = storeToRefs(userStore);

const instance = axios.create({
  baseURL: config.baseURL,
  timeout: 6000,
});

instance.interceptors.request.use(
  (config) => {
    if (isAuthenticated.value) {
      config.headers.Authorization = `Basic ${basicToken.value}`;
    }
    return config;
  },
  (error) => {
    Promise.reject(error);
  },
);

instance.interceptors.response.use(
  (response) => {
    return response.data;
  },
  (error) => {
    return Promise.reject(error);
  },
);

export default instance;
