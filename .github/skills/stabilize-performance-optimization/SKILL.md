---
name: stabilize-performance-optimization
description: Remediate confirmed performance issues from qa-manager artifacts with targeted optimizations, preserving behavior while improving the evidence available for QA re-validation. Use inside stabilization-manager after qa-manager.
user-invocable: true
argument-hint: "[spec path, test plan path, issue number, or QA-reviewed slice to optimize]"
---

# Stabilize Performance Optimization

Remediate performance problems identified during QA. This skill focuses on confirmed failures, bottlenecks, or weak evidence called out by QA; it does not invent new targets or perform speculative micro-optimization.

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
   - `<spec>.performance-validation.md`
5. available logs, test outputs, or repo-supported metrics used by QA
6. relevant source and test code for performance-sensitive paths

## Process

### 1. Triage performance findings

For each relevant QA finding, classify it as:

- `Confirmed fail`
- `High-confidence bottleneck`
- `Insufficient evidence`
- `Spec gap`

Only optimize confirmed failures and high-confidence bottlenecks here. Insufficient evidence and spec gaps should be reflected back to the artifacts instead of guessed at.

### 2. Identify the narrowest effective change

Target the actual hotspot or expensive path described in QA evidence. Prefer changes such as:

- removing unnecessary work
- reducing repeated I/O or allocations
- tightening data flow or batching
- improving algorithmic hotspots

Do not trade away correctness, readability, or contract guarantees for minor speculative gains.

### 3. Preserve proof

Where the repo already has supporting tests, logs, or metrics, update or rerun the relevant evidence-producing checks so QA can re-validate the claim.

### 4. Mark remediated items completed in the QA artifact

Update the remediation outcome so each performance finding is clearly:

- completed when the performance issue is remediated
- still failing
- inconclusive due to missing criteria or evidence

## Quality Bar

- No invented SLAs, thresholds, or benchmarks.
- Optimizations are tied to real evidence or a clearly identified hotspot.
- Behavior stays consistent with the approved spec and tests.
- Follow-up work distinguishes between code remediation and missing observability.

## Final Response

Report:

1. performance findings triaged
2. optimizations implemented
3. evidence updated or rerun
4. findings still failing or inconclusive
5. residual performance risks for QA re-review

## Related skills

- `qa-performance-validation` - produces the performance findings and evidence gaps
- `implement-integration-tests` - may provide contract-level evidence for performance-sensitive boundaries
- `implement-e2e-tests` - may provide workflow-level evidence for user-visible latency
