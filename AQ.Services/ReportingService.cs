using AQ.Domain;
using Microsoft.EntityFrameworkCore;

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
        DateOnly today = DateOnly.FromDateTime(timeProvider.GetUtcNow().Date);
        DateOnly yesterday = today.AddDays(-1);
        DateOnly reportingDateStart = today.AddDays(-6);
        Dictionary<DateOnly, int> counts = await context.Achievements
            .Where(a => a.CompletedDate >= reportingDateStart)
            .GroupBy(a => a.CompletedDate)
            .Select(group => new { Date = group.Key, Sum = group.Sum(a => a.Quantity) })
            .ToDictionaryAsync(group => group.Date, group => group.Sum);
        SummaryStatisticsSnapshot result = new(
            counts.GetValueOrDefault(today, 0), 
            counts.GetValueOrDefault(yesterday, 0), 
            counts.Values.Sum());
        return result;
    }
}

public sealed record SummaryStatisticsSnapshot(
    int TodayCompletionCount,
    int YesterdayCompletionCount,
    int PastSevenDaysCompletionCount
);