// RecipeFormPanel — inline recipe form for use inside the inspector panel.
// Used by RecipesPage for both create and edit flows.
// For the modal variant (MealPlanningPage), see RecipeFormModal.

import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import type { RecipeDetail, RecipeSummary } from "../../../api/types/mealPlanningTypes";
import type { RecipeFormData } from "./RecipeFormModal";

export type { RecipeFormData };

const MEAL_TYPES = ["Breakfast", "Lunch", "Dinner", "MidMorningSnack", "AfternoonSnack"];

interface RecipeFormPanelProps {
  /** When editing an existing recipe, provide it here. Omit for creation. */
  initial?: RecipeSummary | RecipeDetail;
  isSubmitting: boolean;
  error: string | null;
  onSubmit: (data: RecipeFormData) => void;
  onCancel: () => void;
}

export function RecipeFormPanel({
  initial,
  isSubmitting,
  error,
  onSubmit,
  onCancel,
}: RecipeFormPanelProps) {
  const { t } = useTranslation("recipeLibrary");

  const [name, setName] = useState(initial?.name ?? "");
  const [description, setDescription] = useState(initial?.description ?? "");
  const [prepTime, setPrepTime] = useState(
    initial?.prepTimeMinutes != null ? String(initial.prepTimeMinutes) : "",
  );
  const [cookTime, setCookTime] = useState(
    initial?.cookTimeMinutes != null ? String(initial.cookTimeMinutes) : "",
  );
  const [servings, setServings] = useState(
    initial?.servings != null ? String(initial.servings) : "",
  );
  const [isFavorite, setIsFavorite] = useState(initial?.isFavorite ?? false);
  const [allowedMealTypes, setAllowedMealTypes] = useState<string[]>(
    initial?.allowedMealTypes ?? [],
  );
  const [tagsRaw, setTagsRaw] = useState((initial?.tags ?? []).join(", "));

  useEffect(() => {
    setName(initial?.name ?? "");
    setDescription(initial?.description ?? "");
    setPrepTime(initial?.prepTimeMinutes != null ? String(initial.prepTimeMinutes) : "");
    setCookTime(initial?.cookTimeMinutes != null ? String(initial.cookTimeMinutes) : "");
    setServings(initial?.servings != null ? String(initial.servings) : "");
    setIsFavorite(initial?.isFavorite ?? false);
    setAllowedMealTypes(initial?.allowedMealTypes ?? []);
    setTagsRaw((initial?.tags ?? []).join(", "));
  }, [initial]);

  function toggleMealType(mt: string) {
    setAllowedMealTypes((prev) =>
      prev.includes(mt) ? prev.filter((x) => x !== mt) : [...prev, mt],
    );
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    const tags = tagsRaw
      .split(",")
      .map((s) => s.trim())
      .filter(Boolean);
    onSubmit({
      name: name.trim(),
      description: description.trim() || null,
      prepTimeMinutes: prepTime ? parseInt(prepTime, 10) : null,
      cookTimeMinutes: cookTime ? parseInt(cookTime, 10) : null,
      servings: servings ? parseInt(servings, 10) : null,
      isFavorite,
      allowedMealTypes,
      tags,
    });
  }

  const isEditing = !!initial;

  return (
    <div className="rl-form-panel">
      <div className="rl-form-panel-header">
        <h3 className="rl-form-panel-title">
          {isEditing ? t("editHeading") : t("createHeading")}
        </h3>
      </div>
      <form className="rl-form-panel-body" onSubmit={handleSubmit} noValidate>
        {/* Name */}
        <div className="rl-fp-field">
          <label className="rl-fp-label" htmlFor="rfp-name">
            {t("nameLabel")}
          </label>
          <input
            id="rfp-name"
            type="text"
            className="rl-fp-input"
            placeholder={t("namePlaceholder")}
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
            autoFocus
          />
        </div>

        {/* Description */}
        <div className="rl-fp-field">
          <label className="rl-fp-label" htmlFor="rfp-description">
            {t("descriptionLabel")}
          </label>
          <textarea
            id="rfp-description"
            className="rl-fp-input rl-fp-textarea"
            placeholder={t("descriptionPlaceholder")}
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={3}
          />
        </div>

        {/* Times + Servings */}
        <div className="rl-fp-row">
          <div className="rl-fp-field">
            <label className="rl-fp-label" htmlFor="rfp-prep">
              {t("prepTimeLabel")}
            </label>
            <input
              id="rfp-prep"
              type="number"
              className="rl-fp-input rl-fp-input--compact"
              min={0}
              value={prepTime}
              onChange={(e) => setPrepTime(e.target.value)}
            />
          </div>
          <div className="rl-fp-field">
            <label className="rl-fp-label" htmlFor="rfp-cook">
              {t("cookTimeLabel")}
            </label>
            <input
              id="rfp-cook"
              type="number"
              className="rl-fp-input rl-fp-input--compact"
              min={0}
              value={cookTime}
              onChange={(e) => setCookTime(e.target.value)}
            />
          </div>
          <div className="rl-fp-field">
            <label className="rl-fp-label" htmlFor="rfp-servings">
              {t("servingsLabel")}
            </label>
            <input
              id="rfp-servings"
              type="number"
              className="rl-fp-input rl-fp-input--compact"
              min={1}
              value={servings}
              onChange={(e) => setServings(e.target.value)}
            />
          </div>
        </div>

        {/* Allowed meal types */}
        <div className="rl-fp-field">
          <span className="rl-fp-label">{t("allowedMealTypesLabel")}</span>
          <div className="rl-fp-pill-group">
            {MEAL_TYPES.map((mt) => (
              <button
                key={mt}
                type="button"
                className={["rl-fp-pill", allowedMealTypes.includes(mt) ? "is-active" : ""]
                  .filter(Boolean)
                  .join(" ")}
                onClick={() => toggleMealType(mt)}
              >
                {mt}
              </button>
            ))}
          </div>
        </div>

        {/* Tags */}
        <div className="rl-fp-field">
          <label className="rl-fp-label" htmlFor="rfp-tags">
            {t("tagsLabel")}
          </label>
          <input
            id="rfp-tags"
            type="text"
            className="rl-fp-input"
            placeholder="e.g. quick, vegetarian"
            value={tagsRaw}
            onChange={(e) => setTagsRaw(e.target.value)}
          />
        </div>

        {/* Favourite */}
        <div className="rl-fp-field rl-fp-field--inline">
          <label className="rl-fp-label" htmlFor="rfp-favorite">
            {t("isFavoriteLabel")}
          </label>
          <input
            id="rfp-favorite"
            type="checkbox"
            checked={isFavorite}
            onChange={(e) => setIsFavorite(e.target.checked)}
          />
        </div>

        {error && <p className="rl-fp-error">{error}</p>}

        <div className="rl-fp-actions">
          <button
            type="button"
            className="btn btn-sm btn-ghost"
            onClick={onCancel}
            disabled={isSubmitting}
          >
            {t("cancelForm")}
          </button>
          <button
            type="submit"
            className="btn btn-sm btn-primary"
            disabled={isSubmitting || !name.trim()}
          >
            {t("saveRecipe")}
          </button>
        </div>
      </form>
    </div>
  );
}
