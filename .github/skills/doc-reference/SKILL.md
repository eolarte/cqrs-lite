---
name: doc-reference
description: Create or update a Diataxis reference document under docs/guides/<module>/<submodule>/ for APIs, commands, configuration, contracts, or technical facts. Use when readers need authoritative lookup documentation rather than a guided workflow.
user-invocable: true
argument-hint: "[subject or target docs/guides/<module>/<submodule>/ path]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, todo]
---

# Reference Documentation

Create information-oriented reference documentation for fast lookup.

Read `../doc-references/diataxis-guide.md` before writing.

## Process

### 1. Gather context

Read the sources that define the facts:

1. `AGENTS.md`
2. `docs/contexts/shared-glossary.md`
3. nearby guides and specs in the same module, if any
4. relevant code, contracts, config schemas, commands, or examples

Prefer primary repo sources over inference.

### 2. Clarify before writing

You must determine:

1. target audience
2. subject being documented
3. lookup tasks the reader needs to perform
4. exact scope and exclusions
5. destination file under `docs/guides/<module>/<submodule>/`

If the surface area is too broad, narrow it before writing.

### 3. Propose the outline

Produce a detailed outline with section notes and wait for approval.

Reference outlines should usually include:

1. title
2. scope note
3. object, API, command, or option inventory
4. fields, parameters, defaults, and constraints
5. examples
6. related docs

### 4. Write the reference

After approval, generate the full Markdown document.

Requirements:

- keep the path under `docs/guides/<module>/<submodule>/`
- organize for scanning and lookup
- prefer tables, short sections, and clear headings
- distinguish required, optional, defaults, and failure conditions
- avoid turning the document into a tutorial or rationale essay

## Quality Bar

- Facts are explicit, current, and easy to find.
- The document separates normative behavior from examples.
- Readers can answer technical lookup questions without reading linearly.
