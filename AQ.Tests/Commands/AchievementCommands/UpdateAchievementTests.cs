using AQ.Console.Commands.AchievementCommands;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;

namespace AQ.Tests.Commands.AchievementCommands;

public class UpdateAchievementTests : DbTestsBase
{
    private readonly DataContext _dataContext;
    private readonly UpdateAchievement _command;
    private readonly Achievement _achievement;

    public UpdateAchievementTests()
    {
        _dataContext = CreateDataContext();
        _command = new(_dataContext, Console);

        IFixture fixture = new Fixture().Customize(new DefaultCustomization());
        _achievement = fixture.Create<Achievement>();
        _achievement.AchievementClass = fixture.Create<AchievementClass>();
        _dataContext.Achievements.Add(_achievement);
        _dataContext.SaveChanges();
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldUpdate(DateOnly date, int quantity)
    {
        // Arrange
        UpdateAchievement.Settings settings = new()
        {
            Id = _achievement.Id,
            Name = _achievement.AchievementClass.Name,
            Date = date,
            Quantity = quantity,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        Achievement? updated = _dataContext
            .Achievements
            .Include(a => a.AchievementClass)
            .SingleOrDefault(a =>
                a.Id == _achievement.Id &&
                a.AchievementClass.Name == _achievement.AchievementClass.Name &&
                a.CompletedDate == date &&
                a.Quantity == quantity);
                
        // Assert
        Assert.Equal(0, result);
        Assert.NotNull(updated);
        Assert.Contains(updated.ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
    
        [Theory, DefaultAutoData]
    public async Task ShouldFailWhenIdIsNotProvided(DateOnly date, int quantity)
    {
        // Arrange
        UpdateAchievement.Settings settings = new()
        {
            Id = null,
            Name = _achievement.AchievementClass.Name,
            Date = date,
            Quantity = quantity,
        };
        Task Action() => _command.ExecuteAsync(CommandContext, settings);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Action);
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldFailWhenNameIsNotProvided(DateOnly date, int quantity)
    {
        // Arrange
        UpdateAchievement.Settings settings = new()
        {
            Id = _achievement.Id,
            Name = null,
            Date = date,
            Quantity = quantity,
        };
        Task Action() => _command.ExecuteAsync(CommandContext, settings);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Action);
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldFailWhenDateIsNotProvided(int quantity)
    {
        // Arrange
        UpdateAchievement.Settings settings = new()
        {
            Id = _achievement.Id,
            Name = _achievement.AchievementClass.Name,
            Date = null,
            Quantity = quantity,
        };
        Task Action() => _command.ExecuteAsync(CommandContext, settings);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Action);
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldFailWhenQuantityIsNotProvided(DateOnly date)
    {
        // Arrange
        UpdateAchievement.Settings settings = new()
        {
            Id = _achievement.Id,
            Name = _achievement.AchievementClass.Name,
            Date = date,
            Quantity = null
        };
        Task Action() => _command.ExecuteAsync(CommandContext, settings);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Action);
    }
}