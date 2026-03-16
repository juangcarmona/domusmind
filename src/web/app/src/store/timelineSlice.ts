import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { domusmindApi, type EnrichedTimelineResponse } from "../api/domusmindApi";

interface TimelineState {
  data: EnrichedTimelineResponse | null;
  status: "idle" | "loading" | "success" | "error";
  error: string | null;
}

const initialState: TimelineState = {
  data: null,
  status: "idle",
  error: null,
};

export const fetchTimeline = createAsyncThunk(
  "timeline/fetch",
  async (
    {
      familyId,
      types,
      memberFilter,
      statuses,
    }: {
      familyId: string;
      types?: string;
      memberFilter?: string;
      statuses?: string;
    },
    { rejectWithValue },
  ) => {
    try {
      return await domusmindApi.getEnrichedTimeline(familyId, {
        types,
        memberFilter,
        statuses,
      });
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to load timeline",
      );
    }
  },
);

const timelineSlice = createSlice({
  name: "timeline",
  initialState,
  reducers: {
    clearTimeline(state) {
      state.data = null;
      state.status = "idle";
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchTimeline.pending, (state) => {
        state.status = "loading";
        state.error = null;
      })
      .addCase(fetchTimeline.fulfilled, (state, action) => {
        state.data = action.payload;
        state.status = "success";
      })
      .addCase(fetchTimeline.rejected, (state, action) => {
        state.status = "error";
        state.error = action.payload as string;
      });
  },
});

export const { clearTimeline } = timelineSlice.actions;
export default timelineSlice.reducer;
