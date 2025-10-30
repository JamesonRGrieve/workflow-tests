function expect(actual) {
  return {
    to: {
      equal(expected) {
        if (!Object.is(actual, expected)) {
          throw new Error(`expected ${actual} to equal ${expected}`);
        }
      },
    },
  };
}

module.exports = { expect };
