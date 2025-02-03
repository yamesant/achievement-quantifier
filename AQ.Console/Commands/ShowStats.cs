using AQ.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class ShowStats(
    IAnsiConsole console,
    IReportingService reportingService
) : AsyncCommand<EmptyCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, EmptyCommandSettings settings)
    {
        SummaryStatisticsSnapshot snapshot = await reportingService.GetSummaryStatisticsSnapshot();
        console.WriteLine(snapshot.TodayCompletionCount.ToString());
        console.WriteLine(snapshot.YesterdayCompletionCount.ToString());
        console.WriteLine(snapshot.PastSevenDaysCompletionCount.ToString());
        return 0;
    }
}