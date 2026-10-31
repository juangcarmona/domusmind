import { createHash } from 'node:crypto';
import { mkdtemp, readFile, readdir, rm, writeFile } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { dirname, join, relative, resolve } from 'node:path';
import { createRequire } from 'node:module';
import { pathToFileURL } from 'node:url';

const args = process.argv.slice(2);
const check = args.includes('--check');
const docsIndex = args.indexOf('--docs');
if (args.some((arg, index) => arg !== '--check' && arg !== '--docs' && (docsIndex === -1 || index !== docsIndex + 1)) || (docsIndex !== -1 && !args[docsIndex + 1])) throw new Error('Usage: node scripts/architecture-diagrams.mjs [--check] [--docs <directory>]');
const root = resolve(docsIndex === -1 ? 'docs/architecture' : args[docsIndex + 1]);
const marker = /!\[([^\]]+)\]\((\.\/diagrams\/[^)]+\.png)\)\n<!-- diagram-source: (\.\/diagrams\/[^ ]+\.mmd) sha256:([a-f0-9]{64})(?: png-sha256:([a-f0-9]{64}))? -->/g;
const hash = (value) => createHash('sha256').update(value).digest('hex');
const require = createRequire(import.meta.url);
const arcshapeRoot = dirname(require.resolve('@arcshape/cli/package.json'));
const { MERMAID_CONFIG, stripDiagramAccessibilityTitle } = await import(pathToFileURL(join(arcshapeRoot, 'src/arc42-site/render.mjs')));
const { getPuppeteerLaunchOptions } = await import(pathToFileURL(join(arcshapeRoot, 'src/arc42-site/browser-options.mjs')));
const arcshapeRequire = createRequire(join(arcshapeRoot, 'package.json'));
const { run } = await import(pathToFileURL(arcshapeRequire.resolve('@mermaid-js/mermaid-cli')));
const temporary = await mkdtemp(join(tmpdir(), 'domusmind-diagrams-'));
let count = 0;
try {
  for (const section of (await readdir(root)).filter((name) => /^\d\d-/.test(name)).sort()) {
    const document = join(root, section, 'README.md');
    const text = await readFile(document, 'utf8');
    if (/^```mermaid\s*$/m.test(text)) throw new Error(`${document}: Mermaid must be stored in a .mmd source and displayed as PNG`);
    const matches = [...text.matchAll(marker)];
    const referenced = new Set(matches.map((match) => match[3].split('/').at(-1)));
    const images = new Set(matches.map((match) => match[2].split('/').at(-1)));
    if (referenced.size !== matches.length) throw new Error(`${document}: duplicate diagram source`);
    let sources = [];
    let previews = [];
    try {
      const files = await readdir(join(root, section, 'diagrams'));
      sources = files.filter((name) => name.endsWith('.mmd'));
      previews = files.filter((name) => name.endsWith('.png'));
    }
    catch (error) { if (error.code !== 'ENOENT') throw error; }
    if (sources.some((name) => !referenced.has(name)) || sources.length !== referenced.size) throw new Error(`${document}: unreferenced or missing diagram sources`);
    if (check && (previews.some((name) => !images.has(name)) || previews.length !== images.size)) throw new Error(`${document}: unreferenced or missing PNG previews`);
    let updated = text;
    for (const match of matches) {
      const [, title, image, source, expectedSource, expectedImage] = match;
      if (image.slice(0, -4) !== source.slice(0, -4)) throw new Error(`${document}: diagram image and source names differ`);
      const sourcePath = resolve(dirname(document), source);
      const imagePath = resolve(dirname(document), image);
      if (relative(root, sourcePath).startsWith('..') || relative(root, imagePath).startsWith('..')) throw new Error('Diagram path escapes architecture root');
      const content = await readFile(sourcePath, 'utf8');
      const sourceHash = hash(content);
      if (!/^\s*accTitle:\s*\S/m.test(content)) throw new Error(`${sourcePath}: missing accessible title`);
      if (check) {
        const png = await readFile(imagePath);
        if (sourceHash !== expectedSource || hash(png) !== expectedImage || !png.subarray(0, 8).equals(Buffer.from('89504e470d0a1a0a', 'hex'))) throw new Error(`${imagePath}: diagram is missing, modified or stale`);
      } else {
        const input = join(temporary, `${count}.mmd`);
        await writeFile(input, stripDiagramAccessibilityTitle(content));
        await run(input, imagePath, { quiet: true, outputFormat: 'png', puppeteerConfig: getPuppeteerLaunchOptions(), parseMMDOptions: { backgroundColor: 'white', mermaidConfig: MERMAID_CONFIG, viewport: { width: 1600, height: 1200, deviceScaleFactor: 2 } } });
        const imageHash = hash(await readFile(imagePath));
        const replacement = `![${title}](${image})\n<!-- diagram-source: ${source} sha256:${sourceHash} png-sha256:${imageHash} -->`;
        updated = updated.replace(match[0], replacement);
      }
      count++;
    }
    if (!check && updated !== text) await writeFile(document, updated);
  }
  if (!count) throw new Error('No architecture diagrams found');
  console.log(`${count} architecture PNG diagrams ${check ? 'verified' : 'rendered'}.`);
} finally {
  await rm(temporary, { recursive: true, force: true });
}
