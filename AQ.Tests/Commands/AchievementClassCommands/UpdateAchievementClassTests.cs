using AQ.Console.Commands.AchievementClassCommands;
using AQ.Domain;

namespace AQ.Tests.Commands.AchievementClassCommands;

public class UpdateAchievementClassTests : DbTestsBase
{
    private readonly DataContext _dataContext;
    private readonly UpdateAchievementClass _command;

    public UpdateAchievementClassTests()
    {
        _dataContext = CreateDataContext();
        _command = new(_dataContext, Console);
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldUpdate(AchievementClass achievementClass, string name, string unit)
    {
        // Arrange
        _dataContext.AchievementClasses.Add(achievementClass);
        await _dataContext.SaveChangesAsync();
        UpdateAchievementClass.Settings settings = new()
        {
            Id = achievementClass.Id,
            Name = name,
            Unit = unit,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        AchievementClass? updated = _dataContext
            .AchievementClasses
            .SingleOrDefault(a =>
                a.Id == achievementClass.Id && 
                a.Name == name &&
                a.Unit == unit);

        // Assert
        Assert.Equal(0, result);
        Assert.NotNull(updated);
        Assert.Contains(updated.ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
}