namespace DiscordialBot
{
    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;
    using Internal;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class DiscordBot : IDisposable, IBot
    {
        private readonly ILogger<DiscordBot> _logger;
        private readonly DiscordBotOptions _options;
        private readonly DiscordSocketClient _client;
        
        public DiscordBot(ILogger<DiscordBot> logger, IOptions<DiscordBotOptions> options)
        {
            _logger = logger;
            _options = options.Value;
            _client = new DiscordSocketClient();
            
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

        private Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return Task.CompletedTask;

            _logger.LogDebug("Received message: ({Author}) {Message}", message.Author.Username, message.Content);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _client.Log -= LogAsync;
            _client.LoggedIn -= OnLoggedInAsync;
            _client.MessageReceived -= MessageReceivedAsync;
            
            _client.Dispose();
        }
    }
}