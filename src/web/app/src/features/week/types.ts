export interface WeeklyGridEventItem {
  eventId: string;
  title: string;
  startTime: string;
  endTime: string | null;
  status: string;
}

export interface WeeklyGridTaskItem {
  taskId: string;
  title: string;
  dueDate: string | null;
  status: string;
}

export interface WeeklyGridCell {
  date: string;
  events: WeeklyGridEventItem[];
  tasks: WeeklyGridTaskItem[];
}

export interface WeeklyGridMember {
  memberId: string;
  name: string;
  role: string;
  cells: WeeklyGridCell[];
}

export interface WeeklyGridRoutineItem {
  routineId: string;
  name: string;
  cadence: string;
  status: string;
}

export interface WeeklyGridResponse {
  weekStart: string;
  weekEnd: string;
  members: WeeklyGridMember[];
  routines: WeeklyGridRoutineItem[];
}
