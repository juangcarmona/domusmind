import { randomUUID } from "node:crypto";
import { expect, test } from "@playwright/test";

test("onboard a generated household, open today's Agenda, and navigate at desktop and phone widths", async ({ page, browser }, testInfo) => {
  const runKey = randomUUID();
  const email = `e2e-${runKey}@example.test`;
  const password = `Dm-${runKey}-A9!`;

  await test.step("reject public registration before setup", async () => {
    const response = await page.request.post("/api/auth/register", {
      data: { email, password },
    });
    expect(response.status()).toBe(403);
    expect(await response.json()).toEqual({
      code: "auth.registration_disabled",
      message: "Account registration is disabled.",
    });
  });

  await test.step("initialize and sign in with generated test data", async () => {
    await page.goto("/");
    await page.locator("#setup-email").fill(email);
    await page.locator("#setup-password").fill(password);
    await page.getByRole("button", { name: "Set up DomusMind" }).click();
    await expect(page.getByRole("heading", { name: "Welcome to DomusMind" })).toBeVisible();
  });

  await test.step("reject public registration after setup and show login without a registration path", async () => {
    const response = await page.request.post("/api/auth/register", {
      data: { email: `another-${runKey}@example.test`, password },
    });
    expect(response.status()).toBe(403);
    expect(await response.json()).toEqual({
      code: "auth.registration_disabled",
      message: "Account registration is disabled.",
    });

    const loginContext = await browser.newContext({
      baseURL: testInfo.project.use.baseURL as string,
    });
    const loginPage = await loginContext.newPage();
    await loginPage.goto("/");
    await expect(loginPage.getByRole("heading", { name: "Sign in" })).toBeVisible();
    await expect(loginPage.getByRole("button", { name: "Create one" })).toHaveCount(0);
    await loginContext.close();

    const repeatedSetup = await page.request.post("/api/setup/initialize", {
      data: { email, password },
    });
    expect(repeatedSetup.status()).toBe(409);
  });

  await test.step("create a household and add its manager and one member", async () => {
    await page.getByRole("button", { name: /English/ }).click();
    await page.getByRole("button", { name: "Continue" }).click();
    await page.getByRole("button", { name: "Start your household" }).click();
    await page.getByPlaceholder("e.g. Carmona Family").fill(`E2E Household ${runKey.slice(0, 8)}`);
    await page.getByRole("button", { name: "Create Household" }).click();
    await page.getByPlaceholder("Your name").fill("E2E Manager");
    await page.getByRole("button", { name: "Continue" }).click();
    await page.getByPlaceholder("Name").fill("E2E Member");
    await page.getByRole("button", { name: "Add", exact: true }).click();
    await page.getByRole("button", { name: "Set up household" }).click();
    await expect(page).toHaveURL(/\/agenda(?:\?.*)?$/);
    await expect(page.getByRole("complementary", { name: "Primary navigation" })).toBeVisible();
    const householdScope = page.getByRole("group", { name: "Scope" }).getByRole("button", { name: new RegExp(`E2E Household ${runKey.slice(0, 8)}`) });
    await expect(householdScope).toHaveAttribute("aria-pressed", "true");
    await expect(page.getByRole("tab", { name: "Day" })).toHaveAttribute("aria-selected", "true");
    await expect(page.getByText(/^today$/i)).toBeVisible();
    await testInfo.attach("agenda-desktop.png", { body: await page.screenshot(), contentType: "image/png" });
  });

  await test.step("navigate from Agenda to Lists on desktop", async () => {
    await page.getByRole("complementary", { name: "Primary navigation" }).getByRole("link", { name: "Lists" }).click();
    await expect(page).toHaveURL(/\/lists$/);
    await testInfo.attach("lists-desktop.png", { body: await page.screenshot(), contentType: "image/png" });
  });

  await test.step("open Agenda and navigate to Lists at phone width", async () => {
    await page.setViewportSize({ width: 390, height: 844 });
    await page.goto("/agenda");
    await expect(page.getByRole("button", { name: "Open navigation" })).toBeVisible();
    await testInfo.attach("agenda-phone.png", { body: await page.screenshot(), contentType: "image/png" });
    await page.getByRole("button", { name: "Open navigation" }).click();
    const mobileNavigation = page.getByRole("navigation", { name: "Mobile navigation" });
    await mobileNavigation.getByRole("link", { name: "Lists" }).click();
    await expect(page).toHaveURL(/\/lists$/);
    await expect(page.getByRole("button", { name: "Open navigation" })).toBeVisible();
    await testInfo.attach("lists-phone.png", { body: await page.screenshot(), contentType: "image/png" });
    const width = await page.evaluate(() => ({
      viewport: document.documentElement.clientWidth,
      content: document.documentElement.scrollWidth,
    }));
    expect(width.content).toBeLessThanOrEqual(width.viewport + 1);
  });

  await test.step("provision a member account through household settings", async () => {
    await page.goto("/settings");
    await page.getByRole("tab", { name: "People" }).click();
    const memberCard = page.locator(".item-card").filter({
      has: page.getByRole("button", { name: "E2E Member details" }),
    });
    await memberCard.getByRole("button", { name: "Grant access" }).click();
    const modal = page.locator(".modal");
    await modal.locator('input[type="email"]').fill(`member-${runKey}@example.test`);
    await modal.locator('input[type="text"]').fill("E2E provisioned member");
    await modal.getByRole("button", { name: "Grant access" }).click();
    await expect(modal.getByRole("heading", { name: "Share these credentials" })).toBeVisible();
    await expect(modal.getByText(`member-${runKey}@example.test`)).toBeVisible();
    await expect(modal.locator("strong").last()).not.toBeEmpty();

    const memberEmail = `member-${runKey}@example.test`;
    const memberPassword = await modal.locator("strong").last().innerText();
    const accessToken = await page.evaluate(() => localStorage.getItem("dm_access_token"));
    expect(accessToken).toBeTruthy();
    const familyResponse = await page.request.get("/api/families/mine", {
      headers: { Authorization: `Bearer ${accessToken}` },
    });
    expect(familyResponse.ok()).toBeTruthy();
    const family = await familyResponse.json() as { familyId: string };
    const listResponse = await page.request.post("/api/shared-lists", {
      headers: { Authorization: `Bearer ${accessToken}` },
      data: { familyId: family.familyId, name: `Live list ${runKey.slice(0, 8)}`, kind: "General" },
    });
    expect(listResponse.status()).toBe(201);
    const list = await listResponse.json() as { listId: string };

    const memberContext = await browser.newContext({
      baseURL: testInfo.project.use.baseURL as string,
    });
    const memberPage = await memberContext.newPage();
    await memberPage.goto("/");
    await memberPage.getByLabel("Email").fill(memberEmail);
    await memberPage.getByLabel("Password").fill(memberPassword);
    await memberPage.getByRole("button", { name: "Sign in" }).click();
    await expect(memberPage.getByRole("heading", { name: "Set your password" })).toBeVisible();
    const memberNewPassword = `Dm-${runKey}-B8!`;
    await memberPage.getByLabel("Temporary password").fill(memberPassword);
    await memberPage.getByLabel("New password", { exact: true }).fill(memberNewPassword);
    await memberPage.getByLabel("Confirm new password").fill(memberNewPassword);
    await memberPage.getByRole("button", { name: "Set new password" }).click();
    await expect(memberPage.getByRole("heading", { name: "Sign in" })).toBeVisible();
    await memberPage.getByLabel("Email").fill(memberEmail);
    await memberPage.getByLabel("Password").fill(memberNewPassword);
    await memberPage.getByRole("button", { name: "Sign in" }).click();
    await expect(memberPage).toHaveURL(/\/agenda(?:\?.*)?$/);
    await memberPage.goto(`/lists/${list.listId}`);
    await expect(memberPage.getByRole("textbox", { name: "Add item" })).toBeVisible();
    await page.setViewportSize({ width: 1280, height: 800 });
    await page.goto(`/lists/${list.listId}`);
    await expect(page.getByRole("textbox", { name: "Add item" })).toBeVisible();

    await test.step("propagate an added item to the other open browser without refresh", async () => {
      const addItem = page.getByRole("textbox", { name: "Add item" });
      await addItem.fill("Apples");
      await addItem.press("Enter");
      await expect(memberPage.getByText("Apples", { exact: true })).toBeVisible();
    });

    await test.step("propagate an item update to the other open browser", async () => {
      const managerRow = page.locator(".li-row").filter({ hasText: "Apples" });
      await expect(managerRow).toBeVisible();
      await managerRow.locator(".li-row__main").click();
      const itemName = page.getByRole("textbox", { name: "Item name" });
      await itemName.fill("Apples washed");
      await itemName.press("Enter");
      await expect(memberPage.getByText("Apples washed", { exact: true })).toBeVisible();
    });

    await test.step("propagate a checked item to the other open browser", async () => {
      const memberRow = memberPage.locator(".li-row").filter({ hasText: "Apples washed" });
      await memberRow.getByRole("button", { name: "Mark complete" }).click();
      await expect(page.getByRole("button", { name: /Show completed \(1\)/ })).toBeVisible();
    });

    await memberContext.close();
  });
});
