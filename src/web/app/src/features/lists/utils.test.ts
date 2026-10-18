import { describe, expect, it } from "vitest";
import { formatListItemUpdatedDate } from "./utils";

describe("formatListItemUpdatedDate", () => {
  it.each([undefined, null, "", "not-a-date"])(
    "omits a missing or invalid timestamp (%s)",
    (updatedAtUtc) => {
      expect(formatListItemUpdatedDate(updatedAtUtc)).toBeNull();
    },
  );

  it("formats a valid timestamp as a localized month and day", () => {
    const value = "2026-10-18T12:00:00.000Z";
    const expected = new Date(value).toLocaleDateString(undefined, {
      month: "short",
      day: "numeric",
    });

    expect(formatListItemUpdatedDate(value)).toBe(expected);
  });
});
