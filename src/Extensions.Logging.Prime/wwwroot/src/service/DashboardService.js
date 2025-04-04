import request from "@/util/request";

export const DashboardService = {
  getStats() {
    return request({
      url: "/dashboard/stats",
      method: "get",
    });
  },
};
