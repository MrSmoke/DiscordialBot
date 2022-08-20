namespace DiscordialBot;

using Commands;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var config = configBuilder.Build();
        var services = new ServiceCollection();

        services.AddSingleton(_ => config);

        services.AddOptions();
        services.AddLogging(o =>
        {
            o.SetMinimumLevel(LogLevel.Debug);
            o.AddConsole();
        });

        services.Configure<DiscordBotOptions>(config);
        services.AddSingleton<IBot, DiscordBot>();

        // todo: make this nicer
        services.AddSingleton<ICommand, PingCommand>();
        services.AddSingleton<ICommand, Aoe2TauntCommand>();

        var host = new BotHost(services.BuildServiceProvider());

        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
        }
    }
}
