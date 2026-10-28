import { useEffect } from "react";
import { getStoredToken } from "../../api/request";
import { fetchFamilySharedLists, fetchSharedListDetail } from "../../store/listsSlice";
import { useAppDispatch, useAppSelector } from "../../store/hooks";

const reconnectDelayMs = 1000;

export function useListChangeStream(listId: string | null) {
  const dispatch = useAppDispatch();
  const familyId = useAppSelector((state) => state.household.family?.familyId);

  useEffect(() => {
    if (!listId) return;
    const activeListId = listId;

    const controller = new AbortController();
    let reconnectDelay = reconnectDelayMs;

    function refreshList() {
      void dispatch(fetchSharedListDetail(activeListId));
      if (familyId) void dispatch(fetchFamilySharedLists(familyId));
    }

    async function connect() {
      while (!controller.signal.aborted) {
        const token = getStoredToken();
        const bffMode = import.meta.env.VITE_AUTH_MODE === "oidc-bff";
        if (!token && !bffMode) return;

        try {
          const response = await fetch(`/api/shared-lists/${activeListId}/events`, {
            headers: bffMode ? undefined : { Authorization: `Bearer ${token}` },
            credentials: "same-origin",
            signal: controller.signal,
          });

          if (response.status === 401 || response.status === 403 || response.status === 404)
            return;
          if (!response.ok || !response.body)
            throw new Error(`List update stream returned ${response.status}`);

          reconnectDelay = reconnectDelayMs;
          const reader = response.body.getReader();
          const decoder = new TextDecoder();
          let buffer = "";
          let accessRevoked = false;

          while (!controller.signal.aborted) {
            const { value, done } = await reader.read();
            if (done) break;
            buffer += decoder.decode(value, { stream: true });

            let frameBoundary = buffer.indexOf("\n\n");
            while (frameBoundary >= 0) {
              const frame = buffer.slice(0, frameBoundary);
              buffer = buffer.slice(frameBoundary + 2);
              if (frame.split("\n").some((line) => line === "event: closed")) {
                accessRevoked = true;
                break;
              }
              if (frame.split("\n").some((line) => line === "event: refresh"))
                refreshList();
              frameBoundary = buffer.indexOf("\n\n");
            }
            if (accessRevoked) break;
          }
          if (accessRevoked) return;
        } catch (error) {
          if (controller.signal.aborted) return;
          console.warn("List updates disconnected; retrying.", error);
        }

        if (controller.signal.aborted) return;
        await new Promise((resolve) => window.setTimeout(resolve, reconnectDelay));
        reconnectDelay = Math.min(reconnectDelay * 2, 30000);
      }
    }

    void connect();
    return () => controller.abort();
  }, [dispatch, familyId, listId]);
}
