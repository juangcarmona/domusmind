import { useTranslation } from "react-i18next";
import type { FamilyMemberResponse, HouseholdAreaItem } from "../../../api/domusmindApi";

interface AreaOwnerSectionProps {
  area: HouseholdAreaItem;
  members: FamilyMemberResponse[];
  saving: boolean;
  ownerError: string | null;
  onOwnerChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  supporterError: string | null;
  onAddSupporter: (memberId: string) => void;
}

export function AreaOwnerSection({
  area,
  members,
  saving,
  ownerError,
  onOwnerChange,
  supporterError,
  onAddSupporter,
}: AreaOwnerSectionProps) {
  const { t } = useTranslation("areas");
  const hasOwner = !!area.primaryOwnerId;

  const existingSupporters = members.filter((m) => area.secondaryOwnerIds.includes(m.memberId));
  const orphanSupporterIds = area.secondaryOwnerIds.filter(
    (id) => !members.some((m) => m.memberId === id),
  );
  const availableForSupport = members.filter(
    (m) => m.memberId !== area.primaryOwnerId && !area.secondaryOwnerIds.includes(m.memberId),
  );

  return (
    <>
      <div className="area-detail-section">
        <div className="area-detail-section-header">
          <span className="area-detail-section-title">{t("ownerLabel")}</span>
        </div>
        <select
          className="form-control area-row-select"
          value={area.primaryOwnerId ?? ""}
          disabled={saving}
          onChange={onOwnerChange}
          aria-label={t("ownerLabel")}
        >
          {!hasOwner && <option value="">{t("noOwner")}</option>}
          {hasOwner && !members.some((m) => m.memberId === area.primaryOwnerId) && (
            <option value={area.primaryOwnerId!}>
              {area.primaryOwnerName ?? area.primaryOwnerId}
            </option>
          )}
          {members.map((m) => (
            <option key={m.memberId} value={m.memberId}>
              {m.preferredName || m.name}
            </option>
          ))}
        </select>
        {!hasOwner && <p className="area-detail-hint">{t("noOwnerInstruction")}</p>}
        {ownerError && <p className="error-msg" style={{ marginTop: "0.5rem" }}>{ownerError}</p>}
      </div>

      <div className="area-detail-section">
        <div className="area-detail-section-header">
          <span className="area-detail-section-title">{t("supportersLabel")}</span>
        </div>
        {(existingSupporters.length > 0 || orphanSupporterIds.length > 0) && (
          <ul className="area-supporters-list">
            {existingSupporters.map((m) => (
              <li key={m.memberId} className="area-supporter-tag">
                {m.preferredName || m.name}
              </li>
            ))}
            {orphanSupporterIds.map((id) => (
              <li key={id} className="area-supporter-tag">{id}</li>
            ))}
          </ul>
        )}
        {availableForSupport.length > 0 && (
          <select
            className="form-control area-row-select"
            value=""
            disabled={saving}
            onChange={(e) => { if (e.target.value) onAddSupporter(e.target.value); }}
            aria-label={t("addSupporter")}
          >
            <option value="">{t("addSupporter")}</option>
            {availableForSupport.map((m) => (
              <option key={m.memberId} value={m.memberId}>
                {m.preferredName || m.name}
              </option>
            ))}
          </select>
        )}
        {area.secondaryOwnerIds.length === 0 && availableForSupport.length === 0 && (
          <p className="area-detail-hint">{t("noSupporters")}</p>
        )}
        {supporterError && <p className="error-msg" style={{ marginTop: "0.5rem" }}>{supporterError}</p>}
      </div>
    </>
  );
}
