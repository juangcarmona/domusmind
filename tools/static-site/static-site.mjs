#!/usr/bin/env node
import { createWriteStream } from 'node:fs';
import { cp, mkdir, readFile, readdir, writeFile } from 'node:fs/promises';
import { dirname, join, resolve, sep } from 'node:path';
import { Readable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { fileURLToPath } from 'node:url';

const SAFE_PATH = /^[A-Za-z0-9._/-]+$/;

export function safeRelativePath(path) {
  if (typeof path !== 'string' || !SAFE_PATH.test(path) || path.startsWith('/') || path.split('/').includes('..')) {
    throw new Error(`Unsafe site path: ${path}`);
  }
  return path;
}

function normalizeBaseUrl(value) {
  const url = new URL(value);
  if (!['https:', 'http:'].includes(url.protocol) || url.search || url.hash) {
    throw new Error(`Invalid GitHub Pages base URL: ${value}`);
  }
  return url.toString().replace(/\/$/, '');
}

export async function restoreSite(baseUrl, siteDirectory) {
  const base = normalizeBaseUrl(baseUrl);
  const root = resolve(siteDirectory);
  await mkdir(root, { recursive: true });
  const manifestUrl = new URL('site-manifest.json', `${base}/`);
  const response = await fetch(manifestUrl);
  if (response.status === 404) return false;
  if (!response.ok) throw new Error(`Could not read ${manifestUrl}: HTTP ${response.status}`);
  const manifest = await response.json();
  if (!Array.isArray(manifest.files)) throw new Error('Invalid site manifest: files must be an array.');

  for (const rawPath of new Set(manifest.files)) {
    const path = safeRelativePath(rawPath);
    const destination = resolve(root, path);
    if (!destination.startsWith(`${root}${sep}`)) throw new Error(`Site path escapes output: ${path}`);
    const fileResponse = await fetch(new URL(path, `${base}/`));
    if (!fileResponse.ok) throw new Error(`Could not restore ${path}: HTTP ${fileResponse.status}`);
    await mkdir(dirname(destination), { recursive: true });
    await pipeline(Readable.fromWeb(fileResponse.body), createWriteStream(destination));
  }
  return true;
}

export async function buildLanding(siteDirectory, baseUrl, sourceDirectory = new URL('../engineering-site/', import.meta.url)) {
  const base = normalizeBaseUrl(baseUrl);
  const root = resolve(siteDirectory);
  await mkdir(root, { recursive: true });
  await cp(sourceDirectory, root, { recursive: true, force: true });
  const landing = join(root, 'index.html');
  const html = await readFile(landing, 'utf8');
  await writeFile(landing, html.replaceAll('__BASE_URL__', base), 'utf8');
}

async function listFiles(directory, root = directory) {
  const entries = await readdir(directory, { withFileTypes: true });
  const files = [];
  for (const entry of entries) {
    const path = join(directory, entry.name);
    if (entry.isDirectory()) files.push(...await listFiles(path, root));
    else if (entry.isFile()) {
      const relativePath = path.slice(root.length + 1).split(sep).join('/');
      if (relativePath !== 'site-manifest.json') files.push(safeRelativePath(relativePath));
    }
  }
  return files;
}

export async function finalizeSite(siteDirectory) {
  const root = resolve(siteDirectory);
  const files = (await listFiles(root)).sort();
  const manifest = { version: 1, files };
  await writeFile(join(root, 'site-manifest.json'), `${JSON.stringify(manifest, null, 2)}\n`, 'utf8');
  return manifest;
}

async function main(argv) {
  const [command, ...args] = argv;
  if (command === 'restore' && args.length === 2) {
    const restored = await restoreSite(args[0], args[1]);
    process.stdout.write(restored ? 'Restored existing Pages site.\n' : 'No published site exists yet.\n');
  } else if (command === 'build' && args.length === 2) {
    await buildLanding(args[0], args[1]);
  } else if (command === 'finalize' && args.length === 1) {
    const manifest = await finalizeSite(args[0]);
    process.stdout.write(`Manifest includes ${manifest.files.length} files.\n`);
  } else {
    process.stderr.write('Usage: static-site.mjs restore <base-url> <site-dir> | build <site-dir> <base-url> | finalize <site-dir>\n');
    process.exitCode = 2;
  }
}

if (process.argv[1] && resolve(process.argv[1]) === fileURLToPath(import.meta.url)) {
  main(process.argv.slice(2)).catch((error) => {
    process.stderr.write(`${error.stack ?? error.message}\n`);
    process.exitCode = 1;
  });
}
