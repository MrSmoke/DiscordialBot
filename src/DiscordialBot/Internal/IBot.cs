namespace DiscordialBot.Internal;

using System.Threading.Tasks;

internal interface IBot
{
    Task StartAsync();
    Task StopAsync();
}
