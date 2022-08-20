namespace DiscordialBot.Core;

using Internal;
using Microsoft.Extensions.DependencyInjection;

public class BotHost : IAsyncDisposable
{
    public IServiceProvider Services { get; }

    public BotHost(IServiceProvider services)
    {
        Services = services;
    }

    public async Task RunAsync()
    {
        var bot = Services.GetRequiredService<IBot>();

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

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(Services);

        static async ValueTask DisposeAsync(object o)
        {
            switch (o)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
    }
}
