using AQ.Domain;
using AQ.Services;
using Microsoft.Extensions.Time.Testing;

namespace AQ.Tests.Services;

public class ReportingServiceTests : DbTestsBase
{
    private readonly FakeTimeProvider _timeProvider;
    private readonly DataContext _dataContext;
    private readonly IReportingService _reportingService;
    private readonly AchievementClass _achievementClass;

    public ReportingServiceTests()
    {
        _timeProvider = new();
        _dataContext = CreateDataContext();
        _reportingService = new ReportingService(_dataContext, _timeProvider);
        IFixture fixture = new Fixture().Customize(new DefaultCustomization());
        _achievementClass = fixture.Create<AchievementClass>();
        _dataContext.AchievementClasses.Add(_achievementClass);
        _dataContext.SaveChanges();
    }

    [Fact]
    public async Task ShouldGetCorrectStatistics()
    {
        // Arrange
        for (int i = 0; i < 7; i++)
        {
            _timeProvider.Advance(TimeSpan.FromDays(1));
            Achievement achievement = new()
            {
                CompletedDate = DateOnly.FromDateTime(_timeProvider.GetUtcNow().Date),
                Quantity = 1 << i,
                AchievementClass = _achievementClass,
            };
            _dataContext.Achievements.Add(achievement);
            await _dataContext.SaveChangesAsync();
        }

        SummaryStatisticsSnapshot expected = new(64, 32, 127);
        
        // Act
        SummaryStatisticsSnapshot result = await _reportingService.GetSummaryStatisticsSnapshot();

        // Assert
        Assert.Equal(expected, result);
    }
}