/**
 * shell.mjs — build the single self-contained HTML page.
 *
 * Structure mirrors productshape's buildSnapshotHtml:
 *  - one inline <style> with light + dark design tokens;
 *  - one inert JSON data block;
 *  - one ES5 application script with a single hash router;
 *  - no external resources, no forms, nothing persisted outside the address.
 */
import { escapeHtml as esc } from './render.mjs';
import { createHash } from 'node:crypto';

/* ------------------------------------------------------------------ */
/* Design tokens: a calm engineering instrument in two appearances.   */
/* ------------------------------------------------------------------ */
const lightBlock = `  --bg: #f7f5f9;
  --panel: #f1edf5;
  --raise: #ffffff;
  --ink: #211a2b;
  --text: #352c40;
  --muted: #645a73;
  --line: #ddd4e7;
  --line-strong: #bbaec9;
  --accent: #6a4c93;
  --accent-soft: #eee7f5;
  --mark: #d9cbe9;
  --code-bg: #f1edf5;`;

const darkBlock = `  --bg: #141119;
  --panel: #221c2a;
  --raise: #1b1622;
  --ink: #f5f1fa;
  --text: #dfd6e9;
  --muted: #baafc9;
  --line: #342b41;
  --line-strong: #59496c;
  --accent: #b79ad9;
  --accent-soft: #372d45;
  --mark: #6a4c93;
  --code-bg: #221c2a;`;

