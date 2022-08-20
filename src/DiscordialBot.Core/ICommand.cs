namespace DiscordialBot.Core;

public interface ICommand
{
    bool CanHandle(SocketMessage message);
    Task RunAsync(SocketMessage message, CancellationToken token = default);
}