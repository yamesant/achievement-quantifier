using AQ.Console.Commands.AchievementClassCommands;
using AQ.Domain;

namespace AQ.Tests.Commands.AchievementClassCommands;

public class AddAchievementClassTests : DbTestsBase
{
    private readonly DataContext _dataContext;
    private readonly AddAchievementClass _command;

    public AddAchievementClassTests()
    {
        _dataContext = CreateDataContext();
        _command = new(_dataContext, Console);
    }
    
    [Theory, AutoData]
    public async Task ShouldAdd(string name, string unit)
    {
        // Arrange
        AddAchievementClass.Settings settings = new()
        {
            Name = name,
            Unit = unit,
        };

        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        AchievementClass? achievementClass = _dataContext
            .AchievementClasses
            .SingleOrDefault(c => c.Name == name && c.Unit == unit);

        // Assert
        Assert.Equal(0, result);
        Assert.NotNull(achievementClass);
        Assert.Contains(achievementClass.ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
    
    [Theory, AutoData]
    public async Task ShouldFailWhenNameIsNotProvided(string unit)
    {
        // Arrange
        AddAchievementClass.Settings settings = new()
        {
            Name = null,
            Unit = unit,
        };
        Task Action() => _command.ExecuteAsync(CommandContext, settings);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Action);
    }
    
    [Theory, AutoData]
    public async Task ShouldFailWhenUnitIsNotProvided(string name)
    {
        // Arrange
        AddAchievementClass.Settings settings = new()
        {
            Name = name,
            Unit = null,
        };
        Task Action() => _command.ExecuteAsync(CommandContext, settings);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Action);
    }
}