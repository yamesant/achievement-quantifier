using Moq;
using Spectre.Console.Cli;
using Spectre.Console.Testing;

namespace AQ.Tests;

public class CommandTestsBase
{
    protected readonly IRemainingArguments RemainingArguments = new Mock<IRemainingArguments>().Object;
    protected readonly CommandContext CommandContext;
    protected readonly TestConsole Console = new();

    protected CommandTestsBase()
    {
        CommandContext = new([], RemainingArguments, "", null);
    }
}