// ── Meal Planning API contract types ────────────────────────────────────────
// All fields are primitives (string, number, boolean). No domain types.
// Date strings are ISO "YYYY-MM-DD"; timestamps are ISO UTC strings.

// ── MealPlan ─────────────────────────────────────────────────────────────────

export interface CreateMealPlanRequest {
  familyId: string;
  weekStart: string; // "YYYY-MM-DD" (Monday)
}

export interface MealSlotResponse {
  id: string;
  dayOfWeek: string; // "Monday" | "Tuesday" | ... | "Sunday"
  mealType: string;  // "Breakfast" | "Lunch" | "Dinner" | "Snack"
  recipeId: string | null;
  recipeName: string | null;
  notes: string | null;
}

export interface MealPlanResponse {
  id: string;
  familyId: string;
  weekStart: string; // "YYYY-MM-DD"
  createdAtUtc: string;
  slots: MealSlotResponse[];
}

export interface GetMealPlanResponse {
  mealPlan: MealPlanResponse | null;
}

// ── Slot assignment ───────────────────────────────────────────────────────────

export interface AssignMealToSlotRequest {
  recipeId: string | null;
  mealType?: string;
  notes?: string | null;
}

export interface AssignMealToSlotResponse {
  slotId: string;
  mealPlanId: string;
  dayOfWeek: string;
  mealType: string;
  recipeId: string | null;
  recipeName: string | null;
  notes: string | null;
}

// ── Recipe ────────────────────────────────────────────────────────────────────

export interface CreateRecipeRequest {
  familyId: string;
  name: string;
  description?: string | null;
  prepTimeMinutes?: number | null;
  cookTimeMinutes?: number | null;
  servings?: number | null;
}

export interface RecipeResponse {
  id: string;
  familyId: string;
  name: string;
  description: string | null;
  prepTimeMinutes: number | null;
  cookTimeMinutes: number | null;
  servings: number | null;
  createdAtUtc: string;
}

export interface GetFamilyRecipesResponse {
  recipes: RecipeResponse[];
}
