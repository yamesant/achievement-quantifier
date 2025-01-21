using AQ.Console.Commands;
using AQ.Console.Commands.AchievementClassCommands;
using AQ.Console.Commands.AchievementCommands;
using AQ.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<DataContext>(options =>
        {
            string dataSource = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AQ",
                context.HostingEnvironment.EnvironmentName,
                "data.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dataSource)!);
            string connectionString = $"Data Source={dataSource}";
            options.UseSqlite(connectionString);
        });
    })
    .UseSpectreConsole(config =>
    {
        config.SetApplicationName("aq");
        config.AddBranch("achievement", config =>
        {
            config.SetDescription("Manipulate achievements (add/delete/list/update)");
            config.AddCommand<AddAchievement>("add")
                .WithDescription("Add an achievement");
            config.AddCommand<DeleteAchievement>("delete")
                .WithDescription("Delete an achievement");
            config.AddCommand<ListAchievements>("list")
                .WithDescription("List one or more achievements");
            config.AddCommand<UpdateAchievement>("update")
                .WithDescription("Update an achievement");
        });
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