const fs = require("fs");
const path = require("path");
const Module = require("module");

const vendorDir = path.resolve(__dirname, "../vendor");
process.env.NODE_PATH = [process.env.NODE_PATH, vendorDir].filter(Boolean).join(path.delimiter);
Module._initPaths();

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

global.it = (name, fn) => {
  tests.push({
    suite: suiteStack.join(" › "),
    name,
    fn,
  });
};

const testDir = path.resolve(__dirname, "mocha");

function collectTests(dir) {
  for (const entry of fs.readdirSync(dir)) {
    const fullPath = path.join(dir, entry);
    const stats = fs.statSync(fullPath);
    if (stats.isDirectory()) {
      collectTests(fullPath);
    } else if (entry.endsWith(".spec.js")) {
      require(fullPath);
    }
  }
}

collectTests(testDir);

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
