namespace DiscordialBot
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        private static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddLogging(o =>
            {
                o.SetMinimumLevel(LogLevel.Debug);
                o.AddConsole();
            });
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
}