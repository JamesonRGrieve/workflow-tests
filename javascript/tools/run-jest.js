const fs = require("fs");
const path = require("path");
const { execFileSync } = require("child_process");

const projectRoot = path.resolve(__dirname, "../typescript");
const distDir = path.join(projectRoot, "dist");

if (fs.existsSync(distDir)) {
  fs.rmSync(distDir, { recursive: true, force: true });
}

execFileSync("tsc", ["-p", path.join(projectRoot, "tsconfig.json")], { stdio: "inherit" });

const tests = [];
const suiteStack = [];

global.describe = (name, fn) => {
  suiteStack.push(name);
  try {
    fn();
  } finally {
    suiteStack.pop();
  }
};

global.test = (name, fn) => {
  tests.push({
    suite: suiteStack.join(" › "),
    name,
    fn,
  });
};

global.expect = (actual) => ({
  toBe(expected) {
    if (!Object.is(actual, expected)) {
      throw new Error(`Expected ${actual} to be ${expected}`);
    }
  },
  toBeCloseTo(expected, precision = 2) {
    const factor = 10 ** precision;
    if (Math.round(actual * factor) !== Math.round(expected * factor)) {
      throw new Error(`Expected ${actual} to be close to ${expected}`);
    }
  },
});

require(path.join(distDir, "__tests__", "math.test.js"));

let failures = 0;
for (const { suite, name, fn } of tests) {
  try {
    fn();
    const label = suite ? `${suite} ${name}` : name;
    console.log(`PASS ${label}`);
  } catch (error) {
    failures += 1;
    const label = suite ? `${suite} ${name}` : name;
    console.error(`FAIL ${label}`);
    console.error(error instanceof Error ? error.stack : String(error));
  }
}

if (failures > 0) {
  console.error(`${failures} test(s) failed.`);
  process.exitCode = 1;
} else {
  console.log(`${tests.length} test(s) passed.`);
}
