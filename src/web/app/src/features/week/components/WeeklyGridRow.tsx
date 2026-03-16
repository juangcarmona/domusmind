import type { WeeklyGridMember } from "../types";
import { WeeklyGridCell } from "./WeeklyGridCell";

interface WeeklyGridRowProps {
  member: WeeklyGridMember;
}

export function WeeklyGridRow({ member }: WeeklyGridRowProps) {
  return (
    <div className="wg-row">
      <div className="wg-member-label" title={member.role}>
        <span className="wg-member-name">{member.name}</span>
        <span className="wg-member-role">{member.role}</span>
      </div>
      {member.cells.map((cell) => (
        <WeeklyGridCell key={cell.date} cell={cell} />
      ))}
    </div>
  );
}
