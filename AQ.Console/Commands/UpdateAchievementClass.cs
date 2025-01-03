using System.ComponentModel;
using AQ.Data;
using AQ.Models;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class UpdateAchievementClass(
    ILogger<UpdateAchievementClass> logger,
    IRepository repository
    ) : AsyncCommand<UpdateAchievementClass.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-updated achievement class")]
        public long Id { get; init; }
        [CommandOption("-n|--name")]
        [Description("Specifies the new name of the to-be-updated achievement class")]
        public string Name { get; init; } = string.Empty;
        [CommandOption("-u|--unit")]
        [Description("Specifies the new unit of the to-be-updated achievement class")]
        public string Unit { get; init; } = string.Empty;
        public override ValidationResult Validate()
        {
            if (Id <= 0) return ValidationResult.Error("Id must be greater than 0");
            if (Name.Length < 2) return ValidationResult.Error("Name must be at least 2 characters long.");
            if (Unit.Length == 0) return ValidationResult.Error("Unit must be a non-empty string.");
            return ValidationResult.Success();
        }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AchievementClass achievementClass = new()
        {
            Id = settings.Id,
            Name = settings.Name,
            Unit = settings.Unit,
        };
        AchievementClass? result = await repository.Update(achievementClass);
        if (result is null)
        {
            logger.LogError("Failed to update achievement class.");
            return -1;
        }

        logger.LogInformation($"Updated '{result}'.");
        return 0;
    }
}