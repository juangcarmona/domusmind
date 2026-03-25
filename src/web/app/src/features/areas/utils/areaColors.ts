/**
 * Legacy local area color cache.
 *
 * Area colors are now persisted in backend responsibility domains.
 * This local cache is kept as a fallback for flows that still query color
 * client-side before area data has loaded.
 *
 * When no color has been set for an area, the first palette entry is used.
 * New area colors should be persisted through the API, not this helper.
 */

export const AREA_PALETTE = [
  "#6a4c93", // purple
  "#1565c0", // blue
  "#2e7d32", // green
  "#e65100", // orange
  "#c62828", // red
  "#00695c", // teal
  "#4e342e", // brown
];

const STORAGE_KEY = "domusmind:areaColors";

function normalizeAreaName(areaName: string): string {
  return areaName.trim().toLowerCase().replace(/\s+/g, " ");
}

function getAreaNameKey(areaName?: string): string | null {
  if (!areaName?.trim()) return null;
  return `name:${normalizeAreaName(areaName)}`;
}

function loadStore(): Record<string, string> {
  try {
    return JSON.parse(localStorage.getItem(STORAGE_KEY) ?? "{}");
  } catch {
    return {};
  }
}

/** Returns the user-chosen color for this area, or the first palette color as default. */
export function getAreaColor(areaId: string, areaName?: string): string {
  const store = loadStore();
  const byId = store[areaId];
  if (byId) return byId;

  const nameKey = getAreaNameKey(areaName);
  if (nameKey && store[nameKey]) return store[nameKey];

  return AREA_PALETTE[0];
}

/** Persists the user-chosen color for this area to localStorage. */
export function setAreaColor(areaId: string, color: string, areaName?: string): void {
  const store = loadStore();
  store[areaId] = color;
  const nameKey = getAreaNameKey(areaName);
  if (nameKey) store[nameKey] = color;
  localStorage.setItem(STORAGE_KEY, JSON.stringify(store));
}
