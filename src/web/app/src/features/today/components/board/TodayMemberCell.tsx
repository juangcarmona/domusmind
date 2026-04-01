import { useTranslation } from "react-i18next";
import type { CalendarEntry } from "../../utils/calendarEntry";
import { splitForDisplay } from "../../utils/todayPanelHelpers";
import { CalendarEntryItem } from "../shared/CalendarEntryItem";
import { useAppSelector } from "../../../../store/hooks";
import { MemberAvatar } from "../../../settings/components/avatar/MemberAvatar";

interface TodayMemberCellProps {
  memberId: string;
  name: string;
  entries: CalendarEntry[];
  onMemberClick: (memberId: string) => void;
  onItemClick: (sourceType: "event" | "task" | "routine", id: string) => void;
}

/**
 * Compact, non-expandable snapshot cell for a single household member.
 *
 * Shows up to 2 entries (collapsed view from splitForDisplay).
 * Clicking the cell navigates to the member agenda page (via onMemberClick).
 * Clicking an entry item opens the entity edit modal without triggering navigation.
 */
export function TodayMemberCell({
  memberId,
  name,
  entries,
  onMemberClick,
  onItemClick,
}: TodayMemberCellProps) {
  const { t } = useTranslation("today");
  const { visibleCollapsed, overflowCount, isEmpty } = splitForDisplay(entries);

  const householdMember = useAppSelector((s) =>
    s.household.members.find((m) => m.memberId === memberId),
  );
  const displayName = householdMember?.preferredName || name;

  return (
    <div
      className="tp-cell"
      role="button"
      tabIndex={0}
      aria-label={displayName}
      onClick={() => onMemberClick(memberId)}
      onKeyDown={(e) => {
        if (e.key === "Enter" || e.key === " ") {
          e.preventDefault();
          onMemberClick(memberId);
        }
      }}
    >
      <div className="tp-cell-header">
        <MemberAvatar
          initial={householdMember?.avatarInitial ?? displayName[0]?.toUpperCase() ?? "?"}
          avatarIconId={householdMember?.avatarIconId}
          avatarColorId={householdMember?.avatarColorId}
          size={26}
        />
        <span className="tp-cell-name" title={displayName}>{displayName}</span>
        {overflowCount > 0 && (
          <span className="tp-cell-overflow">+{overflowCount}</span>
        )}
      </div>

      <div className="tp-cell-entries">
        {isEmpty ? (
          <span className="tp-cell-empty">{t("day.nothingToday")}</span>
        ) : (
          visibleCollapsed.map((entry) => (
            // Stop propagation so entry clicks open the edit modal
            // rather than also triggering member navigation.
            <div
              key={entry.id}
              className="tp-cell-entry-wrap"
              onClick={(e) => e.stopPropagation()}
            >
              <CalendarEntryItem
                entry={entry}
                onClick={() => onItemClick(entry.sourceType, entry.id)}
              />
            </div>
          ))
        )}
      </div>
    </div>
  );
}
