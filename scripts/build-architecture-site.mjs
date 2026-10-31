import { cp, mkdtemp, mkdir, readFile, readdir, rm, writeFile } from 'node:fs/promises';
import { dirname, join, relative, resolve, sep } from 'node:path';
import { spawnSync } from 'node:child_process';
import { createRequire } from 'node:module';

const root = process.cwd();
const output = resolve(process.argv[2] ?? 'docs/architecture-site/index.html');
const sourceRef = process.argv[3] ?? 'main';
const architecture = join(root, 'docs/architecture');
const decisions = join(root, 'docs/adr');
const temporary = await mkdtemp(join(root, '.arcshape-build-'));
const staged = join(temporary, 'docs/architecture');
const stagedDecisions = join(staged, '09-architecture-decisions/adr');
const mappings = new Map();

async function collect(directory) {
  const files = [];
  for (const entry of await readdir(directory, { withFileTypes: true })) {
    const path = join(directory, entry.name);
    if (entry.isDirectory()) files.push(...await collect(path));
    else if (entry.isFile() && entry.name.endsWith('.md')) files.push(path);
  }
  return files;
}

try {
  await cp(architecture, staged, { recursive: true });
  await mkdir(stagedDecisions, { recursive: true });
  const architectureFiles = await collect(architecture);
  const decisionFiles = await collect(decisions);
  for (const file of architectureFiles) mappings.set(file, join(staged, relative(architecture, file)));
  for (const file of decisionFiles) mappings.set(file, join(stagedDecisions, relative(decisions, file)));
  for (const [original, destination] of mappings) {
    let text = await readFile(original, 'utf8');
    const diagrams = [...text.matchAll(/!\[[^\]]+\]\(\.\/diagrams\/[^)]+\.png\)\n<!-- diagram-source: (\.\/diagrams\/[^ ]+\.mmd) sha256:[a-f0-9]{64} png-sha256:[a-f0-9]{64} -->/g)];
    for (const diagram of diagrams) {
      const source = await readFile(resolve(dirname(original), diagram[1]), 'utf8');
      text = text.replace(diagram[0], `\`\`\`mermaid\n${source}\`\`\``);
    }
    const updated = text.replace(/(?<=\]\()([^\s)]+)(?=\))/g, (link) => {
      if (/^(?:#|\/|https?:|mailto:|<)/i.test(link)) return link;
      const [path, ...fragment] = link.split('#');
      const target = resolve(dirname(original), path);
      const result = relative(dirname(destination), mappings.get(target) ?? target).split(sep).join('/');
      return result + (path.endsWith('/') && !result.endsWith('/') ? '/' : '') + (fragment.length ? `#${fragment.join('#')}` : '');
    });
    await mkdir(dirname(destination), { recursive: true });
    await writeFile(destination, updated);
  }
  const require = createRequire(import.meta.url);
  const cli = require.resolve('@arcshape/cli/package.json').replace(/package\.json$/, 'src/bin/arcshape.mjs');
  const run = spawnSync(process.execPath, [cli, 'site', 'build', '--docs', staged, '--format', 'snapshot', '--diagram-engine', 'mermaid', '--out', output, '--title', 'DomusMind Architecture', '--source-ref', sourceRef], { stdio: 'inherit' });
  if (run.status !== 0) throw new Error(`ArcShape site build failed (${run.status ?? run.error?.message})`);
  const prefix = relative(root, staged).split(sep).join('/');
  const html = (await readFile(output, 'utf8'))
    .replaceAll(`${prefix}/09-architecture-decisions/adr/`, 'docs/adr/')
    .replaceAll(`${prefix}/`, 'docs/architecture/');
  await writeFile(output, html);
} finally {
  await rm(temporary, { recursive: true, force: true });
}
