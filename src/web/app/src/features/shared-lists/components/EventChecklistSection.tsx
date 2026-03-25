import { useState, useEffect, useCallback } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../../store/hooks";
import { unlinkSharedList } from "../../../store/sharedListsSlice";
import { sharedListsApi } from "../../../api/sharedListsApi";
import type { GetSharedListByLinkedEntityResponse } from "../../../api/types/sharedListTypes";
import { AttachChecklistSelector } from "./AttachChecklistSelector";

interface EventChecklistSectionProps {
  eventId: string;
  familyId: string;
}

export function EventChecklistSection({ eventId, familyId }: EventChecklistSectionProps) {
  const { t } = useTranslation("sharedLists");
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const [linked, setLinked] = useState<GetSharedListByLinkedEntityResponse | null>(null);
  const [loadingLinked, setLoadingLinked] = useState(true);
  const [creating, setCreating] = useState(false);
  const [unlinking, setUnlinking] = useState(false);
  const [showAttach, setShowAttach] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadLinked = useCallback(async () => {
    setLoadingLinked(true);
    try {
      const result = await sharedListsApi.getSharedListByLinkedEntity("CalendarEvent", eventId);
      setLinked(result);
    } catch {
      setLinked(null);
    } finally {
      setLoadingLinked(false);
    }
  }, [eventId]);

  useEffect(() => {
    loadLinked();
  }, [loadLinked]);

  async function handleCreate() {
    setCreating(true);
    setError(null);
    try {
      const result = await sharedListsApi.createLinkedSharedListForEvent(eventId, { familyId });
      navigate(`/lists/${result.listId}`);
    } catch (err) {
      setError((err as { message?: string }).message ?? t("checklistError"));
      setCreating(false);
    }
  }

  async function handleUnlink() {
    if (!linked) return;
    setUnlinking(true);
    setError(null);
    const result = await dispatch(unlinkSharedList(linked.listId));
    setUnlinking(false);
    if (unlinkSharedList.fulfilled.match(result)) {
      setLinked(null);
    } else {
      setError((result.payload as string) ?? t("checklistError"));
    }
  }

  function handleAttached() {
    setShowAttach(false);
    loadLinked();
  }

  if (loadingLinked) return null;

  return (
    <div className="form-group">
      <label>{t("checklistSection")}</label>

      {linked ? (
        <div style={{ display: "flex", alignItems: "center", gap: "0.75rem", flexWrap: "wrap" }}>
          <span>{linked.name}</span>
          <button
            type="button"
            className="btn btn-ghost"
            onClick={() => navigate(`/lists/${linked.listId}`)}
          >
            {t("openChecklist")}
          </button>
          <button
            type="button"
            className="btn btn-ghost"
            onClick={handleUnlink}
            disabled={unlinking}
          >
            {unlinking ? "…" : t("unlinkChecklist")}
          </button>
        </div>
      ) : showAttach ? (
        <AttachChecklistSelector
          eventId={eventId}
          onAttached={handleAttached}
          onCancel={() => setShowAttach(false)}
        />
      ) : (
        <>
          <div style={{ display: "flex", gap: "0.5rem", flexWrap: "wrap" }}>
            <button
              type="button"
              className="btn btn-ghost"
              onClick={handleCreate}
              disabled={creating}
            >
              {creating ? t("checklistCreating") : t("createChecklist")}
            </button>
            <button
              type="button"
              className="btn btn-ghost"
              onClick={() => setShowAttach(true)}
            >
              {t("attachChecklist")}
            </button>
          </div>
          {error && <p className="error-msg">{error}</p>}
        </>
      )}

      {linked && error && <p className="error-msg">{error}</p>}
    </div>
  );
}
