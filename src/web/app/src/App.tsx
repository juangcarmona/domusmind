import { useState } from "react";
import { AuthProvider, useAuth } from "./auth/AuthProvider";
import { LoginPage } from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";
import { ProfilePage } from "./pages/ProfilePage";

type Page = "login" | "register" | "profile";

function AppShell() {
  const { user, isLoading } = useAuth();
  const [page, setPage] = useState<Page>("login");

  if (isLoading) {
    return <p style={{ textAlign: "center", marginTop: 80 }}>Loading…</p>;
  }

  if (user) {
    return <ProfilePage onLogout={() => setPage("login")} />;
  }

  if (page === "register") {
    return (
      <RegisterPage
        onSuccess={() => setPage("login")}
        onGoToLogin={() => setPage("login")}
      />
    );
  }

  return (
    <LoginPage
      onSuccess={() => setPage("profile")}
      onGoToLogin={() => setPage("register")}
    />
  );
}

export default function App() {
  return (
    <AuthProvider>
      <AppShell />
    </AuthProvider>
  );
}

