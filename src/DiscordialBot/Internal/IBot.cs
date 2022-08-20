namespace DiscordialBot.Internal;

internal interface IBot
{
    Task StartAsync();
    Task StopAsync();
}
