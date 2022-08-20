namespace DiscordialBot.Commands;

public class PingCommand : BasicCommand
{
    public PingCommand() : base("ping")
    {
    }

    public override Task RunAsync(SocketMessage message, CancellationToken token = default)
    {
        return message.Channel.SendMessageAsync("pong");
    }
}
