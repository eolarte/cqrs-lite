---
name: qa-edge-cases
description: Analyze implemented behavior for boundary conditions, unusual state combinations, negative paths, and missing coverage, then write a persistent edge-case review beside the spec. Use after implementation-manager.
user-invocable: true
argument-hint: "[spec path, test plan path, issue number, or implemented slice to review]"
---

# QA Edge Cases

Review the implemented solution for missing or weakly covered edge cases. This skill analyzes coverage and writes findings; it does not implement tests.

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
5. existing unit, integration, and E2E tests for the implemented slice
6. the relevant source code for the implemented behavior

## Focus Areas

Look for:

- boundary values and limits
- empty, null, missing, or malformed inputs
- ordering and sequencing problems
- duplicate, retry, timeout, or partial-failure behavior
- invalid state transitions
- multi-step workflows with uncommon branches
- fallback and degradation paths

## Process

### 1. Compare spec, plan, and implementation

For each important behavior, compare:

- what the spec says should happen
- what the test plan says is covered
- what the current tests actually prove
- what the implementation likely does at the edges

### 2. Identify coverage status

For each edge case, mark one:

- `Covered`
- `Partially covered`
- `Missing`

Also recommend the correct downstream owner:

- unit test
- integration test
- E2E test
- spec clarification
- implementation fix

### 3. Write the artifact

Create or update:

`docs/specs/<module>/<submodule>/<spec>.edge-cases.md`

Use `references/edge-cases-template.md`.

## Quality Bar

- Every finding maps to a real behavior, contract, or workflow.
- Recommendations point to the right test-pyramid layer.
- Confirmed gaps are separated from speculative concerns.
- The artifact is actionable enough for another engineer or agent to address directly.

## Related skills

- `plan-tests` - defines the intended coverage contract
- `implement-tdd` - covers unit-testable generated logic
- `implement-integration-tests` - covers cross-component and contract behavior
- `implement-e2e-tests` - covers a few high-value end-to-end workflows
