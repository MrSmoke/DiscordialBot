namespace DiscordialBot.Internal
{
    using System.Threading.Tasks;

    public interface IBot
    {
        Task StartAsync();
        Task StopAsync();
    }
}