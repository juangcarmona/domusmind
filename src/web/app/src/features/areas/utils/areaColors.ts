/**
 * Area color management.
 *
 * Colors are a client-side concept: the backend has no color field on
 * responsibility domains. Colors are stored in localStorage keyed by areaId
 * so they survive page reloads and are consistent across the session.
 *
 * When no color has been set for an area, the first palette entry is used.
 * Users pick a color explicitly at creation time or via the Area Detail page.
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

function loadStore(): Record<string, string> {
  try {
    return JSON.parse(localStorage.getItem(STORAGE_KEY) ?? "{}");
  } catch {
    return {};
  }
}

/** Returns the user-chosen color for this area, or the first palette color as default. */
export function getAreaColor(areaId: string): string {
  const store = loadStore();
  return store[areaId] ?? AREA_PALETTE[0];
}

/** Persists the user-chosen color for this area to localStorage. */
export function setAreaColor(areaId: string, color: string): void {
  const store = loadStore();
  store[areaId] = color;
  localStorage.setItem(STORAGE_KEY, JSON.stringify(store));
}
