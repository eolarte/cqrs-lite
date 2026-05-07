---
name: idea-research
description: Research internal prior art and external options for an existing idea, then write findings in docs/ideas/<idea-name>/. Use when an idea needs evidence before refinement or PRD handoff.
user-invocable: true
argument-hint: "[idea-name or idea topic to investigate]"
tools: [vscode/vscodeAPI, vscode/extensions, vscode/askQuestions, execute, read, agent, edit, search, web/fetch, 'microsoftdocs/mcp/*', 'context7/*', todo]
---

# Idea Research

Investigate what already exists and document evidence-backed findings.

## Process

### 1. Read the current idea state

Target folder: `docs/ideas/<idea-name>/`. Use `../idea-references/artifact-map.md` to find existing idea artifacts.

If `01-idea-brief.md` does not exist, ask the user for a short summary before proceeding.

### 2. Frame the research objective

Restate the idea and define 3-7 viability questions, such as:

- Does this capability already exist internally?
- Is there an existing module or pattern we can reuse?
- Are there known domain constraints or contracts this conflicts with?
- Are there mature external tools/services that solve part of this already?
- What are the key trade-offs (build vs buy vs adapt)?

### 3. Research internal sources first

Search docs and code for prior art:

1. `docs/specs/*` for similar feature decisions
2. `docs/guides/*` for operational conventions
3. `docs/contexts/context-map.md` for context boundaries
4. `docs/contexts/shared-glossary.md` for canonical terms
5. Existing modules, interfaces, or tests related to the idea

Capture concrete evidence, not guesses.

### 4. Research external options with MCP tools

Use external research only when internal evidence is not enough.

Use MCP tools as the default source of truth:

- Context7 for libraries/frameworks/SDKs/APIs:
  1. `mcp_context7_resolve-library-id`
  2. `mcp_context7_query-docs`
  3. Capture version-specific constraints or caveats
- Microsoft Learn MCP for Microsoft/Azure topics:
  1. `mcp_microsoftdocs_microsoft_docs_search`
  2. `mcp_microsoftdocs_microsoft_code_sample_search` when examples help
  3. `mcp_microsoftdocs_microsoft_docs_fetch` for high-value pages

Prefer official docs. Record source title plus version/date context when relevant.

### 5. Evaluate and compare

For each viable option, including reuse of internal capability, evaluate:

- Fit to the idea goals
- Complexity and integration cost
- Risks and constraints
- Impact on bounded contexts and contracts
- Short-term path vs long-term maintainability

### 6. Write outputs

Always create:

`docs/ideas/<idea-name>/04-research.md`

Optionally create:

- `docs/ideas/<idea-name>/05-option-comparison.md`
- `docs/ideas/<idea-name>/06-recommendation.md`

Use `references/research-template.md`.

Keep conclusions actionable, distinguish evidence from opinion, and make unknowns explicit.
