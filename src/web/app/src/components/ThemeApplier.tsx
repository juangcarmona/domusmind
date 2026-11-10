import { useEffect } from "react";
import { useAppSelector } from "../store/hooks";

/**
 * Keeps document.documentElement[data-theme] in sync with Redux ui.theme.
 * Renders nothing — pure side-effect component.
 */
export function ThemeApplier() {
  const theme = useAppSelector((s) => s.ui.theme);

  useEffect(() => {
    const root = document.documentElement;
    const systemPreference = window.matchMedia("(prefers-color-scheme: dark)");
    const applyTheme = () => {
      const resolvedTheme =
        theme === "system"
          ? systemPreference.matches
            ? "dark"
            : "light"
          : theme;
      root.setAttribute("data-theme", resolvedTheme);
    };

    applyTheme();
    if (theme !== "system") return;

    systemPreference.addEventListener("change", applyTheme);
    return () => systemPreference.removeEventListener("change", applyTheme);
  }, [theme]);

  return null;
}
