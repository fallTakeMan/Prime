import request from "@/util/request";

export const LogService = {
  getApplicationLogs(index = 1, size = 20) {
    return request({
      url: "/logs/app",
      method: "get",
      params: { index, size },
    });
  },

  deleteApplicationLogs(ids) {
    return request({
      url: "/logs/app",
      method: "delete",
      data: { Ids: ids },
    });
  },

  getHttpLogs(index = 1, size = 20) {
    return request({
      url: "/logs/http",
      method: "get",
      params: { index, size },
    });
  },

  deleteHttpLogs(ids) {
    return request({
      url: "/logs/http",
      method: "delete",
      data: { Ids: ids },
    });
  },

  getExceptionLogs(index = 1, size = 20) {
    return request({
      url: "/logs/exception",
      method: "get",
      params: { index, size },
    });
  },

  deleteExceptionLogs(ids) {
    return request({
      url: "/logs/exception",
      method: "delete",
      data: { Ids: ids },
    });
  },

  getAuditLogs(index = 1, size = 20) {
    return request({
      url: "logs/audit",
      method: "get",
      params: { index, size },
    });
  },

  deleteAuditLogs(ids) {
    return request({
      url: "logs/audit",
      method: "delete",
      data: { Ids: ids },
    });
  },
};
