# Workflow Regression Test Validation

This repository exercises a collection of language-specific regression test runners.
Source code for the systems under test now lives in `src` directories directly under each
language root. Individual test harnesses sit alongside that shared code so they can
consume the same implementation regardless of framework.

```
.
├── csharp
│   ├── src/                  # Shared C# calculator library
│   ├── nunit/                # Self-contained NUnit-style runner
│   └── xunit/                # Self-contained xUnit-style runner
├── javascript
│   ├── src/                  # Reusable JavaScript/TypeScript helpers
│   ├── mocha/                # Mocha-flavoured specs
│   ├── jest/                 # Jest-style TypeScript specs
│   └── storybook/            # Storybook stories executed headlessly
└── python
    ├── src/                  # Deterministic calculator helpers
    ├── pytest/               # Pytest suite
    └── unittest/             # Standard library unittest suite
```

Each test runner should import functionality from its sibling `src` directory. When
adding new examples, keep implementation logic in `src` so that multiple frameworks can
exercise the same code.
