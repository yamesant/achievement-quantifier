using AQ.Domain;

namespace AQ.Services;

public interface IReportingService
{
    Task<SummaryStatisticsSnapshot> GetSummaryStatisticsSnapshot();
}

public class ReportingService(
    DataContext context,
    TimeProvider timeProvider
    ) : IReportingService
{
    public async Task<SummaryStatisticsSnapshot> GetSummaryStatisticsSnapshot()
    {
        SummaryStatisticsSnapshot result = new(1, 2, 3);
        return await Task.FromResult(result);
    }
}

public sealed record SummaryStatisticsSnapshot(
    int TodayCompletionCount,
    int YesterdayCompletionCount,
    int PastSevenDaysCompletionCount
);