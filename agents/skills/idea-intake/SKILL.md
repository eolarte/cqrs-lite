---
name: idea-intake
description: Capture a raw product idea through a short interview and create the initial docs/ideas/<idea-name>/ brief. Use when an early concept needs structure before research, refinement, or PRD work.
user-invocable: true
argument-hint: "[idea, pain point, or opportunity]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, web/fetch, 'microsoftdocs/mcp/*', 'context7/*', todo]
---

# Idea Intake

Turn a rough idea into a compact brief that can later become a PRD.

## Process

### 1. Read context first

Read relevant context:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. nearby `docs/specs/<module>/*`, if present
5. relevant `docs/guides/*`, if they constrain users or operations

Use `../idea-references/artifact-map.md` for canonical idea artifact names and sequence.

If `docs/ideas/` does not exist, create it.

### 2. Capture the idea

Ask for the idea in one sentence unless the user already provided it. If it is clear, restate it briefly and confirm.

Create `docs/ideas/<idea-name>/` using kebab-case.

### 3. Run a focused interview

Ask 3-6 concise questions, one at a time, only where answers change planning. Prioritize:

- Who is this for?
- What problem or opportunity matters?
- What would first-version success look like?
- What is explicitly out of scope?
- What constraints or non-negotiables apply?

### 4. Map to domain

- Align wording with `shared-glossary.md`
- Identify likely bounded context(s)
- Note cross-context dependencies or assumptions
- Mark unknowns as open questions, not decisions

### 5. Write initial idea docs

Always create:

`docs/ideas/<idea-name>/01-idea-brief.md`

Use `references/idea-brief-template.md`. Create `02-requirements-candidates.md` only when separate draft requirements add value.

Keep the brief compact, behavioral, and explicit about uncertainty. Keep initial open questions inside `01-idea-brief.md`; do not create a standalone open-questions file.
