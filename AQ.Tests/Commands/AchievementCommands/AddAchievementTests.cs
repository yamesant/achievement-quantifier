using AQ.Console.Commands.AchievementCommands;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace AQ.Tests.Commands.AchievementCommands;

public class AddAchievementTests : DbTestsBase
{
    private readonly FakeTimeProvider _timeProvider;
    private readonly DataContext _dataContext;
    private readonly AddAchievement _command;
    private readonly AchievementClass _achievementClass;

    public AddAchievementTests()
    {
        _timeProvider = new();
        _dataContext = CreateDataContext();
        _command = new(_dataContext, Console, _timeProvider);
        
        IFixture fixture = new Fixture().Customize(new DefaultCustomization());
        _achievementClass = fixture.Create<AchievementClass>();
        _dataContext.AchievementClasses.Add(_achievementClass);
        _dataContext.SaveChanges();
    }
        
    [Theory, DefaultAutoData]
    public async Task ShouldAdd(DateOnly date, int quantity, string notes)
    {
        // Arrange
        AddAchievement.Settings settings = new()
        {
            Name = _achievementClass.Name,
            Date = date,
            Quantity = quantity,
            Notes = notes,
        };

        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        Achievement? achievement = _dataContext
            .Achievements
            .Include(a => a.AchievementClass)
            .SingleOrDefault(a =>
                a.AchievementClass.Name == _achievementClass.Name &&
                a.CompletedDate == date &&
                a.Quantity == quantity &&
                a.Notes == notes);

        // Assert
        Assert.Equal(0, result);
        Assert.NotNull(achievement);
        Assert.Contains(achievement.ToString().RemoveWhitespace(), Console.Output.RemoveWhitespace());
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldFailWhenNameIsNotProvided(int quantity, DateOnly date, string notes)
    {
        // Arrange
        AddAchievement.Settings settings = new()
        {
            Name = null,
            Date = date,
            Quantity = quantity,
            Notes = notes,
        };
        Task Action() => _command.ExecuteAsync(CommandContext, settings);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Action);
    }
    
    [Theory]
    [InlineAutoData(0)]
    [InlineAutoData(1)]
    [InlineAutoData(2)]
    public async Task ShouldUseTodayWhenDateIsNotProvided(int daysPassed, int quantity, string notes)
    {
        // Arrange
        _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
        DateOnly today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().Date);
        AddAchievement.Settings settings = new()
        {
            Name = _achievementClass.Name,
            Date = null,
            Quantity = quantity,
            Notes = notes,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        Achievement? achievement = _dataContext
            .Achievements
            .Include(a => a.AchievementClass)
            .SingleOrDefault(a =>
                a.AchievementClass.Name == _achievementClass.Name &&
                a.CompletedDate == today &&
                a.Quantity == quantity &&
                a.Notes == notes);

        // Assert
        Assert.Equal(0, result);
        Assert.NotNull(achievement);
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldUseOneWhenQuantityIsNotProvided(DateOnly date, string notes)
    {
        // Arrange
        AddAchievement.Settings settings = new()
        {
            Name = _achievementClass.Name,
            Date = date,
            Quantity = null,
            Notes = notes,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        Achievement? achievement = _dataContext
            .Achievements
            .Include(a => a.AchievementClass)
            .SingleOrDefault(a =>
                a.AchievementClass.Name == _achievementClass.Name &&
                a.CompletedDate == date &&
                a.Quantity == 1 && 
                a.Notes == notes);

        // Assert
        Assert.Equal(0, result);
        Assert.NotNull(achievement);
    }
    
    [Theory, DefaultAutoData]
    public async Task ShouldUseEmptyStringWhenNotesIsNotProvided(DateOnly date, int quantity)
    {
        // Arrange
        AddAchievement.Settings settings = new()
        {
            Name = _achievementClass.Name,
            Date = date,
            Quantity = quantity,
            Notes = null,
        };
        
        // Act
        int result = await _command.ExecuteAsync(CommandContext, settings);
        Achievement? achievement = _dataContext
            .Achievements
            .Include(a => a.AchievementClass)
            .SingleOrDefault(a =>
                a.AchievementClass.Name == _achievementClass.Name &&
                a.CompletedDate == date &&
                a.Quantity == quantity && 
                a.Notes == "");

        // Assert
        Assert.Equal(0, result);
        Assert.NotNull(achievement);
    }
}