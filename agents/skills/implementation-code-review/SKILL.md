---
name: implementation-code-review
description: Review a freshly implemented slice by dispatching focused code-review subagents, collecting actionable comments, and routing those comments back through an implementation subagent until they are resolved or explicitly deferred. Use inside implementation-manager after implement-tdd.
user-invocable: true
argument-hint: "[slice plan path, test plan path, issue number, or implemented slice to review]"
---

# Implementation Code Review

Review a newly implemented slice before moving on to higher test layers. This skill does not replace QA. It performs an implementation-time review pass focused on correctness, maintainability, clean code, and adherence to SOLID design principles, then ensures comments are addressed while the slice context is still fresh.

## Preconditions

Prefer starting from:

1. `docs/specs/<module>/<submodule>/<spec>.slices.md`
2. `docs/specs/<module>/<submodule>/<spec>.test-plan.md`
3. the source PRD, technical spec, and architecture document
4. the current code and tests changed by `implement-tdd`

If `implement-tdd` has not completed for the slice, run that first.

## Process

### 1. Gather review context

Read:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. relevant `docs/specs/<module>/<submodule>/` artifacts
5. the changed source files and tests for the slice

Build a compact review packet that includes:

- the slice goal and acceptance criteria
- the changed files
- the intended public behaviors
- the tests already added or updated
- any known trade-offs or planned follow-up work

### 2. Dispatch focused code-review subagents

Use `code-review` subagents, not one broad generic pass. Split the work into focused review threads based on the actual changes. Good splits include:

- domain logic, invariants, and correctness
- public contracts, boundaries, and dependency direction
- readability, naming, comments, and clean-code maintainability
- maintainability, cohesion, coupling, and SOLID concerns
- test quality and coverage alignment with the slice

For each subagent, provide:

1. the review packet
2. only the relevant files or diff scope for that thread
3. explicit focus areas
4. instructions to report only meaningful comments with file-level detail

Reviewers should validate these implementation expectations when they are relevant to the slice:

- names are intention-revealing and the code is understandable without relying on dense comments
- brief comments exist where they materially improve understanding of non-obvious logic, invariants, or trade-offs
- functions and methods stay small and focused on one job
- classes, modules, and components have a single clear responsibility
- the design stays simple and avoids speculative abstraction
- duplication has been removed where it is meaningful and safe to do so
- cohesion is high and coupling is not increased without a clear reason
- abstractions and interfaces are introduced only when they improve clarity, extension, or boundary control
- high-level logic does not depend directly on low-level infrastructure when an abstraction is warranted at the boundary
- interface shape is appropriately focused and does not force unused behavior on consumers
- interchangeable implementations preserve expected behavior
- failures, validation, and invalid states are explicit and understandable
- tests exercise public behavior and cover the generated logic at the unit-test layer where appropriate

### 3. Consolidate comments

Treat each substantive issue from a review subagent as a review comment. A useful comment should include:

- the concrete problem
- why it matters
- the affected files or code area
- whether the likely fix is code, test, or doc work

Ignore purely stylistic noise. Prioritize correctness, hidden coupling, leaky abstractions, poor naming that obscures intent, missing explanatory comments for non-obvious logic, unnecessary responsibilities, dependency inversion problems, weak interface boundaries, and code that drifts from the approved slice.

### 4. Route comments back to implementation

If any comments exist, dispatch an implementation subagent with:

1. the full list of review comments
2. the original slice and test-plan context
3. the affected files
4. any reviewer rationale needed to preserve intent

The implementation subagent should address every comment directly in code, tests, or docs, or mark a comment as intentionally deferred with a reason tied to the spec or slice boundary.

### 5. Re-review changed areas

After comments are addressed, re-run focused `code-review` subagents only for the areas changed in response to review.

Repeat until:

- no material comments remain, or
- only explicit, justified deferrals remain

## Quality Bar

- Review context is specific enough that subagents can evaluate the slice, not guess at it.
- Comments are actionable and traceable to real code.
- Comment resolution stays inside the approved slice unless a documented spec gap requires escalation.
- Clean code and SOLID concerns are raised only when they create real readability, maintainability, or design risk, not as abstract dogma.
- The review explicitly checks naming clarity, responsibility boundaries, coupling, abstraction quality, and comment quality for non-obvious code.
- Brief comments are encouraged only where they improve comprehension; comments should not be used to excuse confusing structure or poor naming.

## Final Response

Report:

1. review threads dispatched
2. comments found
3. comments resolved versus deferred
4. files changed while addressing review
5. remaining risks that should carry into integration or QA

## Related skills

- `implement-tdd` - implement the slice first
- `implement-integration-tests` - cover real boundaries after the review gate
- `implement-e2e-tests` - add a few high-value end-to-end checks when justified
- `qa-edge-cases` - perform post-implementation QA analysis
