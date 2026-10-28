import { expect, test } from "@playwright/test";

const adminEmail = "admin@domusmind.local";
const memberEmail = "member@domusmind.local";
const developmentPassword = "DevelopmentOnly123!";
const bffCookieName = "__Host-DomusMind-Dev";

test("development BFF enforces browser session and origin boundaries through Aspire", async ({
  page,
  browser,
}, testInfo) => {
  test.skip(
    process.env.E2E_BFF_ENABLED !== "true",
    "Requires the Aspire Keycloak BFF environment.",
  );
  test.setTimeout(150_000);

  const baseURL = String(testInfo.project.use.baseURL);
  const baseOrigin = new URL(baseURL).origin;
  const tokenLikeResponseBodies: Promise<void>[] = [];
  let authorizationUrl: URL | null = null;
  page.on("request", (request) => {
    const url = new URL(request.url());
    if (url.pathname.endsWith("/protocol/openid-connect/auth")) {
      authorizationUrl = url;
    }
  });
  page.on("response", (response) => {
    const contentType = response.headers()["content-type"] ?? "";
    if (
      new URL(response.url()).origin !== baseOrigin ||
      !contentType.includes("application/json")
    )
      return;
    tokenLikeResponseBodies.push(
      (async () => {
        try {
          const body = await response.text();
          expect(body).not.toMatch(
            /access[_-]?token|id[_-]?token|refresh[_-]?token|eyJ[\w-]{10,}\.[\w-]{10,}\./i,
          );
        } catch (error) {
          // Responses canceled by a later navigation are not exposed to browser JavaScript.
          if (
            error instanceof Error &&
            /No resource with given identifier found|Response body is not available for a response that was navigated away from/.test(
              error.message,
            )
          )
            return;
          throw error;
        }
      })(),
    );
  });
  const csrf = async () => {
    const response = await page.request.get("/api/session/csrf");
    expect(response.ok()).toBeTruthy();
    return ((await response.json()) as { token: string }).token;
  };
  const login = async (username: string) => {
    authorizationUrl = null;
    await page.goto("/api/session/login");
    const authorizationParameters = authorizationUrl?.searchParams;
    if (!authorizationParameters)
      throw new Error("The OIDC authorization request was not observed.");
    expect(authorizationParameters.get("request_uri")).toMatch(/^urn:/);
    expect(authorizationParameters.get("client_id")).toBe("domusmind-dev");
    await expect(page.locator("#username")).toBeVisible();
    await page.locator("#username").fill(username);
    await page.locator("#password").fill(developmentPassword);
    await page.locator("#kc-login").click();
    await expect(page).toHaveURL(
      new RegExp(`^${baseURL.replace(/[.*+?^${}()|[\]\\]/g, "\\$&")}(?:/|\\?)`),
    );
  };

  await test.step("initialize the disposable application and authenticate identity A", async () => {
    const token = await csrf();
    const initialized = await page.request.post("/api/setup/initialize", {
      data: {
        email: adminEmail,
        password: "Local-Setup-Only-7724!",
        displayName: "Development Admin",
      },
      headers: { Origin: baseURL, "X-CSRF-TOKEN": token },
    });
    expect(initialized.status()).toBe(201);

    const callbackContext = await browser.newContext({ baseURL });
    const invalidCallback = await callbackContext.request.get(
      "/signin-oidc?code=invalid&state=forged",
    );
    expect(invalidCallback.status()).toBeGreaterThanOrEqual(400);
    expect((await callbackContext.request.get("/api/auth/me")).status()).toBe(
      401,
    );
    await callbackContext.close();

    await login("developer");
    const cookie = (await page.context().cookies()).find(
      (item) => item.name === bffCookieName,
    );
    expect(cookie).toBeTruthy();
    expect(cookie).toMatchObject({
      httpOnly: true,
      secure: true,
      sameSite: "Lax",
      path: "/",
    });

    const me = await page.evaluate(async () => {
      const response = await fetch("/api/auth/me");
      return {
        ok: response.ok,
        body: (await response.json()) as { email: string },
      };
    });
    expect(me.ok).toBeTruthy();
    expect(me.body).toMatchObject({ email: adminEmail });

    const storage = await page.evaluate(async () => {
      const databases = await indexedDB.databases();
      const indexedDb = await Promise.all(
        databases.map(async ({ name }) => {
          if (!name) return [];
          const database = await new Promise<IDBDatabase>((resolve, reject) => {
            const request = indexedDB.open(name);
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
          });
          const storeContents = await Promise.all(
            Array.from(database.objectStoreNames, async (storeName) => {
              const store = database
                .transaction(storeName, "readonly")
                .objectStore(storeName);
              const [keys, values] = await Promise.all([
                new Promise<IDBValidKey[]>((resolve, reject) => {
                  const request = store.getAllKeys();
                  request.onsuccess = () => resolve(request.result);
                  request.onerror = () => reject(request.error);
                }),
                new Promise<unknown[]>((resolve, reject) => {
                  const request = store.getAll();
                  request.onsuccess = () => resolve(request.result);
                  request.onerror = () => reject(request.error);
                }),
              ]);
              return { keys, values };
            }),
          );
          database.close();
          return storeContents;
        }),
      );
      return {
        local: Object.entries(localStorage),
        session: Object.entries(sessionStorage),
        indexedDb,
      };
    });
    expect(JSON.stringify(storage)).not.toMatch(
      /access[_-]?token|id[_-]?token|refresh[_-]?token|eyJ[\w-]{10,}\.[\w-]{10,}\./i,
    );

    const familyToken = await csrf();
    const created = await page.request.post("/api/families", {
      data: { name: "Development BFF household", primaryLanguageCode: "en" },
      headers: { Origin: baseURL, "X-CSRF-TOKEN": familyToken },
    });
    expect(created.status()).toBe(201);
    const family = (await created.json()) as { familyId: string };

    const csrfToken = await csrf();
    const foreignOrigin = await page.request.post(
      `/api/families/${family.familyId}/onboarding`,
      {
        data: { selfName: "Development Admin", additionalMembers: [] },
        headers: {
          Origin: "https://foreign.example",
          "X-CSRF-TOKEN": csrfToken,
        },
      },
    );
    expect(foreignOrigin.status()).toBe(403);

    const missingToken = await page.request.post(
      `/api/families/${family.familyId}/onboarding`,
      {
        data: { selfName: "Development Admin", additionalMembers: [] },
        headers: { Origin: baseURL },
      },
    );
    expect(missingToken.status()).toBe(403);

    const invalidToken = await page.request.post(
      `/api/families/${family.familyId}/onboarding`,
      {
        data: { selfName: "Development Admin", additionalMembers: [] },
        headers: { Origin: baseURL, "X-CSRF-TOKEN": "invalid-token" },
      },
    );
    expect(invalidToken.status()).toBe(403);

    const legitimateOrigin = await page.request.post(
      `/api/families/${family.familyId}/onboarding`,
      {
        data: { selfName: "Development Admin", additionalMembers: [] },
        headers: { Origin: baseURL, "X-CSRF-TOKEN": csrfToken },
      },
    );
    expect(legitimateOrigin.ok()).toBeTruthy();

    await page.goto("/agenda");
    await expect(
      page.getByRole("complementary", { name: "Primary navigation" }),
    ).toBeVisible();
    await page.setViewportSize({ width: 390, height: 844 });
    const viewport = await page.evaluate(() => ({
      width: document.documentElement.clientWidth,
      contentWidth: document.documentElement.scrollWidth,
    }));
    expect(viewport.contentWidth).toBeLessThanOrEqual(viewport.width + 1);

    await page.emulateMedia({ colorScheme: "light" });
    await page.evaluate(() => localStorage.setItem("dm_theme", "light"));
    await page.reload();
    await expect(page.locator("html")).toHaveAttribute("data-theme", "light");
    const lightCanvas = await page.evaluate(() =>
      getComputedStyle(document.documentElement)
        .getPropertyValue("--bg")
        .trim(),
    );

    await page.evaluate(() => localStorage.setItem("dm_theme", "dark"));
    await page.reload();
    await expect(page.locator("html")).toHaveAttribute("data-theme", "dark");
    const darkCanvas = await page.evaluate(() =>
      getComputedStyle(document.documentElement)
        .getPropertyValue("--bg")
        .trim(),
    );
    expect(darkCanvas).not.toBe(lightCanvas);

    await page.evaluate(() => localStorage.setItem("dm_theme", "system"));
    await page.emulateMedia({ colorScheme: "dark" });
    await page.reload();
    await expect(page.locator("html")).toHaveAttribute("data-theme", "dark");
    await page.emulateMedia({ colorScheme: "light" });
    await expect(page.locator("html")).toHaveAttribute("data-theme", "light");

    await page.emulateMedia({ reducedMotion: "reduce" });
    const transitionDuration = await page
      .locator(".theme-toggle")
      .evaluate((button) =>
        getComputedStyle(button).transitionDuration.split(",")[0].trim(),
      );
    expect(Number.parseFloat(transitionDuration)).toBeLessThan(0.001);
    await page.keyboard.press("Tab");
    const focusVisible = await page.evaluate(() => {
      const active = document.activeElement;
      return Boolean(
        active?.matches(":focus-visible") &&
        Number.parseFloat(getComputedStyle(active).outlineWidth) > 0,
      );
    });
    expect(focusVisible).toBeTruthy();
  });

  const adminCookie = (await page.context().cookies()).find(
    (item) => item.name === bffCookieName,
  );
  expect(adminCookie).toBeTruthy();
  if (!adminCookie) throw new Error("The BFF session cookie was not issued.");

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

    const confirmation = page
      .locator("#kc-logout, input[name='accept'], button[name='accept']")
      .first();
    if (await confirmation.count()) await confirmation.click();

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
    const lifetimeSeconds = Number(
      process.env.BFF_TEST_SESSION_LIFETIME_SECONDS ?? "30",
    );
    await page.waitForTimeout((lifetimeSeconds + 2) * 1000);
    const expired = await page.request.get("/api/auth/me");
    expect(expired.status()).toBe(401);
  });

  await Promise.all(tokenLikeResponseBodies);
});
