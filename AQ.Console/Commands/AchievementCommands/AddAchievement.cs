using System.ComponentModel;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AQ.Console.Commands.AchievementCommands;

public sealed class AddAchievement(
    DataContext dataContext,
    IAnsiConsole console,
    TimeProvider timeProvider
    ) : AsyncCommand<AddAchievement.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("Specifies the name of the achievement class that the to-be-added achievement should belong to")]
        public string? Name { get; init; }
        [CommandOption("-d|--date")]
        [TypeConverter(typeof(DateOnlyTypeConverter))]
        [Description("Specifies the completion date in dd/MM/yyyy format of the to-be-added achievement")]
        public DateOnly? Date { get; set; }

        [CommandOption("-q|--quantity")]
        [Description("Specifies the quantity of the to-be-added achievement")]
        public int? Quantity { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.Name is null || settings.Name.Length < 2)
        {
            throw new ArgumentException("Name must be at least 2 characters long", nameof(settings.Name));
        }

        if (settings.Quantity is not null && settings.Quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0", nameof(settings.Quantity));
        }

        settings.Date ??= DateOnly.FromDateTime(timeProvider.GetUtcNow().Date);
        settings.Quantity ??= 1;
        
        AchievementClass? achievementClass = await dataContext
            .AchievementClasses
            .FirstOrDefaultAsync(achievementClass => achievementClass.Name == settings.Name);
        if (achievementClass is null)
        {
            console.WriteLine($"Achievement class with name {settings.Name} not found.");
            return -1;
        }

        Achievement achievement = new()
        {
            AchievementClass = achievementClass,
            CompletedDate = settings.Date.Value,
            Quantity = settings.Quantity.Value,
            Notes = "",
        };

        dataContext
            .Achievements
            .Add(achievement);
        await dataContext.SaveChangesAsync();
        
        console.WriteLine($"Added achievement: {achievement}.");
        return 0;
    }
}