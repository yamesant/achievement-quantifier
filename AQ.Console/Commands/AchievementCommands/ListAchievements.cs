using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementCommands;

public sealed class ListAchievements(
    DataContext dataContext,
    IAnsiConsole console
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

        console.WriteLine($"Found {achievements.Count} achievements.");
        foreach (Achievement achievement in achievements)
        {
            console.WriteLine(achievement.ToString());
        }

        return 0;
    }
}