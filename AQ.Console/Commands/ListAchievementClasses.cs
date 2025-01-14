using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class ListAchievementClasses(
    ILogger<ListAchievementClasses> logger,
    DataContext dataContext
    ) : AsyncCommand<ListAchievementClasses.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("(Optional) Specifies the id on which to filter achievement classes")]
        public long? Id { get; init; }
        [CommandOption("-n|--name")]
        [Description("(Optional) Specifies the name on which to filter achievement classes")]
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
        List<AchievementClass> result = [];
        if (settings.Id.HasValue)
        {
            AchievementClass? achievementClass = await dataContext
                .AchievementClasses
                .FirstOrDefaultAsync(achievementClass => achievementClass.Id == settings.Id.Value);
            if (achievementClass != null && (settings.Name is null || settings.Name == achievementClass.Name))
            {
                result = [achievementClass];
            }
        }
        else if (settings.Name != null)
        {
            AchievementClass? achievementClass = await dataContext
                .AchievementClasses
                .FirstOrDefaultAsync(achievementClass => achievementClass.Name == settings.Name);
            if (achievementClass != null)
            {
                result = [achievementClass];
            }
        }
        else
        {
            result = await dataContext
                .AchievementClasses
                .AsNoTracking()
                .ToListAsync();
        }

        logger.LogInformation($"Found {result.Count} achievement classes.");
        foreach (AchievementClass achievementClass in result)
        {
            AnsiConsole.WriteLine(achievementClass.ToString());
        }

        return 0;
    }
}