import { average, divide, multiply, subtract, sumPair } from "../src/math";

describe("math helpers (jest)", () => {
  test("adds two numbers", () => {
    expect(sumPair(4, 6)).toBe(10);
  });

  test("subtracts two numbers", () => {
    expect(subtract(15, 5)).toBe(10);
  });

  test("multiplies a list of numbers", () => {
    expect(multiply([1, 2, 3, 6])).toBe(36);
  });

  test("divides two numbers", () => {
    expect(divide(12, 5)).toBeCloseTo(2.4);
  });

  test("computes the average", () => {
    expect(average([2, 4, 6, 8])).toBe(5);
  });

});
