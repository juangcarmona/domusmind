import { useEffect, useState } from "react";
import { adminApi, type AdminSummaryResponse } from "../../../api/adminApi";

export function AdminOverviewPage() {
  const [summary, setSummary] = useState<AdminSummaryResponse | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    adminApi.getSummary()
      .then(setSummary)
      .catch((e: { message?: string }) => setError(e.message ?? "Failed to load"));
  }, []);

  if (error) return <p className="error-msg">{error}</p>;
  if (!summary) return <p>Loading…</p>;

  return (
    <div>
      <h2 className="admin-section-title" style={{ marginBottom: "1rem" }}>System Overview</h2>
      <div className="admin-stat-grid">
        <div className="admin-stat">
          <div className="admin-stat-value">{summary.householdCount}</div>
          <div className="admin-stat-label">Households</div>
        </div>
        <div className="admin-stat">
          <div className="admin-stat-value">{summary.userCount}</div>
          <div className="admin-stat-label">Users</div>
        </div>
        <div className="admin-stat">
          <div className="admin-stat-value">{summary.pendingInvitationCount}</div>
          <div className="admin-stat-label">Pending invitations</div>
        </div>
      </div>
      <div className="admin-card" style={{ padding: "1rem", display: "flex", flexDirection: "column", gap: "0.4rem" }}>
        <div style={{ fontSize: "0.875rem" }}>
          <strong>Deployment mode:</strong>{" "}
          <span className="admin-badge admin-badge--operator">{summary.deploymentMode}</span>
        </div>
        <div style={{ fontSize: "0.875rem" }}>
          <strong>System initialized:</strong>{" "}
          <span className={`admin-badge ${summary.isSystemInitialized ? "admin-badge--active" : "admin-badge--disabled"}`}>
            {summary.isSystemInitialized ? "Yes" : "No"}
          </span>
        </div>
      </div>
    </div>
  );
}
