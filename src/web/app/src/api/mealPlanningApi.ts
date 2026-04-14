import { request } from "./request";
import type {
  CreateMealPlanRequest,
  MealPlanResponse,
  GetMealPlanResponse,
  AssignMealToSlotRequest,
  AssignMealToSlotResponse,
  CreateRecipeRequest,
  RecipeResponse,
  GetFamilyRecipesResponse,
} from "./types/mealPlanningTypes";

export const mealPlanningApi = {
  /** Fetch a meal plan by family + week start date. Returns null body when no plan exists for that week. */
  getMealPlan: (familyId: string, weekStart: string) => {
    const params = new URLSearchParams({ familyId, weekStart });
    return request<GetMealPlanResponse>(`/api/meal-plans?${params}`);
  },

  /** Create a new meal plan for a given week. */
  createMealPlan: (body: CreateMealPlanRequest) =>
    request<MealPlanResponse>("/api/meal-plans", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  /** Assign (or clear) a recipe on a meal slot. */
  assignMealToSlot: (slotId: string, body: AssignMealToSlotRequest) =>
    request<AssignMealToSlotResponse>(`/api/meal-plans/${slotId}/meal`, {
      method: "PATCH",
      body: JSON.stringify(body),
    }),

  /** Get all recipes for a family. */
  getFamilyRecipes: (familyId: string) => {
    const params = new URLSearchParams({ familyId });
    return request<GetFamilyRecipesResponse>(`/api/meal-plans/recipes?${params}`);
  },

  /** Create a new recipe for a family. */
  createRecipe: (body: CreateRecipeRequest) =>
    request<RecipeResponse>("/api/meal-plans/recipes", {
      method: "POST",
      body: JSON.stringify(body),
    }),
};
