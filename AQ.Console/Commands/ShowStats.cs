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
        Table table = new()
        {
            Border = TableBorder.Square
        };
        table.AddColumn("Statistic");
        table.AddColumn("Value");
        table.AddRow("Achievements Completed Today", snapshot.TodayCompletionCount.ToString());
        table.AddRow("Achievements Completed Yesterday", snapshot.YesterdayCompletionCount.ToString());
        table.AddRow("Achievements Completed Past 7 Days", snapshot.PastSevenDaysCompletionCount.ToString());
        console.Write(table);
        return 0;
    }
}