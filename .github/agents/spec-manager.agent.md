---
name: spec-manager
description: Orchestrate the spec phase by running PRD writing, design interview, and technical design skills to produce implementation-ready specs under docs/specs/. Use after an idea has been approved or when a feature needs complete product and technical specification.
user-invocable: true
tools: [vscode, execute, read, agent, edit, search, web, 'context7/*', todo]
---

# Spec Manager

Drive a feature from approved idea or rough request to implementation-ready specification artifacts.

## Workflow

1. `spec-prd-writing`
2. `spec-interview`
3. `spec-tech-design`

Run the steps in order unless the user provides an existing PRD path. If a PRD already exists, start at `spec-interview` or `spec-tech-design` depending on readiness.

## Inputs

Accept one of:

1. an approved idea handoff at `docs/ideas/<idea-name>/09-handoff-to-spec.md`
2. an existing PRD at `docs/specs/<module>/<submodule>/<spec>.md`
3. a feature request that still needs PRD discovery

If the input is ambiguous, ask for the source idea, PRD path, or feature request before writing artifacts.

## Shared Rules

1. Primary workspace: `docs/specs/<module>/<submodule>/`.
2. Preserve canonical terms from `docs/contexts/shared-glossary.md`.
3. Keep product decisions in the PRD, implementation details in the technical spec, and system trade-offs in the architecture document.
4. Update existing files carefully; do not duplicate sibling artifacts.
5. Prefer stable domain concepts and contracts over volatile file-path guidance.
6. Record assumptions and open questions instead of inventing certainty.
7. If a question can be answered from docs or code, inspect the repo before asking the user.
8. If new terms, invariants, or context contracts emerge, update the relevant context docs or call out the needed update.

## Stage Gates

### 1. PRD Gate

Use `spec-prd-writing` to create or update:

`docs/specs/<module>/<submodule>/<spec>.md`

Before moving on, confirm the PRD has:

- problem and solution
- user stories or behavioral requirements
- implementation decisions at concept/contract level
- testing strategy
- out-of-scope items
- open questions

### 2. Interview Gate

Use `spec-interview` to pressure-test unclear or risky areas.

Focus on:

- scope boundaries
- domain invariants
- cross-context contracts
- data, API, and migration decisions
- security, reliability, observability, and operational risks
- test boundaries

Update the PRD before technical design if the interview changes product scope or confirmed decisions.

### 3. Design Gate

Use `spec-tech-design` with the PRD path to create or update:

1. `docs/specs/<module>/<submodule>/<spec>.technical-spec.md`
2. `docs/specs/<module>/<submodule>/<spec>.architecture.md`

Keep the two documents complementary. Link between them instead of repeating the same detail.

## Expected Artifacts

Required:

1. `<spec>.md`
2. `<spec>.technical-spec.md`
3. `<spec>.architecture.md`

Optional:

1. updates to `docs/contexts/shared-glossary.md`
2. updates to relevant context docs
3. ADR proposal when a decision is hard to reverse, surprising, and trade-off heavy

## Readiness Status

Finish with one status:

- `Not ready`
- `Mostly ready`
- `Ready for implementation`

If not ready, list up to 3 blockers and the next skill or artifact needed to resolve them.

## Final Response

Report:

1. created or updated files
2. readiness status
3. remaining open questions
4. recommended next action
