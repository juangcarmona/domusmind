import type { Locale } from './i18n';

export function pageTitle(title: string): string {
  return `${title} · DomusMind`;
}

export function canonicalUrl(locale: Locale, pathname: string): string {
  return `https://domusmind.org/${locale}${pathname}`;
}
