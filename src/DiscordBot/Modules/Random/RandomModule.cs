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
                       .Description("Random 001 for Jhazat")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"http://i.imgur.com/knRpikQ.gif");
                       });
                group.CreateCommand("Jhazat nudes")
                       .Description("Random 002 for Jhazat")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"http://i.imgur.com/sxO5h1N.gif");
                       });
                group.CreateCommand("Coco nudes")
                       .Description("Random 003 for Owl")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"http://i.imgur.com/mdzCSvz.jpg");
                       });
                group.CreateCommand("Zeraek nudes")
                       .Description("Random 004 for Zeraek")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"http://i.imgur.com/eLHoLCC.png");
                       });
            });

        }

    }
}
