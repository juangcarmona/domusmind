import { useNavigate } from "react-router-dom";
import { LoginPage } from "../features/auth/pages/LoginPage";

/**
 * Unauthenticated app route. New accounts are created through first-run setup
 * or household member provisioning, not public self-registration.
 */
export function UnauthRoutes() {
  const nav = useNavigate();

  return (
    <LoginPage
      onSuccess={() => nav("/")}
    />
  );
}
