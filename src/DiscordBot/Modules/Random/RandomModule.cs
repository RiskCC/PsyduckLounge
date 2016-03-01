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
                       .Description("Random command for Jhazat")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           // Nope.jpg
                           await e.Channel.SendMessage($"http://goo.gl/DnEeCf");
                       });
            });

        }

    }
}
