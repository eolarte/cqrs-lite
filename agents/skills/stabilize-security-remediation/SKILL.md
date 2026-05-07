---
name: stabilize-security-remediation
description: Fix confirmed vulnerabilities and high-confidence security issues from qa-manager artifacts, add enforcement tests where practical, and prepare the slice for security re-validation. Use inside stabilization-manager after qa-manager.
user-invocable: true
argument-hint: "[spec path, test plan path, issue number, or QA-reviewed slice to secure]"
---

# Stabilize Security Remediation

Remediate confirmed vulnerabilities and high-confidence security issues reported during QA. This skill applies fixes at the correct boundary, adds tests where they can enforce the security behavior, and keeps residual risk explicit.

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
   - `<spec>.security-review.md`
5. relevant source and test code
6. existing validation, authorization, persistence, configuration, and boundary patterns in the repo

## Process

### 1. Triage security findings

For each relevant finding, identify:

- severity
- affected trust boundary or control point
- whether the issue is confirmed, high-confidence, or speculative

Fix confirmed and high-confidence issues here. Route speculative concerns or spec gaps back to the QA artifact instead of treating them as confirmed vulnerabilities.

### 2. Apply the fix at the right boundary

Prefer remediation at the strongest control point available, such as:

- input validation and sanitization boundaries
- authorization or policy enforcement points
- secret or configuration handling paths
- logging, error, or response shaping boundaries

Avoid weak partial mitigations when a stronger boundary fix is possible.

### 3. Add enforceable tests

Where practical, add or update the smallest tests that prove the vulnerability is closed at the correct layer:

- unit tests for validation and decision logic
- integration tests for trust boundaries and contracts
- E2E tests only when lower layers cannot prove the control

### 4. Mark remediated items completed in the QA artifact

Update the security remediation outcome so each finding is clearly:

- completed when the security issue is remediated
- deferred with rationale
- still open and blocking

## Quality Bar

- Fixes are tied to actual findings and applied at the right control point.
- Security posture is not weakened by silent fallbacks or partial handling.
- Tests enforce the remediated behavior where the repo supports it.
- Deferrals clearly state the residual risk and why stabilization did not resolve it.

## Final Response

Report:

1. security findings triaged
2. vulnerabilities or issues fixed
3. tests added or updated
4. findings deferred or still open
5. residual risk to carry into QA re-review

## Related skills

- `qa-security-review` - produces the security findings for remediation
- `implement-tdd` - useful for unit-testable security-sensitive logic
- `implement-integration-tests` - useful for testing boundary and contract enforcement
