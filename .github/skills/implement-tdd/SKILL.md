---
name: implement-tdd
description: Implement an approved feature slice using pragmatic TDD, with explicit unit-test coverage for the generated code where the unit-test layer of the test pyramid applies. Use after plan-slice and plan-tests when a slice has clear acceptance criteria and a behavior-focused test plan.
user-invocable: true
argument-hint: "[slice plan path, test plan path, issue number, or behavior to implement]"
---

# Implement With TDD

Implement one approved feature slice with a tracer-bullet TDD loop: one test, one implementation step, repeat. This skill must cover the unit-test layer of the test pyramid for the generated code whenever the slice introduces or changes unit-testable logic.

## Preconditions

Prefer starting from:

1. `docs/specs/<module>/<submodule>/<spec>.slices.md`
2. `docs/specs/<module>/<submodule>/<spec>.test-plan.md`
3. the source PRD, technical spec, and architecture document

If the slice or test plan is missing, use `plan-slice` or `plan-tests` first unless the user explicitly asks to proceed from conversation context.

## Process

### 0. Apply clean code and SOLID by default

Every code change produced by this skill should optimize for maintainability, readability, and minimal design. While implementing each TDD cycle, apply these defaults:

- prefer clear, intention-revealing names for types, functions, variables, and tests
- keep functions and methods small, focused, and centered on one job
- keep each class, module, or component to a single clear responsibility
- prefer simple designs over clever or speculative ones
- remove meaningful duplication once the behavior is green
- keep related behavior together and avoid unnecessary coupling between modules
- use abstractions only when they improve clarity, reuse, or extension in a concrete way
- add brief comments when they materially improve understanding of non-obvious code, while still preferring self-explanatory names and structure
- write comments sparingly and only when they explain why, an invariant, or a non-obvious tradeoff
- make failures explicit with clear validation and error handling
- preserve testability by keeping side effects at boundaries and core logic easy to exercise through stable public interfaces

Apply SOLID where it materially improves the slice without overengineering:

- Single Responsibility Principle: each unit should have one reason to change
- Open/Closed Principle: extend behavior through existing seams when practical instead of rewriting stable logic
- Liskov Substitution Principle: derived or interchangeable implementations must preserve expected behavior
- Interface Segregation Principle: prefer small focused contracts over broad ones that force unused members
- Dependency Inversion Principle: high-level policy should depend on abstractions at system boundaries, not concrete infrastructure details

Do not force patterns for their own sake. Prefer the smallest correct design that keeps the code readable, testable, and easy to change.

### 1. Gather implementation context

Read:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. relevant `docs/specs/<module>/<submodule>/` artifacts
5. existing code and tests around the public interfaces being changed

Identify the test command before editing code.

### 2. Start with unit tests when the logic is unit-testable

Start with the first RED test from the test plan. For domain logic, pure transformations, validation, mapping, orchestration with clear inputs and outputs, and other unit-testable behavior, that first RED test should be a unit test.

```text
RED: write one behavior test and confirm it fails for the expected reason
GREEN: write the smallest implementation that passes
VERIFY: run the targeted test and relevant existing tests
```

Do not add edge cases, abstractions, or extra behavior before the tracer bullet is green.

If the slice includes both unit-testable logic and broader integration behavior, cover the unit-testable logic first at the unit-test layer, then add higher-level tests only where they add confidence that unit tests cannot provide alone.

### 3. Iterate behavior by behavior

For each remaining planned behavior:

1. Write one focused test.
2. Confirm it fails for the expected reason.
3. Implement the minimum code needed.
4. Run targeted tests.
5. Run broader validation when the change touches shared contracts or behavior.

Keep tests focused on public interfaces and observable outcomes. For unit tests, exercise the smallest stable public interface available for the generated code. Mock only system boundaries.

### 4. Apply the test pyramid deliberately

Default expectation:

- unit tests cover the generated code that contains logic, branching, validation, mapping, calculations, or decision-making
- higher-level tests cover integration points, contracts, and end-to-end behavior that unit tests alone cannot prove
- trivial wiring does not need forced unit-test coverage unless the repo already treats that area as high-risk

If the generated code is unit-testable and no unit test is added, the skill should treat that as a gap and explain why.

### 5. Refactor on green

After related tests pass, refactor only while green:

- remove meaningful duplication
- improve names so intent is obvious without extra comments
- break apart code that has more than one responsibility when the slice exposes that tension
- simplify public interfaces where the spec allows it
- reduce coupling and tighten cohesion where the change made dependencies awkward
- replace broad or leaky contracts with smaller focused ones when the tests support it
- keep implementation details behind stable contracts
- preserve existing repo patterns

Run tests after each refactor step.

### 6. Update docs when behavior changes

If implementation changes a confirmed contract, domain term, invariant, or context boundary, update the relevant docs:

- `docs/contexts/shared-glossary.md`
- `docs/contexts/context-map.md`
- source spec, technical spec, architecture doc, slice plan, or test plan

Call out any doc update that should be reviewed before merging.

## Checklist Per Cycle

```text
[ ] Test name describes behavior in domain language
[ ] Test uses a public interface
[ ] Generated logic is covered by unit tests where the unit-test layer applies
[ ] Test fails for the expected reason before implementation
[ ] Code is minimal for the behavior under test
[ ] Names are intention-revealing and the code is readable without explanatory comments
[ ] Brief comments were added where they meaningfully improve understanding of non-obvious logic
[ ] Each changed function/class has a single clear responsibility
[ ] New abstractions or interfaces are justified by an actual need in the slice
[ ] Dependencies stay at system boundaries and coupling did not increase unnecessarily
[ ] Error handling and invalid states are explicit
[ ] Mocks are limited to system boundaries
[ ] Targeted tests pass
[ ] Broader validation ran when warranted
```

## Anti-Patterns

- Writing all tests before any implementation.
- Skipping unit tests for new or changed logic that is clearly unit-testable.
- Testing private methods or internal collaborator calls.
- Mocking this repo's own modules instead of testing through public behavior.
- Adding speculative abstractions before a test demands them.
- Hiding poor naming or confusing logic behind comments instead of simplifying the code.
- Creating large interfaces, god classes, or multi-purpose methods that violate single responsibility.
- Introducing inheritance or indirection that does not preserve behavior or improve clarity.
- Depending directly on low-level infrastructure inside high-level domain or application logic when a boundary abstraction is already warranted.
- Leaving tests for later after implementation is already done.

## Final Response

Report:

1. implemented slice or behavior
2. files changed
3. tests run and results, including unit tests added for the generated code
4. remaining risks or follow-up slices

## Related skills

- `plan-slice` - define implementation slices
- `plan-tests` - create the behavior-focused test plan
- `spec-interview` - resolve risky design questions before implementation
