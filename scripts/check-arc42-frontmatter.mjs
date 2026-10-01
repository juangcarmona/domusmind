// Asserts the document contract that the `architecture-docs` skill defines for docs/architecture/.
//
// Every numbered section carries exactly three frontmatter keys - title, arc42-section and
// description - and nothing else. The skill is explicit that status, owner, review-date and
// inventory fields are banned because they drift; a checker that only verified the three were
// present would let a fourth be added, so this fails on extras too.
//
// README.md is navigation, not a thirteenth section, so it carries title and description and
// must NOT carry arc42-section.

import { readdirSync, readFileSync } from 'node:fs';
import { join } from 'node:path';

const ROOT = 'docs/architecture';
const REQUIRED = ['title', 'arc42-section', 'description'];
const EXPECTED_SECTIONS = 12;

const errors = [];
const fail = (file, message) => errors.push(`${file}: ${message}`);

/** The frontmatter block as key/value pairs, or null when the file has none. */
function frontmatter(text) {
  if (!text.startsWith('---\n')) return null;
  const end = text.indexOf('\n---', 4);
  if (end === -1) return null;
  const pairs = new Map();
  for (const line of text.slice(4, end).split('\n')) {
    if (!line.trim()) continue;
    const colon = line.indexOf(':');
    if (colon === -1) continue;
    pairs.set(line.slice(0, colon).trim(), line.slice(colon + 1).trim());
  }
  return pairs;
}

const files = readdirSync(ROOT).filter((f) => f.endsWith('.md'));
const numbered = files.filter((f) => /^\d{2}-/.test(f)).sort();

for (const file of files) {
  // Windows checkouts convert to CRLF; the contract is about keys, not line endings.
  const fm = frontmatter(readFileSync(join(ROOT, file), 'utf8').replace(/\r\n/g, '\n'));
  if (!fm) {
    fail(file, 'has no YAML frontmatter');
    continue;
  }

  if (file === 'README.md') {
    for (const key of ['title', 'description']) {
      if (!fm.has(key)) fail(file, `is missing the '${key}' key`);
    }
    // The navigation index is not a section, so claiming a section number would make it one.
    if (fm.has('arc42-section')) {
      fail(file, "carries 'arc42-section'; the navigation index is not a thirteenth section");
    }
    continue;
  }

  if (!/^\d{2}-/.test(file)) {
    fail(file, 'is neither the README nor a NN-prefixed section');
    continue;
  }

  for (const key of REQUIRED) {
    if (!fm.has(key)) fail(file, `is missing the '${key}' key`);
  }
  for (const key of fm.keys()) {
    if (!REQUIRED.includes(key)) {
      fail(file, `carries '${key}'; only ${REQUIRED.join(', ')} are allowed, because anything else drifts`);
    }
  }

  const declared = (fm.get('arc42-section') ?? '').replace(/^["']|["']$/g, '');
  const fromName = file.slice(0, 2);
  if (declared !== fromName) {
    fail(file, `declares arc42-section '${declared}' but its filename says '${fromName}'`);
  }
}

if (numbered.length !== EXPECTED_SECTIONS) {
  errors.push(
    `${ROOT}: found ${numbered.length} numbered sections, expected ${EXPECTED_SECTIONS} (${numbered.join(', ') || 'none'})`,
  );
}

if (errors.length > 0) {
  console.error(`arc42 document contract violated (${errors.length}):\n`);
  for (const e of errors) console.error(`  - ${e}`);
  process.exit(1);
}

console.log(`arc42 document contract holds: ${numbered.length} sections plus the navigation index.`);
