using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementCommands;

public sealed class ListAchievements(
    ILogger<ListAchievements> logger,
    DataContext dataContext
    ) : AsyncCommand<ListAchievements.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("(Optional) Specifies the id on which to filter achievements")]
        public long? Id { get; init; }

        [CommandOption("-n|--name")]
        [Description("(Optional) Specifies the class name on which to filter achievements")]
        public string? Name { get; init; }
        public override ValidationResult Validate()
        {
            if (Id is 0) return ValidationResult.Error("Id must be greater than 0");
            if (Name is { Length: < 2 }) return ValidationResult.Error("Name must be at least 2 characters long.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        List<Achievement> achievements = [];

        if (settings.Id.HasValue)
        {
            Achievement? achievement = await dataContext
                .Achievements
                .Include(achievement => achievement.AchievementClass)
                .FirstOrDefaultAsync(achievement => achievement.Id == settings.Id.Value);
            if (achievement != null && (settings.Name == null || settings.Name == achievement.AchievementClass.Name))
            {
                achievements = [achievement];
            }
        }
        else if (settings.Name != null)
        {
            achievements = await dataContext
                .Achievements
                .Include(achievement => achievement.AchievementClass)
                .Where(achievement => achievement.AchievementClass.Name == settings.Name)
                .AsNoTracking()
                .ToListAsync();
        }
        else
        {
            achievements = await dataContext
                .Achievements
                .Include(achievement => achievement.AchievementClass)
                .AsNoTracking()
                .ToListAsync();
        }

        logger.LogInformation($"Found {achievements.Count} achievements.");
        foreach (Achievement achievement in achievements)
        {
            AnsiConsole.WriteLine(achievement.ToString());
        }

        return 0;
    }
}