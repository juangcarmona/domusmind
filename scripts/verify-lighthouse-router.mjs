import { readFileSync } from 'node:fs';

const router = readFileSync('.github/workflows/lighthouse-sdlc-router.yml', 'utf8');
const autoImplement = router.match(/^  LIGHTHOUSE_AUTO_IMPLEMENT: (.+)$/mu)?.[1];
if (autoImplement !== 'false') {
  throw new Error('Lighthouse auto-implement must stay disabled until the owner-approved Plan gate.');
}
