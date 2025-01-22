using AQ.Console.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Spectre.Console.Cli;

namespace AQ.Tests.Commands;

public class ShowStatusTests : CommandTestsBase
{
    [Fact]
    public void TypicalCase()
    {
        // Arrange
        IHostEnvironment hostEnvironment = new HostingEnvironment()
        {
            EnvironmentName = "Test",
        };
        ShowStatus command = new(hostEnvironment, Console);

        // Act
        int result = command.Execute(CommandContext, new EmptyCommandSettings());

        // Assert
        Assert.Equal(0, result);
        Assert.Equal("The runtime environment is 'Test'\n", Console.Output);
    }
}