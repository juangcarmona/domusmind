import { defineConfig } from "vitest/config";
import react from "@vitejs/plugin-react";

const apiTarget = process.env.API_HTTPS ?? process.env.API_HTTP;

export default defineConfig({
  plugins: [react()],
  test: {
    environment: "node",
  },
  server: apiTarget
    ? {
        proxy: {
          "/api": {
            target: apiTarget,
            changeOrigin: false,
            secure: false,
            xfwd: true,
          },
          "/signin-oidc": { target: apiTarget, changeOrigin: false, secure: false, xfwd: true },
          "/signout-callback-oidc": { target: apiTarget, changeOrigin: false, secure: false, xfwd: true },
        },
      }
    : undefined,
});
