// Meal Planning surface — weekly view.
// /meal-planning              — current week
// /meal-planning/:weekStart   — specific week (ISO Monday)

import { useEffect, useState, useCallback } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import {
  fetchMealPlan,
  createMealPlan,
  assignMealToSlot,
  fetchFamilyRecipes,
  createRecipe,
  setCurrentWeek,
} from "../../../store/mealPlanningSlice";
import { useIsMobile } from "../../../hooks/useIsMobile";
import { PageHeader } from "../../../components/PageHeader";
import { InspectorPanel } from "../../../components/InspectorPanel";
import { BottomSheetDetail } from "../../../components/BottomSheetDetail";
import { EmptyStateCompact } from "../../../components/EmptyStateCompact";
import { WeekNavigator, shiftWeek } from "../components/WeekNavigator";
import { WeekGrid } from "../components/WeekGrid";
import { SlotInspectorContent } from "../components/SlotInspectorContent";
import { CreateRecipeModal } from "../components/CreateRecipeModal";
import type { MealSlotResponse } from "../../../api/types/mealPlanningTypes";
import "../meal-planning.css";

function currentMondayIso(): string {
  const d = new Date();
  const day = d.getDay();
  const diff = day === 0 ? -6 : 1 - day;
  d.setDate(d.getDate() + diff);
  return d.toISOString().slice(0, 10);
}

export function MealPlanningPage() {
  const { weekStart: routeWeekStart } = useParams<{ weekStart?: string }>();
  const navigate = useNavigate();
  const { t } = useTranslation("mealPlanning");
  const dispatch = useAppDispatch();
  const isMobile = useIsMobile();

  const family = useAppSelector((s) => s.household.family);
  const {
    currentPlan,
    planStatus,
    planError,
    currentWeekStart,
    recipes,
    recipesStatus,
    assignStatus,
    createRecipeStatus,
  } = useAppSelector((s) => s.mealPlanning);

  const [selectedSlot, setSelectedSlot] = useState<MealSlotResponse | null>(null);
  const [showCreateRecipe, setShowCreateRecipe] = useState(false);
  const [createRecipeError, setCreateRecipeError] = useState<string | null>(null);

  // Sync route param → store week
  const weekStart = routeWeekStart ?? currentMondayIso();
  useEffect(() => {
    if (weekStart !== currentWeekStart) {
      dispatch(setCurrentWeek(weekStart));
    }
  }, [weekStart, currentWeekStart, dispatch]);

  // Fetch plan whenever week or family changes
  useEffect(() => {
    if (!family?.familyId) return;
    dispatch(fetchMealPlan({ familyId: family.familyId, weekStart }));
  }, [family?.familyId, weekStart, dispatch]);

  // Fetch recipes once per family
  useEffect(() => {
    if (!family?.familyId || recipesStatus !== "idle") return;
    dispatch(fetchFamilyRecipes(family.familyId));
  }, [family?.familyId, recipesStatus, dispatch]);

  // Update selectedSlot from plan after assign (keeps inspector in sync)
  useEffect(() => {
    if (!selectedSlot || !currentPlan) return;
    const updated = currentPlan.slots.find((s) => s.id === selectedSlot.id);
    if (updated) setSelectedSlot(updated);
  }, [currentPlan]); // eslint-disable-line react-hooks/exhaustive-deps

  // ── Navigation ─────────────────────────────────────────────────────────────

  function navigateWeek(direction: "prev" | "next") {
    const next = shiftWeek(weekStart, direction);
    navigate(`/meal-planning/${next}`);
  }

  // ── Actions ────────────────────────────────────────────────────────────────

  function handleCreatePlan() {
    if (!family?.familyId) return;
    dispatch(createMealPlan({ familyId: family.familyId, weekStart }));
  }

  const handleAssign = useCallback(
    (slotId: string, recipeId: string | null, notes: string | null) => {
      dispatch(assignMealToSlot({ slotId, recipeId, notes }));
    },
    [dispatch],
  );

  function handleCreateRecipe(data: {
    name: string;
    description?: string;
    prepTimeMinutes?: number;
    cookTimeMinutes?: number;
    servings?: number;
  }) {
    if (!family?.familyId) return;
    setCreateRecipeError(null);
    dispatch(
      createRecipe({
        familyId: family.familyId,
        ...data,
      }),
    )
      .unwrap()
      .then(() => setShowCreateRecipe(false))
      .catch((err: unknown) => {
        setCreateRecipeError(
          typeof err === "string" ? err : t("createRecipeError"),
        );
      });
  }

  // ── Render ─────────────────────────────────────────────────────────────────

  const inspectorContent = selectedSlot ? (
    <SlotInspectorContent
      slot={selectedSlot}
      recipes={recipes}
      recipesStatus={recipesStatus}
      assignStatus={assignStatus}
      onAssign={handleAssign}
      onCreateRecipe={() => setShowCreateRecipe(true)}
    />
  ) : null;

  return (
    <div className="l-surface mp-surface">
      <PageHeader
        title={t("title")}
        nav={
          <WeekNavigator
            weekStart={weekStart}
            onPrev={() => navigateWeek("prev")}
            onNext={() => navigateWeek("next")}
          />
        }
      />

      <div className="l-surface-body">
        <div className="l-surface-content">
          {planStatus === "loading" && (
            <p className="mp-loading">{t("loading")}</p>
          )}

          {planStatus === "error" && planError && (
            <p className="mp-error">{planError}</p>
          )}

          {planStatus === "success" && !currentPlan && (
            <div className="mp-empty-week">
              <EmptyStateCompact
                message={t("emptyWeek")}
                action={{
                  label: t("createWeekPlan"),
                  onClick: handleCreatePlan,
                }}
              />
              {planError && <p className="mp-form-error">{t("createError")}</p>}
            </div>
          )}

          {currentPlan && (
            <WeekGrid
              plan={currentPlan}
              selectedSlotId={selectedSlot?.id ?? null}
              onSlotClick={(slot) => {
                setSelectedSlot((prev) =>
                  prev?.id === slot.id ? null : slot,
                );
              }}
            />
          )}
        </div>

        {/* Desktop inspector */}
        {!isMobile && selectedSlot && (
          <InspectorPanel
            title={t(`mealTypes.${selectedSlot.mealType}` as Parameters<typeof t>[0])}
            onClose={() => setSelectedSlot(null)}
          >
            {inspectorContent}
          </InspectorPanel>
        )}
      </div>

      {/* Mobile bottom sheet */}
      {isMobile && (
        <BottomSheetDetail
          open={!!selectedSlot}
          title={selectedSlot ? t(`mealTypes.${selectedSlot.mealType}` as Parameters<typeof t>[0]) : undefined}
          onClose={() => setSelectedSlot(null)}
        >
          {inspectorContent}
        </BottomSheetDetail>
      )}

      {/* Create recipe modal */}
      {showCreateRecipe && (
        <CreateRecipeModal
          onConfirm={handleCreateRecipe}
          onCancel={() => {
            setShowCreateRecipe(false);
            setCreateRecipeError(null);
          }}
          isSubmitting={createRecipeStatus === "loading"}
          error={createRecipeError}
        />
      )}
    </div>
  );
}
