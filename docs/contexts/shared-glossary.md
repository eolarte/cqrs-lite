# Ubiquitous Language

The `shared-glossary.md` file contains the canonical definitions of terms used across the project. It serves as the single source of truth for terminology, ensuring that all team members and documentation use consistent language.

When writing specs, guides, or any documentation, refer to `shared-glossary.md` to ensure that you are using the established terms correctly. If you need to introduce new terms or refine existing ones, update this file accordingly to keep it up-to-date.

## Terms

**Command** — A write-side operation that changes application state. Returns void or a minimal result (e.g., generated ID). Implemented as `ICommand` or `ICommand<TResult>`.

**Query** — A read-side operation that returns data without changing state. Implemented as `IQuery<TResult>`.

**Event** — A record that something has happened; used for fan-out notification to zero or more listeners. Implemented as `IEvent`.

**Handler** — The single function responsible for processing one Command or Query. Multiple handlers may respond to a single Event.

**Pipeline Behavior** — Cross-cutting middleware applied before and/or after handler execution (e.g., logging, validation, transactions). Implemented as `IPipelineBehavior<TMessage, TResult>`.

**Dispatcher** — The runtime component that routes a message to its registered handler(s). Also called Mediator or Sender.

**CQRS** — Command Query Responsibility Segregation. Architectural pattern separating the write model (Commands) from the read model (Queries).

**Contracts Package** — A thin NuGet package (`CqrsLite.Abstractions`) containing only message marker interfaces and handler contracts, without the runtime implementation.

