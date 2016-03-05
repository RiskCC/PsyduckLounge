using Discord;
using Discord.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules.Random
{
    public class RandomModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;

            manager.CreateCommands("", group =>
            {
                group.CreateCommand("Kippei nudes")
                       .Description("Random 001")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           // Nope.jpg
                           await e.Channel.SendMessage($"http://i.giphy.com/3ornk3odh0K2ABXmBW.gif");
                       });
                group.CreateCommand("Jhazat nudes")
                       .Description("Random 002")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"http://i.giphy.com/3ornk0Rqhvwa9o2zzG.gif");
                       });
            });

        }

    }
}
