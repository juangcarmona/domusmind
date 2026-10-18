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

test("onboard a generated household and cover primary workflows at desktop and phone widths", async ({ page, browser }, testInfo) => {
  test.setTimeout(600_000);
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

    await test.step("capture, review, refine, and approve Inbox proposals", async () => {
      await page.goto("/inbox");
      await page.getByLabel("What should your household remember?").fill(
        "Task: Pay summer camp fee. Due date: 2030-06-14.",
      );
      await page.getByRole("button", { name: "Save and interpret" }).click();

      const taskItem = page.locator(".inbox-item").filter({ hasText: "Task: Pay summer camp fee" });
      const taskProposal = taskItem.locator(".inbox-proposal")
        .filter({ has: page.getByRole("heading", { name: "Task proposal" }) })
        .filter({ hasText: "Pay summer camp fee" });
      await expect(taskProposal).toBeVisible({ timeout: 30_000 });
      await taskProposal.getByRole("button", { name: "Approve" }).click();
      await expect(taskItem.getByText("Applied", { exact: true })).toBeVisible();

      await page.getByLabel("What should your household remember?").fill(
        "Plan: Family camping trip at Camp Muir. Start date: 2030-06-20. End date: 2030-06-23.",
      );
      await page.getByRole("button", { name: "Save and interpret" }).click();

      const planItem = page.locator(".inbox-item").filter({ hasText: "Plan: Family camping trip at Camp Muir" });
      const planProposal = planItem.locator(".inbox-proposal").filter({ hasText: "Camp Muir" });
      await expect(planProposal).toBeVisible({ timeout: 30_000 });
      const eventsForTrip = await page.request.get(
        `/api/events?familyId=${family.familyId}&from=2030-06-20&to=2030-06-23`,
        { headers: { Authorization: `Bearer ${accessToken}` } },
      );
      expect(eventsForTrip.ok()).toBeTruthy();
      const eventsBeforeApproval = await eventsForTrip.json() as { events: Array<{ title: string }> };
      expect(eventsBeforeApproval.events.some((event) => event.title === "Family camping trip at Camp Muir")).toBe(false);
      await planProposal.getByRole("button", { name: "Refine" }).click();
      const refineForm = planItem.locator(".inbox-refine");
      const dates = refineForm.locator('input[type="date"]');
      await dates.nth(0).fill("2030-06-20");
      await dates.nth(1).fill("2030-06-23");
      await refineForm.getByRole("button", { name: "Save changes" }).click();
      await planItem.locator(".inbox-proposal").filter({ hasText: "Camp Muir" })
        .getByRole("button", { name: "Approve" }).click();
      await expect(planItem.getByText("Applied", { exact: true })).toBeVisible();
      const eventsAfterApproval = await page.request.get(
        `/api/events?familyId=${family.familyId}&from=2030-06-20&to=2030-06-23`,
        { headers: { Authorization: `Bearer ${accessToken}` } },
      );
      expect(eventsAfterApproval.ok()).toBeTruthy();
      const approvedEvents = await eventsAfterApproval.json() as { events: Array<{ title: string }> };
      expect(approvedEvents.events.some((event) => event.title === "Family camping trip at Camp Muir")).toBe(true);

      await page.getByLabel("What should your household remember?").fill(
        "Plan: Rejected river picnic. Start date: 2030-06-25. End date: 2030-06-26.",
      );
      await page.getByRole("button", { name: "Save and interpret" }).click();
      const rejectedItem = page.locator(".inbox-item").filter({ hasText: "Rejected river picnic" });
      const rejectedProposal = rejectedItem.locator(".inbox-proposal").filter({ hasText: "Rejected river picnic" });
      await expect(rejectedProposal).toBeVisible({ timeout: 30_000 });
      await rejectedProposal.getByRole("button", { name: "Reject" }).click();
      await expect(rejectedItem.locator(".inbox-proposal").getByText("Rejected", { exact: true })).toBeVisible();
      const eventsAfterRejection = await page.request.get(
        `/api/events?familyId=${family.familyId}&from=2030-06-25&to=2030-06-26`,
        { headers: { Authorization: `Bearer ${accessToken}` } },
      );
      expect(eventsAfterRejection.ok()).toBeTruthy();
      const rejectedEvents = await eventsAfterRejection.json() as { events: Array<{ title: string }> };
      expect(rejectedEvents.events.some((event) => event.title === "Rejected river picnic")).toBe(false);

      await page.getByLabel("What should your household remember?").fill(
        "Reminder: Add a reminder 30 minutes before the Camp Muir plan starts on 2030-06-20.",
      );
      await page.getByRole("button", { name: "Save and interpret" }).click();
      const reminderItem = page.locator(".inbox-item").filter({ hasText: "Camp Muir plan starts" });
      const reminderProposal = reminderItem.locator(".inbox-proposal").filter({ has: page.getByRole("heading", { name: "Reminder proposal" }) });
      await expect(reminderProposal).toBeVisible({ timeout: 30_000 });
      await reminderProposal.getByRole("button", { name: "Refine" }).click();
      const reminderForm = reminderItem.locator(".inbox-refine");
      const planId = await reminderForm.getByLabel("Plan").locator("option").filter({ hasText: "Camp Muir" }).getAttribute("value");
      expect(planId).toBeTruthy();
      await reminderForm.getByLabel("Plan").selectOption(planId!);
      await reminderForm.getByRole("button", { name: "Save changes" }).click();
      await reminderItem.locator(".inbox-proposal").getByRole("button", { name: "Approve" }).click();
      await expect(reminderItem.getByText("Applied", { exact: true })).toBeVisible();

      await page.getByLabel("What should your household remember?").fill(
        "Timeline note: The family visited Camp Muir for the first time on 2030-06-23.",
      );
      await page.getByRole("button", { name: "Save and interpret" }).click();
      const noteItem = page.locator(".inbox-item").filter({ hasText: "visited Camp Muir for the first time" });
      const noteProposal = noteItem.locator(".inbox-proposal").filter({ has: page.getByRole("heading", { name: "Timeline note proposal" }) });
      await expect(noteProposal).toBeVisible({ timeout: 30_000 });
      await noteProposal.getByRole("button", { name: "Approve" }).click();
      await expect(noteItem.getByText("Applied", { exact: true })).toBeVisible();
      await page.goto("/agenda?date=2030-06-23");
      await expect(page.getByRole("region", { name: "Timeline notes" }).getByText("First Visit to Camp Muir")).toBeVisible();

      const inboxResponse = await page.request.get(`/api/inbox?familyId=${family.familyId}`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      expect(inboxResponse.ok()).toBeTruthy();
      const inboxItems = await inboxResponse.json() as Array<{
        rawText: string;
        proposals: Array<{ status: string; resultId: string | null }>;
      }>;
      const approvedItems = inboxItems.filter((item) =>
        item.rawText.startsWith("Task: Pay summer camp fee")
        || item.rawText.startsWith("Plan: Family camping trip at Camp Muir")
        || item.rawText.startsWith("Reminder: Add a reminder 30 minutes before the Camp Muir plan starts")
        || item.rawText.startsWith("Timeline note: The family visited Camp Muir"),
      );
      expect(approvedItems).toHaveLength(4);
      expect(approvedItems.every((item) => item.proposals.some((proposal) =>
        proposal.status === "Applied" && proposal.resultId,
      ))).toBe(true);
      const rejected = inboxItems.find((item) => item.rawText.startsWith("Plan: Rejected river picnic"));
      expect(rejected?.proposals).toContainEqual(expect.objectContaining({ status: "Rejected", resultId: null }));
    });

    await test.step("use Chats with ordinary messages and an explicit @dm question", async () => {
      await page.goto("/chats");
      await page.getByRole("button", { name: "New conversation" }).click();
      await page.getByLabel("Conversation name").fill(`E2E household chat ${runKey.slice(0, 8)}`);
      await page.getByRole("button", { name: "Create", exact: true }).click();
      const composer = page.getByLabel("Message");
      await composer.fill("We should remember the camp fee this week.");
      await page.getByRole("button", { name: "Send" }).click();
      await expect(page.getByText("We should remember the camp fee this week.")).toBeVisible();
      await expect(page.locator(".chats-message.assistant")).toHaveCount(0);

      await composer.fill("@dm when is Pay summer camp fee due?");
      await page.getByRole("button", { name: "Send" }).click();
      await expect(page.locator(".chats-message.assistant")
        .getByText("The household timeline includes Pay summer camp fee.")).toBeVisible({ timeout: 30_000 });
      await testInfo.attach("chats-assistant-desktop.png", { body: await page.screenshot(), contentType: "image/png" });
    });

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
      const firstDayColumn = page.locator(".mp-week-grid-day-col").first();
      await firstDayColumn.getByRole("button", { name: "Dinner" }).click();
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

    await test.step("use household chat and explicitly invoke the read-only assistant", async () => {
      const memberDirectoryResponse = await page.request.get(`/api/families/${family.familyId}/members`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      expect(memberDirectoryResponse.ok()).toBeTruthy();
      const memberDirectory = await memberDirectoryResponse.json() as Array<{ memberId: string; name: string }>;
      const privateParticipant = memberDirectory.find((member) => member.name === "E2E Member");
      expect(privateParticipant).toBeTruthy();
      const excludedMemberResponse = await page.request.post(`/api/families/${family.familyId}/members`, {
        headers: { Authorization: `Bearer ${accessToken}` },
        data: { name: "E2E Excluded Member", role: "Adult", isManager: false },
      });
      expect(excludedMemberResponse.status()).toBe(201);
      const excludedMember = await excludedMemberResponse.json() as { memberId: string };

      await page.goto("/chats");
      await expect(page.getByRole("note")).toContainText("sent outside this deployment");
      await page.getByRole("button", { name: "New conversation" }).click();
      await page.getByLabel("Conversation name").fill(`Household chat ${runKey.slice(0, 8)}`);
      await page.getByRole("button", { name: "Create", exact: true }).click();
      await expect(page.getByRole("heading", { name: `Household chat ${runKey.slice(0, 8)}` })).toBeVisible();

      await page.getByLabel("Message").fill("Hello from the household chat.");
      await page.getByRole("button", { name: "Send" }).click();
      await expect(page.getByText("Hello from the household chat.")).toBeVisible();
      await expect(page.getByText("The household timeline includes Pay summer camp fee.")).toHaveCount(0);

      const timelineBefore = await page.request.get(`/api/families/${family.familyId}/timeline`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      expect(timelineBefore.ok()).toBeTruthy();
      const tasksBefore = ((await timelineBefore.json()) as { entries: Array<{ entryType: string }> }).entries
        .filter((entry) => entry.entryType === "Task").length;

      await page.getByLabel("Message").fill("@dm What tasks are due?");
      await page.getByRole("button", { name: "Send" }).click();
      await expect(page.getByText("The household timeline includes Pay summer camp fee.")).toBeVisible();

      await page.getByLabel("Message").fill("@dm create a task called assistant-created task");
      await page.getByRole("button", { name: "Send" }).click();
      const timelineAfter = await page.request.get(`/api/families/${family.familyId}/timeline`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      expect(timelineAfter.ok()).toBeTruthy();
      const tasksAfter = ((await timelineAfter.json()) as { entries: Array<{ entryType: string }> }).entries
        .filter((entry) => entry.entryType === "Task").length;
      expect(tasksAfter).toBe(tasksBefore);

      const threadListResponse = await page.request.get(`/api/chats?familyId=${family.familyId}`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      expect(threadListResponse.ok()).toBeTruthy();
      const householdThreads = await threadListResponse.json() as Array<{ chatThreadId: string; title: string }>;
      const householdThread = householdThreads.find((thread) => thread.title === `Household chat ${runKey.slice(0, 8)}`);
      expect(householdThread).toBeTruthy();
      const persistedMessagesResponse = await page.request.get(`/api/chats/${householdThread!.chatThreadId}/messages`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      expect(persistedMessagesResponse.ok()).toBeTruthy();
      const persistedMessages = await persistedMessagesResponse.json() as Array<{
        authorType: string; text: string; replyToMessageId: string | null;
      }>;
      expect(persistedMessages.filter((message) => message.authorType === "Assistant")).toHaveLength(2);
      expect(persistedMessages.find((message) => message.authorType === "Assistant")?.replyToMessageId).toBeTruthy();

      const privateThreadResponse = await page.request.post("/api/chats", {
        headers: { Authorization: `Bearer ${accessToken}` },
        data: {
          familyId: family.familyId,
          title: `Private group ${runKey.slice(0, 8)}`,
          kind: "Private",
          participantMemberIds: [excludedMember.memberId],
        },
      });
      expect(privateThreadResponse.status()).toBe(201);
      const privateThread = await privateThreadResponse.json() as { chatThreadId: string };
      const memberAccessToken = await memberPage.evaluate(() => localStorage.getItem("dm_access_token"));
      expect(memberAccessToken).toBeTruthy();
      const memberThreadsResponse = await memberPage.request.get(`/api/chats?familyId=${family.familyId}`, {
        headers: { Authorization: `Bearer ${memberAccessToken}` },
      });
      expect(memberThreadsResponse.ok()).toBeTruthy();
      const visibleToMember = await memberThreadsResponse.json() as Array<{ title: string }>;
      expect(visibleToMember.some((thread) => thread.title === `Household chat ${runKey.slice(0, 8)}`)).toBe(true);
      expect(visibleToMember.some((thread) => thread.title === `Private group ${runKey.slice(0, 8)}`)).toBe(false);
      const deniedMessages = await memberPage.request.get(`/api/chats/${privateThread.chatThreadId}/messages`, {
        headers: { Authorization: `Bearer ${memberAccessToken}` },
      });
      expect(deniedMessages.status()).toBe(403);
    });
    await memberContext.close();
  });
});
