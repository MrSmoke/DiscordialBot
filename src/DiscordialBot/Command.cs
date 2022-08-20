namespace DiscordialBot;

using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;

public abstract class Command
{
    public abstract bool CanHandle(SocketMessage message);
    public abstract Task RunAsync(SocketMessage message, CancellationToken token = default);
}