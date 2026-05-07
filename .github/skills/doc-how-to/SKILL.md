---
name: doc-how-to
description: Create or update a Diataxis how-to guide under docs/guides/<module>/<submodule>/ for readers solving a specific problem. Use when the reader already knows the basics and needs a practical recipe.
user-invocable: true
argument-hint: "[task or target docs/guides/<module>/<submodule>/ path]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, todo]
---

# How-To Documentation

Create a problem-oriented how-to guide for a reader who wants to accomplish a specific task.

Read `../doc-references/diataxis-guide.md` before writing.

## Process

### 1. Gather context

Read only the relevant project context:

1. `AGENTS.md`
2. `docs/contexts/shared-glossary.md`
3. nearby guides in `docs/guides/<module>/<submodule>/`
4. nearby specs in `docs/specs/<module>/<submodule>/`
5. relevant code or config files that define the actual task flow

### 2. Clarify before writing

You must determine:

1. target audience
2. problem to solve
3. desired end state
4. exact scope
5. exclusions
6. destination file under `docs/guides/<module>/<submodule>/`

If the task is ambiguous, ask focused questions first.

### 3. Propose the outline

Produce a detailed outline with short section notes and wait for approval.

How-to outlines should usually include:

1. title
2. when to use this guide
3. prerequisites
4. task steps
5. verification
6. common failure points or rollback notes, when relevant

### 4. Write the how-to guide

After approval, generate the full Markdown document.

Requirements:

- keep the path under `docs/guides/<module>/<submodule>/`
- optimize for action, not teaching
- keep background short
- prefer direct commands, checks, and expected outcomes
- include warnings only when they materially reduce risk
- keep the document tightly scoped to one job

## Quality Bar

- A reader can solve the stated problem quickly.
- The guide does not wander into conceptual explanation unless needed for a single decision.
- Steps, verification, and constraints match the actual project state.
