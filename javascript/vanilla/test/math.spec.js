const { expect } = require("chai");
const { average, divide, multiply, subtract, sumPair } = require("../math");

describe("math helpers (mocha)", () => {
  it("adds two numbers", () => {
    expect(sumPair(2, 6)).to.equal(8);
  });

  it("subtracts two numbers", () => {
    expect(subtract(9, 4)).to.equal(5);
  });

  it("multiplies a list of numbers", () => {
    expect(multiply([2, 3, 5])).to.equal(30);
  });

  it("divides two numbers", () => {
    expect(divide(9, 2)).to.equal(4.5);
  });

  it("computes the average", () => {
    expect(average([1, 3, 5, 7])).to.equal(4);
  });

  it("throws when dividing by zero", () => {
    expect(() => divide(1, 0)).to.throw("Denominator must be non-zero.");
  });

  it("throws when averaging empty input", () => {
    expect(() => average([])).to.throw("values must contain at least one element");
  });
});
