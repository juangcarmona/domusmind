#!/usr/bin/env node
import { readFileSync } from "node:fs";
import { execFileSync } from "node:child_process";

const workers = ["triage", "refine", "propose", "implement", "apply-review"];
const trackedFiles = execFileSync("git", ["ls-files", "-z"], {
  encoding: "utf8",
})
  .split("\0")
  .filter(Boolean);

for (const worker of workers) {
  const workflow = `.github/workflows/lighthouse-sdlc-${worker}`;
  const checkoutPaths = [];
  for (const file of [`${workflow}.md`, `${workflow}.lock.yml`]) {
    const source = readFileSync(file, "utf8");
    const checkouts = source.split(/(?=^\s*-\s+name:)/mu);
    const checkout = checkouts.find(
      (block) =>
        /uses:\s*actions\/checkout@/u.test(block) &&
        /repository:\s*juangcarmona\/lighthouse/u.test(block),
    );
    const match = checkout?.match(/^\s*path:\s*([^\s]+)/mu);
    if (!match)
      throw Error(`${file} must declare the Lighthouse checkout path.`);

    const checkoutPath = match[1].replaceAll("\\", "/").replace(/\/$/u, "");
    checkoutPaths.push(checkoutPath);
    const collision = trackedFiles.find(
      (path) =>
        path === checkoutPath ||
        path.startsWith(`${checkoutPath}/`) ||
        checkoutPath.startsWith(`${path}/`),
    );
    if (collision)
      throw Error(
        `${file} checks Lighthouse out over tracked project path ${checkoutPath} (${collision}).`,
      );
  }
  if (new Set(checkoutPaths).size !== 1)
    throw Error(
      `${workflow} source and compiled workflow use different checkout paths.`,
    );
}
