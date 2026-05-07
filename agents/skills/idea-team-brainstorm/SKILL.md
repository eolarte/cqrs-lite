---
name: idea-team-brainstorm
description: Facilitate a multi-perspective brainstorming session for an existing idea and write structured brainstorm artifacts under docs/ideas/<idea-name>/brainstorm/.
user-invocable: true
argument-hint: "[idea-name or problem to brainstorm]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, todo]
---

# Idea Team Brainstorm

Generate, challenge, and refine options for an idea through distinct team perspectives.

## Process

### 1. Read the idea

Target folder: `docs/ideas/<idea-name>/`. Use `../idea-references/artifact-map.md` to find existing idea artifacts.

Read `01-idea-brief.md` first. If it is missing, ask the user for a short problem summary before brainstorming.

### 2. Run the brainstorm

Use `references/team-brainstorm-guide.md` for roles, phases, and output shape.

Rules:

- Keep the voices distinct.
- Let the team debate, combine, and challenge weak concepts.
- Make Phase 1 expansive and Phase 2 critical.
- Do not let roleplay overwhelm useful product thinking.

### 3. Write outputs

Create `docs/ideas/<idea-name>/brainstorm/` and write the files listed in the guide.

The final summary must identify the strongest concepts, major disagreements, risks, and recommended next step.
