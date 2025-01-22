using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementClassCommands;

public sealed class DeleteAchievementClass(
    DataContext dataContext,
    IAnsiConsole console
    ) : AsyncCommand<DeleteAchievementClass.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-deleted achievement class")]
        public long? Id { get; init; }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.Id is null or <= 0)
        {
            throw new ArgumentException("Id must be greater than 0", nameof(settings.Id));
        }
        
        await dataContext.Database.BeginTransactionAsync();
        int achievementsDeleted = await dataContext
            .Achievements
            .Where(achievement => achievement.AchievementClassId == settings.Id)
            .ExecuteDeleteAsync();
        int achievementClassesDeleted = await dataContext
            .AchievementClasses
            .Where(achievementClass => achievementClass.Id == settings.Id)
            .ExecuteDeleteAsync();
        await dataContext.Database.CommitTransactionAsync();
        
        console.WriteLine($"Deleted {achievementClassesDeleted} achievement classes and {achievementsDeleted} achievements.");
        return 0;
    }
}