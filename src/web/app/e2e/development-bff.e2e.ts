import { expect, test } from "@playwright/test";

const adminEmail = "admin@domusmind.local";
const memberEmail = "member@domusmind.local";
const developmentPassword = "DevelopmentOnly123!";
const bffCookieName = "__Host-DomusMind-Dev";

test("development BFF enforces browser session and origin boundaries through Aspire", async ({ page, browser }, testInfo) => {
  test.skip(process.env.E2E_BFF_ENABLED !== "true", "Requires the Aspire Keycloak BFF environment.");
  test.setTimeout(150_000);

  const baseURL = String(testInfo.project.use.baseURL);
  const csrf = async () => {
    const response = await page.request.get("/api/session/csrf");
    expect(response.ok()).toBeTruthy();
    return (await response.json() as { token: string }).token;
  };
  const login = async (username: string) => {
    await page.goto("/api/session/login");
    await expect(page.locator("#username")).toBeVisible();
    await page.locator("#username").fill(username);
    await page.locator("#password").fill(developmentPassword);
    await page.locator("#kc-login").click();
    await expect(page).toHaveURL(new RegExp(`^${baseURL.replace(/[.*+?^${}()|[\]\\]/g, "\\$&")}(?:/|\\?)`));
  };

  await test.step("initialize the disposable application and authenticate identity A", async () => {
    const token = await csrf();
    const initialized = await page.request.post("/api/setup/initialize", {
      data: { email: adminEmail, password: "Local-Setup-Only-7724!", displayName: "Development Admin" },
      headers: { Origin: baseURL, "X-CSRF-TOKEN": token },
    });
    expect(initialized.status()).toBe(201);

    await login("developer");
    const cookie = (await page.context().cookies()).find((item) => item.name === bffCookieName);
    expect(cookie).toBeTruthy();
    expect(cookie).toMatchObject({ httpOnly: true, secure: true, sameSite: "Lax", path: "/" });

    const me = await page.request.get("/api/auth/me");
    expect(me.ok()).toBeTruthy();
    const meBody = await me.text();
    expect(meBody).not.toMatch(/access_token|id_token|refresh_token/i);
    expect(JSON.parse(meBody)).toMatchObject({ email: adminEmail });

    const storage = await page.evaluate(() => ({
      local: Object.keys(localStorage),
      session: Object.keys(sessionStorage),
    }));
    expect(JSON.stringify(storage)).not.toMatch(/token|access|refresh|id_/i);

    const familyToken = await csrf();
    const created = await page.request.post("/api/families", {
      data: { name: "Development BFF household", primaryLanguageCode: "en" },
      headers: { Origin: baseURL, "X-CSRF-TOKEN": familyToken },
    });
    expect(created.status()).toBe(201);
    const family = await created.json() as { familyId: string };

    const csrfToken = await csrf();
    const foreignOrigin = await page.request.post(`/api/families/${family.familyId}/onboarding`, {
      data: { selfName: "Development Admin", additionalMembers: [] },
      headers: { Origin: "https://foreign.example", "X-CSRF-TOKEN": csrfToken },
    });
    expect(foreignOrigin.status()).toBe(403);

    const missingToken = await page.request.post(`/api/families/${family.familyId}/onboarding`, {
      data: { selfName: "Development Admin", additionalMembers: [] },
      headers: { Origin: baseURL },
    });
    expect(missingToken.status()).toBe(403);

    const invalidToken = await page.request.post(`/api/families/${family.familyId}/onboarding`, {
      data: { selfName: "Development Admin", additionalMembers: [] },
      headers: { Origin: baseURL, "X-CSRF-TOKEN": "invalid-token" },
    });
    expect(invalidToken.status()).toBe(403);

    const legitimateOrigin = await page.request.post(`/api/families/${family.familyId}/onboarding`, {
      data: { selfName: "Development Admin", additionalMembers: [] },
      headers: { Origin: baseURL, "X-CSRF-TOKEN": csrfToken },
    });
    expect(legitimateOrigin.ok()).toBeTruthy();
  });

  const adminCookie = (await page.context().cookies()).find((item) => item.name === bffCookieName);
  expect(adminCookie).toBeTruthy();
  if (!adminCookie)
    throw new Error("The BFF session cookie was not issued.");

  await test.step("logout revokes a copied A cookie, then switch to identity B", async () => {
    const token = await csrf();
    const logout = await page.request.post("/api/session/logout", {
      maxRedirects: 0,
      headers: { Origin: baseURL, "X-CSRF-TOKEN": token },
    });
    expect(logout.status()).toBe(302);
    const logoutLocation = logout.headers().location;
    expect(logoutLocation).toBeTruthy();
    await page.goto(new URL(logoutLocation!, baseURL).toString());

    const confirmation = page.locator("#kc-logout, input[name='accept'], button[name='accept']").first();
    if (await confirmation.count())
      await confirmation.click();

    const replayContext = await browser.newContext({ baseURL });
    await replayContext.addCookies([adminCookie]);
    const replay = await replayContext.request.get("/api/auth/me");
    expect(replay.status()).toBe(401);
    await replayContext.close();

    await login("development-member");
    const switched = await page.request.get("/api/auth/me");
    expect(switched.ok()).toBeTruthy();
    expect(await switched.json()).toMatchObject({ email: memberEmail });
  });

  await test.step("expire identity B's browser session", async () => {
    const lifetimeSeconds = Number(process.env.BFF_TEST_SESSION_LIFETIME_SECONDS ?? "30");
    await page.waitForTimeout((lifetimeSeconds + 2) * 1000);
    const expired = await page.request.get("/api/auth/me");
    expect(expired.status()).toBe(401);
  });
});
