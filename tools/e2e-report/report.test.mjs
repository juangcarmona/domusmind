import assert from 'node:assert/strict';
import { mkdtemp, mkdir, readFile, rm, writeFile } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';
import { addReportToSite, collectTests, prepareReport } from './report.mjs';

test('collects journeys from nested Playwright suites and preserves result attachments', () => {
  const rows = collectTests([{ title: 'Household', suites: [{ title: 'onboarding', specs: [{ title: 'reaches Agenda', tests: [{ status: 'expected', results: [{ status: 'passed', duration: 42, attachments: [{ name: 'trace', contentType: 'application/zip', path: 'trace.zip' }] }] }] }] }] }]);
  assert.deepEqual(rows, [{ title: 'Household › onboarding › reaches Agenda', status: 'passed', duration: 42, attachments: [{ name: 'trace', contentType: 'application/zip', path: 'trace.zip' }] }]);
});

test('an empty test report produces visible missing evidence, never success', async () => {
  const directory = await mkdtemp(join(tmpdir(), 'domusmind-e2e-report-'));
  try {
    const results = join(directory, 'results.json');
    await writeFile(results, JSON.stringify({ suites: [] }));
    const output = join(directory, 'out');
    const result = await prepareReport({ resultsPath: results, htmlReportDirectory: join(directory, 'absent'), evidenceDirectory: join(directory, 'evidence'), outputDirectory: output, metadata: { commitSha: 'abc123', runId: '42', runUrl: 'https://example.test/42', createdAt: '2026-01-01T00:00:00.000Z' } });
    assert.equal(result.outcome, 'missing-evidence');
    assert.match(await readFile(join(output, 'index.html'), 'utf8'), /No journeys were recorded/);
  } finally { await rm(directory, { recursive: true, force: true }); }
});

test('a failed browser journey is retained as failed evidence', async () => {
  const directory = await mkdtemp(join(tmpdir(), 'domusmind-e2e-failed-report-'));
  try {
    const results = join(directory, 'results.json');
    await writeFile(results, JSON.stringify({ suites: [{ title: 'household.e2e.ts', specs: [{ title: 'reaches Agenda', tests: [{ status: 'unexpected', results: [{ status: 'failed', duration: 23, attachments: [] }] }] }] }] }));
    const output = join(directory, 'out');
    const result = await prepareReport({ resultsPath: results, htmlReportDirectory: join(directory, 'absent'), evidenceDirectory: join(directory, 'evidence'), outputDirectory: output, metadata: { commitSha: 'abc123', runId: '42', runUrl: 'https://example.test/42', createdAt: '2026-01-01T00:00:00.000Z' } });
    assert.equal(result.outcome, 'failed');
    const report = await readFile(join(output, 'index.html'), 'utf8');
    assert.match(report, /reaches Agenda/);
    assert.match(report, /failed/i);
  } finally { await rm(directory, { recursive: true, force: true }); }
});

test('creates a stable latest report and keeps only the newest configured number', async () => {
  const directory = await mkdtemp(join(tmpdir(), 'domusmind-e2e-site-'));
  try {
    const site = join(directory, 'site');
    const report = join(directory, 'report');
    await mkdir(report, { recursive: true });
    for (let index = 1; index <= 3; index += 1) {
      await writeFile(join(report, 'latest.json'), JSON.stringify({ createdAt: `2026-01-0${index}T00:00:00.000Z`, outcome: 'passed', commitSha: `sha-${index}`, runId: `${index}` }));
      await writeFile(join(report, 'index.html'), `<p>report ${index}</p>`);
      await addReportToSite(site, report, { runKey: `run-${index}`, retention: 2 });
    }
    const indexPage = await readFile(join(site, 'e2e', 'index.html'), 'utf8');
    assert.match(indexPage, /run-3/);
    assert.match(indexPage, /run-2/);
    assert.doesNotMatch(indexPage, /run-1/);
    assert.match(await readFile(join(site, 'e2e', 'latest', 'index.html'), 'utf8'), /run-3/);
    await assert.rejects(readFile(join(site, 'e2e', 'runs', 'run-1', 'index.html')));
  } finally { await rm(directory, { recursive: true, force: true }); }
});
