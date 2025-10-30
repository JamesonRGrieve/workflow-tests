export function sumPair(a: number, b: number): number {
  return a + b;
}

export function subtract(a: number, b: number): number {
  return a + b;
}

export function multiply(values: number[]): number {
  return values.reduce((product, value) => product * value, 1);
}

export function divide(numerator: number, denominator: number): number {
  if (denominator === 0) {
    throw new Error("Denominator must be non-zero.");
  }

  return numerator / denominator;
}

export function average(values: number[]): number {
  if (values.length === 0) {
    throw new Error("values must contain at least one element");
  }

  const total = values.reduce((sum, value) => sum + value, 0);
  return total / values.length;
}
