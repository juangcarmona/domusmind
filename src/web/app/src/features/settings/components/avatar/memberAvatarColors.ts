export interface MemberAvatarColor {
  id: number;
  /** Background fill — light theme */
  bg: string;
  /** Foreground (text / icon tint) — light theme */
  fg: string;
  /** Background fill — dark theme */
  bgDark: string;
  /** Foreground (text / icon tint) — dark theme */
  fgDark: string;
}

export const MEMBER_AVATAR_COLORS: readonly MemberAvatarColor[] = [
  { id: 1,  bg: "#dbeafe", fg: "#1d4ed8", bgDark: "#1e3a5f", fgDark: "#93c5fd" }, // blue
  { id: 2,  bg: "#dcfce7", fg: "#166534", bgDark: "#14532d", fgDark: "#86efac" }, // green
  { id: 3,  bg: "#fce7f3", fg: "#be185d", bgDark: "#500724", fgDark: "#f9a8d4" }, // pink
  { id: 4,  bg: "#fef9c3", fg: "#a16207", bgDark: "#451a03", fgDark: "#fde047" }, // yellow
  { id: 5,  bg: "#ede9fe", fg: "#6d28d9", bgDark: "#2e1065", fgDark: "#c4b5fd" }, // purple
  { id: 6,  bg: "#fee2e2", fg: "#b91c1c", bgDark: "#450a0a", fgDark: "#fca5a5" }, // red
  { id: 7,  bg: "#ffedd5", fg: "#c2410c", bgDark: "#431407", fgDark: "#fdba74" }, // orange
  { id: 8,  bg: "#cffafe", fg: "#0e7490", bgDark: "#083344", fgDark: "#67e8f9" }, // cyan
  { id: 9,  bg: "#d1fae5", fg: "#065f46", bgDark: "#022c22", fgDark: "#6ee7b7" }, // teal
  { id: 10, bg: "#e0e7ff", fg: "#3730a3", bgDark: "#1e1b4b", fgDark: "#a5b4fc" }, // indigo
  { id: 11, bg: "#fdf4ff", fg: "#7e22ce", bgDark: "#3b0764", fgDark: "#e879f9" }, // fuchsia
  { id: 12, bg: "#f0fdf4", fg: "#15803d", bgDark: "#052e16", fgDark: "#4ade80" }, // lime green
  { id: 13, bg: "#fff7ed", fg: "#9a3412", bgDark: "#431407", fgDark: "#fb923c" }, // amber
  { id: 14, bg: "#ecfeff", fg: "#155e75", bgDark: "#082f49", fgDark: "#22d3ee" }, // sky
  { id: 15, bg: "#f5f3ff", fg: "#5b21b6", bgDark: "#2e1065", fgDark: "#a78bfa" }, // violet
  { id: 16, bg: "#fff1f2", fg: "#be123c", bgDark: "#4c0519", fgDark: "#fb7185" }, // rose
  { id: 17, bg: "#f0fdfa", fg: "#0f766e", bgDark: "#022c22", fgDark: "#2dd4bf" }, // mint teal
  { id: 18, bg: "#fefce8", fg: "#854d0e", bgDark: "#422006", fgDark: "#fbbf24" }, // warm gold
  { id: 19, bg: "#f1f5f9", fg: "#334155", bgDark: "#1e293b", fgDark: "#94a3b8" }, // slate
  { id: 20, bg: "#fdf2f8", fg: "#9d174d", bgDark: "#4a0e2f", fgDark: "#f0abfc" }, // lilac
];

/**
 * Resolves bg/fg style values for a given colorId and theme.
 * When colorId is null/0 the returned values are CSS `var()` expressions
 * that adapt to the current theme automatically.
 */
export function getAvatarColor(
  colorId: number | null | undefined,
  isDark: boolean,
): { bg: string; fg: string } {
  if (!colorId) {
    return {
      bg: "color-mix(in srgb, var(--primary) 15%, transparent)",
      fg: "var(--primary)",
    };
  }
  const color = MEMBER_AVATAR_COLORS.find((c) => c.id === colorId);
  if (!color) {
    return {
      bg: "color-mix(in srgb, var(--primary) 15%, transparent)",
      fg: "var(--primary)",
    };
  }
  return isDark ? { bg: color.bgDark, fg: color.fgDark } : { bg: color.bg, fg: color.fg };
}
