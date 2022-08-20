namespace DiscordialBot.Commands;

using Core.Commands;

public class PingCommand : BasicCommand
{
    public PingCommand() : base("ping")
    {
    }

    protected override ValueTask<string> ReplyAsync(CancellationToken token)
    {
        return new ValueTask<string>("pong");
    }
}
