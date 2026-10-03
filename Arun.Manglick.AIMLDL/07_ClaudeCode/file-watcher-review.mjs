import Anthropic from "@anthropic-ai/sdk";
import chokidar from "chokidar";
import { readFile } from "node:fs/promises";
import { extname, relative } from "node:path";

const WATCHED_DIR = ".";
const REVIEW_EXTENSIONS = new Set([".js", ".ts", ".astro", ".jsx", ".tsx"]);
const IGNORE_PATTERNS = [
  "**/node_modules/**",
  "**/.git/**",
  "**/dist/**",
  "**/build/**",
  "**/.claude/**",
  "**/file-watcher-review.mjs",
];
const DEBOUNCE_MS = 500;
const MAX_FILE_SIZE = 100 * 1024;

const REVIEW_PROMPT = `You are a code review agent. Review the file that was just modified. Focus on:
(1) Code style - naming, formatting, consistency with surrounding code,
(2) Performance - inefficient patterns, unnecessary work,
(3) Security - XSS, injection, exposed secrets,
(4) Maintainability - readability, complexity, duplication.
For Astro files check content collection usage and Tailwind patterns.
Report only genuine issues. Format: list each issue with severity [CRITICAL/WARNING/INFO], file location, description, and suggested fix.
If no issues found, say 'No issues detected.' Be concise - no preamble.`;

const client = new Anthropic();
const debounceTimers = new Map();

async function reviewFile(filePath) {
  const relPath = relative(".", filePath);

  let content;
  try {
    content = await readFile(filePath, "utf-8");
  } catch (err) {
    if (err.code === "ENOENT") return;
    throw err;
  }

  if (content.length > MAX_FILE_SIZE) {
    console.log(`\n--- Skipped: ${relPath} (exceeds 100 KB) ---\n`);
    return;
  }

  console.log(`\n--- Review: ${relPath} (${new Date().toISOString()}) ---`);

  const response = await client.messages.create({
    model: "claude-haiku-4-5",
    max_tokens: 2048,
    system: REVIEW_PROMPT,
    messages: [
      {
        role: "user",
        content: `Review this file (${relPath}):\n\n${content}`,
      },
    ],
  });

  for (const block of response.content) {
    if (block.type === "text") {
      console.log(block.text);
    }
  }

  console.log("----------------------------------------------------\n");
}

function handleChange(filePath) {
  const ext = extname(filePath).toLowerCase();
  if (!REVIEW_EXTENSIONS.has(ext)) return;

  if (debounceTimers.has(filePath)) {
    clearTimeout(debounceTimers.get(filePath));
  }

  debounceTimers.set(
    filePath,
    setTimeout(async () => {
      debounceTimers.delete(filePath);
      try {
        await reviewFile(filePath);
      } catch (err) {
        console.error(`[error] Review failed for ${filePath}:`, err.message);
      }
    }, DEBOUNCE_MS)
  );
}

const watcher = chokidar.watch(WATCHED_DIR, {
  ignored: IGNORE_PATTERNS,
  persistent: true,
  ignoreInitial: true,
  awaitWriteFinish: {
    stabilityThreshold: 300,
    pollInterval: 100,
  },
});

watcher.on("change", handleChange);

watcher.on("ready", () => {
  console.log("Watching for file changes...");
  console.log(`Extensions: ${[...REVIEW_EXTENSIONS].join(", ")}`);
  console.log(`Ignored: ${IGNORE_PATTERNS.join(", ")}`);
  console.log("Press Ctrl+C to stop.\n");
});

watcher.on("error", (err) => {
  console.error("[watcher error]", err.message);
});
