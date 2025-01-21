using AQ.Console.Commands.AchievementClassCommands;
using AQ.Domain;

namespace AQ.Tests.Commands.AchievementClassCommands;

public class ListAchievementClassesTests : DbTestsBase
{
    private readonly DataContext _dataContext;
    private readonly ListAchievementClasses _command;
    private readonly AchievementClass[] _achievementClasses;

    public ListAchievementClassesTests()
    {
        _dataContext = CreateDataContext();
        _command = new(_dataContext, Console);

        IFixture fixture = new Fixture().Customize(new DefaultCustomization());
        _achievementClasses = fixture.CreateMany<AchievementClass>(3).ToArray();
        _dataContext.AchievementClasses.AddRange(_achievementClasses);
        _dataContext.SaveChanges();
    }

    [Fact]
    public async Task ShouldGetAll()
    {
        // Arrange
        ListAchievementClasses.Settings settings = new()
        {
            Id = null,
            Name = null,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 3 achievement classes.\n", Console.Output);
        foreach (AchievementClass achievementClass in _achievementClasses)
        {
            Assert.Contains(achievementClass.ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
        }
    }
    
    [Fact]
    public async Task ShouldGetById()
    {
        // Arrange
        ListAchievementClasses.Settings settings = new()
        {
            Id = _achievementClasses[0].Id,
            Name = null,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 1 achievement classes.\n", Console.Output);
        Assert.Contains(_achievementClasses[0].ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
    
    [Fact]
    public async Task ShouldGetByName()
    {
        // Arrange
        ListAchievementClasses.Settings settings = new()
        {
            Id = null,
            Name = _achievementClasses[1].Name,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 1 achievement classes.\n", Console.Output);
        Assert.Contains(_achievementClasses[1].ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
    
    [Fact]
    public async Task ShouldGetIntersection()
    {
        // Arrange
        ListAchievementClasses.Settings settings = new()
        {
            Id = _achievementClasses[0].Id,
            Name = _achievementClasses[0].Name,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 1 achievement classes.\n", Console.Output);
        Assert.Contains(_achievementClasses[0].ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
    
    [Fact]
    public async Task ShouldGetNothingWhenIntersectionIsEmpty()
    {
        // Arrange
        ListAchievementClasses.Settings settings = new()
        {
            Id = _achievementClasses[0].Id,
            Name = _achievementClasses[1].Name,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 0 achievement classes.\n", Console.Output);
    }
}