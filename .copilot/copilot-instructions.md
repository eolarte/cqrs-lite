# Copilot instructions for cqrs-lite

## Project overview
- `cqrs-lite` is a small `.NET 10` CQRS library built around `Microsoft.Extensions.DependencyInjection`.
- Public contracts are defined in `src/CqrsLite/Abstractions.cs`.
- Request dispatching is implemented in `src/CqrsLite/Dispatcher.cs`.
- DI setup is centered on `src/CqrsLite/CqrsLiteServiceCollectionExtensions.cs` and `src/CqrsLite/CqrsLiteBuilder.cs`.
- Handler discovery and validation live in `src/CqrsLite/HandlerScanner.cs`.
- Test coverage is concentrated in `test/CqrsLite.Tests`, with fixture assemblies in `test/CqrsLite.TestHandlers`, `test/CqrsLite.InvalidHandlers`, and `test/CqrsLite.InvalidShapeHandlers`.

## Coding conventions
- Follow the existing C# style: file-scoped namespaces, `sealed` or `static` types where appropriate, and nullable-aware code.
- Use `ArgumentNullException.ThrowIfNull(...)` for public guard clauses, matching the current API surface.
- Keep the library lightweight and DI-friendly; prefer extending existing builder and dispatcher patterns over introducing new abstractions.
- Preserve deterministic runtime behavior for missing handlers, duplicate handlers, invalid scanned handlers, and pipeline execution order.
- Match the current async style in library internals, including `ConfigureAwait(false)` where the code already uses it.

## Testing expectations
- Use xUnit in `test/CqrsLite.Tests`.
- Prefer scenario-style test names that describe the setup and expected outcome, matching the existing suite.
- When modifying dispatch, scanning, or pipeline behavior, add or update tests that cover both success paths and explicit failure paths.
- Use `dotnet test CqrsLite.sln` for full validation and `dotnet test test/CqrsLite.Tests/CqrsLite.Tests.csproj --filter "<pattern>"` for targeted runs.

## Working guidance
- Start from the solution root and use solution-level `dotnet` commands unless you have a focused project-level reason not to.
- Treat `src/CqrsLite` as the production package and the `test/` projects as behavioral coverage and scanner fixtures.
- If you change public behavior, keep docs in sync with the code.
