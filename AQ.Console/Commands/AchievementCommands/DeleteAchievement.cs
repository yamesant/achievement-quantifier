using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementCommands;

public sealed class DeleteAchievement(
    DataContext dataContext,
    IAnsiConsole console
    ) : AsyncCommand<DeleteAchievement.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-deleted achievement")]
        public long? Id { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.Id is null or <= 0)
        {
            throw new ArgumentException("Id must be greater than 0", nameof(settings.Id));
        }
        
        int achievementsDeleted = await dataContext
            .Achievements
            .Where(achievement => achievement.Id == settings.Id)
            .ExecuteDeleteAsync();
        console.WriteLine($"Deleted {achievementsDeleted} achievements.");
        return 0;
    }
}