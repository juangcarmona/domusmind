import type { WeeklyGridCell as WeeklyGridCellType } from "../types";
import { eventToItem, taskToItem } from "./WeeklyGridItem";

interface WeeklyGridCellProps {
  cell: WeeklyGridCellType;
}

export function WeeklyGridCell({ cell }: WeeklyGridCellProps) {
  const hasItems = cell.events.length > 0 || cell.tasks.length > 0;

  return (
    <div className={`wg-cell${hasItems ? "" : " wg-cell--empty"}`}>
      {cell.events.map((e) => eventToItem(e))}
      {cell.tasks.map((t) => taskToItem(t))}
    </div>
  );
}