const style = `
:root {
${lightBlock}
  --sans: Aptos, 'Segoe UI', 'Helvetica Neue', Arial, sans-serif;
  --mono: ui-monospace, SFMono-Regular, 'SF Mono', 'Cascadia Mono', Consolas, 'Liberation Mono', Menlo, monospace;
  color-scheme: light;
}
@media (prefers-color-scheme: dark) {
  :root:not([data-appearance='light']) {
${darkBlock}
    color-scheme: dark;
  }
}
:root[data-appearance='dark'] {
${darkBlock}
  color-scheme: dark;
}
* { box-sizing: border-box; }
[hidden] { display: none !important; }
html, body { height: 100%; }
body {
  margin: 0; display: flex; flex-direction: column; overflow: hidden;
  font: 15px/1.6 var(--sans); color: var(--text); background: var(--bg);
  -webkit-text-size-adjust: 100%;
}
h1, h2, h3, h4, h5, h6 { color: var(--ink); line-height: 1.3; }
a { color: var(--accent); text-decoration: none; }
a:hover { text-decoration: underline; }
button { font: inherit; color: inherit; }
code, kbd, .rev { font-family: var(--mono); font-size: 0.88em; }
code { background: var(--code-bg); border: 1px solid var(--line); border-radius: 3px; padding: 0.05em 0.35em; }
pre { background: var(--panel); border: 1px solid var(--line); border-radius: 4px; padding: 0.6rem 0.8rem; overflow-x: auto; font-size: 0.85rem; line-height: 1.5; }
pre code { background: none; border: none; padding: 0; font-size: inherit; }
:focus-visible { outline: 2px solid var(--accent); outline-offset: 1px; }

.skip {
  position: absolute; left: -9999px; top: 0; z-index: 20;
  background: var(--raise); color: var(--ink); padding: 0.4rem 0.8rem;
  border: 1px solid var(--line-strong);
}
.skip:focus { left: 0.5rem; top: 0.5rem; }

/* ---- Header ---- */
header.site {
  display: flex; align-items: center; gap: 0.9rem; flex: none;
  padding: 0.45rem 1rem; border-bottom: 1px solid var(--line);
  background: var(--raise);
}
header.site h1 { margin: 0; min-width: 0; font-size: 0.98rem; overflow-wrap: anywhere; }
.revshort {
  font-family: var(--mono); font-size: 0.74rem; color: var(--muted);
  border: 1px solid var(--line); border-radius: 3px; padding: 0.05rem 0.4rem; white-space: nowrap;
}
.nav-toggle { display: none; }
.desktop-nav-toggle { display: inline-flex; width: 2rem; }
header.site .spacer { flex: 1; }
.iconbtn {
  display: inline-flex; align-items: center; justify-content: center;
  flex: none;
  width: 1.7rem; height: 1.7rem; border: 1px solid var(--line-strong); border-radius: 4px;
  background: var(--raise); cursor: pointer; color: var(--muted);
}
.iconbtn:hover { color: var(--ink); border-color: var(--accent); }
.appearance { display: inline-flex; flex: none; border: 1px solid var(--line-strong); border-radius: 4px; overflow: hidden; }
.appearance button {
  border: none; background: var(--raise); color: var(--muted); cursor: pointer;
  padding: 0.15rem 0.5rem; font-size: 0.78rem;
}
.appearance button + button { border-left: 1px solid var(--line); }
.appearance button[aria-pressed='true'] { background: var(--accent-soft); color: var(--ink); }

/* ---- Layout ---- */
#shell { flex: 1 1 auto; min-height: 0; display: flex; }
aside.nav {
  flex: none; width: 17.5rem; overflow-y: auto; border-right: 1px solid var(--line);
  background: var(--panel); padding: 0.9rem 0.75rem;
}
aside.nav .nav-heading { display: flex; align-items: center; justify-content: space-between; }
aside.nav .nav-close { display: none; }
#nav-backdrop { display: none; }
#shell.nav-collapsed aside.nav { width: 5.2rem; padding-inline: 0.55rem; }
#shell.nav-collapsed aside.nav h2, #shell.nav-collapsed .nav-foot { display: none; }
#shell.nav-collapsed aside.nav a { display: grid; justify-items: center; gap: 0.1rem; padding: 0.42rem 0.1rem; }
#shell.nav-collapsed aside.nav .t { display: block; font-size: 0.62rem; text-align: center; }
#shell.nav-collapsed aside.nav .num { font-size: 0.68rem; }
#shell.nav-collapsed .nav-icon { width: 1.05rem; height: 1.05rem; }
.nav-icon { width: 1rem; height: 1rem; flex: none; vertical-align: middle; }
main#main { flex: 1 1 auto; min-width: 0; overflow-y: auto; padding: 1.4rem 2.2rem 4rem; scroll-behavior: smooth; }
.doc { max-width: 78ch; margin: 0 auto; }
.doc > h2.doc-title { margin: 0 0 0.2rem; font-size: 1.45rem; }
.doc > .section-kicker { margin: 0 0 0.1rem; font: 0.76rem var(--mono); color: var(--muted); }
p.doc-desc { margin: 0 0 1.2rem; color: var(--muted); font-size: 0.9rem; }

/* ---- Sidebar ---- */
aside.nav h2 { margin: 0 0 0.5rem; font-size: 0.72rem; text-transform: uppercase; letter-spacing: 0.08em; color: var(--muted); }
aside.nav ol { list-style: none; margin: 0; padding: 0; }
aside.nav li { margin: 0; border-bottom: 1px solid var(--line); }
aside.nav a {
  display: flex; gap: 0.55rem; align-items: baseline; padding: 0.32rem 0.4rem;
  color: var(--text); border-left: 2px solid transparent;
}
aside.nav a:hover { color: var(--ink); text-decoration: none; background: var(--raise); }
aside.nav a[aria-current='page'] { color: var(--ink); border-left-color: var(--accent); background: var(--raise); }
aside.nav .num { font-family: var(--mono); font-size: 0.74rem; color: var(--muted); flex: none; }
aside.nav .t { flex: 1 1 auto; min-width: 0; font-size: 0.86rem; }
aside.nav .t-short { display: none; }
#shell.nav-collapsed aside.nav .t-full { display: none; }
#shell.nav-collapsed aside.nav .t-short { display: inline; }
.nav-foot { margin-top: 1rem; padding-top: 0.6rem; border-top: 1px solid var(--line); font-size: 0.78rem; color: var(--muted); }
.nav-foot .prevnext { display: flex; justify-content: space-between; gap: 0.5rem; }
.nav-foot .prevnext a { font-size: 0.78rem; }

/* ---- Content ---- */
.doc h3 { margin: 1.6rem 0 0.5rem; font-size: 1.12rem; border-bottom: 1px solid var(--line); padding-bottom: 0.25rem; }
.doc h4 { margin: 1.35rem 0 0.35rem; font-size: 1rem; }
.doc h5 { margin: 1.1rem 0 0.3rem; font-size: 0.9rem; }
.doc h6 { margin: 1rem 0 0.3rem; font-size: 0.85rem; }
.doc p, .doc ul, .doc ol { margin: 0.55rem 0; }
.doc ul, .doc ol { padding-left: 1.4rem; }
.doc li { margin: 0.18rem 0; }
.doc table { border-collapse: collapse; margin: 0.8rem 0; font-size: 0.88rem; display: block; overflow-x: auto; max-width: 100%; }
.doc th, .doc td { border: 1px solid var(--line); padding: 0.32rem 0.6rem; text-align: left; vertical-align: top; }
.doc th { background: var(--panel); color: var(--ink); }
.doc blockquote { margin: 0.6rem 0; padding: 0.15rem 0.9rem; border-left: 3px solid var(--line-strong); color: var(--muted); }
.doc hr { border: none; border-top: 1px solid var(--line); margin: 1.4rem 0; }
.xref-dead code { word-break: break-all; }
figure.diagram {
  margin: 1rem 0; padding: 0.7rem; border: 1px solid var(--line); border-radius: 4px;
  /* Diagrams keep mermaid's light palette: dark text on a light paper ground,
     so they stay readable in both appearances. */
  background: #ffffff; color: #333; overflow-x: auto; text-align: center;
}
figure.diagram svg { max-width: 100%; height: auto; }
.diagram-missing { color: var(--muted); font-style: italic; }
mark { background: var(--mark); color: inherit; border-radius: 2px; padding: 0 0.05em; }
.overview-intro { max-width: 70ch; margin: 0 auto 1.3rem; }
.overview-intro h2 { margin: 0 0 0.25rem; font-size: 1.7rem; }
.overview-intro p { margin: 0.3rem 0; color: var(--muted); }
.section-grid { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 0.8rem; max-width: 78rem; margin: auto; }
.section-card { display: flex; flex-direction: column; min-height: 10.5rem; padding: 1rem; border: 1px solid var(--line); border-radius: 6px; color: var(--text); background: var(--raise); border-top: 3px solid var(--group-color, var(--accent)); }
.section-card:hover, .section-card:focus-visible { text-decoration: none; border-color: var(--group-color, var(--accent)); box-shadow: 0 3px 14px rgba(33,26,43,0.09); }
.card-top { display: flex; align-items: center; gap: 0.55rem; color: var(--muted); font: 0.76rem var(--mono); }
.card-group { margin-left: auto; font: 0.68rem var(--sans); text-transform: uppercase; letter-spacing: 0.06em; }
.section-card h3 { margin: 0.55rem 0 0.3rem; font-size: 1rem; color: var(--ink); }
.section-card p { margin: 0; color: var(--muted); font-size: 0.84rem; line-height: 1.45; }
.card-arrow { margin-top: auto; padding-top: 0.65rem; text-align: right; color: var(--group-color, var(--accent)); }

/* ---- Search dialog ---- */
.dlg { position: fixed; inset: 0; background: rgba(0,0,0,0.35); display: flex; align-items: flex-start; justify-content: center; padding-top: 12vh; z-index: 30; }
.dlgbox {
  width: min(38rem, 92vw); background: var(--raise); border: 1px solid var(--line-strong);
  border-radius: 6px; box-shadow: 0 12px 40px rgba(0,0,0,0.25); overflow: hidden;
}
.dlgbox input {
  width: 100%; border: none; border-bottom: 1px solid var(--line); padding: 0.7rem 0.9rem;
  font: inherit; color: var(--ink); background: var(--raise); outline: none;
}
.dlgbox ul { list-style: none; margin: 0; padding: 0; max-height: 50vh; overflow-y: auto; }
.dlgbox li a { display: block; padding: 0.45rem 0.9rem; color: var(--text); border-left: 2px solid transparent; }
.dlgbox li a:hover, .dlgbox li a.sel { background: var(--accent-soft); color: var(--ink); border-left-color: var(--accent); text-decoration: none; }
.dlgbox .where { display: block; font-size: 0.76rem; color: var(--muted); }
.dlgbox .empty { padding: 0.7rem 0.9rem; color: var(--muted); font-size: 0.88rem; }
kbd {
  border: 1px solid var(--line-strong); border-bottom-width: 2px; border-radius: 3px;
  padding: 0 0.3em; font-size: 0.8em; background: var(--raise); color: var(--muted);
}

/* ---- Keyboard help ---- */
.keysbox { width: min(28rem, 92vw); }
.keysbox dl { margin: 0; padding: 0.6rem 0.9rem 0.9rem; }
.keysbox dt { font-weight: 650; color: var(--ink); margin-top: 0.55rem; }
.keysbox dd { margin: 0.1rem 0 0; color: var(--muted); font-size: 0.88rem; }

/* ---- Toast ---- */
.toast {
  position: fixed; bottom: 1rem; left: 50%; transform: translateX(-50%);
  background: var(--raise); border: 1px solid var(--line-strong); border-radius: 4px;
  padding: 0.35rem 0.8rem; font-size: 0.85rem; color: var(--ink); z-index: 40;
  opacity: 0; transition: opacity 0.15s; pointer-events: none;
}
.toast.show { opacity: 1; }

/* ---- Narrow viewports ---- */
@media (max-width: 860px) {
  header.site .nav-toggle { display: inline-flex; width: auto; padding: 0.1rem 0.45rem; gap: 0.35rem; }
  header.site .desktop-nav-toggle { display: none; }
  #shell { display: block; position: relative; }
  aside.nav { position: fixed; z-index: 25; inset: 0 auto 0 0; width: min(20rem, 86vw); max-height: none; border: none; border-right: 1px solid var(--line); box-shadow: 8px 0 30px rgba(0,0,0,0.18); transform: translateX(-105%); transition: transform 0.18s ease; }
  #shell.drawer-open aside.nav { transform: translateX(0); }
  aside.nav .nav-close { display: inline-flex; }
  #nav-backdrop { display: block; position: fixed; z-index: 24; inset: 0; border: 0; padding: 0; background: rgba(0,0,0,0.45); }
  #shell:not(.drawer-open) #nav-backdrop { display: none; }
  #shell.nav-collapsed aside.nav { width: min(20rem, 86vw); }
  #shell.nav-collapsed aside.nav h2, #shell.nav-collapsed .nav-foot { display: block; }
  #shell.nav-collapsed aside.nav a { display: flex; justify-items: initial; gap: 0.55rem; padding: 0.32rem 0.4rem; }
  #shell.nav-collapsed aside.nav .t { text-align: left; font-size: 0.86rem; }
  #shell.nav-collapsed aside.nav .t-full { display: inline; }
  #shell.nav-collapsed aside.nav .t-short { display: none; }
  #shell.nav-collapsed aside.nav .num { font-size: 0.74rem; }
  #shell.nav-collapsed .nav-icon { width: 1rem; height: 1rem; }
  main#main { padding: 1rem 1rem 3rem; }
}
@media (max-width: 1050px) { .section-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
@media (max-width: 520px) {
  header.site { flex-wrap: wrap; gap: 0.5rem; }
  header.site h1 { flex: 1 1 calc(100% - 6rem); }
  header.site .spacer { display: none; }
  header.site .appearance { margin-left: auto; }
  .section-grid { grid-template-columns: 1fr; }
}
@media print {
  body { overflow: visible; }
  aside.nav, header.site .appearance, header.site .iconbtn, header.site .nav-toggle, #nav-backdrop { display: none !important; }
  main#main { overflow: visible; }
}
@media (prefers-reduced-motion: reduce) {
  * { animation-duration: 0.001ms !important; transition-duration: 0.001ms !important; scroll-behavior: auto !important; }
}
`.trim();

