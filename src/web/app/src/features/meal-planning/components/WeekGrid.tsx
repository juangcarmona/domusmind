import { useTranslation } from "react-i18next";
import type { MealPlanResponse, MealSlotResponse } from "../../../api/types/mealPlanningTypes";
import { MealSlotCell, MEAL_TYPE_ORDER } from "./MealSlotCell";

const DAYS_ORDER = [
  "Monday",
  "Tuesday",
  "Wednesday",
  "Thursday",
  "Friday",
  "Saturday",
  "Sunday",
] as const;

interface WeekGridProps {
  plan: MealPlanResponse;
  selectedSlotId: string | null;
  onSlotClick: (slot: MealSlotResponse) => void;
}

/**
 * WeekGrid — 7-column grid of meal slots, one column per day.
 * Pure/presentational: receives plan data, emits slot click.
 */
export function WeekGrid({ plan, selectedSlotId, onSlotClick }: WeekGridProps) {
  const { t } = useTranslation("mealPlanning");

  return (
    <div className="mp-week-grid">
      {/* Day header row */}
      <div className="mp-week-grid-header">
        {DAYS_ORDER.map((day) => (
          <div key={day} className="mp-week-grid-day-header">
            <span className="mp-week-grid-day-label-full">
              {t(`days.${day}` as Parameters<typeof t>[0])}
            </span>
            <span className="mp-week-grid-day-label-short">
              {t(`daysShort.${day}` as Parameters<typeof t>[0])}
            </span>
          </div>
        ))}
      </div>

      {/* Slot columns */}
      <div className="mp-week-grid-body">
        {DAYS_ORDER.map((day) => {
          const daySlots = (plan.slots ?? []).filter((s) => s.dayOfWeek === day);
          // Sort slots by the canonical meal type order
          const sorted = [...daySlots].sort(
            (a, b) =>
              MEAL_TYPE_ORDER.indexOf(a.mealType as typeof MEAL_TYPE_ORDER[number]) -
              MEAL_TYPE_ORDER.indexOf(b.mealType as typeof MEAL_TYPE_ORDER[number]),
          );

          return (
            <div key={day} className="mp-week-grid-day-col">
              {sorted.map((slot) => (
                <MealSlotCell
                  key={slot.id}
                  slot={slot}
                  selected={slot.id === selectedSlotId}
                  onClick={onSlotClick}
                />
              ))}
            </div>
          );
        })}
      </div>
    </div>
  );
}
