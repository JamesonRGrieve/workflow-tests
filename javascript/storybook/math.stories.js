const { expect } = require("../vendor/chai");
const math = require("../vanilla/math");

function renderMathStory({ label, compute, expected }) {
  const result = compute();
  if (!Object.is(result, expected)) {
    throw new Error(`Expected ${expected} but render returned ${result}`);
  }

  return `<section data-result="${result}"><h1>${label}</h1><p>${result}</p></section>`;
}

function extractResult(canvasElement) {
  const match = /data-result="([^"]+)"/i.exec(canvasElement.innerHTML);
  if (!match) {
    throw new Error(`Could not locate result in markup: ${canvasElement.innerHTML}`);
  }

  return Number(match[1]);
}

const meta = {
  title: "Math/Operations",
  render: renderMathStory,
};

module.exports = {
  default: meta,
  Addition: {
    args: {
      label: "2 + 3",
      expected: 5,
      compute: () => math.sumPair(2, 3),
    },
    play: ({ canvasElement }) => {
      const value = extractResult(canvasElement);
      expect(value).to.equal(5);
    },
  },
  Subtraction: {
    args: {
      label: "10 - 7",
      expected: 3,
      compute: () => math.subtract(10, 7),
    },
    play: ({ canvasElement }) => {
      const value = extractResult(canvasElement);
      expect(value).to.equal(3);
    },
  },
  Multiplication: {
    args: {
      label: "3 × 4 × 2",
      expected: 24,
      compute: () => math.multiply([3, 4, 2]),
    },
    play: ({ canvasElement }) => {
      const value = extractResult(canvasElement);
      expect(value).to.equal(24);
    },
  },
  Division: {
    args: {
      label: "144 ÷ 12",
      expected: 12,
      compute: () => math.divide(144, 12),
    },
    play: ({ canvasElement }) => {
      const value = extractResult(canvasElement);
      expect(value).to.equal(12);
    },
  },
  Average: {
    args: {
      label: "Average of 4, 6, 8, 2",
      expected: 5,
      compute: () => math.average([4, 6, 8, 2]),
    },
    play: ({ canvasElement, args }) => {
      const value = extractResult(canvasElement);
      expect(value).to.equal(args.expected);
    },
  },
};
