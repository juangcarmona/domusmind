import { useState } from "react";
import { useAppSelector } from "../../../../store/hooks";
import { MEMBER_AVATAR_COLORS } from "./memberAvatarColors";
import { MEMBER_AVATAR_ICONS } from "./memberAvatarIcons";
import { MemberAvatar } from "./MemberAvatar";

export interface AvatarPickerProps {
  initial: string;
  iconId: number | null;
  colorId: number | null;
  onIconChange: (id: number | null) => void;
  onColorChange: (id: number | null) => void;
}

/**
 * Compact avatar picker — embedded inside a profile-edit form.
 *
 * Shows:
 *  - Live avatar preview
 *  - Color swatch grid (20 colors + default)
 *  - Icon grid (20 icons + "none / initials only")
 *
 * Icon images are loaded from /avatars/icons/avatar-{id}.png.
 * Missing assets degrade gracefully to a numbered placeholder.
 */
export function AvatarPicker({ initial, iconId, colorId, onIconChange, onColorChange }: AvatarPickerProps) {
  const theme = useAppSelector((s) => s.ui.theme);
  const isDark =
    theme === "dark" ||
    (theme === "system" &&
      typeof window !== "undefined" &&
      window.matchMedia("(prefers-color-scheme: dark)").matches);

  const [iconErrors, setIconErrors] = useState<Set<number>>(new Set());

  function handleIconError(id: number) {
    setIconErrors((prev) => {
      const next = new Set(prev);
      next.add(id);
      return next;
    });
  }

  return (
    <div className="avatar-picker">
      {/* Live preview */}
      <div className="avatar-picker-preview">
        <MemberAvatar initial={initial} avatarIconId={iconId} avatarColorId={colorId} size={56} />
      </div>

      {/* Color section */}
      <div className="avatar-picker-section">
        <span className="avatar-picker-label">Color</span>
        <div className="avatar-picker-colors">
          {/* Default (CSS-variable primary — theme-adaptive) */}
          <button
            type="button"
            aria-label="Default color"
            aria-pressed={!colorId}
            className={`avatar-color-swatch${!colorId ? " is-selected" : ""}`}
            style={{
              background: "color-mix(in srgb, var(--primary) 20%, transparent)",
              outline: !colorId ? "2px solid var(--primary)" : undefined,
              outlineOffset: 2,
            }}
            onClick={() => onColorChange(null)}
          />
          {MEMBER_AVATAR_COLORS.map((c) => {
            const selected = colorId === c.id;
            const bg = isDark ? c.bgDark : c.bg;
            const ring = isDark ? c.fgDark : c.fg;
            return (
              <button
                key={c.id}
                type="button"
                aria-label={`Color ${c.id}`}
                aria-pressed={selected}
                className={`avatar-color-swatch${selected ? " is-selected" : ""}`}
                style={{
                  background: bg,
                  outline: selected ? `2px solid ${ring}` : undefined,
                  outlineOffset: 2,
                }}
                onClick={() => onColorChange(c.id)}
              />
            );
          })}
        </div>
      </div>

      {/* Icon section */}
      <div className="avatar-picker-section">
        <span className="avatar-picker-label">Icon</span>
        <div className="avatar-picker-icons">
          {/* "No icon" — show initials */}
          <button
            type="button"
            aria-label="No icon — use initials"
            aria-pressed={!iconId}
            className={`avatar-icon-cell${!iconId ? " is-selected" : ""}`}
            onClick={() => onIconChange(null)}
          >
            <span style={{ fontSize: "0.65rem", color: "var(--muted)", userSelect: "none" }}>ABC</span>
          </button>

          {MEMBER_AVATAR_ICONS.map((icon) => {
            const selected = iconId === icon.id;
            const failed = iconErrors.has(icon.id);
            return (
              <button
                key={icon.id}
                type="button"
                aria-label={`Icon ${icon.id}`}
                aria-pressed={selected}
                className={`avatar-icon-cell${selected ? " is-selected" : ""}`}
                onClick={() => onIconChange(icon.id)}
              >
                {!failed ? (
                  <img
                    src={icon.src}
                    alt=""
                    aria-hidden="true"
                    style={{
                      width: 24,
                      height: 24,
                      objectFit: "contain",
                      filter: isDark ? "brightness(0) invert(1)" : undefined,
                    }}
                    onError={() => handleIconError(icon.id)}
                  />
                ) : (
                  <span style={{ fontSize: "0.65rem", color: "var(--muted)" }}>{icon.id}</span>
                )}
              </button>
            );
          })}
        </div>
      </div>
    </div>
  );
}
