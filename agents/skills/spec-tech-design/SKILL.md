---
name: spec-tech-design
description: Generate a developer-focused technical spec and an architect-focused architecture document from an existing PRD in docs/specs/.
user-invocable: true
argument-hint: "[path to PRD under docs/specs/<module>/<submodule>/<spec>.md]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, web/fetch, 'microsoftdocs/mcp/*', 'context7/*', todo]
---

# Technical Design from PRD

Create two implementation-ready documents from a PRD:

1. a **Technical Specification** for developers
2. an **Architecture Document** for architects and senior engineers

These documents should complement each other. Do not restate the same content in two voices.

## Input

The argument must be a PRD path in this format:

`docs/specs/<module>/<submodule>/<spec>.md`

If the argument is missing, ambiguous, or outside `docs/specs/`, stop and ask for the exact PRD path.

## Outputs

Given:

`docs/specs/<module>/<submodule>/<spec>.md`

Create or update these files alongside the PRD:

1. `docs/specs/<module>/<submodule>/<spec>.technical-spec.md`
2. `docs/specs/<module>/<submodule>/<spec>.architecture.md`

If either file already exists, update it carefully and preserve intentional content.

## Process

### 1. Read the PRD and nearby context

Always read:

1. the target PRD
2. `AGENTS.md`
3. `docs/contexts/context-map.md`, if present
4. `docs/contexts/shared-glossary.md`, if present
5. nearby specs in the same module or submodule, if present
6. relevant guides under `docs/guides/`, when they constrain implementation or operations

If the PRD references other docs, APIs, workflows, or constraints, read those too before writing.

### 2. Ground the design in the repo

Explore the codebase enough to align with existing architecture, naming, contracts, and operational patterns.

Extract:

- actors and responsibilities
- workflows and state transitions
- boundaries, dependencies, and integration points
- data contracts, persistence, and external interactions
- operational concerns: security, observability, scale, performance
- assumptions, ambiguities, and open questions

Do not invent unsupported SLAs, security guarantees, data models, or platform constraints.

### 3. Resolve gaps carefully

When the PRD is incomplete:

- ask the user targeted questions when an open issue materially affects scope, interfaces, data design, security, operations, or trade-offs
- infer only the safest reasonable defaults from the repo and docs
- state assumptions explicitly
- if a point still cannot be resolved, keep it as an open question

### 4. Split the two documents cleanly

Use the reference templates in `references/` and keep the documents distinct:

- **Technical Specification:** implementation shape, contracts, error handling, testing, logs, and tracing
- **Architecture Document:** system boundaries, major flows, decisions, trade-offs, scale, security posture, and observability

If a topic belongs primarily in one document, summarize it briefly in the other and link back instead of duplicating it.

### 5. Use visuals only when they help

You may use:

- Mermaid diagrams
- ASCII diagrams
- tables
- syntax-highlighted code blocks

Rules:

- keep visuals minimal and accurate
- add a short lead-in sentence for each visual
- prefer tables for responsibilities, contracts, decisions, risks, or metrics
- prefer Mermaid only when the structure is clearer graphically than in prose

### 6. Write with high signal

- prefer stable responsibilities and contracts over fragile file-path guidance
- tie major choices back to the PRD, repo patterns, or surrounding docs
- be explicit about trade-offs, assumptions, and uncertainty
- remove boilerplate and placeholders before finishing

## Reference templates

Use these files as the writing skeletons:

1. `references/technical-spec-template.md`
2. `references/architecture-template.md`

Adapt them to the feature. Do not leave irrelevant sections as empty boilerplate.

## Quality bar

Both outputs must be:

- specific to the input PRD
- aligned with the repo and existing docs
- useful for implementation and review
- concise, concrete, and honest about uncertainty

Before finishing, confirm both files exist in the expected `docs/specs/...` location.

## Related skills

- `spec-prd-writing` - create the PRD first if it does not exist yet
- `interview` - pressure-test risky or ambiguous decisions before locking the design
