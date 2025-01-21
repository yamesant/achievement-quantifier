using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementClassCommands;

public sealed class DeleteAchievementClass(
    ILogger<DeleteAchievementClass> logger,
    DataContext dataContext
) : AsyncCommand<DeleteAchievementClass.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-deleted achievement class")]
        public long Id { get; init; }
        public override ValidationResult Validate()
        {
            if (Id <= 0) return ValidationResult.Error("Id must be greater than 0");
            return ValidationResult.Success();
        }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
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
        
        logger.LogInformation($"Deleted {achievementClassesDeleted} achievement classes and {achievementsDeleted} achievements.");
        return 0;
    }
}