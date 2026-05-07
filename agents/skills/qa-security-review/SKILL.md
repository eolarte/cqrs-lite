---
name: qa-security-review
description: Review the implemented solution for security risks, missing safeguards, and unsafe trust-boundary assumptions, then write a persistent security review artifact beside the spec. Use after implementation-manager.
user-invocable: true
argument-hint: "[spec path, test plan path, issue number, or implemented slice to review]"
---

# QA Security Review

Review the implemented solution for security risks and missing controls. This skill analyzes code and contracts; it does not perform live penetration testing.

## Inputs

Read:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. relevant files under `docs/specs/<module>/<submodule>/`:
   - `<spec>.md`
   - `<spec>.technical-spec.md`
   - `<spec>.architecture.md`
   - `<spec>.test-plan.md`
5. relevant source and test code
6. existing auth, validation, persistence, external boundary, and configuration patterns in the repo

## Focus Areas

Look for:

- missing input validation or sanitization
- weak or missing authorization assumptions
- unsafe trust boundaries
- sensitive data exposure in logs, errors, or responses
- secret or configuration handling risks
- insecure defaults
- unsafe external interaction patterns

## Process

### 1. Review implementation against contracts

Compare the implemented behavior with the intended boundaries and constraints from the spec artifacts.

### 2. Record findings

For each finding, capture:

- finding summary
- affected area or contract
- severity:
  - `High`
  - `Medium`
  - `Low`
  - `Informational`
- current mitigation, if any
- recommended follow-up
- whether the issue should be enforced with additional tests

Separate confirmed issues from risk hypotheses.

### 3. Write the artifact

Create or update:

`docs/specs/<module>/<submodule>/<spec>.security-review.md`

Use `references/security-review-template.md`.

## Quality Bar

- Findings are tied to actual code, boundaries, or contracts.
- Severity reflects impact and confidence, not just possibility.
- Recommendations point to the correct next step: spec, implementation, or tests.
- The artifact is actionable enough for another engineer or agent to address directly.

## Related skills

- `plan-tests` - may need updates when security-related coverage is missing
- `implement-tdd` - can enforce unit-testable security-sensitive logic
- `implement-integration-tests` - can enforce boundary and contract security behavior
