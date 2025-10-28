const fs = require("fs");
const path = require("path");

function collectStoryFiles(rootDir) {
  const stack = [rootDir];
  const files = [];

  while (stack.length) {
    const current = stack.pop();
    const stat = fs.statSync(current);

    if (stat.isDirectory()) {
      for (const entry of fs.readdirSync(current)) {
        stack.push(path.join(current, entry));
      }
    } else if (current.endsWith(".stories.js")) {
      files.push(current);
    }
  }

  return files.sort();
}

function stripHtml(html) {
  return html.replace(/<[^>]*>/g, "").trim();
}

function coerceCanvas(renderResult) {
  if (renderResult == null) {
    return { innerHTML: "", textContent: "" };
  }

  if (typeof renderResult === "string") {
    return {
      innerHTML: renderResult,
      textContent: stripHtml(renderResult),
    };
  }

  if (
    typeof renderResult === "object" &&
    renderResult !== null &&
    typeof renderResult.innerHTML === "string"
  ) {
    return {
      innerHTML: renderResult.innerHTML,
      textContent: stripHtml(renderResult.innerHTML),
    };
  }

  return {
    innerHTML: String(renderResult),
    textContent: stripHtml(String(renderResult)),
  };
}

function getRender(meta, story, storyName) {
  if (typeof story === "function") {
    return story;
  }

  if (story && typeof story.render === "function") {
    return story.render;
  }

  if (meta && typeof meta.render === "function") {
    return meta.render;
  }

  throw new Error(`Story "${storyName}" does not define a render function.`);
}

function getPlay(meta, story) {
  if (story && typeof story.play === "function") {
    return story.play;
  }

  if (meta && typeof meta.play === "function") {
    return meta.play;
  }

  return null;
}

async function runStory({ file, storyName, storyExport, meta }) {
  const args = (storyExport && storyExport.args) || (meta && meta.args) || {};
  const render = getRender(meta, storyExport, storyName);
  const canvas = coerceCanvas(render(args));

  const play = getPlay(meta, storyExport);
  if (play) {
    await play({ canvasElement: canvas, args, step: async (_name, fn) => fn() });
  }

  return canvas;
}

async function runStoryFile(file) {
  const exports = require(file);
  const meta = exports.default || {};
  const stories = Object.entries(exports).filter(([name]) => name !== "default");

  if (!stories.length) {
    throw new Error(`Story file ${file} does not export any stories.`);
  }

  const results = [];
  for (const [storyName, storyExport] of stories) {
    const canvas = await runStory({
      file,
      storyName,
      storyExport,
      meta,
    });

    results.push({ storyName, canvas });
  }

  return results;
}

async function main() {
  const storyRoot = path.join(__dirname, "..", "storybook");
  const files = collectStoryFiles(storyRoot);

  if (!files.length) {
    console.error("No storybook stories found.");
    process.exitCode = 1;
    return;
  }

  const failures = [];
  let executed = 0;

  for (const file of files) {
    try {
      const storyResults = await runStoryFile(file);
      executed += storyResults.length;
      for (const { storyName } of storyResults) {
        console.log(`✓ ${path.relative(process.cwd(), file)} › ${storyName}`);
      }
    } catch (error) {
      failures.push({ file, error });
      console.error(`✗ ${path.relative(process.cwd(), file)}\n  ${error.message}`);
    }
  }

  if (failures.length) {
    process.exitCode = 1;
    return;
  }

  console.log(`\nExecuted ${executed} stor${executed === 1 ? "y" : "ies"}.`);
}

main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
