using AQ.Console.Commands.AchievementClassCommands;
using AQ.Domain;

namespace AQ.Tests.Commands.AchievementClassCommands;

public class DeleteAchievementClassTests : DbTestsBase
{
    private readonly DataContext _dataContext;
    private readonly DeleteAchievementClass _command;

    public DeleteAchievementClassTests()
    {
        _dataContext = CreateDataContext();
        _command = new(_dataContext, Console);
    }

    [Theory, DefaultAutoData]
    public async Task ShouldDelete(AchievementClass achievementClass)
    {
        // Arrange
        _dataContext.AchievementClasses.Add(achievementClass);
        await _dataContext.SaveChangesAsync();
        DeleteAchievementClass.Settings settings = new()
        {
            Id = achievementClass.Id,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        AchievementClass? deleted = _dataContext
            .AchievementClasses
            .SingleOrDefault(a => a.Id == achievementClass.Id);

        // Assert
        Assert.Equal(0, result);
        Assert.Null(deleted);
        Assert.Equal("Deleted 1 achievement classes and 0 achievements.\n", Console.Output);
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldDeleteWithAchievements(AchievementClass achievementClass, Achievement achievement1, Achievement achievement2)
    {
        // Arrange
        achievementClass.Achievements = [achievement1, achievement2];
        _dataContext.AchievementClasses.Add(achievementClass);
        await _dataContext.SaveChangesAsync();
        DeleteAchievementClass.Settings settings = new()
        {
            Id = achievementClass.Id,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        AchievementClass? deleted = _dataContext
            .AchievementClasses
            .SingleOrDefault(a => a.Id == achievementClass.Id);

        // Assert
        Assert.Equal(0, result);
        Assert.Null(deleted);
        Assert.Equal("Deleted 1 achievement classes and 2 achievements.\n", Console.Output);
    }
    
    [Fact]
    public async Task ShouldNotDelete()
    {
        // Arrange
        DeleteAchievementClass.Settings settings = new()
        {
            Id = 1,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Equal("Deleted 0 achievement classes and 0 achievements.\n", Console.Output);
    }
}