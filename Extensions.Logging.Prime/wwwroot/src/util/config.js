const env = import.meta.env;

const config = {
  development: {
    baseURL: env.VITE_API_BASE_URL,
  },
  production: {
    baseURL: window.hosted_api,
  },
};

export default config[env.MODE];
