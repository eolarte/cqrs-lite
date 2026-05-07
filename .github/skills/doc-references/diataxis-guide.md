# Diataxis Writing Guide

Use this file to keep all `doc-` skills aligned.

## Universal Rules

- Write for one primary audience at a time.
- Optimize every section for the user's immediate goal.
- Prefer project terminology already defined in `docs/contexts/shared-glossary.md`.
- Read nearby docs before writing so tone and naming match the repo.
- Avoid copying sibling docs. Summarize briefly and point readers to the other document when overlap is intentional.
- Keep examples accurate to the current repo and codebase.
- Save outputs under `docs/guides/<module>/<submodule>/`.

## Document Type Boundaries

### Tutorial

- Learning-oriented.
- Assumes the reader is being guided, not troubleshooting.
- Uses sequential steps that build confidence through a successful outcome.
- Explains only what is needed to complete the lesson.

### How-to Guide

- Problem-oriented.
- Assumes the reader already knows the basics and wants to accomplish one task.
- Uses direct steps, prerequisites, and verification.
- Avoids long background sections.

### Reference

- Information-oriented.
- Organizes facts for quick lookup.
- Favors tables, signatures, option lists, constraints, defaults, and examples.
- Avoids narrative teaching and opinionated persuasion.

### Explanation

- Understanding-oriented.
- Clarifies concepts, trade-offs, rationale, architecture, and mental models.
- Uses examples to illuminate ideas, not to prescribe a task flow.
- Avoids step-by-step procedural instruction unless a tiny example is needed for context.

## Mandatory Workflow

Every `doc-` skill must follow this sequence:

1. Clarify the document type, target audience, user goal, scope, exclusions, and intended file path.
2. Propose a detailed outline with short section descriptions and wait for approval.
3. Generate full Markdown only after the outline is approved.

## Context Checklist

Read only what is relevant:

1. `AGENTS.md`
2. `docs/contexts/shared-glossary.md`
3. `docs/contexts/context-map.md`, if it affects terminology or boundaries
4. nearby `docs/guides/<module>/<submodule>/`, if present
5. nearby `docs/specs/<module>/<submodule>/`, if present
6. the relevant code paths when examples, APIs, or workflows must match implementation reality
