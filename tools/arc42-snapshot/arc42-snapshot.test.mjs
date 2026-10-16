import test from 'node:test';
import assert from 'node:assert/strict';
import { fileURLToPath } from 'node:url';
import { dirname, resolve } from 'node:path';
import { loadDecisionRecords } from './generate.mjs';
import { loadDocs } from './loadDocs.mjs';
import { renderMarkdown } from './render.mjs';

const repoRoot = resolve(dirname(fileURLToPath(import.meta.url)), '../..');
const architectureDirectory = resolve(repoRoot, 'docs/architecture');

test('loads the complete arc42 contract and architecture decision records', async () => {
  const docs = await loadDocs(architectureDirectory);
  const records = await loadDecisionRecords(architectureDirectory);
  assert.equal(docs.sections.length, 12);
  assert.equal(records.length, 10);
  assert.ok(records.some(({ name }) => name === '0010-use-guid-backed-strongly-typed-identifiers.md'));
  assert.ok(records.every(({ content }) => content.includes('#')));
});

test('rejects an invalid architecture document folder instead of publishing a partial snapshot', async () => {
  await assert.rejects(loadDocs(resolve(repoRoot, 'docs/adr')), /no NN-\*\.md section files found/);
});

test('turns source links into revision-pinned GitHub links in the offline snapshot', async () => {
  const result = await renderMarkdown('[Public site source](../../src/web/public)', {
    fileToRoute: new Map(),
    sourceFile: resolve(architectureDirectory, '07-deployment-view.md'),
    repositoryRoot: repoRoot,
    repositoryBaseUrl: { base: 'https://github.com/juangcarmona/domusmind', revision: 'abc123' },
  });
  assert.match(result.html, /https:\/\/github\.com\/juangcarmona\/domusmind\/tree\/abc123\/src\/web\/public/);
  assert.doesNotMatch(result.html, /xref-dead/);
});

test('removes duplicate document titles and hides authored metadata comments', async () => {
  const result = await renderMarkdown('<!-- internal citation -->\n# Architecture\n\nText.', {
    fileToRoute: new Map(),
    title: 'Architecture',
  });
  assert.doesNotMatch(result.html, /internal citation/);
  assert.equal((result.html.match(/<h1\b/g) ?? []).length, 0);
  assert.match(result.html, /Text\./);
});
