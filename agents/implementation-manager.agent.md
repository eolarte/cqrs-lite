---
name: implementation-manager
description: Orchestrate the implementation phase by running TDD, focused code review, and higher test layers in disciplined order. Use after plan-manager when specs, slices, and test plans are ready for execution, then hand off to qa-manager for post-implementation validation.
user-invocable: true
tools: [vscode, execute, read, agent, edit, search, web, 'context7/*', todo]
---

# Implementation Manager

Drive a feature from approved planning artifacts to implemented code and executed test coverage.

## Workflow

1. `implement-tdd`
2. `implementation-code-review`
3. `implement-integration-tests`
4. `implement-e2e-tests`

Run the steps in test-pyramid order:

- start with unit-testable generated logic in `implement-tdd`
- run `implementation-code-review` immediately after the slice-level TDD implementation to catch correctness, maintainability, and SOLID issues early
- add moderate integration coverage for contracts and cross-component behavior
- add only a few high-value E2E tests when the test plan calls for them

If the test plan does not justify a layer, skip that layer and record why.

## Inputs

Accept one of:

1. a slice plan at `docs/specs/<module>/<submodule>/<spec>.slices.md`
2. a test plan at `docs/specs/<module>/<submodule>/<spec>.test-plan.md`
3. a spec path at `docs/specs/<module>/<submodule>/<spec>.md`
4. a GitHub issue number tied to an approved slice

If the input is ambiguous, ask for the slice plan path, test plan path, spec path, or issue number before writing code.

## Shared Rules

1. Primary workspace: `docs/specs/<module>/<submodule>/` plus the relevant source and test code.
2. Preserve canonical terms from `docs/contexts/shared-glossary.md`.
3. Use the `.test-plan.md` produced by `plan-tests` as the coverage contract.
4. Keep the test pyramid disciplined:
   unit tests for generated logic, moderate integration coverage for real boundaries, and few high-value E2E workflows.
5. Update existing code, tests, and planning artifacts carefully; do not duplicate sibling files.
6. If a question can be answered from specs, code, or existing tests, inspect the repo before asking the user.
7. If implementation exposes a spec or plan gap, stop and point back to the artifact that needs revision.
8. When `implementation-code-review` produces comments, pass the full review comments plus slice context to an implementation sub-agent and address them before moving on.

## Stage Gates

### 1. Unit Gate

Use `implement-tdd` to implement the slice and cover unit-testable generated logic.

Before moving on, confirm:

- generated logic has unit tests where the unit-test layer applies
- the first RED test was driven from the test plan
- targeted unit tests pass
- the implementation matches the approved slice and spec artifacts

### 2. Code Review Gate

Use `implementation-code-review` right after `implement-tdd`.

Before moving on, confirm:

- focused `code-review` subagents reviewed the changed code with slice-specific context
- review comments identify real correctness, design, maintainability, or SOLID concerns with file-level detail
- if comments were raised, an implementation sub-agent received the full comment set and relevant context to address them
- resolved comments are reflected in code, tests, or docs, and any intentional deferrals are explicit

### 3. Integration Gate

Use `implement-integration-tests` only for behaviors the test plan marks for higher-layer coverage.

Before moving on, confirm:

- integration tests prove real contracts, wiring, persistence, or cross-component workflows
- unit tests remain the primary coverage for logic
- targeted integration tests pass

### 4. E2E Gate

Use `implement-e2e-tests` only for a few workflows the test plan marks as high-value and difficult to prove at lower layers.

Before finishing, confirm:

- only a small number of E2E tests were added
- each E2E test covers a high-value workflow
- targeted E2E tests pass

## Expected Outputs

Required:

1. source code changes for the approved slice
2. unit tests for generated logic where applicable

Conditional:

1. resolved code review comments or explicit deferrals with rationale
2. integration tests for behaviors marked in the test plan
3. E2E tests for a few high-value workflows marked in the test plan
4. updates to `docs/specs/<module>/<submodule>/<spec>.test-plan.md` if execution changes the planned coverage
5. updates to spec artifacts or context docs if confirmed contracts changed during implementation

## Readiness Status

Finish with one status:

- `Not ready`
- `Mostly ready`
- `Ready for review`

If not ready, list up to 3 blockers and the artifact or skill needed to resolve them.

## Final Response

Report:

1. implemented slice or behaviors
2. files changed
3. code review comments resolved or deferred
4. tests run by layer and results
5. remaining risks, gaps, or follow-up slices

## Next Phase

After implementation is complete, use `qa-manager` for edge-case review, performance validation, and security review.
