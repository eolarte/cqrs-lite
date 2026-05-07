---
name: qa-performance-validation
description: Validate whether the implemented solution meets documented performance criteria using repo-supported metrics, logs, or test outputs, then write a persistent performance validation artifact beside the spec. Use after integration and E2E execution when available.
user-invocable: true
argument-hint: "[spec path, test plan path, issue number, or implemented slice to validate]"
---

# QA Performance Validation

Validate whether the implemented solution meets the performance criteria defined by the specs. This skill reviews evidence and writes findings; it does not invent thresholds.

## Inputs

Read:

1. `AGENTS.md`
2. `docs/contexts/context-map.md`
3. `docs/contexts/shared-glossary.md`
4. relevant files under `docs/specs/<module>/<submodule>/`:
   - `<spec>.md`
   - `<spec>.technical-spec.md`
   - `<spec>.architecture.md`
   - `<spec>.test-plan.md`
5. available integration or E2E outputs, logs, or repo-supported performance metrics
6. relevant implementation and test code where performance-sensitive paths are described

## Focus Areas

Validate:

- latency or throughput expectations, if documented
- resource-sensitive paths
- hot paths called out in the technical or architecture docs
- whether current automated tests generate enough evidence to judge the performance claim

## Process

### 1. Find explicit criteria

Use only criteria that already exist in the specs or related artifacts.

If no performance criteria exist, treat that as a spec gap and mark the result `Inconclusive`.

### 2. Review available evidence

For each criterion, capture:

- the measured signal or proxy
- where it came from
- whether it is sufficient evidence
- pass, fail, or inconclusive status

### 3. Write the artifact

Create or update:

`docs/specs/<module>/<submodule>/<spec>.performance-validation.md`

Use `references/performance-validation-template.md`.

## Quality Bar

- No invented thresholds, SLAs, or targets.
- Missing data is distinguished from failing data.
- Findings clearly state whether current integration and E2E coverage produce enough performance evidence.
- Follow-ups are actionable and tied back to the right artifact or phase.

## Related skills

- `plan-tests` - defines intended higher-layer validation
- `implement-integration-tests` - may generate useful performance evidence
- `implement-e2e-tests` - may generate high-value workflow timing signals
