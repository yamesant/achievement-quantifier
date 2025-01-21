using AQ.Console.Commands.AchievementCommands;
using AQ.Domain;

namespace AQ.Tests.Commands.AchievementCommands;

public class DeleteAchievementTests : DbTestsBase
{
    private readonly DataContext _dataContext;
    private readonly DeleteAchievement _command;
    private readonly AchievementClass _achievementClass;

    public DeleteAchievementTests()
    {
        _dataContext = CreateDataContext();
        _command = new(_dataContext, Console);

        IFixture fixture = new Fixture().Customize(new DefaultCustomization());
        _achievementClass = fixture.Create<AchievementClass>();
        _dataContext.AchievementClasses.Add(_achievementClass);
        _dataContext.SaveChanges();
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldDelete(Achievement achievement)
    {
        // Arrange
        achievement.AchievementClass = _achievementClass;
        _dataContext.Achievements.Add(achievement);
        await _dataContext.SaveChangesAsync();
        DeleteAchievement.Settings settings = new()
        {
            Id = achievement.Id,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        Achievement? deleted = _dataContext
            .Achievements
            .SingleOrDefault(a => a.Id == achievement.Id);

        // Assert
        Assert.Equal(0, result);
        Assert.Null(deleted);
        Assert.Equal("Deleted 1 achievements.\n", Console.Output);
    }
    
    [Fact]
    public async Task ShouldNotDelete()
    {
        // Arrange
        DeleteAchievement.Settings settings = new()
        {
            Id = 1,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Equal("Deleted 0 achievements.\n", Console.Output);
    }
}