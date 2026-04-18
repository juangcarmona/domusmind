// Recipe Library surface — full household recipe collection.
// /recipes — view and manage the household recipe library.

import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import {
  fetchRecipes,
  fetchRecipeDetail,
  updateRecipe,
  deleteRecipe,
  clearSelectedRecipe,
  clearUpdateError,
  clearDeleteError,
} from "../../../store/recipeLibrarySlice";
import { createRecipe } from "../../../store/mealPlanningSlice";
import { useIsMobile } from "../../../hooks/useIsMobile";
import { InspectorPanel } from "../../../components/InspectorPanel";
import { BottomSheetDetail } from "../../../components/BottomSheetDetail";
import { RecipeDetailPanel } from "../components/RecipeDetailPanel";
import { RecipeFormPanel } from "../components/RecipeFormPanel";
import type { RecipeFormData } from "../components/RecipeFormModal";
import type { RecipeSummary } from "../../../api/types/mealPlanningTypes";
import "../recipe-library.css";

type InspectorMode = "detail" | "edit" | "create" | null;

export function RecipesPage() {
  const { t } = useTranslation("recipeLibrary");
  const dispatch = useAppDispatch();
  const isMobile = useIsMobile();

  const family = useAppSelector((s) => s.household.family);
  const {
    recipes,
    listStatus,
    selectedRecipe,
    detailStatus,
    updateStatus,
    updateError,
    deleteStatus,
    deleteError,
  } = useAppSelector((s) => s.recipeLibrary);

  const [search, setSearch] = useState("");
  const [selectedId, setSelectedId] = useState<string | null>(null);
  const [inspectorMode, setInspectorMode] = useState<InspectorMode>(null);
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

  const createStatus = useAppSelector((s) => s.mealPlanning.createRecipeStatus);
  const createError = useAppSelector((s) => s.mealPlanning.createError);

  useEffect(() => {
    if (!family?.familyId || listStatus !== "idle") return;
    dispatch(fetchRecipes(family.familyId));
  }, [family?.familyId, listStatus, dispatch]);

  useEffect(() => {
    if (!selectedId || detailStatus !== "idle") return;
    if (selectedRecipe?.id !== selectedId) {
      dispatch(fetchRecipeDetail(selectedId));
    }
  }, [selectedId, detailStatus, selectedRecipe, dispatch]);

  useEffect(() => {
    if (deleteStatus === "idle" && showDeleteConfirm) {
      setShowDeleteConfirm(false);
      setSelectedId(null);
      setInspectorMode(null);
      dispatch(clearSelectedRecipe());
    }
  }, [deleteStatus]); // eslint-disable-line react-hooks/exhaustive-deps

  function handleSelectRecipe(id: string) {
    if (id === selectedId && inspectorMode === "detail") return;
    setSelectedId(id);
    setInspectorMode("detail");
    setShowDeleteConfirm(false);
    dispatch(clearSelectedRecipe());
  }

  function handleNewRecipe() {
    setSelectedId(null);
    setInspectorMode("create");
    setShowDeleteConfirm(false);
    dispatch(clearSelectedRecipe());
  }

  function handleEditRecipe() {
    setInspectorMode("edit");
    dispatch(clearUpdateError());
  }

  function handleDeleteRequest() {
    setShowDeleteConfirm(true);
    dispatch(clearDeleteError());
  }

  function handleDeleteConfirm() {
    if (!selectedId) return;
    dispatch(deleteRecipe(selectedId));
  }

  function handleCloseInspector() {
    setSelectedId(null);
    setInspectorMode(null);
    setShowDeleteConfirm(false);
    dispatch(clearSelectedRecipe());
  }

  function handleFormSubmit(data: RecipeFormData) {
    if (!family?.familyId) return;
    if (inspectorMode === "edit" && selectedId) {
      dispatch(
        updateRecipe({
          recipeId: selectedId,
          body: {
            name: data.name,
            description: data.description,
            prepTimeMinutes: data.prepTimeMinutes,
            cookTimeMinutes: data.cookTimeMinutes,
            servings: data.servings,
            isFavorite: data.isFavorite,
            allowedMealTypes: data.allowedMealTypes,
            tags: data.tags,
          },
        }),
      ).then((result) => {
        if (result.meta.requestStatus === "fulfilled") {
          setInspectorMode("detail");
          dispatch(fetchRecipeDetail(selectedId));
          dispatch(fetchRecipes(family.familyId!));
        }
      });
    } else {
      dispatch(
        createRecipe({
          familyId: family.familyId,
          name: data.name,
          description: data.description ?? undefined,
          prepTimeMinutes: data.prepTimeMinutes ?? undefined,
          cookTimeMinutes: data.cookTimeMinutes ?? undefined,
          servings: data.servings ?? undefined,
          isFavorite: data.isFavorite,
          allowedMealTypes: data.allowedMealTypes,
          tags: data.tags,
        }),
      ).then((result) => {
        if (result.meta.requestStatus === "fulfilled") {
          setInspectorMode(null);
          dispatch(fetchRecipes(family.familyId!));
        }
      });
    }
  }

  const filtered: RecipeSummary[] = recipes.filter((r) =>
    r.name.toLowerCase().includes(search.toLowerCase()),
  );

  const inspectorContent = (() => {
    if (inspectorMode === "create") {
      return (
        <RecipeFormPanel
          isSubmitting={createStatus === "loading"}
          error={createError}
          onSubmit={handleFormSubmit}
          onCancel={handleCloseInspector}
        />
      );
    }

    if (inspectorMode === "edit" && selectedRecipe && selectedRecipe.id === selectedId) {
      return (
        <RecipeFormPanel
          initial={selectedRecipe}
          isSubmitting={updateStatus === "loading"}
          error={updateError}
          onSubmit={handleFormSubmit}
          onCancel={() => setInspectorMode("detail")}
        />
      );
    }

    if (showDeleteConfirm) {
      return (
        <div className="rl-delete-confirm">
          <p className="rl-delete-confirm-body">{t("deleteConfirmBody")}</p>
          {deleteError && (
            <p className="rl-delete-confirm-error">
              {deleteError.includes("active") ? t("deleteBlockedBySlot") : deleteError}
            </p>
          )}
          <div className="rl-delete-confirm-actions">
            <button
              type="button"
              className="btn btn-sm btn-ghost"
              onClick={() => setShowDeleteConfirm(false)}
              disabled={deleteStatus === "loading"}
            >
              {t("deleteCancel")}
            </button>
            <button
              type="button"
              className="btn btn-sm btn-danger"
              onClick={handleDeleteConfirm}
              disabled={deleteStatus === "loading"}
            >
              {t("deleteConfirmAction")}
            </button>
          </div>
        </div>
      );
    }

    if (selectedRecipe && selectedRecipe.id === selectedId) {
      return (
        <RecipeDetailPanel
          recipe={selectedRecipe}
          onEdit={handleEditRecipe}
          onDelete={handleDeleteRequest}
        />
      );
    }

    if (detailStatus === "loading") {
      return <div className="rl-inspector-message">{t("detailLoading")}</div>;
    }

    if (detailStatus === "error") {
      return <div className="rl-inspector-message">{t("detailError")}</div>;
    }

    return null;
  })();

  const showInspector = inspectorMode !== null;

  return (
    <div className="rl-surface l-surface">
      <div className="rl-header">
        <h1 className="rl-header-title">{t("title")}</h1>
        <div className="rl-header-actions">
          <button type="button" className="btn btn-sm btn-primary" onClick={handleNewRecipe}>
            {t("newRecipe")}
          </button>
        </div>
      </div>

      <div className="l-surface-body">
        <div className="rl-list-pane l-surface-content">
          <div className="rl-search">
            <input
              type="search"
              className="rl-search-input"
              placeholder={t("searchPlaceholder")}
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              aria-label={t("searchPlaceholder")}
            />
          </div>

          {listStatus === "loading" && <p className="rl-list-empty">{t("loading")}</p>}

          {listStatus !== "loading" && filtered.length === 0 && (
            <div className="rl-list-empty">
              {recipes.length === 0 ? (
                <>
                  <p>{t("empty")}</p>
                  <p className="rl-list-empty-hint">{t("emptyHint")}</p>
                </>
              ) : (
                <p>{t("noMatch")}</p>
              )}
            </div>
          )}

          {filtered.length > 0 && (
            <ul className="rl-list" role="listbox" aria-label={t("title")}>
              {filtered.map((recipe) => (
                <li key={recipe.id} role="option" aria-selected={recipe.id === selectedId}>
                  <button
                    type="button"
                    className={[
                      "rl-recipe-row",
                      recipe.id === selectedId && inspectorMode === "detail" ? "is-selected" : "",
                    ]
                      .filter(Boolean)
                      .join(" ")}
                    onClick={() => handleSelectRecipe(recipe.id)}
                  >
                    <div className="rl-recipe-row-body">
                      <span className="rl-recipe-row-name">
                        {recipe.isFavorite && (
                          <span className="rl-recipe-favorite-icon" aria-hidden="true">
                            {"★ "}
                          </span>
                        )}
                        {recipe.name}
                      </span>
                      <div className="rl-recipe-row-meta">
                        {recipe.totalTimeMinutes != null && (
                          <span className="rl-recipe-row-meta-item">
                            {t("minutes", { count: recipe.totalTimeMinutes })}
                          </span>
                        )}
                        {recipe.servings != null && (
                          <span className="rl-recipe-row-meta-item">
                            {t("servingsShort", { count: recipe.servings })}
                          </span>
                        )}
                        {recipe.ingredientCount > 0 && (
                          <span className="rl-recipe-row-meta-item">
                            {t("ingredientsShort", { count: recipe.ingredientCount })}
                          </span>
                        )}
                      </div>
                    </div>
                  </button>
                </li>
              ))}
            </ul>
          )}
        </div>

        {!isMobile && showInspector && (
          <InspectorPanel onClose={handleCloseInspector}>
            {inspectorContent}
          </InspectorPanel>
        )}
      </div>

      {isMobile && showInspector && (
        <BottomSheetDetail open={showInspector} onClose={handleCloseInspector}>
          {inspectorContent}
        </BottomSheetDetail>
      )}
    </div>
  );
}
