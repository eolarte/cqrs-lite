using System.Reflection;
using CqrsLite;
using CqrsLite.AmbiguousHandlers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CqrsLite.Tests;

public sealed class HandlerScannerEdgeCaseTest
{
    [Fact]
    public void ScanningAnAssemblyWithAHandlerImplementingBothICommandHandlerAndICommandHandlerUnitThrowsInvalidHandlerRegistrationException()
    {
        var services = new ServiceCollection();
        
        // AmbiguousCommandHandler implements both ICommandHandler<AmbiguousCommand> (the void-result
        // form) and ICommandHandler<AmbiguousCommand, Unit> (the explicit Unit-result form).
        // Because ICommand : ICommand<Unit>, these map to two handler interfaces for the same
        // message contract — an invalid handler shape that the scanner must reject.
        var exception = Assert.Throws<InvalidHandlerRegistrationException>(() =>
            services.AddCqrsLite(builder => builder.ScanHandlers(typeof(AmbiguousCommandHandler).Assembly)));
        
        Assert.Contains("exactly one", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
