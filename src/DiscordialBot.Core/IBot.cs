namespace DiscordialBot.Core;

public interface IBot
{
    Task StartAsync();
    Task StopAsync();
}
