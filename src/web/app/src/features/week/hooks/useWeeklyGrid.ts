import { useState, useEffect, useCallback } from "react";
import { weekApi } from "../api/weekApi";
import type { WeeklyGridResponse } from "../types";
import type { ApiError } from "../../../api/domusmindApi";

function toIsoDate(d: Date): string {
  return d.toISOString().slice(0, 10);
}

function startOfWeek(d: Date): Date {
  const day = d.getDay(); // 0 = Sunday
  const diff = d.getDate() - day + (day === 0 ? -6 : 1); // Monday
  return new Date(d.getFullYear(), d.getMonth(), diff);
}

interface UseWeeklyGridResult {
  grid: WeeklyGridResponse | null;
  loading: boolean;
  error: string | null;
  weekStart: Date;
  prevWeek: () => void;
  nextWeek: () => void;
  refresh: () => void;
}

export function useWeeklyGrid(familyId: string): UseWeeklyGridResult {
  const [weekStart, setWeekStart] = useState<Date>(() => startOfWeek(new Date()));
  const [grid, setGrid] = useState<WeeklyGridResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchGrid = useCallback(
    async (start: Date) => {
      if (!familyId) return;
      setLoading(true);
      setError(null);
      try {
        const data = await weekApi.getWeeklyGrid(familyId, toIsoDate(start));
        setGrid(data);
      } catch (err) {
        const apiErr = err as Partial<ApiError>;
        setError(apiErr.message ?? "Failed to load weekly grid.");
      } finally {
        setLoading(false);
      }
    },
    [familyId],
  );

  useEffect(() => {
    fetchGrid(weekStart);
  }, [fetchGrid, weekStart]);

  const prevWeek = useCallback(() => {
    setWeekStart((d) => {
      const prev = new Date(d);
      prev.setDate(prev.getDate() - 7);
      return prev;
    });
  }, []);

  const nextWeek = useCallback(() => {
    setWeekStart((d) => {
      const next = new Date(d);
      next.setDate(next.getDate() + 7);
      return next;
    });
  }, []);

  const refresh = useCallback(() => fetchGrid(weekStart), [fetchGrid, weekStart]);

  return { grid, loading, error, weekStart, prevWeek, nextWeek, refresh };
}
