using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementCommands;

public sealed class UpdateAchievement(
    DataContext dataContext,
    IAnsiConsole console
    ) : AsyncCommand<UpdateAchievement.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id")]
        [Description("Specifies the id of the to-be-updated achievement")]
        public long? Id { get; init; }

        [CommandOption("-n|--name")]
        [Description("Specify the new name of the to-be-updated achievement")]
        public string? Name { get; init; } = string.Empty;

        [CommandOption("-d|--date")]
        [TypeConverter(typeof(DateOnlyTypeConverter))]
        [Description("Specifies the new completion date in dd/MM/yyyy format of the to-be-updated achievement")]
        public DateOnly? Date { get; init; }

        [CommandOption("-q|--quantity")]
        [Description("Specifies the new quantity of the to-be-updated achievement")]
        public int? Quantity { get; init; }

        public override ValidationResult Validate()
        {
            if (Id is null or <= 0) return ValidationResult.Error("Id must be greater than 0.");
            if (Name is null || Name.Length < 2) return ValidationResult.Error("Name must be at least 2 characters long.");
            if (Date is null) return ValidationResult.Error("Date must be provided.");
            if (Quantity is null or <= 0) return ValidationResult.Error("Quantity must be greater than 0.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Achievement? achievement = await dataContext
            .Achievements
            .Include(achievement => achievement.AchievementClass)
            .FirstOrDefaultAsync(achievement => achievement.Id == settings.Id);
        if (achievement is null)
        {
            console.WriteLine($"Achievement with id {settings.Id} not found.");
            return -1;
        }

        achievement.CompletedDate = settings.Date!.Value;
        achievement.Quantity = settings.Quantity!.Value;
        
        if (achievement.AchievementClass.Name != settings.Name)
        {
            AchievementClass? achievementClass = await dataContext
                .AchievementClasses
                .FirstOrDefaultAsync(achievementClass => achievementClass.Name == settings.Name);
            if (achievementClass is null)
            {
                console.WriteLine($"Achievement class with name {settings.Name} not found.");
                return -1;
            }

            achievement.AchievementClass = achievementClass;
        }

        await dataContext.SaveChangesAsync();
        
        console.WriteLine($"Updated achievement: {achievement}.");
        return 0;
    }
}