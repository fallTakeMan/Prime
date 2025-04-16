<script setup>
import { ref } from "vue";

import AppMenuItem from "./AppMenuItem.vue";

const model = ref([
  {
    label: "Home",
    items: [{ label: "Dashboard", icon: "pi pi-fw pi-home", to: "/" }],
  },
  {
    label: "Logs",
    items: [
      {
        label: "ApplicationLogs",
        icon: "pi pi-fw pi-code",
        to: "/logs/application",
      },
      {
        label: "HttpLogs",
        icon: "pi pi-fw pi-wave-pulse",
        to: "/logs/http",
        class: "rotated-icon",
      },
      {
        label: "ExceptionLogs",
        icon: "pi pi-fw pi-exclamation-circle",
        to: "/logs/exception",
      },
      {
        label: "AuditLogs",
        icon: "pi pi-fw pi-database",
        to: "/logs/audit",
      },
    ],
  },
]);
</script>

<template>
  <ul class="layout-menu">
    <template v-for="(item, i) in model" :key="item">
      <app-menu-item
        v-if="!item.separator"
        :item="item"
        :index="i"
      ></app-menu-item>
      <li v-if="item.separator" class="menu-separator"></li>
    </template>
  </ul>
</template>

<style lang="scss" scoped></style>
