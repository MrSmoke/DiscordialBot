namespace DiscordialBot.Core;

public static class DiscordBotExtensions
{
    public static async Task WaitForTokenShutdownAsync(this IBot bot, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(bot);

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
