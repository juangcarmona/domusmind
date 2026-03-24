import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import {
  domusmindApi,
  type HouseholdAreaItem,
} from "../api/domusmindApi";

interface AreasState {
  items: HouseholdAreaItem[];
  status: "idle" | "loading" | "success" | "error";
  error: string | null;
}

const initialState: AreasState = {
  items: [],
  status: "idle",
  error: null,
};

export const fetchAreas = createAsyncThunk(
  "areas/fetch",
  async (familyId: string, { rejectWithValue }) => {
    try {
      const res = await domusmindApi.getAreas(familyId);
      return res.areas;
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to load areas",
      );
    }
  },
);

export const createArea = createAsyncThunk(
  "areas/create",
  async (
    { familyId, name }: { familyId: string; name: string },
    { rejectWithValue },
  ) => {
    try {
      const res = await domusmindApi.createArea({ name, familyId });
      return {
        areaId: res.responsibilityDomainId,
        name: res.name,
        primaryOwnerId: null,
        primaryOwnerName: null,
        secondaryOwnerIds: [],
      } as HouseholdAreaItem;
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to create area",
      );
    }
  },
);

export const assignPrimaryOwner = createAsyncThunk(
  "areas/assignPrimary",
  async (
    { areaId, memberId, familyId }: { areaId: string; memberId: string; familyId: string },
    { rejectWithValue, dispatch },
  ) => {
    try {
      await domusmindApi.assignPrimaryOwner(areaId, { memberId });
      dispatch(fetchAreas(familyId));
      return { areaId, memberId };
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to assign owner",
      );
    }
  },
);

export const transferArea = createAsyncThunk(
  "areas/transfer",
  async (
    {
      areaId,
      newPrimaryOwnerId,
      familyId,
    }: { areaId: string; newPrimaryOwnerId: string; familyId: string },
    { rejectWithValue, dispatch },
  ) => {
    try {
      await domusmindApi.transferArea(areaId, { newPrimaryOwnerId });
      dispatch(fetchAreas(familyId));
      return { areaId, newPrimaryOwnerId };
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to transfer area",
      );
    }
  },
);

export const renameArea = createAsyncThunk(
  "areas/rename",
  async (
    { areaId, name }: { areaId: string; name: string },
    { rejectWithValue },
  ) => {
    try {
      const res = await domusmindApi.renameArea(areaId, { name });
      return { areaId: res.responsibilityDomainId, name: res.name };
    } catch (err: unknown) {
      return rejectWithValue(
        (err as { message?: string }).message ?? "Failed to rename area",
      );
    }
  },
);

const areasSlice = createSlice({
  name: "areas",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchAreas.pending, (state) => {
        state.status = "loading";
        state.error = null;
      })
      .addCase(fetchAreas.fulfilled, (state, action) => {
        state.items = action.payload;
        state.status = "success";
      })
      .addCase(fetchAreas.rejected, (state, action) => {
        state.status = "error";
        state.error = action.payload as string;
      })
      .addCase(createArea.fulfilled, (state, action) => {
        state.items.push(action.payload);
      })
      .addCase(renameArea.fulfilled, (state, action) => {
        const { areaId, name } = action.payload;
        const item = state.items.find((a) => a.areaId === areaId);
        if (item) item.name = name;
      });
  },
});

export default areasSlice.reducer;
