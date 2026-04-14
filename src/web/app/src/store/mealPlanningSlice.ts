import { createSlice, createAsyncThunk, type PayloadAction } from "@reduxjs/toolkit";
import { mealPlanningApi } from "../api/mealPlanningApi";
import type {
  MealPlanResponse,
  MealSlotResponse,
  RecipeResponse,
} from "../api/types/mealPlanningTypes";

// ── State ─────────────────────────────────────────────────────────────────────

interface MealPlanningState {
  // Current week's meal plan (null = no plan exists for this week)
  currentPlan: MealPlanResponse | null;
  planStatus: "idle" | "loading" | "success" | "error";
  planError: string | null;

  // Currently displayed week (ISO "YYYY-MM-DD", always a Monday)
  currentWeekStart: string;

  // Family recipes library
  recipes: RecipeResponse[];
  recipesStatus: "idle" | "loading" | "success" | "error";

  // Slot mutation
  assignStatus: "idle" | "loading" | "error";

  // Recipe creation
  createRecipeStatus: "idle" | "loading" | "error";
}

function currentMondayIso(): string {
  const d = new Date();
  const day = d.getDay(); // 0=Sun, 1=Mon …
  const diff = day === 0 ? -6 : 1 - day;
  d.setDate(d.getDate() + diff);
  return d.toISOString().slice(0, 10);
}

const initialState: MealPlanningState = {
  currentPlan: null,
  planStatus: "idle",
  planError: null,
  currentWeekStart: currentMondayIso(),
  recipes: [],
  recipesStatus: "idle",
  assignStatus: "idle",
  createRecipeStatus: "idle",
};

// ── Thunks ────────────────────────────────────────────────────────────────────

export const fetchMealPlan = createAsyncThunk(
  "mealPlanning/fetchPlan",
  async (
    { familyId, weekStart }: { familyId: string; weekStart: string },
    { rejectWithValue },
  ) => {
    try {
      const res = await mealPlanningApi.getMealPlan(familyId, weekStart);
      return res.mealPlan;
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to load meal plan",
      );
    }
  },
);

export const createMealPlan = createAsyncThunk(
  "mealPlanning/createPlan",
  async (
    { familyId, weekStart }: { familyId: string; weekStart: string },
    { rejectWithValue },
  ) => {
    try {
      await mealPlanningApi.createMealPlan({ familyId, weekStart });
      // Fetch the full plan with slots after creation
      const res = await mealPlanningApi.getMealPlan(familyId, weekStart);
      return res.mealPlan;
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to create meal plan",
      );
    }
  },
);

export const assignMealToSlot = createAsyncThunk(
  "mealPlanning/assignSlot",
  async (
    {
      slotId,
      recipeId,
      mealType,
      notes,
    }: {
      slotId: string;
      recipeId: string | null;
      mealType?: string;
      notes?: string | null;
    },
    { rejectWithValue },
  ) => {
    try {
      return await mealPlanningApi.assignMealToSlot(slotId, {
        recipeId,
        mealType,
        notes,
      });
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to assign meal",
      );
    }
  },
);

export const fetchFamilyRecipes = createAsyncThunk(
  "mealPlanning/fetchRecipes",
  async (familyId: string, { rejectWithValue }) => {
    try {
      const res = await mealPlanningApi.getFamilyRecipes(familyId);
      return res.recipes;
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to load recipes",
      );
    }
  },
);

export const createRecipe = createAsyncThunk(
  "mealPlanning/createRecipe",
  async (
    {
      familyId,
      name,
      description,
      prepTimeMinutes,
      cookTimeMinutes,
      servings,
    }: {
      familyId: string;
      name: string;
      description?: string | null;
      prepTimeMinutes?: number | null;
      cookTimeMinutes?: number | null;
      servings?: number | null;
    },
    { rejectWithValue },
  ) => {
    try {
      return await mealPlanningApi.createRecipe({
        familyId,
        name,
        description,
        prepTimeMinutes,
        cookTimeMinutes,
        servings,
      });
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to create recipe",
      );
    }
  },
);

// ── Slice ─────────────────────────────────────────────────────────────────────

const mealPlanningSlice = createSlice({
  name: "mealPlanning",
  initialState,
  reducers: {
    setCurrentWeek(state, action: PayloadAction<string>) {
      state.currentWeekStart = action.payload;
      // Clear current plan so the page re-fetches for the new week
      state.currentPlan = null;
      state.planStatus = "idle";
      state.planError = null;
    },
    clearPlan(state) {
      state.currentPlan = null;
      state.planStatus = "idle";
      state.planError = null;
    },
  },
  extraReducers: (builder) => {
    // ── fetchMealPlan ─────────────────────────────────────────────────────────
    builder
      .addCase(fetchMealPlan.pending, (state) => {
        state.planStatus = "loading";
        state.planError = null;
      })
      .addCase(fetchMealPlan.fulfilled, (state, action) => {
        state.planStatus = "success";
        state.currentPlan = action.payload;
      })
      .addCase(fetchMealPlan.rejected, (state, action) => {
        state.planStatus = "error";
        state.planError = action.payload as string;
      });

    // ── createMealPlan ────────────────────────────────────────────────────────
    builder
      .addCase(createMealPlan.pending, (state) => {
        state.planStatus = "loading";
        state.planError = null;
      })
      .addCase(createMealPlan.fulfilled, (state, action) => {
        state.planStatus = "success";
        state.currentPlan = action.payload;
      })
      .addCase(createMealPlan.rejected, (state, action) => {
        state.planStatus = "error";
        state.planError = action.payload as string;
      });

    // ── assignMealToSlot ──────────────────────────────────────────────────────
    builder
      .addCase(assignMealToSlot.pending, (state) => {
        state.assignStatus = "loading";
      })
      .addCase(assignMealToSlot.fulfilled, (state, action) => {
        state.assignStatus = "idle";
        // Patch the slot in current plan
        if (state.currentPlan) {
          const slot = state.currentPlan.slots.find(
            (s: MealSlotResponse) => s.id === action.payload.slotId,
          );
          if (slot) {
            slot.recipeId = action.payload.recipeId;
            slot.recipeName = action.payload.recipeName;
            slot.mealType = action.payload.mealType;
            slot.notes = action.payload.notes;
          }
        }
      })
      .addCase(assignMealToSlot.rejected, (state) => {
        state.assignStatus = "error";
      });

    // ── fetchFamilyRecipes ────────────────────────────────────────────────────
    builder
      .addCase(fetchFamilyRecipes.pending, (state) => {
        state.recipesStatus = "loading";
      })
      .addCase(fetchFamilyRecipes.fulfilled, (state, action) => {
        state.recipesStatus = "success";
        state.recipes = action.payload;
      })
      .addCase(fetchFamilyRecipes.rejected, (state) => {
        state.recipesStatus = "error";
      });

    // ── createRecipe ──────────────────────────────────────────────────────────
    builder
      .addCase(createRecipe.pending, (state) => {
        state.createRecipeStatus = "loading";
      })
      .addCase(createRecipe.fulfilled, (state, action) => {
        state.createRecipeStatus = "idle";
        state.recipes = [action.payload, ...state.recipes];
      })
      .addCase(createRecipe.rejected, (state) => {
        state.createRecipeStatus = "error";
      });
  },
});

export const { setCurrentWeek, clearPlan } = mealPlanningSlice.actions;
export default mealPlanningSlice.reducer;