/* ------------------------------------------------------------------ */
/* The application: one router, views rendered from inert data.        */
/* ------------------------------------------------------------------ */
const script = String.raw`
(function () {
  'use strict';
  var doc = document;
  var root = doc.documentElement;
  var dataEl = doc.getElementById('arc42-data');
  if (!dataEl) return;
  var DATA = JSON.parse(dataEl.textContent || '{}');
  var README = DATA.readme || {};
  var SECTIONS = DATA.sections || [];
  var byNumber = {};
  for (var i = 0; i < SECTIONS.length; i += 1) byNumber[SECTIONS[i].number] = SECTIONS[i];
  var groups = [
    { name: 'Context', color: '#447c73', first: 1, last: 3 },
    { name: 'Structure', color: '#5874a5', first: 4, last: 6 },
    { name: 'Operations', color: '#bd7744', first: 7, last: 8 },
    { name: 'Decisions', color: '#8b629f', first: 9, last: 9 },
    { name: 'Reference', color: '#72814c', first: 10, last: 12 }
  ];
  var iconPaths = ['<circle cx="12" cy="12" r="8"/><circle cx="12" cy="12" r="2"/><path d="M12 2v3m0 14v3M2 12h3m14 0h3"/>', '<path d="M4 5h16M4 12h16M4 19h16"/><circle cx="7" cy="5" r="1"/><circle cx="17" cy="12" r="1"/><circle cx="9" cy="19" r="1"/>', '<path d="M4 5h16v14H4zM8 9h8M8 13h5"/>', '<path d="M4 19V5m0 14h16M7 16l4-5 3 2 5-7"/>', '<rect x="4" y="4" width="6" height="6" rx="1"/><rect x="14" y="4" width="6" height="6" rx="1"/><rect x="4" y="14" width="6" height="6" rx="1"/><rect x="14" y="14" width="6" height="6" rx="1"/>', '<path d="M4 6h6v5H4zM14 13h6v5h-6zM10 8h3a2 2 0 0 1 2 2v3"/>', '<path d="M4 20V4m0 16h16M7 16l3-4 3 2 5-7"/><path d="M15 7h3v3"/>', '<circle cx="12" cy="12" r="9"/><path d="M12 7v5l4 2"/>', '<path d="M7 3h8l4 4v14H7zM15 3v5h4M10 12h6m-6 4h6"/>', '<path d="M4 5h16v14H4zM8 9h8M8 13h8M8 17h4"/>', '<path d="M12 3 3 20h18L12 3z"/><path d="M12 9v5m0 3h.01"/>', '<circle cx="10.5" cy="10.5" r="6.5"/><path d="m16 16 5 5M8 10h5m-2.5-2.5v5"/>'];
  function groupFor(number) {
    var n = parseInt(number, 10);
    for (var g = 0; g < groups.length; g += 1) if (n >= groups[g].first && n <= groups[g].last) return groups[g];
    return groups[groups.length - 1];
  }
  function icon(number) {
    var idx = Math.max(0, Math.min(11, parseInt(number, 10) - 1));
    return '<svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true" focusable="false">' + iconPaths[idx] + '</svg>';
  }

  var el = function (tag, cls, text) {
    var node = doc.createElement(tag);
    if (cls) node.className = cls;
    if (text !== undefined && text !== null) node.textContent = String(text);
    return node;
  };
  var clear = function (node) { while (node.firstChild) node.removeChild(node.firstChild); };

  /* ---------- Search index (built lazily, offline) ---------- */
  var index = null;
  function buildIndex() {
    index = [];
    var push = function (number, route, where, text) {
      var plain = text.replace(/<[^>]+>/g, ' ').replace(/&[a-z]+;/g, ' ');
      index.push({ number: number, route: route, where: where, text: plain });
    };
    push('', '#/introduction', 'Introduction', (README.title || '') + ' ' + (README.html || ''));
    for (var i = 0; i < SECTIONS.length; i += 1) {
      var s = SECTIONS[i];
      push(s.number, '#/section/' + s.number, s.title, s.title + ' ' + s.description + ' ' + (s.html || ''));
    }
  }
  function search(query) {
    if (!index) buildIndex();
    var terms = query.toLowerCase().split(/\s+/).filter(Boolean);
    if (terms.length === 0) return [];
    var results = [];
    for (var i = 0; i < index.length; i += 1) {
      var entry = index[i];
      var hay = entry.text.toLowerCase();
      var score = 0;
      var all = true;
      for (var t = 0; t < terms.length; t += 1) {
        var term = terms[t];
        var inTitle = entry.where.toLowerCase().indexOf(term) !== -1;
        var hits = hay.split(term).length - 1;
        if (hits === 0 && !inTitle) { all = false; break; }
        score += inTitle ? 12 : 0;
        score += Math.min(hits, 8);
      }
      if (all) results.push({ score: score, entry: entry });
    }
    results.sort(function (a, b) { return b.score - a.score || (a.entry.number < b.entry.number ? -1 : 1); });
    return results.slice(0, 24);
  }

  /* ---------- Views ---------- */
  function renderDoc(host, title, description, html) {
    clear(host);
    var article = el('article', 'doc');
    var h = el('h2', 'doc-title', title);
    article.appendChild(h);
    if (description) article.appendChild(el('p', 'doc-desc', description));
    var body = el('div', 'doc-body');
    body.innerHTML = html; /* generator-escaped markdown output only */
    article.appendChild(body);
    host.appendChild(article);
    doc.getElementById('main').scrollTop = 0;
  }

  function showOverview(host) {
    clear(host);
    var intro = el('section', 'overview-intro');
    intro.appendChild(el('h2', null, 'Architecture overview'));
    intro.appendChild(el('p', null, 'Browse the twelve arc42 sections. The introduction explains where architectural and product truth lives.'));
    var grid = el('div', 'section-grid');
    grid.setAttribute('aria-label', 'arc42 architecture sections');
    for (var i = 0; i < SECTIONS.length; i += 1) {
      var section = SECTIONS[i];
      var group = groupFor(section.number);
      var card = doc.createElement('a');
      card.className = 'section-card';
      card.href = '#/section/' + section.number;
      card.style.setProperty('--group-color', group.color);
      card.setAttribute('aria-label', 'Section ' + section.number + ': ' + section.title);
      var top = el('div', 'card-top');
      top.innerHTML = icon(section.number);
      top.appendChild(el('span', null, section.number));
      top.appendChild(el('span', 'card-group', group.name));
      card.appendChild(top);
      card.appendChild(el('h3', null, section.title));
      card.appendChild(el('p', null, section.description || 'Read this arc42 section.'));
      card.appendChild(el('span', 'card-arrow', '\u2192'));
      grid.appendChild(card);
    }
    host.appendChild(intro);
    host.appendChild(grid);
  }

  function showIntroduction(host) {
    renderDoc(host, README.title || 'Introduction', README.description || '', README.html || '');
  }

  function showSection(host, number) {
    var section = byNumber[number];
    if (!section) {
      clear(host);
      var box = el('div', 'doc');
      box.appendChild(el('h2', 'doc-title', 'Section not found'));
      var p = el('p');
      p.appendChild(doc.createTextNode('This snapshot does not contain section ' + number + '.'));
      box.appendChild(p);
      var ways = el('p');
      var home = doc.createElement('a');
      home.href = '#/';
      home.textContent = 'Open the overview';
      ways.appendChild(home);
      ways.appendChild(doc.createTextNode(' or '));
      var list = doc.createElement('a');
      list.href = '#/section/' + (SECTIONS[0] ? SECTIONS[0].number : '01');
      list.textContent = 'start at the first section';
      ways.appendChild(list);
      ways.appendChild(doc.createTextNode('.'));
      box.appendChild(ways);
      host.appendChild(box);
      return;
    }
    clear(host);
    var article = el('article', 'doc');
    article.appendChild(el('p', 'section-kicker', 'Section ' + section.number));
    article.appendChild(el('h2', 'doc-title', section.title));
    if (section.description) article.appendChild(el('p', 'doc-desc', section.description));
    var body = el('div', 'doc-body');
    body.innerHTML = section.html || '';
    article.appendChild(body);
    host.appendChild(article);
    doc.getElementById('main').scrollTop = 0;
  }

  /* ---------- Sidebar state ---------- */
  function syncNav(route) {
    var links = doc.querySelectorAll('aside.nav a[data-route]');
    for (var i = 0; i < links.length; i += 1) {
      var current = links[i].getAttribute('data-route') === route;
      if (current) links[i].setAttribute('aria-current', 'page');
      else links[i].removeAttribute('aria-current');
    }
    var m = /^#\/section\/(\d+)/.exec(route);
    var number = m ? m[1] : null;
    var prev = doc.getElementById('nav-prev');
    var next = doc.getElementById('nav-next');
    var set = function (a, target, direction, label) {
      clear(a);
      if (!target) { a.style.visibility = 'hidden'; return; }
      a.style.visibility = 'visible';
      a.href = '#/section/' + target.number;
      a.setAttribute('aria-label', direction + ' section: ' + target.title);
      a.title = direction + ' section: ' + target.title;
      var arrow = el('span', 'nav-arrow', label);
      arrow.setAttribute('aria-hidden', 'true');
      a.appendChild(arrow);
      a.appendChild(el('span', null, target.title));
    };
    if (number) {
      var pos = -1;
      for (var s = 0; s < SECTIONS.length; s += 1) if (SECTIONS[s].number === number) pos = s;
      set(prev, pos > 0 ? SECTIONS[pos - 1] : null, 'Previous', '\u2190');
      set(next, pos >= 0 && pos < SECTIONS.length - 1 ? SECTIONS[pos + 1] : null, 'Next', '\u2192');
    } else {
      set(prev, null, '', ''); set(next, null, '', '');
    }
  }

  /* ---------- Router ---------- */
  function go() {
    var hash = location.hash || '#/';
    var host = doc.getElementById('content');
    var route = hash.split('?')[0];
    var m = /^#\/section\/(\d{2})(?:\/([^/]*))?$/.exec(route);
    var introMatch = /^#\/introduction(?:\/([^/]*))?$/.exec(route);
    var legacyIntroAnchor = !m && !introMatch && /^#\/[^/]+$/.test(route);
    var introRoute = !!introMatch || legacyIntroAnchor;
    if (m) showSection(host, m[1]);
    else if (introRoute) showIntroduction(host);
    else showOverview(host);
    syncNav(m ? '#/section/' + m[1] : (introRoute ? '#/introduction' : '#/'));
    var anchor = m ? m[2] : (introMatch ? introMatch[1] : (legacyIntroAnchor ? route.slice(2) : ''));
    if (anchor) {
      try {
        var heading = doc.getElementById(decodeURIComponent(anchor));
        if (heading && host.contains(heading)) heading.scrollIntoView({ block: 'start' });
      } catch (error) { /* malformed anchors leave the document at its start */ }
    }
  }

  /* ---------- Appearance: carried in the address only ---------- */
  function applyAppearance() {
    var m = /[?&]a=(light|dark)/.exec(location.hash);
    var choice = m ? m[1] : 'auto';
    if (choice !== 'auto') root.setAttribute('data-appearance', choice);
    else root.removeAttribute('data-appearance');
    var buttons = doc.querySelectorAll('.appearance button');
    for (var i = 0; i < buttons.length; i += 1) {
      buttons[i].setAttribute('aria-pressed', String(buttons[i].getAttribute('data-appearance-set') === choice));
    }
  }
  function setAppearance(choice) {
    var hash = (location.hash || '#/').split('?')[0];
    var next = choice === 'auto' ? hash : hash + (hash.indexOf('?') === -1 ? '?' : '&') + 'a=' + choice;
    location.hash = next;
  }

  /* ---------- Search dialog ---------- */
  var findState = { sel: 0, results: [] };
  function openFind() {
    var dlg = doc.getElementById('find');
    var input = doc.getElementById('find-input');
    dlg.hidden = false;
    input.value = '';
    runSearch('');
    input.focus();
  }
  function closeFind() { doc.getElementById('find').hidden = true; }
  function runSearch(query) {
    var list = doc.getElementById('find-results');
    clear(list);
    findState.results = search(query);
    findState.sel = 0;
    if (query.trim() === '') {
      var li = el('li');
      li.appendChild(el('div', 'empty', 'Type to search across the introduction and all ' + SECTIONS.length + ' sections.'));
      list.appendChild(li);
      return;
    }
    if (findState.results.length === 0) {
      var li2 = el('li');
      li2.appendChild(el('div', 'empty', 'No matches.'));
      list.appendChild(li2);
      return;
    }
    for (var i = 0; i < findState.results.length; i += 1) {
      var r = findState.results[i].entry;
      var a = doc.createElement('a');
      a.href = r.route;
      a.addEventListener('click', closeFind);
      var where = el('span', null, r.where);
      a.appendChild(where);
      a.appendChild(el('span', 'where', r.number ? 'section ' + r.number : 'overview'));
      list.appendChild(a);
    }
    var first = list.querySelector('a');
    if (first) first.classList.add('sel');
  }
  function moveSel(delta) {
    var links = doc.querySelectorAll('#find-results a');
    if (links.length === 0) return;
    links[findState.sel].classList.remove('sel');
    findState.sel = Math.min(Math.max(findState.sel + delta, 0), links.length - 1);
    links[findState.sel].classList.add('sel');
    links[findState.sel].scrollIntoView({ block: 'nearest' });
  }

  /* ---------- Keyboard shortcuts ---------- */
  function shortcuts(event) {
    if (event.defaultPrevented) return;
    var target = event.target;
    var typing = target && (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA' || target.isContentEditable);
    var findOpen = !doc.getElementById('find').hidden;
    if (findOpen) {
      if (event.key === 'Escape') { closeFind(); event.preventDefault(); }
      else if (event.key === 'ArrowDown') { moveSel(1); event.preventDefault(); }
      else if (event.key === 'ArrowUp') { moveSel(-1); event.preventDefault(); }
      else if (event.key === 'Enter') {
        var links = doc.querySelectorAll('#find-results a');
        if (links[findState.sel]) { location.hash = links[findState.sel].getAttribute('href'); closeFind(); }
        event.preventDefault();
      }
      return;
    }
    if (event.key === 'Escape' && doc.getElementById('shell').classList.contains('drawer-open')) {
      closeDrawer(true);
      event.preventDefault();
      return;
    }
    if (typing) return;
    if (event.key === '/') { openFind(); event.preventDefault(); }
    else if (event.key === '?') { doc.getElementById('keys').hidden = false; event.preventDefault(); }
    else if (event.key === 'Escape') { doc.getElementById('keys').hidden = true; }
    else if (event.key === 'ArrowRight' || event.key === 'PageDown') {
      var nx = doc.getElementById('nav-next');
      if (nx && nx.style.visibility !== 'hidden') location.hash = nx.getAttribute('href');
    }
    else if (event.key === 'ArrowLeft' || event.key === 'PageUp') {
      var pv = doc.getElementById('nav-prev');
      if (pv && pv.style.visibility !== 'hidden') location.hash = pv.getAttribute('href');
    }
  }

  /* ---------- Toast ---------- */
  var toastTimer = null;
  function toast(message) {
    var t = doc.getElementById('toast');
    t.textContent = message;
    t.classList.add('show');
    if (toastTimer) clearTimeout(toastTimer);
    toastTimer = setTimeout(function () { t.classList.remove('show'); }, 1600);
  }

  var drawerOpener = null;
  function openDrawer() {
    drawerOpener = doc.getElementById('nav-toggle');
    doc.getElementById('shell').classList.add('drawer-open');
    doc.getElementById('nav-backdrop').hidden = false;
    drawerOpener.setAttribute('aria-expanded', 'true');
    doc.getElementById('nav-close').focus();
  }
  function closeDrawer(restoreFocus) {
    doc.getElementById('shell').classList.remove('drawer-open');
    doc.getElementById('nav-backdrop').hidden = true;
    doc.getElementById('nav-toggle').setAttribute('aria-expanded', 'false');
    if (restoreFocus && drawerOpener) drawerOpener.focus();
  }

  /* ---------- Wire up ---------- */
  window.addEventListener('hashchange', function () { applyAppearance(); go(); });
  doc.addEventListener('keydown', shortcuts);
  doc.getElementById('ov-find').addEventListener('click', openFind);
  doc.getElementById('nav-toggle').addEventListener('click', openDrawer);
  doc.getElementById('nav-close').addEventListener('click', function () { closeDrawer(true); });
  doc.getElementById('nav-backdrop').addEventListener('click', function () { closeDrawer(true); });
  doc.getElementById('collapse-toggle').addEventListener('click', function () {
    var shell = doc.getElementById('shell');
    var collapsed = shell.classList.toggle('nav-collapsed');
    this.setAttribute('aria-expanded', String(!collapsed));
    this.setAttribute('aria-label', collapsed ? 'Expand sections navigation' : 'Collapse sections navigation');
    this.title = collapsed ? 'Expand sections navigation' : 'Collapse sections navigation';
    this.textContent = collapsed ? '⇥' : '⇤';
  });
  var navLinks = doc.querySelectorAll('aside.nav a[data-route]');
  for (var n = 0; n < navLinks.length; n += 1) navLinks[n].addEventListener('click', function () {
    if (doc.getElementById('shell').classList.contains('drawer-open')) closeDrawer(false);
  });
  doc.getElementById('keys-open').addEventListener('click', function () { doc.getElementById('keys').hidden = false; });
  doc.getElementById('keys-close').addEventListener('click', function () { doc.getElementById('keys').hidden = true; });
  doc.getElementById('keys').addEventListener('click', function (event) { if (event.target === this) this.hidden = true; });
  var findInput = doc.getElementById('find-input');
  findInput.addEventListener('input', function () { runSearch(findInput.value); });
  var appearanceButtons = doc.querySelectorAll('.appearance button');
  for (var b = 0; b < appearanceButtons.length; b += 1) {
    appearanceButtons[b].addEventListener('click', function () { setAppearance(this.getAttribute('data-appearance-set')); });
  }
  applyAppearance();
  go();
})();
`;

