namespace DiscordialBot.Commands;

public class Aoe2TauntCommand : ICommand
{
    private readonly Dictionary<string, Aoe2Response> _taunts = new()
    {
        { "1", new("Yes.", "e/e1/Yes.ogg") },
        { "2", new("No.", "7/7a/No.ogg") },
        { "3", new("I need food.", "b/b3/Food%2C_please.ogg") },
        { "7", new("Ahh!", "2/21/Ahh.ogg") },
        { "14", new("Start the game already!", "0/04/Start_the_game.ogg") }
    };

    public bool CanHandle(SocketMessage message)
    {
        return _taunts.ContainsKey(message.Content);
    }

    public Task RunAsync(SocketMessage message, CancellationToken token = default)
    {
        var response = _taunts[message.Content];

        var str = response.Message;

        if (response.AudioUrlPart is not null)
        {
            var url = "https://static.wikia.nocookie.net/ageofempires/images/" + response.AudioUrlPart;
            str += $" [\u25B6\uFE0F]({url})";
        }

        var embed = new EmbedBuilder
        {
            Description = str,
            Color = Color.Blue
        };

        return message.Channel.SendMessageAsync(embed: embed.Build());
    }

    private record Aoe2Response(string Message, string? AudioUrlPart = null);
}
