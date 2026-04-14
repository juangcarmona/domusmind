import { useState } from "react";
import { useTranslation } from "react-i18next";
import type { MealSlotResponse, RecipeResponse } from "../../../api/types/mealPlanningTypes";
import { RecipePickerPanel } from "./RecipePickerPanel";

interface SlotInspectorContentProps {
  slot: MealSlotResponse;
  recipes: RecipeResponse[];
  recipesStatus: "idle" | "loading" | "success" | "error";
  assignStatus: "idle" | "loading" | "error";
  onAssign: (slotId: string, recipeId: string | null, notes: string | null) => void;
  onCreateRecipe: () => void;
}

/**
 * SlotInspectorContent — inspector body for a selected meal slot.
 * Shows current recipe, notes editor, and the recipe picker.
 */
export function SlotInspectorContent({
  slot,
  recipes,
  recipesStatus,
  assignStatus,
  onAssign,
  onCreateRecipe,
}: SlotInspectorContentProps) {
  const { t } = useTranslation("mealPlanning");
  const [notes, setNotes] = useState(slot.notes ?? "");
  const [pendingRecipeId, setPendingRecipeId] = useState<string | null>(
    slot.recipeId,
  );

  // Reset local state when slot changes
  const slotKey = slot.id;

  function handleSave() {
    onAssign(slot.id, pendingRecipeId, notes.trim() || null);
  }

  const isDirty =
    pendingRecipeId !== slot.recipeId || notes.trim() !== (slot.notes ?? "");

  return (
    <div className="mp-slot-inspector" key={slotKey}>
      {/* Slot identity */}
      <div className="mp-slot-inspector-meta">
        <span className="mp-slot-inspector-day">
          {t(`days.${slot.dayOfWeek}` as Parameters<typeof t>[0])}
        </span>
        <span className="mp-slot-inspector-type">
          {t(`mealTypes.${slot.mealType}` as Parameters<typeof t>[0])}
        </span>
      </div>

      {/* Notes */}
      <div className="mp-form-field">
        <label className="mp-form-label" htmlFor={`slot-notes-${slot.id}`}>
          {t("slotNotes")}
        </label>
        <textarea
          id={`slot-notes-${slot.id}`}
          className="mp-form-input mp-form-textarea"
          placeholder={t("slotNotesPlaceholder")}
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          rows={2}
        />
      </div>

      {/* Recipe picker */}
      <RecipePickerPanel
        recipes={recipes}
        recipesStatus={recipesStatus}
        currentRecipeId={pendingRecipeId}
        onSelect={(id) => setPendingRecipeId(id)}
        onCreateNew={onCreateRecipe}
      />

      {/* Save action */}
      {isDirty && (
        <div className="mp-slot-inspector-actions">
          <button
            type="button"
            className="btn btn-sm btn-primary"
            onClick={handleSave}
            disabled={assignStatus === "loading"}
          >
            {t("saveSlot")}
          </button>
        </div>
      )}

      {assignStatus === "error" && (
        <p className="mp-form-error">{t("assignError")}</p>
      )}
    </div>
  );
}
