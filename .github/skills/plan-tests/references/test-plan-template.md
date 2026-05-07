# [Feature or Slice] Test Plan

**Status:** Draft
**Date:** [YYYY-MM-DD]
**Related Spec:** `docs/specs/<module>/<submodule>/<spec>.md`
**Related Slice:** `[slice title or issue]`

## Test Goal

[What implementation confidence this test plan should provide.]

## Behaviors to Test

| Behavior | Approach | Public Interface | Expected Outcome |
| --- | --- | --- | --- |
| [A system can...] | Test-first / Test-after | [interface] | [observable result] |

## First RED Test

[The smallest behavior that proves the implementation path works.]

## Test Data and Setup

- [Fixture, builder, sample command/query/event, or setup requirement.]

## Mocking Boundaries

| Boundary | Mock Strategy | Reason |
| --- | --- | --- |
| [external API/time/file system/etc.] | [mock/fake/temp resource] | [why] |

## Do Not Test

- [Trivial wiring, implementation detail, or behavior already covered elsewhere.]

## Test Commands

- `[command to run targeted tests]`
- `[command to run broader validation]`

## Open Questions

- [Question that blocks a reliable test plan, or "None".]
