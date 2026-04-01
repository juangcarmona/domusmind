import { useState } from "react";
import { useAppSelector } from "../../../../store/hooks";
import { getAvatarColor } from "./memberAvatarColors";
import { getAvatarIcon } from "./memberAvatarIcons";

export interface MemberAvatarProps {
  initial: string;
  avatarIconId?: number | null;
  avatarColorId?: number | null;
  size?: number;
}

/**
 * Canonical member avatar component.
 *
 * Render priority:
 *   1. chosen icon + chosen color
 *   2. initial + chosen / default color
 *   3. "?" + default color  (when initial is empty)
 *
 * Icon assets are expected at /avatars/icons/avatar-{id}.png.
 * If an asset fails to load the component falls back to the initial.
 */
export function MemberAvatar({ initial, avatarIconId, avatarColorId, size = 36 }: MemberAvatarProps) {
  const theme = useAppSelector((s) => s.ui.theme);
  const isDark =
    theme === "dark" ||
    (theme === "system" &&
      typeof window !== "undefined" &&
      window.matchMedia("(prefers-color-scheme: dark)").matches);

  const [iconFailed, setIconFailed] = useState(false);

  const { bg, fg } = getAvatarColor(avatarColorId, isDark);
  const icon = getAvatarIcon(avatarIconId);
  const iconSrc = icon && !iconFailed ? icon.src : null;

  return (
    <div
      style={{
        width: size,
        height: size,
        borderRadius: "50%",
        background: bg,
        color: fg,
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        fontWeight: 700,
        fontSize: size >= 40 ? "1rem" : "0.85rem",
        flexShrink: 0,
        overflow: "hidden",
      }}
    >
      {iconSrc ? (
        <img
          src={iconSrc}
          alt=""
          aria-hidden="true"
          style={{
            width: "62%",
            height: "62%",
            objectFit: "contain",
            // SVG icons have dark fills — invert to white in dark mode
            filter: isDark ? "brightness(0) invert(1)" : undefined,
          }}
          onError={() => setIconFailed(true)}
        />
      ) : (
        initial || "?"
      )}
    </div>
  );
}
