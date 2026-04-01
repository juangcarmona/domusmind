export interface MemberAvatarIcon {
  id: number;
  /** Key suffix used for accessible labels and future i18n. */
  labelKey: string;
  /** Public URL - served from /public/avatars/. */
  src: string;
}

function icon(id: number, labelKey: string): MemberAvatarIcon {
  return { id, labelKey, src: `/avatars/avatar_${id}.svg` };
}

/**
 * Fixed catalog of 20 member avatar icons.
 *
 * SVG assets are expected at:
 *   /public/avatars/avatar_{id}.svg
 *
 * Dark mode adaptation is handled via CSS filter in MemberAvatar
 * (brightness(0) invert(1) turns black SVG fills to white).
 */
export const MEMBER_AVATAR_ICONS: readonly MemberAvatarIcon[] = [
  icon(1,  "person"),
  icon(2,  "baby"),
  icon(3,  "child"),
  icon(4,  "teen"),
  icon(5,  "graduate"),
  icon(6,  "elder"),
  icon(7,  "dog"),
  icon(8,  "cat"),
  icon(9,  "bunny"),
  icon(10, "bird"),
  icon(11, "heart"),
  icon(12, "person2"),
  icon(13, "person3"),
  icon(14, "person4"),
  icon(15, "person5"),
  icon(16, "person6"),
  icon(17, "person7"),
  icon(18, "face"),
  icon(19, "face2"),
  icon(20, "person8"),
];

export function getAvatarIcon(iconId: number | null | undefined): MemberAvatarIcon | null {
  if (!iconId) return null;
  return MEMBER_AVATAR_ICONS.find((i) => i.id === iconId) ?? null;
}
