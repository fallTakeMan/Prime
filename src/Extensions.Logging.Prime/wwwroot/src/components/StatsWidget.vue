<script setup>
import { ref, onBeforeMount } from "vue";
import { DashboardService } from "@/service/DashboardService";

const appLogRecords = ref(0);
const appLogsToday = ref(0);
const httpLogRecords = ref(0);
const httpLogsToday = ref(0);
const exceptionLogRecords = ref(0);
const exceptionLogsToday = ref(0);
const auditLogRecords = ref(0);
const auditLogsToday = ref(0);

const getStats = () => {
  DashboardService.getStats()
    .then((resp) => {
      appLogRecords.value = resp.data.appLogRecords;
      appLogsToday.value = resp.data.appLogsToday;
      httpLogRecords.value = resp.data.httpLogRecords;
      httpLogsToday.value = resp.data.httpLogsToday;
      exceptionLogRecords.value = resp.data.exceptionLogRecords;
      exceptionLogsToday.value = resp.data.exceptionLogsToday;
      auditLogRecords.value = resp.data.auditLogRecords;
      auditLogsToday.value = resp.data.auditLogsToday;
    })
    .catch((error) => {
      console.log(error);
    });
};
onBeforeMount(() => {
  getStats();
});
</script>
<template>
  <div class="col-span-12 lg:col-span-6 xl:col-span-3">
    <div class="card mb-0">
      <div class="flex justify-between mb-4">
        <div>
          <span class="block text-muted-color font-medium mb-4"
            >Application Logs</span
          >
          <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">
            {{ appLogRecords }}
          </div>
        </div>
        <div
          class="flex items-center justify-center bg-blue-100 dark:bg-blue-400/10 rounded-border"
          style="width: 2.5rem; height: 2.5rem"
        >
          <i class="pi pi-code text-blue-500 !text-xl"></i>
        </div>
      </div>
      <span class="text-primary font-medium">{{ appLogsToday }} new </span>
      <span class="text-muted-color">since today</span>
    </div>
  </div>

  <div class="col-span-12 lg:col-span-6 xl:col-span-3">
    <div class="card mb-0">
      <div class="flex justify-between mb-4">
        <div>
          <span class="block text-muted-color font-medium mb-4">Http Logs</span>
          <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">
            {{ httpLogRecords }}
          </div>
        </div>
        <div
          class="flex items-center justify-center bg-green-100 dark:bg-green-400/10 rounded-border"
          style="width: 2.5rem; height: 2.5rem"
        >
          <i class="pi pi-wave-pulse text-green-500 !text-xl"></i>
        </div>
      </div>
      <span class="text-primary font-medium">{{ httpLogsToday }} new </span>
      <span class="text-muted-color">since today</span>
    </div>
  </div>

  <div class="col-span-12 lg:col-span-6 xl:col-span-3">
    <div class="card mb-0">
      <div class="flex justify-between mb-4">
        <div>
          <span class="block text-muted-color font-medium mb-4"
            >Exception Logs</span
          >
          <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">
            {{ exceptionLogRecords }}
          </div>
        </div>
        <div
          class="flex items-center justify-center bg-red-100 dark:bg-red-400/10 rounded-border"
          style="width: 2.5rem; height: 2.5rem"
        >
          <i class="pi pi-exclamation-circle text-red-500 !text-xl"></i>
        </div>
      </div>
      <span class="text-primary font-medium"
        >{{ exceptionLogsToday }} new
      </span>
      <span class="text-muted-color">since today</span>
    </div>
  </div>

  <div class="col-span-12 lg:col-span-6 xl:col-span-3">
    <div class="card mb-0">
      <div class="flex justify-between mb-4">
        <div>
          <span class="block text-muted-color font-medium mb-4"
            >Audit Logs</span
          >
          <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">
            {{ auditLogRecords }}
          </div>
        </div>
        <div
          class="flex items-center justify-center bg-neutral-100 dark:bg-neutral-400/10 rounded-border"
          style="width: 2.5rem; height: 2.5rem"
        >
          <i class="pi pi-database text-neutral-500 !text-xl"></i>
        </div>
      </div>
      <span class="text-primary font-medium">{{ auditLogsToday }} new </span>
      <span class="text-muted-color">since today</span>
    </div>
  </div>
</template>
