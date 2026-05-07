---
name: idea-presentation
description: Present a refined idea for user signoff and prepare the spec-prd-writing handoff when approved. Use after intake, research, interview, or brainstorm artifacts exist.
user-invocable: true
argument-hint: "[idea-name or final idea to present]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, web/fetch, 'microsoftdocs/mcp/*', 'context7/*', todo]
---

# Idea Presentation

Present the refined idea as a concise, decision-oriented team pitch.

## Process

### 1. Read the final idea state

Target folder: `docs/ideas/<idea-name>/`. Use `../idea-references/artifact-map.md` to find existing idea artifacts.

If the idea is not yet coherent enough to pitch, stop and state what is still missing.

### 2. Build the presentation narrative

Synthesize the current state into a concise pitch that answers:

- What problem or opportunity matters here?
- What idea is being proposed?
- Why this approach over the alternatives?
- What makes the team confident now?
- What risks still need active tracking?
- What exact approval is being requested from the user?

Keep it grounded in prior artifacts. Do not invent certainty that earlier work does not support.

Favor crisp headings, short bullets, comparison framing, concrete examples, tables, and ASCII-only diagrams when they clarify the decision.

### 3. Present like an enthusiastic team

Tone:

- enthusiastic but credible
- excited about the value, not theatrical
- concise, sharp, and decision-oriented
- honest about trade-offs and risks

Use brief team-perspective callouts only when helpful:

- architecture/system shape
- delivery and sequencing confidence
- backend/data/security implications
- QA/reliability concerns

Do not turn it into long roleplay. The pitch should help the user approve, adjust, or redirect the idea.

### 4. Validate signoff

After presenting, ask whether the user wants to:

1. approve the idea and continue to `spec-prd-writing`
2. approve with changes to capture first
3. send the idea back for more refinement

If the user does not approve, record requested changes or blockers and point to the artifact that should be updated.

### 5. Write outputs

Always create:

`docs/ideas/<idea-name>/08-idea-presentation.md`

When the user signs off, also create:

`docs/ideas/<idea-name>/09-handoff-to-spec.md`

Use `references/presentation-template.md`.

For `09-handoff-to-spec.md`, include only:

1. confirmed scope
2. key decisions
3. risks to track
4. open questions, if any
5. suggested next command: `spec-prd-writing`

Use `references/handoff-to-spec-template.md` when creating the handoff.
