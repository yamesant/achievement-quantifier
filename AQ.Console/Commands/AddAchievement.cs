using System.ComponentModel;
using AQ.Data;
using AQ.Models;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands;

public sealed class AddAchievement(
    ILogger<AddAchievement> logger,
    IRepository repository) : AsyncCommand<AddAchievement.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("Specifies the name of the achievement class that the to-be-added achievement should belong to")]
        public string Name { get; init; } = string.Empty;
        [CommandOption("-d|--date")]
        [TypeConverter(typeof(DateOnlyTypeConverter))]
        [Description("Specifies the completion date in dd/MM/yyyy format of the to-be-added achievement")]
        public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [CommandOption("-q|--quantity")]
        [Description("Specifies the quantity of the to-be-added achievement")]
        [DefaultValue(1)]
        public int Quantity { get; init; }

        public override ValidationResult Validate()
        {
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
            AchievementClass = achievementClass,
            CompletedDate = settings.Date,
            Quantity = settings.Quantity
        };

        Achievement? result = await repository.Insert(achievement);
        logger.LogInformation($"Added achievement: {result}.");
        return 0;
    }
}