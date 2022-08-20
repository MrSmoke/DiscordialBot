namespace DiscordialBot;

using System;
using Discord.WebSocket;

public abstract class BasicCommand : Command
{
    private readonly string _command;

    protected BasicCommand(string command)
    {
        _command = command;
    }

    public override bool CanHandle(SocketMessage message)
    {
        return _command.Equals(message.Content, StringComparison.OrdinalIgnoreCase);
    }
}