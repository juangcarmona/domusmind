import { NavLink, Outlet } from "react-router-dom";
import { useAuth } from "../../../auth/AuthProvider";
import { HouseholdLogo } from "../../../components/HouseholdLogo";
import "../admin.css";

export function AdminShell() {
  const { logout } = useAuth();

  return (
    <div className="admin-layout">
      <header className="admin-header">
        <HouseholdLogo size={28} />
        <span className="admin-header-title">DomusMind</span>
        <span className="admin-header-badge">Operator</span>
        <a href="/" className="admin-back-link" onClick={(e) => { e.preventDefault(); window.location.href = "/"; }}>
          ← App
        </a>
        <button
          type="button"
          className="btn btn-secondary"
          style={{ marginLeft: "0.5rem", fontSize: "0.8rem", padding: "0.25rem 0.75rem" }}
          onClick={() => void logout()}
        >
          Sign out
        </button>
      </header>
      <nav className="admin-nav">
        <NavLink to="/admin" end className={({ isActive }) => `admin-nav-link${isActive ? " active" : ""}`}>
          Overview
        </NavLink>
        <NavLink to="/admin/households" className={({ isActive }) => `admin-nav-link${isActive ? " active" : ""}`}>
          Households
        </NavLink>
        <NavLink to="/admin/users" className={({ isActive }) => `admin-nav-link${isActive ? " active" : ""}`}>
          Users
        </NavLink>
        <NavLink to="/admin/invitations" className={({ isActive }) => `admin-nav-link${isActive ? " active" : ""}`}>
          Invitations
        </NavLink>
      </nav>
      <main className="admin-content">
        <Outlet />
      </main>
    </div>
  );
}
