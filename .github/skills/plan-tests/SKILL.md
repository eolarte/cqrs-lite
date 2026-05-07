---
name: plan-tests
description: Create a behavior-focused test plan for an approved implementation slice. Use after plan-slice and before implement-tdd to decide what to test, what not to test, and which tests should drive implementation.
user-invocable: true
argument-hint: "[slice plan path, issue number, or behavior to test]"
---

# Test Planning

Create a pragmatic test plan for a feature slice. This skill plans tests only; it does not implement code.

## Philosophy

Good tests verify observable behavior through public interfaces. They use domain language, survive refactors, and mock only system boundaries.

Bad tests verify private methods, internal state, or collaborator calls. If a test breaks when behavior is unchanged, it is probably testing implementation detail.

## Process

### 1. Gather context

Read the relevant inputs:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. relevant files under `docs/specs/<module>/<submodule>/`:
   - `<spec>.md`
   - `<spec>.technical-spec.md`
   - `<spec>.architecture.md`
   - `<spec>.slices.md`, if present

If the user passes a GitHub issue number, fetch it with `gh issue view <number>`.

### 2. Inspect test infrastructure

Find:

- existing test files and naming conventions
- test runner and commands
- fixtures, builders, helpers, and assertions already in use
- public interfaces that tests should exercise

Prefer the repo's existing test style over introducing a new pattern.

### 3. Identify behaviors

List behaviors in this form:

`A [actor/system] can [action] resulting in [observable outcome].`

Classify each behavior:

- **Unit test-first:** core logic, domain rules, validation, mapping, calculations, and other unit-testable behavior in the generated code
- **Test-first at a higher layer:** context boundary contracts and non-trivial APIs
- **Test-after:** UI, glue code, or orchestration with complex state
- **Skip:** trivial wiring with little behavioral risk

### 4. Define boundaries

For each behavior, decide:

- public interface to test through
- data setup needed
- expected observable outcome
- whether the behavior must be covered by a unit test under the test pyramid
- system boundaries to mock, such as external APIs, file system, time, or randomness
- internals that must not be mocked

Never mock this repo's own modules to make a unit test easier. If a behavior is hard to test through a public interface, call out the design risk.

### 5. Write the test plan

Save the plan beside the source spec or slice plan:

`docs/specs/<module>/<submodule>/<spec>.test-plan.md`

Use `references/test-plan-template.md`. If only one slice is being planned inside a larger test plan, update the relevant section instead of duplicating the file.

### 6. Confirm readiness

Finish with one status:

- `Ready for implement-tdd`
- `Needs slice clarification`
- `Needs spec clarification`

If not ready, list up to 3 blockers.

## Quality Bar

- Behaviors are observable and named in domain language.
- Unit-testable generated logic is explicitly marked for unit-test coverage.
- Behaviors that require integration coverage are explicitly marked for moderate integration testing.
- Only a few high-value workflows are marked for E2E coverage.
- The plan states what to test and what not to test.
- Mocking is limited to system boundaries.
- The first RED test is identified.
- The plan is specific enough for `implement-tdd` to execute without re-planning.

## Related skills

- `plan-slice` - break specs into implementation slices
- `implement-tdd` - execute the approved slice using the test plan and TDD loop
- `implement-integration-tests` - cover cross-component and contract behaviors with moderate integration coverage
- `implement-e2e-tests` - cover a few high-value end-to-end workflows
- `qa-edge-cases` - review missing or weakly covered boundary conditions after implementation
- `qa-performance-validation` - validate performance evidence against documented criteria
- `qa-security-review` - review the implemented solution for security risks and missing safeguards
- `spec-interview` - resolve ambiguous or risky behavior before planning tests
