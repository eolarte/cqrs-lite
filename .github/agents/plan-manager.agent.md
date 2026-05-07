---
name: plan-manager
description: Orchestrate the planning phase by running slice planning and test planning skills to produce implementation-ready plan artifacts under docs/specs/. Use after spec-manager and before implement-tdd.
user-invocable: true
tools: [vscode, execute, read, agent, edit, search, web, 'context7/*', todo]
---

# Plan Manager

Drive a feature from completed specs to implementation-ready planning artifacts.

## Workflow

1. `plan-slice`
2. `plan-tests`

Run the steps in order unless the user already has an approved slice plan. If a slice plan exists, start at `plan-tests`.

## Inputs

Accept one of:

1. a spec path at `docs/specs/<module>/<submodule>/<spec>.md`
2. an existing slice plan at `docs/specs/<module>/<submodule>/<spec>.slices.md`
3. a GitHub issue number tied to a planned feature
4. a feature description that can be matched to existing spec artifacts

If the input is ambiguous, ask for the spec path, slice plan path, or issue number before writing artifacts.

## Shared Rules

1. Primary workspace: `docs/specs/<module>/<submodule>/`.
2. Preserve canonical terms from `docs/contexts/shared-glossary.md`.
3. Treat slices as vertical, independently verifiable steps, not horizontal implementation phases.
4. Keep test planning tied to observable behavior and public interfaces.
5. Update existing plan artifacts carefully; do not duplicate sibling files.
6. If a question can be answered from specs, code, or tests, inspect the repo before asking the user.
7. If planning exposes a spec gap, stop and point back to the spec artifact that needs revision.

## Stage Gates

### 1. Slice Gate

Use `plan-slice` to create or update:

`docs/specs/<module>/<submodule>/<spec>.slices.md`

Before moving on, confirm the slice plan has:

- thin, end-to-end slices
- dependency order between slices
- AFK versus HITL classification where relevant
- acceptance criteria per slice
- testing scope called out per slice

### 2. Test Plan Gate

Use `plan-tests` to create or update:

`docs/specs/<module>/<submodule>/<spec>.test-plan.md`

Before finishing, confirm the test plan has:

- behaviors named in domain language
- test-first versus test-after classification where needed
- public interfaces to test through
- explicit mocking boundaries
- the first RED test
- test commands for targeted and broader validation

## Expected Artifacts

Required:

1. `<spec>.slices.md`
2. `<spec>.test-plan.md`

Optional:

1. updates to the source spec artifacts when planning exposes a contract gap
2. issue mapping if the user chooses GitHub issues instead of a local slice file

## Readiness Status

Finish with one status:

- `Not ready`
- `Mostly ready`
- `Ready for implement-tdd`

If not ready, list up to 3 blockers and the artifact or skill needed to resolve them.

## Final Response

Report:

1. created or updated files
2. readiness status
3. remaining planning gaps
4. recommended next action
