import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

const apiTarget = process.env.API_HTTPS ?? process.env.API_HTTP;

if (!apiTarget) {
  throw new Error("API_HTTPS or API_HTTP was not provided by Aspire.");
}

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/api": {
        target: apiTarget,
        changeOrigin: true,
        secure: false,
      },
    },
  },
});