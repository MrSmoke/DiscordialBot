namespace DiscordialBot.Core.Commands;

public interface ICommand
{
    bool CanHandle(SocketMessage message);
    Task RunAsync(SocketMessage message, CancellationToken token = default);
}
