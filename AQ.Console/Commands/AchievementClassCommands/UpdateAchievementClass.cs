using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementClassCommands;

public sealed class UpdateAchievementClass(
    DataContext dataContext,
    IAnsiConsole console
    ) : AsyncCommand<UpdateAchievementClass.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-updated achievement class")]
        public long? Id { get; init; }
        [CommandOption("-n|--name")]
        [Description("Specifies the new name of the to-be-updated achievement class")]
        public string? Name { get; init; }
        [CommandOption("-u|--unit")]
        [Description("Specifies the new unit of the to-be-updated achievement class")]
        public string? Unit { get; init; }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.Id is null or <= 0)
        {
            throw new ArgumentException("Id must be greater than 0", nameof(settings.Id));
        }
        
        if (settings.Name is null || settings.Name.Length < 2)
        {
            throw new ArgumentException("Name must be at least 2 characters long", nameof(settings.Name));
        }
        
        if (settings.Unit is null || settings.Unit.Length == 0)
        {
            throw new ArgumentException("Unit must be a non-empty string", nameof(settings.Unit));
        }
        
        AchievementClass? achievementClass = await dataContext
            .AchievementClasses
            .FirstOrDefaultAsync(achievementClass => achievementClass.Id == settings.Id);
        if (achievementClass is null)
        {
            console.WriteLine($"Achievement class with id {settings.Id} not found.");
            return -1;
        }

        achievementClass.Name = settings.Name;
        achievementClass.Unit = settings.Unit;
        await dataContext.SaveChangesAsync();

        console.WriteLine($"Updated '{achievementClass}'.");
        return 0;
    }
}