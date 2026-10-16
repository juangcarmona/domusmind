import { defineConfig, devices } from "@playwright/test";

const reportDirectory = process.env.PLAYWRIGHT_REPORT_DIR ?? "playwright-report";

export default defineConfig({
  testDir: "./e2e",
  testMatch: "**/*.e2e.ts",
  fullyParallel: false,
  workers: 1,
  forbidOnly: Boolean(process.env.CI),
  retries: process.env.CI ? 1 : 0,
  failOnFlakyTests: Boolean(process.env.CI),
  timeout: 90_000,
  expect: { timeout: 10_000 },
  reporter: [
    ["list"],
    ["json", { outputFile: process.env.PLAYWRIGHT_JSON ?? "test-results/results.json" }],
    ["html", { outputFolder: reportDirectory, open: "never" }],
  ],
  outputDir: process.env.PLAYWRIGHT_OUTPUT_DIR ?? "test-results/artifacts",
  use: {
    baseURL: process.env.E2E_BASE_URL ?? "http://127.0.0.1:61546",
    trace: "on",
    screenshot: "only-on-failure",
    video: "on",
  },
  projects: [{ name: "chromium", use: { ...devices["Desktop Chrome"], viewport: { width: 1440, height: 960 } } }],
});
