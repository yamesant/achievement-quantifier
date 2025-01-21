using AQ.Console.Commands.AchievementCommands;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;

namespace AQ.Tests.Commands.AchievementCommands;

public class AddAchievementTests : DbTestsBase
{
    private readonly DataContext _dataContext;
    private readonly AddAchievement _command;
    private readonly AchievementClass _achievementClass;

    public AddAchievementTests()
    {
        _dataContext = CreateDataContext();
        _command = new(_dataContext, Console);
        
        IFixture fixture = new Fixture().Customize(new DefaultCustomization());
        _achievementClass = fixture.Create<AchievementClass>();
        _dataContext.AchievementClasses.Add(_achievementClass);
        _dataContext.SaveChanges();
    }
        
    [Theory, DefaultAutoData]
    public async Task ShouldAdd(DateOnly date, int quantity)
    {
        // Arrange
        AddAchievement.Settings settings = new()
        {
            Name = _achievementClass.Name,
            Date = date,
            Quantity = quantity,
        };

        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        Achievement? achievement = _dataContext
            .Achievements
            .Include(a => a.AchievementClass)
            .SingleOrDefault(a =>
                a.AchievementClass.Name == _achievementClass.Name &&
                a.CompletedDate == date &&
                a.Quantity == quantity);

        // Assert
        Assert.Equal(0, result);
        Assert.NotNull(achievement);
        Assert.Contains(achievement.ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
}