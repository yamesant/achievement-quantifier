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
        config.AddCommand<ShowStatus>("status")
            .WithDescription("Shows status");
    }).RunConsoleAsync();