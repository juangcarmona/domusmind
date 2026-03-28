import React from "react";
import type { CalendarEntry } from "../../today/utils/calendarEntry";
import { ENTRY_GLYPH } from "../../today/utils/calendarEntry";

// ----------------------------------------------------------------
// 24-hour, 30-minute slot grid
// ----------------------------------------------------------------

/** Total slots = 48 (0:00 through 23:30). Slot index = hour * 2 + (half ? 1 : 0). */
const SLOT_COUNT = 48;

/** Day-phase boundary: slots 12 (06:00) through 43 (21:30) inclusive are "day". */
const DAY_SLOT_START = 6 * 2;   // 12
const DAY_SLOT_END   = 21 * 2 + 1; // 43

function slotIndex(hour: number, minute: number): number {
  return hour * 2 + (minute >= 30 ? 1 : 0);
}

function slotLabel(slotIdx: number): string {
  const hour = Math.floor(slotIdx / 2);
  const half = slotIdx % 2 === 1;
  return half
    ? `${String(hour).padStart(2, "0")}:30`
    : `${String(hour).padStart(2, "0")}:00`;
}

function isNightSlot(slotIdx: number): boolean {
  return slotIdx < DAY_SLOT_START || slotIdx > DAY_SLOT_END;
}

// ----------------------------------------------------------------

interface HourTimelineProps {
  /** All timed entries for the day (entry.time is non-null). */
  timedEntries: CalendarEntry[];
  onItemClick: (type: "event" | "task" | "routine", id: string) => void;
  /**
   * Called when an empty slot background is clicked.
   * Receives "HH:MM" (the nearest :00 or :30 slot start).
   */
  onSlotClick?: (time: string) => void;
}

/**
 * Vertical 24-hour, 30-minute-slot time grid.
 *
 * - 48 slots total: 00:00, 00:30, 01:00 … 23:30
 * - Day phase (06:00–21:59): normal background
 * - Night phase (22:00–05:59): subtle dimmed background
 * - Hour labels shown on :00 slots; :30 slots show a subdued tick label
 * - Entries bucket to nearest :00 or :30 slot by flooring minutes
 * - Entries with times like 20:15 render in the 20:00 slot (not forced to :30)
 * - Empty slots are optionally clickable for "create at time" action
 */
export function HourTimeline({ timedEntries, onItemClick, onSlotClick }: HourTimelineProps) {
  // Bucket entries by slot index.
  const bySlot = new Map<number, CalendarEntry[]>();
  for (let i = 0; i < SLOT_COUNT; i++) bySlot.set(i, []);

  for (const entry of timedEntries) {
    if (!entry.time) continue;
    const [hStr, mStr] = entry.time.split(":");
    const h = Math.min(23, Math.max(0, parseInt(hStr, 10)));
    const m = parseInt(mStr ?? "0", 10);
    const idx = slotIndex(h, m);
    bySlot.get(idx)!.push(entry);
  }

  return (
    <div className="hour-timeline" aria-label="Hourly schedule">
      {Array.from({ length: SLOT_COUNT }, (_, idx) => {
        const isHalf = idx % 2 === 1;
        const entries = bySlot.get(idx) ?? [];
        const hasItems = entries.length > 0;
        const night = isNightSlot(idx);
        const label = slotLabel(idx);
        const clickable = !hasItems && !!onSlotClick;

        return (
          <div
            key={idx}
            className={[
              "ht-slot",
              isHalf ? "ht-slot--half" : "ht-slot--hour",
              night ? "ht-slot--night" : "ht-slot--day",
              hasItems ? "ht-slot--has-items" : "ht-slot--empty",
            ].join(" ")}
            data-slot={label}
          >
            {/* Time label column */}
            <div className={`ht-label${isHalf ? " ht-label--half" : ""}`} aria-hidden="true">
              {!isHalf ? label : <span className="ht-label-tick" />}
            </div>

            {/* Slot body */}
            <div
              className={`ht-body${clickable ? " ht-body--clickable" : ""}`}
              onClick={clickable ? () => onSlotClick!(label) : undefined}
              role={clickable ? "button" : undefined}
              tabIndex={clickable ? 0 : undefined}
              onKeyDown={
                clickable
                  ? (e) => {
                      if (e.key === "Enter" || e.key === " ") {
                        e.preventDefault();
                        onSlotClick!(label);
                      }
                    }
                  : undefined
              }
            >
              {entries.map((entry) => (
                <TimelineItem key={entry.id} entry={entry} onItemClick={onItemClick} />
              ))}
            </div>
          </div>
        );
      })}
    </div>
  );
}

// ----------------------------------------------------------------
// TimelineItem
// ----------------------------------------------------------------

interface TimelineItemProps {
  entry: CalendarEntry;
  onItemClick: (type: "event" | "task" | "routine", id: string) => void;
}

function TimelineItem({ entry, onItemClick }: TimelineItemProps) {
  const glyph = ENTRY_GLYPH[entry.displayType];
  const timeStr = entry.time ?? null;

  const style = entry.color
    ? ({ ["--wg-item-accent" as string]: entry.color } as React.CSSProperties)
    : undefined;

  const typeClass =
    entry.displayType === "overdue" ? "wg-item--overdue" : `wg-item--${entry.sourceType}`;

  const classes = [
    "wg-item",
    "ht-item",
    typeClass,
    entry.isCompleted ? "wg-item--completed" : "",
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <div
      className={classes}
      style={style}
      title={`${glyph}${timeStr ? ` ${timeStr}` : ""} ${entry.title}`}
      onClick={() => onItemClick(entry.sourceType, entry.id)}
      role="button"
      tabIndex={0}
      onKeyDown={(e) => {
        if (e.key === "Enter" || e.key === " ") {
          e.preventDefault();
          onItemClick(entry.sourceType, entry.id);
        }
      }}
    >
      <span className="wg-item-glyph" aria-hidden="true">
        {glyph}
        {timeStr && <span className="ht-item-time"> {timeStr}</span>}
      </span>
      <span className="wg-item-title">{entry.title}</span>
    </div>
  );
}
