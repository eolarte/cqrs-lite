---
name: doc-tutorial
description: Create or update a Diataxis tutorial under docs/guides/<module>/<submodule>/ for newcomers learning by doing. Use when the reader needs a guided lesson that leads to one successful outcome.
user-invocable: true
argument-hint: "[topic or target docs/guides/<module>/<submodule>/ path]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, todo]
---

# Tutorial Documentation

Create a learning-oriented tutorial that guides a reader to a concrete successful outcome.

Read `../doc-references/diataxis-guide.md` before writing.

## Process

### 1. Gather context

Read the relevant project context first:

1. `AGENTS.md`
2. `docs/contexts/shared-glossary.md`
3. `docs/contexts/context-map.md`, if relevant
4. nearby docs in `docs/guides/<module>/<submodule>/`, if any
5. nearby specs in `docs/specs/<module>/<submodule>/`, if any
6. relevant code paths when commands, snippets, or workflows must match the repo

### 2. Clarify before writing

You must determine:

1. target audience
2. learner's goal
3. exact scope
4. what is explicitly out of scope
5. destination file under `docs/guides/<module>/<submodule>/`

If any of these are missing, ask targeted questions. Do not draft the full tutorial yet.

### 3. Propose the outline

Produce a detailed outline with brief notes for each section and wait for approval.

Tutorial outlines should usually include:

1. title
2. what the reader will build, learn, or achieve
3. prerequisites
4. step-by-step lesson flow
5. verification of success
6. short next steps

### 4. Write the tutorial

After approval, generate the full Markdown document.

Requirements:

- keep the path under `docs/guides/<module>/<submodule>/`
- assume the reader is learning, not debugging
- use short, sequential steps
- explain only the concepts needed for the lesson
- keep examples minimal but correct
- end with a clear successful outcome and next step

## Quality Bar

- A newcomer can complete the lesson without guessing missing steps.
- The tutorial has one primary learning objective.
- Explanations stay lightweight and do not turn the document into a conceptual essay or reference dump.
