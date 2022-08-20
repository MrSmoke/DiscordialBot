namespace DiscordialBot.Core;

public abstract class Command : ICommand
{
    public abstract bool CanHandle(SocketMessage message);
    public abstract Task RunAsync(SocketMessage message, CancellationToken token = default);
}
