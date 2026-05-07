---
name: stabilize-bug-fix
description: Triage and remediate actionable bug reports from qa-manager artifacts, fix confirmed defects, and update tests or docs so each finding is resolved or explicitly deferred. Use inside stabilization-manager after qa-manager.
user-invocable: true
argument-hint: "[spec path, test plan path, issue number, or QA-reviewed slice to stabilize]"
---

# Stabilize Bug Fixes

Remediate confirmed bugs and implementation defects reported during QA. This skill owns bug triage and fixes during stabilization; it does not replace broader redesign or speculative cleanup.

## Inputs

Read:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. relevant files under `docs/specs/<module>/<submodule>/`:
   - `<spec>.md`
   - `<spec>.technical-spec.md`
   - `<spec>.architecture.md`
   - `<spec>.slices.md`, if present
   - `<spec>.test-plan.md`
   - `<spec>.edge-cases.md`
5. relevant source and test code for the affected behavior
6. any related performance or security QA artifacts if the bug overlaps those areas

## Process

### 1. Triage QA findings

Classify each relevant finding as one of:

- `Confirmed bug`
- `High-confidence implementation gap`
- `Missing test coverage`
- `Spec gap`
- `Inconclusive`

Fix only confirmed bugs and high-confidence implementation gaps here. Route spec gaps and inconclusive findings back to the relevant artifact instead of guessing.

### 2. Reproduce or capture the defect

When practical, reproduce the defect with an existing failing scenario or add the smallest targeted test that proves the reported behavior is wrong.

### 3. Fix one bug at a time

For each bug:

1. implement the smallest safe change that resolves the behavior
2. update unit, integration, or E2E tests only where they are the correct enforcement layer
3. keep the fix inside the approved slice unless the QA finding proves a wider contract bug

### 4. Mark remediated items completed in the QA artifact

Update the affected QA artifact or remediation notes so each handled finding is clearly:

- completed when the bug is remediated
- deferred with rationale
- redirected to spec or QA follow-up

## Quality Bar

- Every fix ties back to a concrete QA finding.
- Tests prove observable behavior, not internal implementation details.
- Stabilization does not expand into unrelated refactors.
- Deferrals are explicit and justified by scope, evidence, or artifact gaps.

## Final Response

Report:

1. bugs triaged
2. bugs fixed
3. tests added or updated
4. findings deferred or redirected
5. residual risk to carry into QA re-review

## Related skills

- `qa-edge-cases` - produces bug-oriented QA findings
- `implement-tdd` - useful when a bug fix requires new unit-testable logic coverage
- `implement-integration-tests` - useful when the defect is at a real boundary or contract
