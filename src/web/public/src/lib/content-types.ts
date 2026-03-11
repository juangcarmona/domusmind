export interface UiContent {
  siteName: string;
  tagline: string;
  nav: Record<string, string>;
  themeLabel: string;
  languageLabel: string;
  footer: Record<string, string>;
}

export interface LinkItem {
  label: string;
  href: string;
}

export interface SectionContent {
  title: string;
  body?: string;
  list?: string[];
}

export interface HomeContent {
  title: string;
  description: string;
  hero: {
    title: string;
    body: string;
    primaryCta: LinkItem;
    secondaryCta: LinkItem;
  };
  sections: SectionContent[];
  cta: {
    developer: { title: string; body: string; label: string; href: string };
    family: { title: string; body: string; label: string; href: string };
  };
}

export interface SectionsPageContent {
  title: string;
  description: string;
  sections: SectionContent[];
}

export interface DocsContent extends SectionsPageContent {
  links: LinkItem[];
}

export interface FaqContent {
  title: string;
  description: string;
  items: Array<{ q: string; a: string }>;
}
