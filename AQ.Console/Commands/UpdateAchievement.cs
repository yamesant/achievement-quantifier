using System.ComponentModel;
using AQ.Data;
using AQ.Models;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class UpdateAchievement(
    ILogger<UpdateAchievement> logger,
    IRepository repository) : AsyncCommand<UpdateAchievement.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-updated achievement")]
        public long Id { get; init; }

        [CommandOption("-n|--name")]
        [Description("Specify the new name of the to-be-updated achievement")]
        public string Name { get; init; } = string.Empty;

        [CommandOption("-d|--date")]
        [TypeConverter(typeof(DateOnlyTypeConverter))]
        [Description("Specifies the new completion date in dd/MM/yyyy format of the to-be-updated achievement")]
        public DateOnly Date { get; init; }

        [CommandOption("-q|--quantity")]
        [Description("Specifies the new quantity of the to-be-updated achievement")]
        public int Quantity { get; init; }

        public override ValidationResult Validate()
        {
            if (Id <= 0) return ValidationResult.Error("Id must be greater than 0.");
            if (Name.Length < 2) return ValidationResult.Error("Name must be at least 2 characters long.");
            if (Quantity <= 0) return ValidationResult.Error("Quantity must be greater than 0.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AchievementClass? achievementClass = await repository.GetAchievementClassByName(settings.Name);
        if (achievementClass is null)
        {
            logger.LogError($"Achievement class with name {settings.Name} not found.");
            return -1;
        }

        Achievement achievement = new()
        {
            Id = settings.Id,
            AchievementClass = achievementClass,
            CompletedDate = settings.Date,
            Quantity = settings.Quantity,
        };
        
        Achievement? result = await repository.Update(achievement);
        if (result == null)
        {
            logger.LogError("Failed to update achievement.");
            return -1;
        }

        logger.LogInformation($"Updated achievement: {result}.");
        return 0;
    }
}