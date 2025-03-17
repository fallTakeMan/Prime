import { createApp } from "vue";
import { createPinia, storeToRefs } from "pinia";
import { useAuthStore } from "@/stores/User";
import App from "./App.vue";
import router from "./router";
import PrimeVue from "primevue/config";
import Aura from "@primeuix/themes/aura";
import ToastService from "primevue/toastservice";
import Tooltip from "primevue/tooltip";

import "@/assets/styles.scss";
import "@/assets/tailwind.css";

const pinia = createPinia();
const app = createApp(App);
app.use(router);
app.use(pinia);

const userStore = useAuthStore();
const { isAuthenticated } = storeToRefs(userStore);

router.beforeEach((to, from, next) => {
  if (to.name !== "login" && !isAuthenticated.value) next({ name: "login" });
  else next();
});

app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      darkModeSelector: ".app-dark",
    },
  },
});
app.use(ToastService);
app.directive("tooltip", Tooltip);
app.mount("#app");
