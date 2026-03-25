import { useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import { useAppDispatch } from "../../../store/hooks";
import { createArea, updateAreaColor } from "../../../store/areasSlice";
import { AREA_PALETTE } from "../utils/areaColors";

interface CreateAreaModalProps {
  familyId: string;
  onClose: () => void;
}

export function CreateAreaModal({ familyId, onClose }: CreateAreaModalProps) {
  const { t } = useTranslation("areas");
  const { t: tCommon } = useTranslation("common");
  const dispatch = useAppDispatch();

  const [name, setName] = useState("");
  const [color, setColor] = useState(AREA_PALETTE[0]);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const isCustomColor = !AREA_PALETTE.includes(color);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    if (!name.trim()) return;
    setSubmitting(true);
    setError(null);
    const result = await dispatch(createArea({ familyId, name: name.trim() }));
    setSubmitting(false);
    if (createArea.fulfilled.match(result)) {
      if (color !== result.payload.color) {
        await dispatch(updateAreaColor({ areaId: result.payload.areaId, color }));
      }
      onClose();
    } else {
      setError((result.payload as string) ?? t("createError"));
    }
  }

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div className="modal" onClick={(e) => e.stopPropagation()}>
        <h2>{t("createHeading")}</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="area-name-input">{t("nameLabel")}</label>
            <input
              id="area-name-input"
              className="form-control"
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              autoFocus
              placeholder={t("namePlaceholder")}
            />
            <span className="form-hint">{t("createHint")}</span>
          </div>
          <div className="form-group">
            <label>{t("colorLabel")}</label>
            <div className="area-color-picker">
              {AREA_PALETTE.map((c) => (
                <button
                  key={c}
                  type="button"
                  className={`area-color-swatch${color === c ? " area-color-swatch--active" : ""}`}
                  style={{ background: c }}
                  onClick={() => setColor(c)}
                  aria-label={c}
                />
              ))}
              <div
                className={`area-color-custom-trigger${isCustomColor ? " area-color-custom-trigger--active" : ""}`}
                style={isCustomColor ? { background: color } : undefined}
                title={t("customColor")}
              >
                {!isCustomColor && <span className="area-color-custom-label" aria-hidden="true">+</span>}
                <input
                  type="color"
                  value={color}
                  onChange={(e) => setColor(e.target.value)}
                  aria-label={t("customColor")}
                />
              </div>
            </div>
          </div>
          {error && <p className="error-msg">{error}</p>}
          <div className="modal-footer">
            <button type="button" className="btn btn-ghost" onClick={onClose}>
              {tCommon("cancel")}
            </button>
            <button
              type="submit"
              className="btn"
              disabled={submitting || !name.trim()}
            >
              {submitting ? tCommon("creating") : tCommon("create")}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
