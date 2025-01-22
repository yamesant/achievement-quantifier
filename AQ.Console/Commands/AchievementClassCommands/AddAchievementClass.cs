using System.ComponentModel;
using AQ.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementClassCommands;

public sealed class AddAchievementClass(
    DataContext dataContext,
    IAnsiConsole console
    ) : AsyncCommand<AddAchievementClass.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("Specifies the name of the to-be-added achievement class")]
        public string? Name { get; init; }
        [CommandOption("-u|--unit")]
        [Description("Specifies the unit of the to-be-added achievement class")]
        public string? Unit { get; init; }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.Name is null || settings.Name.Length < 2)
        {
            throw new ArgumentException("Name must be at least 2 characters long", nameof(settings.Name));
        }
        
        if (settings.Unit is null || settings.Unit.Length == 0)
        {
            throw new ArgumentException("Unit must be a non-empty string", nameof(settings.Unit));
        }
        
        AchievementClass achievementClass = new()
        {
            Name = settings.Name,
            Unit = settings.Unit,
        };
        dataContext
            .AchievementClasses
            .Add(achievementClass);
        await dataContext.SaveChangesAsync();

        console.WriteLine($"Added '{achievementClass}'.");
        return 0;
    }
}