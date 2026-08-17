# Eolarte.CqrsLite

`CqrsLite` is a lightweight CQRS dispatcher for .NET applications.

It provides:

- command and query contracts;
- dependency-injection registration and handler assembly scanning;
- explicit command and query dispatch through `IDispatcher`;
- composable pipeline behaviors.

The package is currently distributed through GitHub Packages.

## Usage

```csharp
services.AddCqrsLite(builder =>
{
    builder.ScanHandlers(typeof(ApplicationAssemblyMarker).Assembly);
});
```

Commands are dispatched with `IDispatcher.Send`; queries are dispatched with
`IDispatcher.Query`.
