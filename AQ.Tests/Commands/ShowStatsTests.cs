using AQ.Console.Commands;
using AQ.Services;
using Moq;
using Spectre.Console.Cli;

namespace AQ.Tests.Commands;

public class ShowStatsTests : CommandTestsBase
{
    private readonly Mock<IReportingService> _reportingServiceStub = new();
    [Fact]
    public async Task TypicalCase()
    {
        // Arrange
        SummaryStatisticsSnapshot snapshot = new(5, 1, 30);
        _reportingServiceStub
            .Setup(x => x.GetSummaryStatisticsSnapshot())
            .ReturnsAsync(snapshot);
        ShowStats command = new(Console, _reportingServiceStub.Object);
        string expectedOutput = """
                                ┌────────────────────────────────────┬───────┐
                                │ Statistic                          │ Value │
                                ├────────────────────────────────────┼───────┤
                                │ Achievements Completed Today       │ 5     │
                                │ Achievements Completed Yesterday   │ 1     │
                                │ Achievements Completed Past 7 Days │ 30    │
                                └────────────────────────────────────┴───────┘

                                """;

        // Act
        int result = await command.ExecuteAsync(CommandContext, new EmptyCommandSettings());

        // Assert
        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, Console.Output);
    }
}