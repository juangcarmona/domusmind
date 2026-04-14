import { useTranslation } from "react-i18next";
import type { MealSlotResponse } from "../../../api/types/mealPlanningTypes";

const MEAL_TYPE_ORDER = ["Breakfast", "Lunch", "Dinner", "Snack"] as const;

interface MealSlotCellProps {
  slot: MealSlotResponse;
  selected: boolean;
  onClick: (slot: MealSlotResponse) => void;
}

/**
 * MealSlotCell — a single meal-type cell within a day column in the week grid.
 * Pure/presentational: emits onClick, no dispatch.
 */
export function MealSlotCell({ slot, selected, onClick }: MealSlotCellProps) {
  const { t } = useTranslation("mealPlanning");
  const hasRecipe = !!slot.recipeId;

  return (
    <button
      type="button"
      className={[
        "mp-slot-cell",
        hasRecipe ? "mp-slot-cell--filled" : "mp-slot-cell--empty",
        selected ? "mp-slot-cell--selected" : "",
      ]
        .filter(Boolean)
        .join(" ")}
      onClick={() => onClick(slot)}
      aria-pressed={selected}
      title={
        hasRecipe
          ? slot.recipeName ?? t("assignedRecipe")
          : t("slotEmpty")
      }
    >
      <span className="mp-slot-cell-type">
        {t(`mealTypes.${slot.mealType}` as Parameters<typeof t>[0])}
      </span>
      {hasRecipe ? (
        <span className="mp-slot-cell-recipe">{slot.recipeName}</span>
      ) : (
        <span className="mp-slot-cell-empty">{t("noRecipeAssigned")}</span>
      )}
      {slot.notes && <span className="mp-slot-cell-notes">{slot.notes}</span>}
    </button>
  );
}

export { MEAL_TYPE_ORDER };
