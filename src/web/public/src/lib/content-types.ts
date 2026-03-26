export interface UiContent {
  siteName: string;
  tagline: string;
  nav: Record<string, string>;
  themeLabel: string;
  languageLabel: string;
  footer: Record<string, string>;
  fallbackNotice?: string;
}

export interface SectionContent {
  title: string;
  body?: string;
  list?: string[];
}

export interface LinkContent {
  label: string;
  href: string;
}

export interface HeroProofPanelContent {
  eyebrow: string;
  title: string;
  groups: string[][];
  caption: string;
}

export interface HeroContent {
  headline: string;
  supportLine: string;
  proofBullets: string[];
  primaryCta: LinkContent;
  secondaryCta: LinkContent;
  proofPanel: HeroProofPanelContent;
}

export interface RealityContent {
  title: string;
  items: string[];
  summary: string;
}

export interface BreakdownItemContent {
  title: string;
  detail: string;
}

export interface BreakdownContent {
  title: string;
  items: BreakdownItemContent[];
}

export interface ShiftContent {
  title: string;
  statement: string;
  outcomes: string[];
}

export interface ProofSlotContent {
  title: string;
  caption: string;
  placeholder: string;
  alt: string;
  imageSrc?: string;
  preview?: {
    eyebrow: string;
    groups: string[][];
  };
}

export interface ProofContent {
  title: string;
  primary: ProofSlotContent;
  secondary?: ProofSlotContent;
}

export interface StepContent {
  title: string;
  body: string;
}

export interface HowItWorksContent {
  title: string;
  steps: StepContent[];
}

export interface CurrentStateGroupContent {
  title: string;
  items: string[];
}

export interface CurrentStateContent {
  title: string;
  groups: CurrentStateGroupContent[];
}

export interface EarlyAccessContent {
  title: string;
  body: string;
  primaryCta: LinkContent;
  secondaryCta?: LinkContent;
}

export interface HomeFrontmatter {
  title: string;
  description: string;
  hero: HeroContent;
  reality: RealityContent;
  breakdown: BreakdownContent;
  shift: ShiftContent;
  proof: ProofContent;
  howItWorks: HowItWorksContent;
  currentState: CurrentStateContent;
  earlyAccess: EarlyAccessContent;
}

export interface PageFrontmatter {
  title: string;
  description: string;
}
