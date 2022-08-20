namespace DiscordialBot.Core.Commands;

public abstract class BasicCommand : ICommand
{
    private readonly string _command;

    protected BasicCommand(string command)
    {
        _command = command;
    }

    public bool CanHandle(SocketMessage message)
    {
        return _command.Equals(message.Content, StringComparison.OrdinalIgnoreCase);
    }

    public async Task RunAsync(SocketMessage message, CancellationToken token = default)
    {
        var response = await ReplyAsync(token);

        await message.Channel.SendMessageAsync(response);
    }

    protected abstract ValueTask<string> ReplyAsync(CancellationToken token);
}
