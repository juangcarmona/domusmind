import test from 'node:test';
import assert from 'node:assert/strict';
import { mkdtemp, mkdir, readFile, rm, writeFile } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { buildLanding, finalizeSite, restoreSite, safeRelativePath } from './static-site.mjs';

async function temporaryDirectory() {
  return mkdtemp(join(tmpdir(), 'domusmind-site-'));
}

test('rejects unsafe manifest paths', () => {
  for (const path of ['../index.html', '/index.html', 'e2e/../../index.html', 'bad name.html']) {
    assert.throws(() => safeRelativePath(path), /Unsafe site path/);
  }
  assert.equal(safeRelativePath('architecture/index.html'), 'architecture/index.html');
});

test('restores only manifest files and preserves unknown site sections', async () => {
  const directory = await temporaryDirectory();
  const originalFetch = globalThis.fetch;
  const files = new Map([
    ['https://example.github.io/domusmind/site-manifest.json', JSON.stringify({ files: ['e2e/latest.html', '../escape'] })],
    ['https://example.github.io/domusmind/e2e/latest.html', 'nightly report'],
  ]);
  globalThis.fetch = async (url) => {
    const value = files.get(String(url));
    return value === undefined ? new Response('not found', { status: 404 }) : new Response(value);
  };
  try {
    await assert.rejects(restoreSite('https://example.github.io/domusmind/', directory), /Unsafe site path/);
    files.set('https://example.github.io/domusmind/site-manifest.json', JSON.stringify({ files: ['e2e/latest.html'] }));
    assert.equal(await restoreSite('https://example.github.io/domusmind/', directory), true);
    assert.equal(await readFile(join(directory, 'e2e/latest.html'), 'utf8'), 'nightly report');
  } finally {
    globalThis.fetch = originalFetch;
    await rm(directory, { recursive: true, force: true });
  }
});

test('builds a branded landing page under a repository Pages base path and records all files', async () => {
  const directory = await temporaryDirectory();
  try {
    await buildLanding(directory, 'https://example.github.io/domusmind/');
    await mkdir(join(directory, 'architecture'), { recursive: true });
    await writeFile(join(directory, 'architecture', 'index.html'), 'snapshot');
    const landing = await readFile(join(directory, 'index.html'), 'utf8');
    assert.match(landing, /<base href="https:\/\/example\.github\.io\/domusmind\/"/);
    assert.match(landing, /How DomusMind is built/);
    const manifest = await finalizeSite(directory);
    assert.deepEqual(manifest.files, ['architecture/index.html', 'favicon.svg', 'index.html', 'site.css']);
    assert.equal((await readFile(join(directory, 'site-manifest.json'), 'utf8')).includes('architecture/index.html'), true);
  } finally {
    await rm(directory, { recursive: true, force: true });
  }
});
