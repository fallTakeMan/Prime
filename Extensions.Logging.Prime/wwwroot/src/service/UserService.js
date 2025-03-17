import request from "@/util/request";

export const UserService = {
  login(account) {
    return request({
      url: "/account/signin",
      method: "post",
      data: account,
    });
  },

  getUser(token) {
    let decodedString = decodeURIComponent(atob(token));
    return decodedString.split(":")[0];
  },
};
