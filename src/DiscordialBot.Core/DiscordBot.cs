namespace DiscordialBot.Core;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class DiscordBot : IDisposable, IBot
{
    private readonly ILogger<DiscordBot> _logger;
    private readonly IEnumerable<ICommand> _commands;
    private readonly DiscordBotOptions _options;
    private readonly DiscordSocketClient _client;

    public DiscordBot(
        ILogger<DiscordBot> logger,
        IOptions<DiscordBotOptions> options,
        IEnumerable<ICommand> commands)
    {
        _logger = logger;
        _commands = commands;
        _options = options.Value;
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.Guilds
        });

        _client.Log += LogAsync;
        _client.LoggedIn += OnLoggedInAsync;
        _client.MessageReceived += MessageReceivedAsync;
    }

    public async Task StartAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _options.AccessToken);
        await _client.StartAsync();
    }

    public Task StopAsync()
    {
        return _client.StopAsync();
    }

    private Task LogAsync(LogMessage message)
    {
        _logger.LogDebug(message.Exception, message.Message);

        return Task.CompletedTask;
    }

    private Task OnLoggedInAsync()
    {
        _logger.LogInformation("Logged in");

        return Task.CompletedTask;
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        // The bot should never respond to itself.
        if (message.Author.Id == _client.CurrentUser.Id)
            return;

        _logger.LogDebug("Received message: ({Author}) {Message}", message.Author.Username, message.Content);

        foreach (var command in _commands)
        {
            try
            {
                if (!command.CanHandle(message))
                    continue;

                await command.RunAsync(message);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to ");
            }
        }
    }

    public void Dispose()
    {
        _client.Log -= LogAsync;
        _client.LoggedIn -= OnLoggedInAsync;
        _client.MessageReceived -= MessageReceivedAsync;

        _client.Dispose();
    }
}
