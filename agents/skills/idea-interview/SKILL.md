---
name: idea-interview
description: Clarify open questions for an existing idea and write a refinement document in docs/ideas/<idea-name>/. Use when uncertainty blocks research, presentation, or PRD handoff.
user-invocable: true
argument-hint: "[idea-name or key open question to refine]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, web/fetch, 'microsoftdocs/mcp/*', 'context7/*', todo]
---

# Idea Interview

Clarify unresolved decisions about an idea and convert uncertainty into refined, testable draft requirements.

## Process

### 1. Read the current idea state

Target folder: `docs/ideas/<idea-name>/`. Use `../idea-references/artifact-map.md` to find existing idea artifacts.

If `01-idea-brief.md` is missing, ask for a short summary first.

If brainstorm results exist, summarize them first and ask which ideas, options, or concerns should carry into the interview.

### 2. Select priority questions

Build a shortlist of 3-7 questions that block refinement. Prioritize:

- Scope and boundaries
- Domain invariants and business rules
- User-visible behavior and edge cases
- Risky assumptions
- Integration or context contracts

### 3. Interview in dependency order

Ask one question at a time. Resolve prerequisites first and stop when enough is known.

For each question:

1. Say why it matters
2. Offer 2-3 concrete options when useful
3. Recommend one option briefly
4. Confirm the choice

### 4. Convert answers into refinements

Convert answers into:

- Refined requirement statements (observable behavior)
- Updated assumptions (kept/removed)
- Explicitly deferred items
- New or narrowed open questions

Separate confirmed decisions from tentative ones.

### 5. Write outputs

Always create:

`docs/ideas/<idea-name>/07-clarification-interview.md`

When appropriate, also update:

- `docs/ideas/<idea-name>/02-requirements-candidates.md`
- `docs/ideas/<idea-name>/01-idea-brief.md` if early open questions or assumptions need to be reconciled

Use `references/clarification-interview-template.md`.

Keep questions high-signal, decisions explicit, and requirements behavioral.
