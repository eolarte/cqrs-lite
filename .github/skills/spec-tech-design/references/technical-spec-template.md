# [Feature Name] Technical Specification

**Status:** Draft
**Source PRD:** `docs/specs/<module>/<submodule>/<spec>.md`
**Audience:** Developers, Tech Leads
**Date:** [YYYY-MM-DD]

## Overview

Summarize the feature, implementation scope, and intended engineering outcome.

## Solution Design

Describe the solution in implementation terms:

- modules and responsibilities
- interfaces and contracts
- data model or payload shape
- dependencies and external interactions
- failure modes and error handling
- assumptions and open questions

Add diagrams, tables, or code snippets only when they clarify the design.

## Design Patterns

Explain the patterns being applied or deliberately avoided, with rationale.

Examples:

- CQRS
- domain events
- adapter or port-and-adapter boundaries
- pipeline or middleware
- repository pattern
- idempotency
- retry or circuit-breaker behavior

## Testing Strategy

Describe how the solution should be tested:

- unit tests
- integration tests
- contract tests
- end-to-end tests
- boundary and failure-path coverage
- what to mock and what not to mock

Use a table when it improves coverage mapping.

## Security Considerations

Cover security-relevant concerns such as:

- authentication and authorization
- sensitive data handling
- input validation and trust boundaries
- secret handling
- auditability
- abuse and misuse paths

If impact is low, say so and explain why.

## Performance Considerations

Cover:

- latency-sensitive flows
- throughput expectations
- storage or query implications
- caching opportunities
- async processing, batching, or backpressure
- likely bottlenecks and how to detect them

## Logs

Specify what should be logged and why:

- lifecycle events
- warnings and recoverable failures
- error cases
- correlation fields
- fields that must not be logged

## Tracing

Describe tracing expectations:

- spans or operations to instrument
- trace boundaries across services or modules
- correlation with logs and metrics
- critical paths worth tracing

If distributed tracing is not relevant, name the lighter-weight observability alternative.

## References

Link to supporting material:

- source PRD
- related specs
- relevant guides
- context docs
- external standards or vendor docs used as input
