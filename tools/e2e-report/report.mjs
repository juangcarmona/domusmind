import { cp, mkdir, readFile, readdir, rm, writeFile } from 'node:fs/promises';
import { join, resolve } from 'node:path';

function escapeHtml(value) {
  return String(value).replaceAll('&', '&amp;').replaceAll('<', '&lt;').replaceAll('>', '&gt;').replaceAll('"', '&quot;').replaceAll("'", '&#39;');
}

export function collectTests(suites = []) {
  const rows = [];
  function visit(items, parents = []) {
    for (const suite of items) {
      const titles = suite.title ? [...parents, suite.title] : parents;
      visit(suite.suites ?? [], titles);
      const specs = suite.specs ?? (suite.tests ? [{ title: '', tests: suite.tests }] : []);
      for (const spec of specs) {
        for (const test of spec.tests ?? []) {
          const result = (test.results ?? []).at(-1);
          const status = test.status ?? result?.status ?? 'missing';
          rows.push({
            title: [...titles, spec.title, test.title].filter(Boolean).join(' › '),
            status: status === 'expected' ? 'passed' : status === 'unexpected' ? 'failed' : status,
            duration: result?.duration ?? 0,
            attachments: (result?.attachments ?? []).map(({ name, contentType, path }) => ({ name, contentType, path })),
          });
        }
      }
    }
  }
  visit(suites);
  return rows;
}

export async function prepareReport({ resultsPath, htmlReportDirectory, evidenceDirectory, outputDirectory, metadata }) {
  const output = resolve(outputDirectory);
  await rm(output, { recursive: true, force: true });
  await mkdir(output, { recursive: true });

  let tests = [];
  let sourceReportError = null;
  try {
    const results = JSON.parse(await readFile(resultsPath, 'utf8'));
    tests = collectTests(results.suites);
  } catch (error) {
    sourceReportError = error.message;
  }

  const outcome = tests.length === 0 ? 'missing-evidence' : tests.every((test) => test.status === 'expected' || test.status === 'passed') ? 'passed' : 'failed';
  await writeFile(join(output, 'latest.json'), `${JSON.stringify({ ...metadata, outcome, journeyCount: tests.length }, null, 2)}\n`);
  await writeFile(join(output, 'results.json'), `${JSON.stringify({ metadata, outcome, sourceReportError, tests }, null, 2)}\n`);

  if (tests.length > 0) {
    try { await cp(htmlReportDirectory, join(output, 'playwright'), { recursive: true }); }
    catch { sourceReportError = 'Playwright HTML report was not produced.'; }
  }
  try { await cp(evidenceDirectory, join(output, 'evidence'), { recursive: true }); } catch { /* Missing screenshots are reflected by the empty evidence folder. */ }

  const rows = tests.map((test) => `<tr><td>${escapeHtml(test.title)}</td><td>${escapeHtml(test.status)}</td><td>${Math.round(test.duration)} ms</td></tr>`).join('\n');
  const reportLink = tests.length > 0 && sourceReportError === null ? '<p><a href="playwright/index.html">Open the detailed Playwright report</a></p>' : '';
  const warning = outcome === 'missing-evidence' ? '<p role="alert">No journeys were recorded. This report does not represent a successful run.</p>' : '';
  const html = `<!doctype html><html lang="en"><meta charset="utf-8"><meta name="viewport" content="width=device-width"><title>DomusMind E2E report</title><style>body{font:16px system-ui,sans-serif;max-width:960px;margin:3rem auto;padding:0 1rem;color:#332d43}h1{color:#493a6b}table{border-collapse:collapse;width:100%}th,td{text-align:left;padding:.65rem;border-bottom:1px solid #ddd}code{overflow-wrap:anywhere}.failed,[role=alert]{color:#a1223b}</style><h1>DomusMind browser report</h1><p class="${outcome}"><strong>${escapeHtml(outcome.toUpperCase())}</strong> · ${tests.length} journey record(s)</p>${warning}<dl><dt>Commit</dt><dd><code>${escapeHtml(metadata.commitSha)}</code></dd><dt>Workflow run</dt><dd><a href="${escapeHtml(metadata.runUrl)}">${escapeHtml(metadata.runId)}</a></dd><dt>Created (UTC)</dt><dd>${escapeHtml(metadata.createdAt)}</dd></dl>${reportLink}<table><thead><tr><th>Journey</th><th>Outcome</th><th>Duration</th></tr></thead><tbody>${rows}</tbody></table></html>`;
  await writeFile(join(output, 'index.html'), `${html}\n`);
  return { outcome, journeyCount: tests.length, publishable: true };
}

