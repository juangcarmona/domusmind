import { randomUUID } from "node:crypto";
import { expect, test } from "@playwright/test";

function getHouseholdWeekStart(firstDayOfWeek: string | null | undefined): string {
  const dayIndexes: Record<string, number> = {
    Sunday: 0,
    Monday: 1,
    Tuesday: 2,
    Wednesday: 3,
    Thursday: 4,
    Friday: 5,
    Saturday: 6,
  };
  const startDay = dayIndexes[firstDayOfWeek ?? "Monday"] ?? 1;
  const date = new Date();
  date.setDate(date.getDate() - ((date.getDay() - startDay + 7) % 7));
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
}

test("onboard a generated household, open today's Agenda, and navigate at desktop and phone widths", async ({ page, browser }, testInfo) => {
  test.setTimeout(180_000);
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
    const family = await familyResponse.json() as { familyId: string; firstDayOfWeek?: string | null };
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
      const detailResponse = await page.request.get(`/api/shared-lists/${list.listId}`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      expect(detailResponse.ok()).toBeTruthy();
      const detail = await detailResponse.json() as {
        items: Array<{ itemId: string; name: string; quantity: string | null; note: string | null }>;
      };
      const item = detail.items.find((candidate) => candidate.name === "Apples");
      if (!item) throw new Error("The added item was not returned by the list detail query.");
      const updateResponse = await page.request.patch(
        `/api/shared-lists/${list.listId}/items/${item.itemId}`,
        {
          headers: { Authorization: `Bearer ${accessToken}` },
          data: { name: "Apples washed", quantity: item.quantity, note: item.note },
        },
      );
      expect(updateResponse.status()).toBe(200);
      await expect(memberPage.getByText("Apples washed", { exact: true })).toBeVisible();
    });

    await test.step("propagate a checked item to the other open browser", async () => {
      const memberRow = memberPage.locator(".li-row").filter({ hasText: "Apples washed" });
      await memberRow.getByRole("button", { name: "Mark complete" }).click();
      await expect(page.getByRole("button", { name: /Show completed \(1\)/ })).toBeVisible();

      await memberPage.getByRole("button", { name: /Show completed \(1\)/ }).click();
      const checkedMemberRow = memberPage.locator(".li-row").filter({ hasText: "Apples washed" });
      await checkedMemberRow.getByRole("button", { name: "Mark incomplete" }).click();
      await expect(page.getByText("Apples washed", { exact: true })).toBeVisible();
      await expect(page.getByRole("button", { name: /Show completed/ })).toHaveCount(0);
    });

    await memberContext.close();

    await test.step("create and update a household plan using the phone Agenda interaction", async () => {
      await page.setViewportSize({ width: 390, height: 844 });
      await page.goto("/agenda");
      await page.getByRole("button", { name: "Add entry" }).click();
      await expect(page.getByRole("heading", { name: "What are you adding?" })).toBeVisible();
      await page.getByRole("button", { name: /^Plan/ }).click();
      const planTitle = `E2E phone plan ${runKey.slice(0, 8)}`;
      await page.getByLabel("Title").fill(planTitle);
      await page.getByRole("button", { name: "Schedule" }).click();
      const planEntry = page.getByRole("button", { name: new RegExp(planTitle) });
      await expect(planEntry).toBeVisible();
      await planEntry.click();
      await expect(page.getByRole("dialog")).toBeVisible();
      const editTitle = `${planTitle} updated`;
      await page.getByLabel("Title").fill(editTitle);
      await page.getByRole("button", { name: "Save" }).click();
      await expect(page.locator(".wg-item").filter({ hasText: editTitle })).toBeVisible();
    });

    await test.step("plan a meal, request its shared shopping list, and see the meal in Agenda", async () => {
      const weekStart = getHouseholdWeekStart(family.firstDayOfWeek);
      const recipeName = `E2E dinner ${runKey.slice(0, 8)}`;
      const recipeResponse = await page.request.post("/api/recipes", {
        headers: { Authorization: `Bearer ${accessToken}` },
        data: {
          recipeId: randomUUID(),
          familyId: family.familyId,
          name: recipeName,
          description: "Synthetic browser journey recipe",
          prepTimeMinutes: 10,
          cookTimeMinutes: 20,
          servings: 2,
          allowedMealTypes: ["Dinner"],
          ingredients: [
            { name: `E2E pasta ${runKey.slice(0, 8)}`, quantity: 1, unit: "pack" },
            { name: `E2E tomato ${runKey.slice(0, 8)}`, quantity: 2, unit: "cans" },
          ],
        },
      });
      expect(recipeResponse.status()).toBe(201);

      await page.setViewportSize({ width: 1280, height: 800 });
      await page.goto(`/meal-planning/${weekStart}`);
      await page.getByRole("button", { name: "Start from scratch" }).click();
      const mondayColumn = page.locator(".mp-week-grid-day-col").first();
      await mondayColumn.getByRole("button", { name: "Dinner" }).click();
      await page.getByRole("button", { name: "Recipe", exact: true }).click();
      await page.getByRole("button", { name: recipeName }).click();
      await page.getByRole("button", { name: "Save" }).click();
      await expect(page.getByText(recipeName, { exact: true })).toBeVisible();
      await page.getByRole("button", { name: "Shopping list", exact: true }).click();
      await expect(page.getByText("Shopping list ready", { exact: true })).toBeVisible();

      const mealPlanResponse = await page.request.get(
        `/api/meal-plans/family/${family.familyId}/week/${weekStart}`,
        { headers: { Authorization: `Bearer ${accessToken}` } },
      );
      expect(mealPlanResponse.ok()).toBeTruthy();
      const mealPlanResult = await mealPlanResponse.json() as {
        mealPlan: { shoppingListId: string | null } | null;
      };
      expect(mealPlanResult.mealPlan?.shoppingListId).toBeTruthy();
      await page.goto(`/lists/${mealPlanResult.mealPlan!.shoppingListId}`);
      await expect(page.getByText(`E2E pasta ${runKey.slice(0, 8)}`, { exact: true })).toBeVisible();
      await expect(page.getByText(`E2E tomato ${runKey.slice(0, 8)}`, { exact: true })).toBeVisible();

      await page.goto(`/agenda?date=${weekStart}`);
      await expect(page.getByText(new RegExp(`Dinner: ${recipeName}`))).toBeVisible();
    });
  });
});
