using System.ComponentModel;
using AQ.Data;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class DeleteAchievementClass(
    ILogger<DeleteAchievementClass> logger,
    IRepository repository
    ) : AsyncCommand<DeleteAchievementClass.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-deleted achievement class")]
        public long Id { get; init; }
        public override ValidationResult Validate()
        {
            if (Id <= 0) return ValidationResult.Error("Id must be greater than 0");
            return ValidationResult.Success();
        }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        (int achievementClassesDeleted, int achievementsDeleted) = await repository.DeleteAchievementClass(settings.Id);
        logger.LogInformation($"Deleted {achievementClassesDeleted} achievement classes and {achievementsDeleted} achievements.");
        return 0;
    }
}