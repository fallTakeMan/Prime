<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "primevue/usetoast";
import FloatingConfigurator from "@/components/FloatingConfigurator.vue";
import { UserService } from "@/service/UserService";
import { useAuthStore } from "@/stores/User";

const toast = useToast();
const router = useRouter();
const userName = ref("");
const password = ref("");
const signin = () => {
  const account = {
    UserName: userName.value,
    Password: password.value,
  };
  UserService.login(account)
    .then((resp) => {
      if (resp) {
        const userStore = useAuthStore();
        userStore.auth(resp);
        router.push({ path: "/" });
      }
    })
    .catch((error) => {
      toast.add({
        severity: "error",
        summary: "login failed",
        detail: error.message,
        life: 6000,
      });
    });
};
</script>

<template>
  <FloatingConfigurator />
  <div
    class="bg-surface-50 dark:bg-surface-950 flex items-center justify-center min-h-screen min-w-[100vw] overflow-hidden"
  >
    <div class="flex flex-col items-center justify-center">
      <div
        style="
          border-radius: 56px;
          padding: 0.3rem;
          background: linear-gradient(
            180deg,
            var(--primary-color) 10%,
            rgba(33, 150, 243, 0) 30%
          );
        "
      >
        <div
          class="w-full bg-surface-0 dark:bg-surface-900 py-20 px-8 sm:px-20"
          style="border-radius: 53px"
        >
          <div class="text-center mb-8">
            <div
              class="text-surface-900 dark:text-surface-0 text-3xl font-medium mb-4"
            >
              Welcome to PrimeLogger!
            </div>
            <span class="text-muted-color font-medium"
              >Sign in to continue</span
            >
          </div>

          <div>
            <label
              for="txtUserName"
              class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2"
              >UserName</label
            >
            <InputText
              id="txtUserName"
              type="text"
              placeholder="User Name"
              class="w-full md:w-[30rem] mb-8"
              v-model="userName"
            />

            <label
              for="txtPassword"
              class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2"
              >Password</label
            >
            <Password
              id="txtPassword"
              v-model="password"
              placeholder="Password"
              :toggleMask="true"
              class="mb-4"
              fluid
              :feedback="false"
            ></Password>

            <Button label="Sign In" class="w-full" @click="signin"></Button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
