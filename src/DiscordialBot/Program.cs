namespace DiscordialBot;

using System;
using System.Threading;
using System.Threading.Tasks;
using Internal;
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

        await using var serviceProvider = services.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            await RunAsync(serviceProvider);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
        }
    }

    private static async Task RunAsync(IServiceProvider serviceProvider)
    {
        var bot = serviceProvider.GetRequiredService<IBot>();

        var done = new ManualResetEventSlim(false);
        using var cts = new CancellationTokenSource();
        using var lifetime = new BotLifetime(cts, done);

        try
        {
            Console.WriteLine("Starting application...");

            await bot.StartAsync();

            Console.WriteLine("Application started. Press Ctrl+C to shut down.");

            await bot.WaitForTokenShutdownAsync(cts.Token);

            lifetime.SetExitedGracefully();
        }
        finally
        {
            done.Set();
        }
    }
}
