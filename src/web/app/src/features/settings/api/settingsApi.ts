import {
  domusmindApi,
  type UpdateFamilySettingsRequest,
  type UpdateFamilySettingsResponse,
} from "../../../api/domusmindApi";

export const settingsApi = {
  updateHouseholdSettings: (
    familyId: string,
    body: UpdateFamilySettingsRequest,
  ): Promise<UpdateFamilySettingsResponse> =>
    domusmindApi.updateFamilySettings(familyId, body),
};

export type { UpdateFamilySettingsRequest, UpdateFamilySettingsResponse };
