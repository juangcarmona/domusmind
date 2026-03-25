import { useEffect, useRef, useState, type KeyboardEvent } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate, useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import {
  fetchSharedListDetail,
  addItemToSharedList,
  toggleSharedListItem,
  updateSharedListItem,
  removeSharedListItem,
  optimisticToggleItem,
  optimisticRenameItem,
  optimisticRemoveItem,
  clearDetail,
} from "../../../store/sharedListsSlice";
import type { SharedListItemDetail } from "../../../api/types/sharedListTypes";

// ── Item row ──────────────────────────────────────────────────────────────────

interface ItemRowProps {
  item: SharedListItemDetail;
  listId: string;
}

function ItemRow({ item, listId }: ItemRowProps) {
  const { t } = useTranslation("sharedLists");
  const dispatch = useAppDispatch();
  const [editing, setEditing] = useState(false);
  const [draft, setDraft] = useState(item.name);

  function startEditing(e: React.MouseEvent) {
    e.stopPropagation();
    setDraft(item.name);
    setEditing(true);
  }

  function commitRename() {
    const trimmed = draft.trim();
    if (trimmed && trimmed !== item.name) {
      dispatch(optimisticRenameItem({ itemId: item.itemId, name: trimmed }));
      dispatch(updateSharedListItem({ listId, itemId: item.itemId, name: trimmed }));
    }
    setEditing(false);
  }

  function cancelRename() {
    setDraft(item.name);
    setEditing(false);
  }

  function handleRenameKey(e: KeyboardEvent<HTMLInputElement>) {
    if (e.key === "Enter") { e.preventDefault(); commitRename(); }
    else if (e.key === "Escape") { e.preventDefault(); cancelRename(); }
  }

  function handleRowClick() {
    if (editing) return;
    dispatch(optimisticToggleItem({ itemId: item.itemId }));
    dispatch(toggleSharedListItem({ listId, itemId: item.itemId }));
  }

  function handleRemove(e: React.MouseEvent) {
    e.stopPropagation();
    dispatch(optimisticRemoveItem({ itemId: item.itemId }));
    dispatch(removeSharedListItem({ listId, itemId: item.itemId }));
  }

  return (
    <div
      className={`shared-list-item${item.checked ? " shared-list-item--checked" : ""}${editing ? " shared-list-item--editing" : ""}`}
      onClick={handleRowClick}
    >
      <input
        type="checkbox"
        className="shared-list-item__checkbox"
        checked={item.checked}
        readOnly
        aria-label={item.name}
        tabIndex={-1}
      />

      {editing ? (
        <input
          className="shared-list-item__rename-input"
          value={draft}
          onChange={(e) => setDraft(e.target.value)}
          onKeyDown={handleRenameKey}
          onBlur={commitRename}
          autoFocus
          aria-label={t("renameItem")}
          onClick={(e) => e.stopPropagation()}
        />
      ) : (
        <span
          className="shared-list-item__name"
          onClick={startEditing}
          role="button"
          tabIndex={0}
          onKeyDown={(e) => {
            if (e.key === "Enter") { e.preventDefault(); startEditing(e as unknown as React.MouseEvent); }
          }}
        >
          {item.name}
        </span>
      )}

      <button
        type="button"
        className="shared-list-item__remove"
        onClick={handleRemove}
        aria-label={t("removeItem")}
        tabIndex={-1}
      >
        ×
      </button>
    </div>
  );
}

// ── Page ───────────────────────────────────────────────────────────────────────

export function SharedListDetailPage() {
  const { listId } = useParams<{ listId: string }>();
  const { t } = useTranslation("sharedLists");
  const { t: tCommon } = useTranslation("common");
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const detail = useAppSelector((s) => s.sharedLists.detail);
  const detailStatus = useAppSelector((s) => s.sharedLists.detailStatus);
  const addInputRef = useRef<HTMLInputElement>(null);
  const [addItemName, setAddItemName] = useState("");
  const [addError, setAddError] = useState<string | null>(null);

  useEffect(() => {
    if (listId) {
      dispatch(fetchSharedListDetail(listId));
    }
    return () => {
      dispatch(clearDetail());
    };
  }, [listId, dispatch]);

  // Auto-focus add input once list data arrives
  useEffect(() => {
    if (detailStatus === "success") {
      addInputRef.current?.focus();
    }
  }, [detailStatus]);

  async function handleAddKey(e: KeyboardEvent<HTMLInputElement>) {
    if (e.key !== "Enter") return;
    const name = addItemName.trim();
    if (!name || !listId) return;
    setAddError(null);
    setAddItemName("");
    const result = await dispatch(addItemToSharedList({ listId, name }));
    if (!addItemToSharedList.fulfilled.match(result)) {
      setAddError((result.payload as string) ?? t("addError"));
    }
    addInputRef.current?.focus();
  }

  if (detailStatus === "loading" && !detail) {
    return (
      <div className="page-wrap">
        <div className="loading-wrap">{t("loadingDetail")}</div>
      </div>
    );
  }

  if (detailStatus === "error" || (!detail && detailStatus !== "loading")) {
    return (
      <div className="page-wrap">
        <p className="error-msg">{tCommon("failed")}</p>
      </div>
    );
  }

  if (!detail) return null;

  const sorted = [...detail.items].sort((a, b) => a.order - b.order);
  const unchecked = sorted.filter((i) => !i.checked);
  const checked = sorted.filter((i) => i.checked);

  return (
    <div className="page-wrap">
      <div className="page-header">
        <div>
          <button
            type="button"
            className="btn btn-ghost btn-sm"
            onClick={() => navigate("/lists")}
          >
            ← {t("backToLists")}
          </button>
          <h1 style={{ marginTop: "0.5rem" }}>{detail.name}</h1>
        </div>
      </div>

      <div className="shared-list-items-wrap">
        {unchecked.map((item) => (
          <ItemRow key={item.itemId} item={item} listId={detail.listId} />
        ))}

        {sorted.length === 0 && (
          <p className="shared-list-empty-hint">{t("noItems")}</p>
        )}

        {checked.length > 0 && (
          <>
            <div className="shared-list-section-label">{t("checkedSection")}</div>
            {checked.map((item) => (
              <ItemRow key={item.itemId} item={item} listId={detail.listId} />
            ))}
          </>
        )}

        <div className="shared-list-add-row">
          <input
            ref={addInputRef}
            type="text"
            className="shared-list-add-input"
            placeholder={t("addItemPlaceholder")}
            value={addItemName}
            onChange={(e) => setAddItemName(e.target.value)}
            onKeyDown={handleAddKey}
            aria-label={t("addItem")}
          />
        </div>
      </div>

      {addError && <p className="error-msg" style={{ marginTop: "0.5rem" }}>{addError}</p>}
    </div>
  );
}

