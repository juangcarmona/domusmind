import { readFile, readdir } from 'node:fs/promises';

const output = process.argv[2];
if (!output) throw new Error('Usage: node scripts/check-architecture-site.mjs <html-file>');
const html = await readFile(output, 'utf8');
const records = (await readdir('docs/adr')).filter((name) => /^\d{4}-.+\.md$/.test(name));
if (records.length === 0) throw new Error('No ADRs found');
for (const name of records) {
  if (!html.includes(`#/section/09/adr/${name.slice(0, -3)}`)) throw new Error(`ADR missing from reader: ${name}`);
}
if (html.includes('.arcshape-build-')) throw new Error('Reader contains temporary source paths');
console.log(`ArcShape reader includes ${records.length} ADRs and no temporary source paths.`);
