using AQ.Console.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using Spectre.Console.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    
        logging.AddFilter("Default", LogLevel.Information);
        logging.AddFilter("System", LogLevel.Warning);
        logging.AddFilter("Microsoft", LogLevel.Warning);
    })
    .UseSpectreConsole(config =>
    {
        config.SetApplicationName("aq");
        config.AddBranch("class", config =>
        {
            config.SetDescription("Manipulate achievement classes (add/delete/list/update)");
            config.AddCommand<AddAchievementClass>("add")
                .WithDescription("Add an achievement class");
            config.AddCommand<DeleteAchievementClass>("delete")
                .WithDescription("Delete an achievement class and all achievements belonging to it");
            config.AddCommand<ListAchievementClasses>("list")
                .WithDescription("List one or more achievement classes");
            config.AddCommand<UpdateAchievementClass>("update")
                .WithDescription("Update an achievement class");
        });
        config.AddCommand<ShowStatus>("status")
            .WithDescription("Shows status");
    }).RunConsoleAsync();