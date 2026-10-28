import test from 'node:test';
import assert from 'node:assert/strict';
import { fileURLToPath } from 'node:url';
import { dirname, resolve } from 'node:path';
import { loadDecisionRecords } from './generate.mjs';
import { loadDocs } from './loadDocs.mjs';
import { renderMarkdown } from './render.mjs';
import { JSDOM } from 'jsdom';
import { buildSnapshotHtml } from './shell.mjs';

const repoRoot = resolve(dirname(fileURLToPath(import.meta.url)), '../..');
const architectureDirectory = resolve(repoRoot, 'docs/architecture');

function sampleSnapshot(url = 'https://example.test/', readmeHtml = '<p>Product and architecture truth.</p>') {
  const sections = Array.from({ length: 12 }, (_, index) => {
    const number = String(index + 1).padStart(2, '0');
    const title = `Section title ${String.fromCharCode(65 + index)}`;
    return { number, title, description: `Description for ${number}.`, html: `<p>Body for ${number}.</p>` };
  });
  const html = buildSnapshotHtml({
    readme: { title: 'Architecture guide', description: 'Where truth lives.', html: readmeHtml },
    sections,
    stackName: 'DomusMind',
    revision: 'abcdef123456',
  });
  return new JSDOM(html, { runScripts: 'dangerously', url, pretendToBeVisual: true });
}

const tick = () => new Promise((resolve) => setTimeout(resolve, 10));

test('loads the complete arc42 contract and architecture decision records', async () => {
  const docs = await loadDocs(architectureDirectory);
  const records = await loadDecisionRecords(architectureDirectory);
  assert.equal(docs.sections.length, 12);
  assert.equal(records.length, 11);
  assert.ok(records.some(({ name }) => name === '0010-use-guid-backed-strongly-typed-identifiers.md'));
  assert.ok(records.some(({ name }) => name === '0011-pilot-oidc-bff-in-development.md'));
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

test('README links and heading fragments route through the named introduction view', async () => {
  const result = await renderMarkdown('[This introduction](#where-truth-lives) and [the guide](README.md#where-truth-lives)', {
    fileToRoute: new Map([['README.md', '#/introduction']]),
    currentRoute: '#/introduction',
  });
  assert.equal((result.html.match(/href="#\/introduction\/where-truth-lives"/g) ?? []).length, 2);
  const dom = sampleSnapshot('https://example.test/#/introduction/where-truth-lives', '<h2 id="where-truth-lives">Where truth lives</h2>');
  assert.equal(dom.window.document.querySelector('h2.doc-title').textContent, 'Architecture guide');
  assert.match(dom.window.document.querySelector('#content').textContent, /Where truth lives/);
  dom.window.close();
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

test('overview presents twelve section cards and routes to a title without a duplicate number', async () => {
  const dom = sampleSnapshot();
  const { document, location } = dom.window;
  assert.equal(document.querySelectorAll('.section-card').length, 12);
  assert.equal(document.querySelectorAll('.section-card .nav-icon').length, 12);
  assert.match(document.querySelector('.section-card').getAttribute('aria-label'), /Section 01: Section title A/);
  assert.match(document.querySelector('.section-card').textContent, /Context/);

  document.querySelector('.section-card').click();
  await tick();
  assert.equal(location.hash, '#/section/01');
  assert.equal(document.querySelector('h2.doc-title').textContent, 'Section title A');
  assert.equal(document.querySelectorAll('h2.doc-title').length, 1);
  assert.equal(document.querySelector('.section-kicker').textContent, 'Section 01');
  assert.equal(document.querySelector('.doc-desc').textContent, 'Description for 01.');
  dom.window.close();
});

test('introduction remains reachable as a named route and is indexed separately from overview', () => {
  const dom = sampleSnapshot('https://example.test/#/introduction');
  const { document } = dom.window;
  assert.equal(document.querySelector('h2.doc-title').textContent, 'Architecture guide');
  assert.match(document.querySelector('#content').textContent, /Product and architecture truth/);
  assert.equal(document.querySelector('a[data-route="#/introduction"]').getAttribute('aria-current'), 'page');
  dom.window.close();
});

test('desktop navigation collapses with abbreviations and keeps accessible section names', () => {
  const dom = sampleSnapshot();
  const { document } = dom.window;
  const toggle = document.getElementById('collapse-toggle');
  toggle.click();
  assert.ok(document.getElementById('shell').classList.contains('nav-collapsed'));
  assert.equal(toggle.getAttribute('aria-expanded'), 'false');
  assert.equal(document.querySelector('a[data-route="#/section/01"]').getAttribute('aria-label'), 'Section 01: Section title A');
  assert.equal(document.querySelector('a[data-route="#/section/01"] .t-short').textContent, 'Intro');
  dom.window.close();
});

test('mobile drawer closes by Escape, backdrop, and section selection', async () => {
  const dom = sampleSnapshot();
  const { document, KeyboardEvent } = dom.window;
  const opener = document.getElementById('nav-toggle');
  const open = () => opener.click();
  const shell = document.getElementById('shell');
  open();
  assert.ok(shell.classList.contains('drawer-open'));
  assert.equal(document.activeElement.id, 'nav-close');
  document.dispatchEvent(new KeyboardEvent('keydown', { key: 'Escape', bubbles: true }));
  assert.ok(!shell.classList.contains('drawer-open'));
  assert.equal(document.activeElement, opener);

  open();
  document.getElementById('nav-backdrop').click();
  assert.ok(!shell.classList.contains('drawer-open'));

  open();
  document.querySelector('a[data-route="#/section/02"]').click();
  await tick();
  assert.ok(!shell.classList.contains('drawer-open'));
  assert.equal(dom.window.location.hash, '#/section/02');
  dom.window.close();
});

test('previous and next navigation names destinations without repeating section numbers', () => {
  const dom = sampleSnapshot('https://example.test/#/section/02');
  const { document } = dom.window;
  const previous = document.getElementById('nav-prev');
  const next = document.getElementById('nav-next');
  assert.equal(previous.getAttribute('aria-label'), 'Previous section: Section title A');
  assert.equal(next.getAttribute('aria-label'), 'Next section: Section title C');
  assert.equal(previous.textContent, '←Section title A');
  assert.equal(next.textContent, '→Section title C');
  assert.doesNotMatch(next.textContent, /03/);
  dom.window.close();
});
