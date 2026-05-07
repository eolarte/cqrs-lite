---
name: doc-explanation
description: Create or update a Diataxis explanation document under docs/guides/<module>/<submodule>/ for concepts, rationale, architecture, and trade-offs. Use when readers need understanding rather than step-by-step instructions.
user-invocable: true
argument-hint: "[concept or target docs/guides/<module>/<submodule>/ path]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, todo]
---

# Explanation Documentation

Create understanding-oriented documentation that helps readers build the right mental model.

Read `../doc-references/diataxis-guide.md` before writing.

## Process

### 1. Gather context

Read the relevant conceptual material:

1. `AGENTS.md`
2. `docs/contexts/shared-glossary.md`
3. `docs/contexts/context-map.md`, if boundaries matter
4. nearby guides and specs in the same module
5. relevant code paths for architectural reality and naming

### 2. Clarify before writing

You must determine:

1. target audience
2. concept to explain
3. reader's understanding goal
4. exact scope
5. exclusions
6. destination file under `docs/guides/<module>/<submodule>/`

Ask focused questions when the concept or audience is underspecified.

### 3. Propose the outline

Produce a detailed outline with short section notes and wait for approval.

Explanation outlines should usually include:

1. title
2. framing problem or question
3. core concepts
4. design rationale or trade-offs
5. examples or comparisons
6. implications for users or implementers

### 4. Write the explanation

After approval, generate the full Markdown document.

Requirements:

- keep the path under `docs/guides/<module>/<submodule>/`
- optimize for understanding, not procedure
- make trade-offs and rationale explicit
- use examples only to illuminate the concept
- avoid drifting into API reference or task recipe content

## Quality Bar

- The reader leaves with a clearer mental model.
- The document explains why, not just what.
- Claims align with the codebase and existing project docs.
