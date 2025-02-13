using AQ.Domain;
using AQ.Services;
using Microsoft.Extensions.Time.Testing;

namespace AQ.Tests.Services;

public class ReportingServiceTests : DbTestsBase
{
    private readonly FakeTimeProvider _timeProvider;
    private readonly DataContext _dataContext;
    private readonly IReportingService _reportingService;
    private readonly IFixture _fixture;
    private readonly AchievementClass _achievementClass;

    public ReportingServiceTests()
    {
        _timeProvider = new();
        _dataContext = CreateDataContext();
        _reportingService = new ReportingService(_dataContext, _timeProvider);
        _fixture = new Fixture().Customize(new DefaultCustomization());
        _achievementClass = _fixture.Create<AchievementClass>();
        _dataContext.AchievementClasses.Add(_achievementClass);
        _dataContext.SaveChanges();
    }
    
    public static IEnumerable<object[]> GetQuantities()
    {
        yield return [
            (int[][]) [[1], [2], [4], [8], [16], [32], [64]],
            new SummaryStatisticsSnapshot(64, 32, 127),
        ];
        yield return [
            (int[][]) [[10000], [1, 1], [], [], [], [2], [], [1, 3, 1]],
            new SummaryStatisticsSnapshot(5, 0, 9),
        ];
    }

    [Theory]
    [MemberData(nameof(GetQuantities))]
    public async Task ShouldGetCorrectStatistics(int[][] quantities, SummaryStatisticsSnapshot expected)
    {
        // Arrange
        for (int i = 0; i < quantities.Length; i++)
        {
            _timeProvider.Advance(TimeSpan.FromDays(1));
            DateOnly date = DateOnly.FromDateTime(_timeProvider.GetUtcNow().Date);
            foreach (int quantity in quantities[i])
            {
                Achievement achievement = _fixture.Build<Achievement>()
                    .With(x => x.CompletedDate, date)
                    .With(x => x.Quantity, quantity)
                    .With(x => x.AchievementClass, _achievementClass)
                    .Create();
                _dataContext.Achievements.Add(achievement);
                await _dataContext.SaveChangesAsync();
            }
        }
        
        // Act
        SummaryStatisticsSnapshot result = await _reportingService.GetSummaryStatisticsSnapshot();

        // Assert
        Assert.Equal(expected, result);
    }
}