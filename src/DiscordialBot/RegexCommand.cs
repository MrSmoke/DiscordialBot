namespace DiscordialBot;

using System.Text.RegularExpressions;
using Discord.WebSocket;

public abstract class RegexCommand : Command
{
    private readonly Regex _regex;

    protected RegexCommand(string regexString)
    {
        _regex = new Regex(regexString, RegexOptions.Compiled);
    }

    public override bool CanHandle(SocketMessage message)
    {
        return _regex.IsMatch(message.Content);
    }
}