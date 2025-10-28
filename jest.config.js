/** @type {import('jest').Config} */
module.exports = {
  roots: ["<rootDir>/javascript/typescript"],
  transform: {
    "^.+\\.[tj]sx?$": ["ts-jest", { tsconfig: "<rootDir>/javascript/typescript/tsconfig.json" }]
  },
  testRegex: "(/__tests__/.*|(\\.|/)(test|spec))\\.[tj]sx?$",
  moduleFileExtensions: ["ts", "tsx", "js", "jsx", "json", "node"],
  clearMocks: true
};