export async function addReportToSite(siteDirectory, reportDirectory, { runKey, retention = 14 }) {
  const root = resolve(siteDirectory);
  const runs = join(root, 'e2e', 'runs');
  const current = join(runs, runKey);
  await mkdir(runs, { recursive: true });
  await cp(reportDirectory, current, { recursive: true, force: true });

  const runEntries = await readdir(runs, { withFileTypes: true });
  const reports = [];
  for (const entry of runEntries) {
    if (!entry.isDirectory()) continue;
    try {
      const metadata = JSON.parse(await readFile(join(runs, entry.name, 'latest.json'), 'utf8'));
      reports.push({ key: entry.name, ...metadata });
    } catch { /* Unrecognized old directories are left untouched and excluded from the index. */ }
  }
  reports.sort((a, b) => b.createdAt.localeCompare(a.createdAt));
  const retained = reports.slice(0, retention);
  const keep = new Set(retained.map((report) => report.key));
  for (const report of reports) if (!keep.has(report.key)) await rm(join(runs, report.key), { recursive: true, force: true });

  const e2eDirectory = join(root, 'e2e');
  await mkdir(e2eDirectory, { recursive: true });
  const rows = retained.map((report) => `<li><a href="runs/${encodeURIComponent(report.key)}/">${escapeHtml(report.createdAt)} — ${escapeHtml(report.outcome)}</a> · <code>${escapeHtml(report.commitSha)}</code></li>`).join('\n');
  await writeFile(join(e2eDirectory, 'index.html'), `<!doctype html><html lang="en"><meta charset="utf-8"><meta name="viewport" content="width=device-width"><title>DomusMind E2E reports</title><style>body{font:16px system-ui,sans-serif;max-width:960px;margin:3rem auto;padding:0 1rem;color:#332d43}h1{color:#493a6b}li{padding:.6rem 0}code{overflow-wrap:anywhere}</style><h1>Browser journey reports</h1><p>Latest report: <a href="latest/">open latest</a>. Retention: ${retained.length} of ${retention} runs.</p><ol>${rows}</ol></html>\n`);
  await writeFile(join(e2eDirectory, 'latest.json'), `${JSON.stringify(retained[0] ?? {}, null, 2)}\n`);
  await rm(join(e2eDirectory, 'latest'), { recursive: true, force: true });
  await mkdir(join(e2eDirectory, 'latest'), { recursive: true });
  await writeFile(join(e2eDirectory, 'latest', 'index.html'), `<!doctype html><meta http-equiv="refresh" content="0;url=../runs/${encodeURIComponent(retained[0]?.key ?? '')}/"><a href="../runs/${encodeURIComponent(retained[0]?.key ?? '')}/">Open latest report</a>\n`);
  return { reports: retained.length, latest: retained[0]?.key };
}

if (process.argv[1] && resolve(process.argv[1]) === new URL(import.meta.url).pathname) {
  const [command, ...args] = process.argv.slice(2);
  if (command === 'prepare' && args.length === 5) {
    const [resultsPath, htmlDirectory, evidenceDirectory, outputDirectory, metadataPath] = args;
    const metadata = JSON.parse(await readFile(metadataPath, 'utf8'));
    const result = await prepareReport({ resultsPath, htmlReportDirectory: htmlDirectory, evidenceDirectory, outputDirectory, metadata });
    process.stdout.write(`${JSON.stringify(result)}\n`);
    if (result.outcome === 'missing-evidence') process.exitCode = 1;
  } else if (command === 'publish' && args.length >= 3) {
    const [siteDirectory, reportDirectory, runKey, retention] = args;
    const result = await addReportToSite(siteDirectory, reportDirectory, { runKey, retention: Number(retention ?? 14) });
    process.stdout.write(`${JSON.stringify(result)}\n`);
  } else {
    process.stderr.write('Usage: report.mjs prepare <results.json> <html-dir> <evidence-dir> <output-dir> <metadata.json> | publish <site-dir> <report-dir> <run-key> [retention]\n');
    process.exitCode = 2;
  }
}
