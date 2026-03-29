import { request } from "./request";

export interface DeploymentModeResponse {
  deploymentMode: "SingleInstance" | "CloudHosted";
  canCreateHousehold: boolean;
  requiresInvitation: boolean;
  supportsEmail: boolean;
  supportsAdminTools: boolean;
}

export const platformApi = {
  getDeploymentMode: (): Promise<DeploymentModeResponse> =>
    request<DeploymentModeResponse>("/api/platform/deployment-mode", undefined, null),
};
