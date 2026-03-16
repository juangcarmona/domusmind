import { useState, type FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { createFamily, addMember } from "../store/householdSlice";
import { HouseholdLogo } from "../components/HouseholdLogo";

type Step = 0 | 1 | 2 | 3;

const STEP_COUNT = 4;

export function OnboardingPage() {
  const dispatch = useAppDispatch();
  const nav = useNavigate();
  const household = useAppSelector((s) => s.household);

  const [step, setStep] = useState<Step>(0);
  const [householdName, setHouseholdName] = useState("");
  const [people, setPeople] = useState<{ name: string; role: string }[]>([]);
  const [personName, setPersonName] = useState("");
  const [personRole, setPersonRole] = useState("Adult");
  const [addError, setAddError] = useState<string | null>(null);
  const [createError, setCreateError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const familyId = household.family?.familyId;

  async function handleCreateHousehold(e: FormEvent) {
    e.preventDefault();
    if (!householdName.trim()) return;
    setSubmitting(true);
    setCreateError(null);
    const result = await dispatch(createFamily(householdName.trim()));
    setSubmitting(false);
    if (createFamily.fulfilled.match(result)) {
      setStep(2);
    } else {
      setCreateError(result.payload as string ?? "Something went wrong");
    }
  }

  function handleAddPerson() {
    const name = personName.trim();
    if (!name) return;
    setPeople((prev) => [...prev, { name, role: personRole }]);
    setPersonName("");
    setAddError(null);
  }

  function handleRemovePerson(i: number) {
    setPeople((prev) => prev.filter((_, idx) => idx !== i));
  }

  async function handleSavePeople() {
    if (!familyId) return;
    setSubmitting(true);
    for (const p of people) {
      await dispatch(addMember({ familyId, name: p.name, role: p.role }));
    }
    setSubmitting(false);
    setStep(3);
  }

  function handleFinish() {
    nav("/timeline");
  }

  function renderDots() {
    return (
      <div className="progress-dots">
        {Array.from({ length: STEP_COUNT }).map((_, i) => (
          <div
            key={i}
            className={`progress-dot ${i < step ? "done" : i === step ? "active" : ""}`}
          />
        ))}
      </div>
    );
  }

  /* ---- Step 0: Welcome ---- */
  if (step === 0) {
    return (
      <div className="onboarding-wrap">
        <div className="onboarding-card">
          <div className="logo-wrap">
            <HouseholdLogo size={48} />
          </div>
          {renderDots()}
          <h1>Welcome to DomusMind</h1>
          <p>Your household coordination system. Set up in minutes.</p>
          <button
            className="btn"
            style={{ width: "100%", justifyContent: "center" }}
            onClick={() => setStep(1)}
          >
            Start your household
          </button>
        </div>
      </div>
    );
  }

  /* ---- Step 1: Name household ---- */
  if (step === 1) {
    return (
      <div className="onboarding-wrap">
        <div className="onboarding-card">
          <div className="logo-wrap">
            <HouseholdLogo size={48} />
          </div>
          {renderDots()}
          <p className="onboarding-step-label">Step 1 of 3</p>
          <h1>Name your household</h1>
          <p>What should we call your household?</p>
          <form onSubmit={handleCreateHousehold}>
            <div className="form-group">
              <input
                className="form-control"
                type="text"
                placeholder="e.g. Carmona Family"
                value={householdName}
                onChange={(e) => setHouseholdName(e.target.value)}
                required
                autoFocus
              />
            </div>
            {createError && <p className="error-msg">{createError}</p>}
            <button
              type="submit"
              className="btn"
              style={{ width: "100%", justifyContent: "center" }}
              disabled={submitting || !householdName.trim()}
            >
              {submitting ? "Creating…" : "Create Household"}
            </button>
          </form>
        </div>
      </div>
    );
  }

  /* ---- Step 2: Add people ---- */
  if (step === 2) {
    return (
      <div className="onboarding-wrap">
        <div className="onboarding-card">
          <div className="logo-wrap">
            <HouseholdLogo size={48} />
          </div>
          {renderDots()}
          <p className="onboarding-step-label">Step 2 of 3</p>
          <h1>Who lives here?</h1>
          <p>Add the people in your household. You can always add more later.</p>

          {people.length > 0 && (
            <div className="people-chips">
              {people.map((p, i) => (
                <span key={i} className="people-chip">
                  {p.name}
                  <span style={{ fontSize: "0.75rem", opacity: 0.7, marginLeft: "0.1rem" }}>
                    ({p.role})
                  </span>
                  <button
                    type="button"
                    onClick={() => handleRemovePerson(i)}
                    aria-label={`Remove ${p.name}`}
                  >
                    ×
                  </button>
                </span>
              ))}
            </div>
          )}

          <div className="inline-form" style={{ marginBottom: "1rem" }}>
            <div className="form-group" style={{ flex: 2 }}>
              <input
                className="form-control"
                type="text"
                placeholder="Name"
                value={personName}
                onChange={(e) => setPersonName(e.target.value)}
                onKeyDown={(e) => {
                  if (e.key === "Enter") {
                    e.preventDefault();
                    handleAddPerson();
                  }
                }}
              />
            </div>
            <div className="form-group" style={{ flex: 1 }}>
              <select
                className="form-control"
                value={personRole}
                onChange={(e) => setPersonRole(e.target.value)}
              >
                <option value="Adult">Adult</option>
                <option value="Child">Child</option>
                <option value="Teen">Teen</option>
              </select>
            </div>
            <button
              type="button"
              className="btn btn-ghost"
              onClick={handleAddPerson}
              disabled={!personName.trim()}
            >
              Add
            </button>
          </div>

          {addError && <p className="error-msg">{addError}</p>}

          <div style={{ display: "flex", gap: "0.5rem" }}>
            <button
              className="btn"
              style={{ flex: 1, justifyContent: "center" }}
              onClick={handleSavePeople}
              disabled={submitting}
            >
              {submitting ? "Saving…" : people.length > 0 ? "Save & continue" : "Skip for now"}
            </button>
          </div>
        </div>
      </div>
    );
  }

  /* ---- Step 3: Done ---- */
  return (
    <div className="onboarding-wrap">
      <div className="onboarding-card">
        <div className="logo-wrap">
          <HouseholdLogo size={48} />
        </div>
        {renderDots()}
        <h1>Your household is ready</h1>
        <p>
          {household.family?.name} is set up and ready to use. Open your
          timeline to see what matters today.
        </p>
        <button
          className="btn"
          style={{ width: "100%", justifyContent: "center" }}
          onClick={handleFinish}
        >
          Open Timeline →
        </button>
      </div>
    </div>
  );
}
