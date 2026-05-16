# AGENTS.md

## Project basics
- `cqrs-lite` is a lightweight CQRS library for C#.
- The main library lives in `src/CqrsLite` and is built from `src/CqrsLite/CqrsLite.csproj`.
- The solution file is `CqrsLite.sln` and includes the main library plus test and handler-fixture projects under `test/`.
- Core implementation files are `Abstractions.cs`, `Dispatcher.cs`, `CqrsLiteBuilder.cs`, `CqrsLiteServiceCollectionExtensions.cs`, `HandlerScanner.cs`, and `Exceptions.cs`.
- The project targets `.NET 10` with nullable reference types and implicit usings enabled.

## Dev environment tips
- Restore and build from the repo root with `dotnet restore CqrsLite.sln` and `dotnet build CqrsLite.sln`.
- Run the full test suite with `dotnet test CqrsLite.sln`.
- For focused work, run `dotnet test test/CqrsLite.Tests/CqrsLite.Tests.csproj --filter "<pattern>"`.
- Dependency injection registration starts with `services.AddCqrsLite(...)`.
- Handlers can be registered explicitly with `AddCommandHandler(...)` / `AddQueryHandler(...)` or discovered with `ScanHandlers(...)`.
- Shared agent assets are canonical under `.agents`. Use `python3 .agents/skills/sync-agent-config/scripts/sync_agent_configs.py --check|--sync|--validate` instead of hand-editing mirrored tool folders.

## Testing instructions
- There is no repository CI workflow under `.github/workflows` yet, so use the solution-level `dotnet` commands locally.
- Tests live in `test/CqrsLite.Tests` and use xUnit.
- Keep coverage around explicit registration, assembly scanning, pipeline behaviors, cancellation propagation, and deterministic failure paths.
- When behavior changes, update or add tests in `test/CqrsLite.Tests` and the handler-fixture projects when needed.

## PR instructions
- Keep changes scoped and update tests with behavior changes.
- Before committing, run `dotnet build CqrsLite.sln` and `dotnet test CqrsLite.sln`.
- If you change public APIs or developer workflow, update `README.md`, `AGENTS.md`, or `.copilot/copilot-instructions.md` to match.
