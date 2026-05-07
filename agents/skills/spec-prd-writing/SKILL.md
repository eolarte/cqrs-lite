---
name: spec-prd-writing
description: Produce a feature PRD through a focused interview, grounded in the project's context map, glossary, and existing specs. Use when starting a feature, enhancement, or significant change.
user-invocable: true
argument-hint: "[feature or problem to specify]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, web/fetch, 'microsoftdocs/mcp/*', 'context7/*', todo]
---

# PRD Writing

Create a concise PRD for a feature or change. Ground it in the project domain, current docs, and codebase reality.

## Process

### 1. Discover project context

Read only the relevant context first:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. nearby `docs/specs/<module>/*`, if present
5. relevant `docs/guides/*`, if they affect user workflows or operations

### 2. Understand the request

If the request is unclear, ask 3-5 targeted questions. Prefer questions that clarify:

- user, problem, and desired outcome
- success criteria
- explicit non-goals
- constraints such as timeline, compatibility, migration, or technical limits

Skip the interview when the conversation already gives enough context.

### 3. Explore the codebase

Inspect relevant code to identify:

- current behavior and adjacent features
- existing patterns, naming, and contracts
- bounded contexts touched by the change
- interfaces to reuse, extend, or avoid

### 4. Identify modules and test boundaries

Sketch the major modules or domain concepts. For each, determine:

- bounded context ownership
- whether it is new or extends an existing concept
- cross-context contracts, if any
- test value and boundary coverage

Check with the user before locking scope when module boundaries, context ownership, or test expectations are uncertain.

### 5. Write the spec

Use `references/prd-template.md` as the skeleton. Adapt or remove sections so the result is specific, not boilerplate.

Requirements:

- use canonical terms from `docs/contexts/shared-glossary.md`
- describe domain concepts and contracts, not volatile file paths
- include edge cases, errors, and explicit non-goals
- save to `docs/specs/<module>/<submodule>/<spec>.md`
- if multiple contexts are involved, save under the primary context and document cross-context impact

### 6. Domain model updates

When the PRD introduces domain changes:

- update `docs/contexts/shared-glossary.md` for new or refined terms
- update relevant context docs with new invariants or contracts, if they exist
- propose an ADR only for decisions that are hard to reverse, surprising, and trade-off heavy

## Quality Bar

- The PRD is specific enough for `spec-tech-design` to generate implementation docs.
- Assumptions and open questions are explicit.
- User-facing scope and implementation-facing decisions are both present, but not mixed with code-level detail.
- Empty placeholders and irrelevant sections are removed before finishing.

## Related skills

- `spec-tech-design` - generate technical and architecture docs from the PRD
- `interview` - pressure-test ambiguous or risky requirements before finalizing
