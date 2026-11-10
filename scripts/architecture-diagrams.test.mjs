import assert from 'node:assert/strict';
import { createHash } from 'node:crypto';
import { mkdtemp, mkdir, rm, writeFile } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join, resolve } from 'node:path';
import { spawnSync } from 'node:child_process';

const root = await mkdtemp(join(tmpdir(), 'domusmind-diagram-test-'));
const section = join(root, '01-example');
const files = join(section, 'diagrams');
const script = resolve('scripts/architecture-diagrams.mjs');
const source = 'flowchart LR\n  accTitle: Example\n  a --> b\n';
const png = Buffer.from('89504e470d0a1a0a', 'hex');
const hash = (content) => createHash('sha256').update(content).digest('hex');
const check = () => spawnSync(process.execPath, [script, '--check', '--docs', root], { encoding: 'utf8' });
try {
  await mkdir(files, { recursive: true });
  await writeFile(join(section, 'README.md'), `![Example](./diagrams/example.png)\n<!-- diagram-source: ./diagrams/example.mmd sha256:${hash(source)} png-sha256:${hash(png)} -->\n`);
  await writeFile(join(files, 'example.mmd'), source);
  await writeFile(join(files, 'example.png'), png);
  assert.equal(check().status, 0, 'matching source and image must pass');
  await writeFile(join(files, 'example.mmd'), `${source}  b --> c\n`);
  assert.notEqual(check().status, 0, 'changed source must fail');
  await writeFile(join(files, 'example.mmd'), source);
  await writeFile(join(files, 'example.png'), Buffer.concat([png, Buffer.from('changed')]));
  assert.notEqual(check().status, 0, 'changed image must fail');
  await writeFile(join(files, 'example.png'), png);
  await writeFile(join(files, 'unreferenced.mmd'), source);
  assert.notEqual(check().status, 0, 'unreferenced source must fail');
  await rm(join(files, 'unreferenced.mmd'));
  await writeFile(join(files, 'unreferenced.png'), png);
  assert.notEqual(check().status, 0, 'unreferenced image must fail');
  console.log('Diagram freshness checks passed.');
} finally {
  await rm(root, { recursive: true, force: true });
}
