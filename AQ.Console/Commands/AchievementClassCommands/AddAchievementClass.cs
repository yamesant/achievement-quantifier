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
        public string Name { get; init; } = string.Empty;
        [CommandOption("-u|--unit")]
        [Description("Specifies the unit of the to-be-added achievement class")]
        public string Unit { get; init; } = string.Empty;
        public override ValidationResult Validate()
        {
            if (Name.Length < 2) return ValidationResult.Error("Name must be at least 2 characters long.");
            if (Unit.Length == 0) return ValidationResult.Error("Unit must be a non-empty string.");
            return ValidationResult.Success();
        }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
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