/* ------------------------------------------------------------------ */
/* Snapshot assembly                                                   */
/* ------------------------------------------------------------------ */

/** Stable JSON: sorted keys, no incidental whitespace variation. */
function stableJson(value) {
  return JSON.stringify(value).replace(/</g, '\\u003c');
}

function iconMarkup(number) {
  const paths = ['<circle cx="12" cy="12" r="8"/><circle cx="12" cy="12" r="2"/><path d="M12 2v3m0 14v3M2 12h3m14 0h3"/>', '<path d="M4 5h16M4 12h16M4 19h16"/><circle cx="7" cy="5" r="1"/><circle cx="17" cy="12" r="1"/><circle cx="9" cy="19" r="1"/>', '<path d="M4 5h16v14H4zM8 9h8M8 13h5"/>', '<path d="M4 19V5m0 14h16M7 16l4-5 3 2 5-7"/>', '<rect x="4" y="4" width="6" height="6" rx="1"/><rect x="14" y="4" width="6" height="6" rx="1"/><rect x="4" y="14" width="6" height="6" rx="1"/><rect x="14" y="14" width="6" height="6" rx="1"/>', '<path d="M4 6h6v5H4zM14 13h6v5h-6zM10 8h3a2 2 0 0 1 2 2v3"/>', '<path d="M4 20V4m0 16h16M7 16l3-4 3 2 5-7"/><path d="M15 7h3v3"/>', '<circle cx="12" cy="12" r="9"/><path d="M12 7v5l4 2"/>', '<path d="M7 3h8l4 4v14H7zM15 3v5h4M10 12h6m-6 4h6"/>', '<path d="M4 5h16v14H4zM8 9h8M8 13h8M8 17h4"/>', '<path d="M12 3 3 20h18L12 3z"/><path d="M12 9v5m0 3h.01"/>', '<circle cx="10.5" cy="10.5" r="6.5"/><path d="m16 16 5 5M8 10h5m-2.5-2.5v5"/>'];
  const idx = Math.max(0, Math.min(11, Number(number) - 1));
  return `<svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true" focusable="false">${paths[idx]}</svg>`;
}

