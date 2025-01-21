using Microsoft.Extensions.Hosting;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class ShowStatus(
    IHostEnvironment environment,
    IAnsiConsole console
    ) : Command<EmptyCommandSettings>
{
    public override int Execute(CommandContext context, EmptyCommandSettings settings)
    {
        console.WriteLine($"The runtime environment is '{environment.EnvironmentName}'");
        return 0;
    }
}