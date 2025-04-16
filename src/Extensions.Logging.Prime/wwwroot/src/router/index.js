import { createRouter, createWebHashHistory } from "vue-router";

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      path: "/",
      component: () => import("@/layout/AppLayout.vue"),
      children: [
        {
          path: "/",
          name: "home",
          component: () => import("@/views/Dashboard.vue"),
        },
        {
          path: "/logs/application",
          name: "application-logs",
          component: () => import("@/views/ApplicationLogs.vue"),
        },
        {
          path: "/logs/http",
          name: "http-logs",
          component: () => import("@/views/HttpLogs.vue"),
        },
        {
          path: "/logs/exception",
          name: "exception-logs",
          component: () => import("@/views/ExceptionLogs.vue"),
        },
        {
          path: "logs/audit",
          name: "audit-logs",
          component: () => import("@/views/AuditLogs.vue"),
        },
      ],
    },
    {
      path: "/auth/login",
      name: "login",
      component: () => import("@/views/Login.vue"),
    },
  ],
});

export default router;
