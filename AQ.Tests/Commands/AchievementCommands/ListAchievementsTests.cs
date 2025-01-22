using AQ.Console.Commands.AchievementCommands;
using AQ.Domain;

namespace AQ.Tests.Commands.AchievementCommands;

public class ListAchievementsTests : DbTestsBase
{
    private readonly ListAchievements _command;
    private readonly AchievementClass _achievementClass1;
    private readonly AchievementClass _achievementClass2;
    private readonly Achievement[] _achievements;

    public ListAchievementsTests()
    {
        DataContext dataContext = CreateDataContext();
        _command = new(dataContext, Console);

        IFixture fixture = new Fixture().Customize(new DefaultCustomization());
        _achievements = fixture.CreateMany<Achievement>(3).ToArray();
        _achievementClass1 = fixture.Create<AchievementClass>();
        _achievementClass2 = fixture.Create<AchievementClass>();
        _achievements[0].AchievementClass = _achievementClass1;
        _achievements[1].AchievementClass = _achievementClass1;
        _achievements[2].AchievementClass = _achievementClass2;
        dataContext.Achievements.AddRange(_achievements);
        dataContext.SaveChanges();
    }

    [Fact]
    public async Task ShouldGetAll()
    {
        // Arrange
        ListAchievements.Settings settings = new()
        {
            Id = null,
            Name = null,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 3 achievements.\n", Console.Output);
        foreach (Achievement achievement in _achievements)
        {
            Assert.Contains(achievement.ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
        }
    }
    
    [Fact]
    public async Task ShouldGetByClassName()
    {
        // Arrange
        ListAchievements.Settings settings = new()
        {
            Id = null,
            Name = _achievementClass1.Name,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 2 achievements.\n", Console.Output);
        foreach (Achievement achievement in _achievementClass1.Achievements)
        {
            Assert.Contains(achievement.ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
        }
    }
    
    [Fact]
    public async Task ShouldGetById()
    {
        // Arrange
        ListAchievements.Settings settings = new()
        {
            Id = _achievements[2].Id,
            Name = null,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 1 achievements.\n", Console.Output);
        Assert.Contains(_achievements[2].ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
    
    [Fact]
    public async Task ShouldGetIntersection()
    {
        // Arrange
        ListAchievements.Settings settings = new()
        {
            Id = _achievements[2].Id,
            Name = _achievements[2].AchievementClass.Name,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 1 achievements.\n", Console.Output);
        Assert.Contains(_achievements[2].ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
    
    [Fact]
    public async Task ShouldGetNothingWhenIntersectionIsEmpty()
    {
        // Arrange
        ListAchievements.Settings settings = new()
        {
            Id = _achievements[0].Id,
            Name = _achievementClass2.Name,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);

        // Assert
        Assert.Equal(0, result);
        Assert.Contains("Found 0 achievements.\n", Console.Output);
    }
}