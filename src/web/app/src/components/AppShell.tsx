import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthProvider";
import { HouseholdLogo } from "./HouseholdLogo";
import { useAppSelector } from "../store/hooks";

export function AppShell({ children }: { children: React.ReactNode }) {
  const { logout } = useAuth();
  const nav = useNavigate();
  const family = useAppSelector((s) => s.household.family);

  async function handleLogout() {
    await logout();
    nav("/login");
  }

  return (
    <div className="app-layout">
      <header className="site-header">
        <NavLink to="/timeline" className="brand">
          <HouseholdLogo className="brand-mark" />
          {family?.name ?? "DomusMind"}
        </NavLink>

        <nav aria-label="Primary">
          <ul>
            <li>
              <NavLink to="/timeline" className={({ isActive }) => isActive ? "active" : undefined}>
                Timeline
              </NavLink>
            </li>
            <li>
              <NavLink to="/people" className={({ isActive }) => isActive ? "active" : undefined}>
                People
              </NavLink>
            </li>
            <li>
              <NavLink to="/areas" className={({ isActive }) => isActive ? "active" : undefined}>
                Areas
              </NavLink>
            </li>
            <li>
              <NavLink to="/plans" className={({ isActive }) => isActive ? "active" : undefined}>
                Plans
              </NavLink>
            </li>
            <li>
              <NavLink to="/tasks" className={({ isActive }) => isActive ? "active" : undefined}>
                Chores
              </NavLink>
            </li>
          </ul>
        </nav>

        <div className="header-controls">
          <button className="btn btn-ghost btn-sm" onClick={handleLogout}>
            Sign out
          </button>
        </div>
      </header>

      <main className="app-main">{children}</main>
    </div>
  );
}
