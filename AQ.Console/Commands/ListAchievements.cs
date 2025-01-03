using System.ComponentModel;
using AQ.Data;
using AQ.Models;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class ListAchievements(
    ILogger<ListAchievements> logger,
    IRepository repository) : AsyncCommand<ListAchievements.Settings>
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
        List<Achievement> achievements = new();

        if (settings.Id.HasValue)
        {
            Achievement? achievement = await repository.GetAchievementById(settings.Id.Value);
            if (achievement != null && (settings.Name == null || settings.Name == achievement.AchievementClass.Name))
            {
                achievements = [achievement];
            }
        }
        else if (settings.Name != null)
        {
            achievements = await repository.GetAchievementsByClassName(settings.Name);
        }
        else
        {
            achievements = await repository.GetAllAchievements();
        }

        logger.LogInformation($"Found {achievements.Count} achievements.");
        foreach (Achievement achievement in achievements)
        {
            AnsiConsole.WriteLine(achievement.ToString());
        }

        return 0;
    }
}