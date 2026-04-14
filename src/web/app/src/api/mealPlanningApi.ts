import { request } from "./request";
import type {
  CreateMealPlanRequest,
  CreateMealPlanResponse,
  GetMealPlanResponse,
  UpdateMealSlotRequest,
  UpdateMealSlotResponse,
  CopyMealPlanFromPreviousWeekRequest,
  CopyMealPlanFromPreviousWeekResponse,
  RequestShoppingListRequest,
  RequestShoppingListResponse,
  CreateRecipeRequest,
  CreateRecipeResponse,
  GetFamilyRecipesResponse,
} from "./types/mealPlanningTypes";

export const mealPlanningApi = {
  /** Fetch a meal plan by family + week start date. Returns null mealPlan when no plan exists. */
  getMealPlan: (familyId: string, weekStart: string) =>
    request<GetMealPlanResponse>(
      `/api/meal-plans/family/${familyId}/week/${weekStart}`,
    ),

  /** Create a new meal plan for a given week. */
  createMealPlan: (body: CreateMealPlanRequest) =>
    request<CreateMealPlanResponse>("/api/meal-plans", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  /** Update a single meal slot (source type, recipe, free text, notes). */
  updateMealSlot: (
    planId: string,
    dayOfWeek: string,
    mealType: string,
    body: UpdateMealSlotRequest,
  ) =>
    request<UpdateMealSlotResponse>(
      `/api/meal-plans/${planId}/slots/${dayOfWeek}/${mealType}`,
      { method: "PUT", body: JSON.stringify(body) },
    ),

  /** Create a new meal plan by copying the previous week's plan. */
  copyFromPreviousWeek: (body: CopyMealPlanFromPreviousWeekRequest) =>
    request<CopyMealPlanFromPreviousWeekResponse>(
      "/api/meal-plans/copy-from-previous-week",
      { method: "POST", body: JSON.stringify(body) },
    ),

  /** Generate a shopping list from the meal plan's recipe slots. */
  requestShoppingList: (planId: string, body: RequestShoppingListRequest) =>
    request<RequestShoppingListResponse>(
      `/api/meal-plans/${planId}/shopping-list`,
      { method: "POST", body: JSON.stringify(body) },
    ),

  /** Get all recipes for a family. */
  getFamilyRecipes: (familyId: string) =>
    request<GetFamilyRecipesResponse>(`/api/recipes/family/${familyId}`),

  /** Create a new recipe for a family. */
  createRecipe: (body: CreateRecipeRequest) =>
    request<CreateRecipeResponse>("/api/recipes", {
      method: "POST",
      body: JSON.stringify(body),
    }),
};