const abbreviations = ['Intro', 'Constr.', 'Scope', 'Strategy', 'Blocks', 'Runtime', 'Deploy', 'Crosscut', 'ADRs', 'Quality', 'Risks', 'Glossary'];

/**
 * @param {object} input
 * @param {{title: string, description: string, html: string}} input.readme
 * @param {Array<{number: string, title: string, description: string, html: string}>} input.sections
 * @param {string} input.stackName
 * @param {string|undefined} input.revision
 */
export function buildSnapshotHtml({ readme, sections, stackName, revision }) {
  const revisionShort = revision
    ? `<span class="revshort" title="revision ${esc(revision)}">${esc(revision.slice(0, 7))}</span>`
    : '<span class="revshort" title="Not generated from a Git checkout">no revision</span>';

  const navItems = sections
    .map(
      (s) =>
        `        <li><a href="#/section/${s.number}" data-route="#/section/${s.number}" aria-label="Section ${s.number}: ${esc(s.title)}" title="${esc(s.title)}"><span class="num">${s.number}</span>${iconMarkup(s.number)}<span class="t"><span class="t-full">${esc(s.title)}</span><span class="t-short">${esc(abbreviations[Number(s.number) - 1] || s.title)}</span></span></a></li>`,
    )
    .join('\n');

  const data = {
    readme: { title: readme.title, description: readme.description, html: readme.html },
    sections: sections.map((s) => ({
      number: s.number,
      title: s.title,
      description: s.description,
      html: s.html,
    })),
  };

  const lines = [
    '<!DOCTYPE html>',
    '<html lang="en">',
    '<head>',
    '<meta charset="utf-8">',
    '<meta name="viewport" content="width=device-width, initial-scale=1">',
    `<meta http-equiv="Content-Security-Policy" content="default-src 'none'; script-src 'sha256-${createHash('sha256').update(`\n${script}\n`).digest('base64')}'; style-src 'unsafe-inline'; base-uri 'none'; form-action 'none'">`,
    '<meta name="color-scheme" content="light dark">',
    `<title>${esc(stackName)} — arc42 Architecture Snapshot</title>`,
    `<style>\n${style}\n</style>`,
    '</head>',
    '<body>',
    '<a class="skip" href="#main">Skip to content</a>',
    '<header class="site">',
    `<h1>${esc(stackName)} · arc42</h1>`,
    revisionShort,
    '<span class="spacer"></span>',
    '<div class="appearance" role="group" aria-label="Appearance">',
    '  <button type="button" data-appearance-set="auto" aria-pressed="true">Auto</button>',
    '  <button type="button" data-appearance-set="light" aria-pressed="false">Light</button>',
    '  <button type="button" data-appearance-set="dark" aria-pressed="false">Dark</button>',
    '</div>',
    '<button type="button" class="iconbtn nav-toggle" id="nav-toggle" aria-expanded="false" aria-controls="arc42-nav" aria-label="Open sections navigation">☰ <span>Sections</span></button>',
    '<button type="button" class="iconbtn nav-toggle desktop-nav-toggle" id="collapse-toggle" aria-expanded="true" aria-controls="arc42-nav" aria-label="Collapse sections navigation" title="Collapse sections navigation">⇤</button>',
    '<button type="button" class="iconbtn" id="ov-find" aria-haspopup="dialog" aria-label="Search (/)" title="Search (/)">\u2315</button>',
    '<button type="button" class="iconbtn" id="keys-open" aria-haspopup="dialog" aria-label="Keyboard shortcuts (?)" title="Keyboard shortcuts (?)">?</button>',
    '</header>',
    '<div id="shell">',
    '<aside class="nav" id="arc42-nav" aria-label="arc42 sections">',
    '  <div class="nav-heading"><h2>Sections</h2><button type="button" class="iconbtn nav-close" id="nav-close" aria-label="Close sections navigation">×</button></div>',
    '  <ol>',
    `    <li><a href="#/" data-route="#/" aria-label="Overview" title="Overview"><span class="num">⌂</span><span class="t">Overview</span></a></li>`,
    `    <li><a href="#/introduction" data-route="#/introduction" aria-label="Introduction: where architectural and product truth lives" title="Introduction"><span class="num">i</span><span class="t">Introduction</span></a></li>`,
    navItems,
    '  </ol>',
    '  <div class="nav-foot">',
    '    <div class="prevnext">',
    '      <a id="nav-prev" href="#/" aria-label="Previous section"></a>',
    '      <a id="nav-next" href="#/" aria-label="Next section"></a>',
    '    </div>',
    '  </div>',
    '</aside>',
    '<button type="button" id="nav-backdrop" aria-label="Close sections navigation" tabindex="-1" hidden></button>',
    '<main id="main" tabindex="-1">',
    '  <div id="content"></div>',
    '</main>',
    '</div>',
    '<div class="dlg" id="find" role="dialog" aria-modal="true" aria-labelledby="find-title" hidden>',
    '<div class="dlgbox">',
    `  <input id="find-input" type="text" placeholder="Search ${esc(stackName)} docs…" aria-label="Search query" autocomplete="off">`,
    '  <ul id="find-results"></ul>',
    '</div>',
    '</div>',
    '<div class="dlg" id="keys" role="dialog" aria-modal="true" aria-labelledby="keys-title" hidden>',
    '<div class="dlgbox keysbox">',
    '  <input type="text" value="Keyboard shortcuts" aria-label="Keyboard shortcuts" readonly>',
    '  <dl>',
    '    <dt><kbd>/</kbd></dt><dd>Search the introduction and all sections</dd>',
    '    <dt><kbd>←</kbd> / <kbd>→</kbd></dt><dd>Previous / next section</dd>',
    '    <dt><kbd>?</kbd></dt><dd>Show this list</dd>',
    '    <dt><kbd>Esc</kbd></dt><dd>Close dialogs</dd>',
    '  </dl>',
    '  <p style="margin:0; padding:0.6rem 0.9rem 0.9rem; text-align:right;"><button type="button" id="keys-close">Close</button></p>',
    '</div>',
    '</div>',
    '<p class="toast" id="toast" role="status" aria-live="polite"></p>',
    `<script id="arc42-data" type="application/json">${stableJson(data)}</script>`,
    `<script>\n${script}\n</script>`,
    '</body>',
    '</html>',
  ];
  return `${lines.filter((l) => l !== '').join('\n')}\n`;
}
