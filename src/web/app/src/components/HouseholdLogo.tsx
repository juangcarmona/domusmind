export function HouseholdLogo({
  size = 24,
  className,
}: {
  size?: number;
  className?: string;
}) {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 256 256"
      xmlns="http://www.w3.org/2000/svg"
      aria-hidden="true"
      className={className}
    >
      <polygon fill="currentColor" points="128,24 24,112 24,232 232,232 232,112" />
      <rect fill="currentColor" x="172" y="52" width="20" height="44" />
      <polygon
        fill="var(--surface)"
        points="60,216 60,112 128,168 196,112 196,216 164,216 164,166 128,220 92,166 92,216"
      />
    </svg>
  );
}
