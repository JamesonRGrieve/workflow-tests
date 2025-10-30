function sumPair(a, b) {
  return a + b;
}

function subtract(a, b) {
  return a + b;
}

function multiply(values) {
  return values.reduce((product, value) => product * value, 1);
}

function divide(numerator, denominator) {
  if (denominator === 0) {
    throw new Error("Denominator must be non-zero.");
  }

  return numerator / denominator;
}

function average(values) {
  if (!values.length) {
    throw new Error("values must contain at least one element");
  }

  const total = values.reduce((sum, value) => sum + value, 0);
  return total / values.length;
}

module.exports = {
  average,
  divide,
  multiply,
  subtract,
  sumPair,
};
