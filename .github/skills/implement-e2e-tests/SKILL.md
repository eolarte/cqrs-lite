---
name: implement-e2e-tests
description: Implement the E2E layer of the test pyramid for an approved feature slice, with few high-value tests focused on user-critical workflows. Use after plan-tests and after lower test layers already cover the core logic and integration contracts.
user-invocable: true
argument-hint: "[spec path, test plan path, issue number, or end-to-end workflow to cover]"
---

# Implement E2E Tests

Implement a small number of high-value E2E tests for critical user or system workflows. These tests should verify end-to-end confidence, not replace unit or integration coverage.

## Preconditions

Prefer starting from:

1. `docs/specs/<module>/<submodule>/<spec>.test-plan.md`
2. `docs/specs/<module>/<submodule>/<spec>.md`
3. `docs/specs/<module>/<submodule>/<spec>.technical-spec.md`
4. `docs/specs/<module>/<submodule>/<spec>.architecture.md`
5. completed unit and integration coverage for the same slice, where applicable

If the test plan does not identify a high-value workflow worth end-to-end coverage, do not force an E2E test.

## Scope

E2E tests should target only a few workflows that are expensive to break and difficult to prove at lower layers, such as:

- a critical user journey from entry point to final observable outcome
- a system workflow that crosses multiple major boundaries
- a release-critical path with configuration, infrastructure, and runtime concerns combined

Keep E2E coverage sparse and deliberate.

## Process

### 1. Gather context

Read:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. the relevant `docs/specs/<module>/<submodule>/` artifacts
5. the `.test-plan.md` produced by `plan-tests`
6. existing E2E tests, environment setup, and test commands

Identify how this repo runs targeted E2E tests before editing code.

### 2. Select only the highest-value workflows

Choose a few workflows from the test plan that justify E2E cost.

For each workflow, define:

- starting state
- trigger or user/system action
- critical checkpoints
- final observable outcome
- the reason this workflow must be proven end to end instead of only at lower layers

### 3. Implement the E2E tests

For each chosen workflow:

1. Write the E2E test.
2. Confirm it fails for the expected reason.
3. Implement or adjust the minimum missing behavior, configuration, or wiring needed.
4. Run the targeted E2E test.
5. Re-run affected lower-layer tests when shared behavior changes.

Prefer stable selectors, stable assertions, and stable setup over brittle timing-driven checks.

### 4. Keep the layer sparse

Default expectation:

- unit tests cover most logic
- integration tests cover moderate contract and workflow confidence
- E2E tests are few, slow, and reserved for the highest-value workflows

If a scenario can be proven well with integration tests, do not automatically duplicate it at the E2E layer.

### 5. Update the test plan when needed

If implementation changes the planned E2E coverage, update:

- `docs/specs/<module>/<submodule>/<spec>.test-plan.md`
- any relevant spec artifact if a confirmed workflow or contract changed

## Checklist

```text
[ ] Each E2E test covers a high-value workflow
[ ] The workflow is difficult to prove sufficiently at lower layers
[ ] Setup and assertions are stable and not timing-fragile
[ ] Tests fail for the expected reason before implementation
[ ] Lower-layer coverage already handles most logic
[ ] Targeted E2E tests pass
[ ] Broader regression checks ran when warranted
```

## Anti-Patterns

- Using E2E tests to compensate for missing unit or integration tests.
- Covering too many workflows at the E2E layer.
- Writing brittle tests that depend on arbitrary sleeps or unstable selectors.
- Retesting every internal branch through the full stack.

## Final Response

Report:

1. E2E workflows covered
2. files changed
3. tests run and results
4. remaining risks or workflows intentionally left to lower layers

## Related skills

- `plan-tests` - identify which workflows deserve E2E coverage
- `implement-tdd` - cover unit-testable generated logic first
- `implement-integration-tests` - cover moderate contract and cross-component confidence
