namespace DiscordialBot.Internal;

using System.Threading;
using System.Threading.Tasks;

internal static class DiscordBotExtensions
{
    public static async Task WaitForTokenShutdownAsync(this IBot bot, CancellationToken cancellationToken)
    {
        var waitForStop = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        cancellationToken.Register(state =>
        {
            var (discordBot, tcs) = ((IBot bot, TaskCompletionSource tcs)) state!;

            discordBot.StopAsync().ContinueWith(_ =>
            {
                tcs.TrySetResult();
            }, TaskContinuationOptions.None);
        }, (bot, waitForStop));

        await waitForStop.Task;
    }
}
