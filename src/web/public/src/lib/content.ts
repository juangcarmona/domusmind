import { defaultLocale, type Locale } from './i18n';

export type LocalizedContent = Record<string, unknown>;

const modules = import.meta.glob('../content/*/*.json', { eager: true, import: 'default' }) as Record<string, LocalizedContent>;

function getContentPath(locale: Locale, key: string): string {
  return `../content/${locale}/${key}.json`;
}

export function getContent<T extends LocalizedContent = LocalizedContent>(locale: Locale, key: string): T {
  const localizedPath = getContentPath(locale, key);
  const canonicalPath = getContentPath(defaultLocale, key);
  return (modules[localizedPath] ?? modules[canonicalPath]) as T;
}
