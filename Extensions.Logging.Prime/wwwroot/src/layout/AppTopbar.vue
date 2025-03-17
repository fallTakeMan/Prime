<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useLayout } from "@/layout/composables/layout";
import AppConfigurator from "./AppConfigurator.vue";
import { UserService } from "@/service/UserService";
import { useAuthStore } from "@/stores/User";
import { storeToRefs } from "pinia";

const { toggleMenu, toggleDarkMode, isDarkTheme } = useLayout();

const userName = ref();
const router = useRouter();
const profilePop = ref();
const userStore = useAuthStore();
const { isAuthenticated, basicToken } = storeToRefs(userStore);

const clickProfile = (event) => {
  profilePop.value.toggle(event);
};
const logout = () => {
  isAuthenticated.value = false;
  basicToken.value = null;
  router.push({ name: "login" });
};
const getUser = () => {
  userName.value = UserService.getUser(basicToken.value);
};
onMounted(() => {
  getUser();
});
</script>

<template>
  <div class="layout-topbar">
    <div class="layout-topbar-logo-container">
      <button
        class="layout-menu-button layout-topbar-action"
        @click="toggleMenu"
      >
        <i class="pi pi-bars"></i>
      </button>
      <router-link to="/" class="layout-topbar-logo">
        <span>PrimeLogger</span>
      </router-link>
    </div>

    <div class="layout-topbar-actions">
      <div class="layout-config-menu">
        <button
          type="button"
          class="layout-topbar-action"
          @click="toggleDarkMode"
        >
          <i
            :class="['pi', { 'pi-moon': isDarkTheme, 'pi-sun': !isDarkTheme }]"
          ></i>
        </button>
        <div class="relative">
          <button
            v-styleclass="{
              selector: '@next',
              enterFromClass: 'hidden',
              enterActiveClass: 'animate-scalein',
              leaveToClass: 'hidden',
              leaveActiveClass: 'animate-fadeout',
              hideOnOutsideClick: true,
            }"
            type="button"
            class="layout-topbar-action layout-topbar-action-highlight"
          >
            <i class="pi pi-palette"></i>
          </button>
          <AppConfigurator />
        </div>
      </div>

      <button
        class="layout-topbar-menu-button layout-topbar-action"
        v-styleclass="{
          selector: '@next',
          enterFromClass: 'hidden',
          enterActiveClass: 'animate-scalein',
          leaveToClass: 'hidden',
          leaveActiveClass: 'animate-fadeout',
          hideOnOutsideClick: true,
        }"
      >
        <i class="pi pi-ellipsis-v"></i>
      </button>

      <div class="layout-topbar-menu hidden lg:block">
        <div class="layout-topbar-menu-content">
          <button
            type="button"
            class="layout-topbar-action"
            @click="clickProfile"
          >
            <i class="pi pi-user"></i>
            <span>Profile</span>
          </button>

          <Popover ref="profilePop">
            <div class="flex flex-col gap-4">
              <ul class="list-none p-0 m-0 flex flex-col">
                <li
                  :key="1"
                  class="flex items-center gap-2 px-2 py-3 rounded-border"
                >
                  <i class="pi pi-user"></i>
                  <div>
                    <span class="font-medium">{{ userName }}</span>
                  </div>
                </li>
              </ul>
              <ul class="list-none p-0 m-0 flex flex-col">
                <li
                  :key="2"
                  class="flex items-center gap-2 px-2 py-3 hover:bg-emphasis cursor-pointer rounded-border"
                  @click="logout"
                >
                  <i class="pi pi-sign-out"></i>
                  <div>
                    <span class="font-medium">Logout</span>
                  </div>
                </li>
              </ul>
            </div>
          </Popover>
        </div>
      </div>
    </div>
  </div>
</template>
