using DSharpPlus;
using DSharpPlus.SlashCommands;
using Modcord.Commands;

namespace Modcord
{
    public class Bot
    {
        public DiscordClient? Client { get; private set; }
        public SlashCommandsExtension SlashCommandsExtension { get; private set; }
        public Bot()
        {
            var configuration_Discord = new DiscordConfiguration()
            {
                Token = TokenJsonReader.ReadTokenFromJsonAsync().Result.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(configuration_Discord);
            SlashCommandsExtension = Client.UseSlashCommands();
            SlashCommandsExtension.RegisterCommands<SlashCommands>();
            //Client.Ready += OnClientReady;
        }

        public async Task RunAsync()
        {
            await Client!.ConnectAsync();
            await Task.Delay(-1);
        }

        /*
        public Task? OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return null;
        }
        */
    }
}
