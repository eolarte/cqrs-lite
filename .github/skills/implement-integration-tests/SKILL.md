---
name: implement-integration-tests
description: Implement the integration-test layer of the test pyramid for an approved feature slice, with moderate coverage focused on contracts, boundaries, and cross-component behavior. Use after plan-tests and alongside implement-tdd when the spec and test plan identify behaviors that unit tests alone cannot prove.
user-invocable: true
argument-hint: "[spec path, test plan path, issue number, or integration behavior to cover]"
---

# Implement Integration Tests

Implement moderate-coverage integration tests for a feature slice. These tests should prove contracts and interactions across real components that unit tests do not fully validate.

## Preconditions

Prefer starting from:

1. `docs/specs/<module>/<submodule>/<spec>.test-plan.md`
2. `docs/specs/<module>/<submodule>/<spec>.md`
3. `docs/specs/<module>/<submodule>/<spec>.technical-spec.md`
4. `docs/specs/<module>/<submodule>/<spec>.architecture.md`
5. `docs/specs/<module>/<submodule>/<spec>.slices.md`, if present

If the test plan is missing or does not identify higher-layer behaviors, use `plan-tests` first.

## Scope

Integration tests should target:

- context boundary contracts
- API and handler interactions
- persistence or messaging behavior through real integrations or realistic test substitutes
- workflow behavior that crosses modules or layers
- serialization, mapping, and configuration paths that unit tests alone cannot prove

Keep coverage moderate. Do not try to exhaustively retest all unit-level branches here.

## Process

### 1. Gather context

Read:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. the relevant `docs/specs/<module>/<submodule>/` artifacts
5. existing integration tests, test utilities, fixtures, and environment setup

Identify how this repo runs targeted integration tests before editing code.

### 2. Select high-value integration behaviors

From the test plan, choose the behaviors that need integration coverage because they depend on real boundaries, wiring, data flow, persistence, message routing, or framework behavior.

For each selected behavior, state:

- the contract or interaction being proven
- which real components should participate
- which boundaries can still use fakes or controlled substitutes
- the expected observable result

### 3. Implement tests before or with the integration code path

For each behavior:

1. Write the integration test.
2. Confirm it fails for the expected reason.
3. Implement or adjust the minimum code and configuration needed.
4. Run the targeted integration test.
5. Re-run related unit tests when shared behavior changes.

Prefer real component composition. Fake only the boundaries that are outside the repo's control or too expensive to run in the test environment.

### 4. Keep the layer disciplined

Default expectation:

- unit tests remain the primary coverage for logic and branching
- integration tests prove contracts, wiring, persistence, and cross-component workflows
- integration tests are fewer than unit tests and more focused than E2E tests

If a test can be expressed more cheaply and clearly as a unit test, move it down a layer instead of bloating integration coverage.

### 5. Update the test plan when needed

If implementation changes the planned integration coverage, update:

- `docs/specs/<module>/<submodule>/<spec>.test-plan.md`
- any relevant spec artifact if a confirmed contract changed

## Checklist

```text
[ ] Each integration test proves a real contract or cross-component behavior
[ ] Real components are used where the contract matters
[ ] External systems are controlled with appropriate test substitutes
[ ] Tests fail for the expected reason before implementation
[ ] Unit tests still cover the lower-level logic
[ ] Targeted integration tests pass
[ ] Broader regression checks ran when warranted
```

## Anti-Patterns

- Rewriting unit tests as slower integration tests.
- Using full E2E flows to prove behavior that belongs at the integration layer.
- Mocking the repo's own internal modules so heavily that the integration contract is no longer real.
- Adding broad, brittle environment setup for behavior that could be proven with a smaller test.

## Final Response

Report:

1. integration behaviors covered
2. files changed
3. tests run and results
4. remaining gaps better suited for E2E or unit coverage

## Related skills

- `plan-tests` - identify which behaviors require integration coverage
- `implement-tdd` - cover unit-testable generated logic first
- `implement-e2e-tests` - cover a small set of end-to-end user-critical workflows
