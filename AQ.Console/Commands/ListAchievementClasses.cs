using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class ListAchievementClasses(ILogger<ListAchievementClasses> logger) : AsyncCommand<ListAchievementClasses.Settings>
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
        if (settings.Id.HasValue)
        {
            logger.LogInformation($"Filtering on achievement class id '{settings.Id.Value}'");
        }
        else if (settings.Name != null)
        {
            logger.LogInformation($"Filtering on achievement class name '{settings.Name}'");
        }
        else
        {
            logger.LogInformation("Getting all achievement classes");
        }
        
        return 0;
    }
}