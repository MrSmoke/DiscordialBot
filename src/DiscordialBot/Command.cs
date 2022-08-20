namespace DiscordialBot;

public abstract class Command
{
    public abstract bool CanHandle(SocketMessage message);
    public abstract Task RunAsync(SocketMessage message, CancellationToken token = default);
}
