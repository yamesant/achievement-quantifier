using System.ComponentModel;
using AQ.Data;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class DeleteAchievement(
    ILogger<DeleteAchievement> logger,
    IRepository repository) : AsyncCommand<DeleteAchievement.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-deleted achievement")]
        public long Id { get; init; }

        public override ValidationResult Validate()
        {
            if (Id <= 0) return ValidationResult.Error("Id must be greater than 0.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        int achievementsDeleted = await repository.DeleteAchievement(settings.Id);
        logger.LogInformation($"Deleted {achievementsDeleted} achievements.");
        return 0;
    }
}