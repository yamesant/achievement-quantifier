using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementClassCommands;

public sealed class ListAchievementClasses(
    DataContext dataContext,
    IAnsiConsole console
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
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.Id is not null && settings.Id <= 0)
        {
            throw new ArgumentException("Id must be greater than 0", nameof(settings.Id));
        }

        if (settings.Name is not null && settings.Name.Length < 2)
        {
            throw new ArgumentException("Name must be at least 2 characters long.", nameof(settings.Name));
        }
        
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

        console.WriteLine($"Found {result.Count} achievement classes.");
        foreach (AchievementClass achievementClass in result)
        {
            console.WriteLine(achievementClass.ToString());
        }

        return 0;
    }
}