using Discord;
using Discord.Commands;
using Discord.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules.Random
{
    internal class RandomModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private System.Random x = new System.Random();

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
                group.CreateCommand("Risk plz")
                       .Description("Random 005 for Risk")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           if (x.NextDouble() > 0.5)
                           {
                               await e.Channel.SendMessage($"http://i.imgur.com/GUqtQJw.gif");
                           }
                           else
                           {
                               await e.Channel.SendMessage($"http://i.imgur.com/6r5RTzu.gif");
                           }
                       });

                group.CreateCommand("sleep")
                       .Parameter("Text", ParameterType.Optional)
                       .Description("Random 006 for sleep")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"{GetSleep(e)}");
                       });
            });

        }

        private string GetSleep(CommandEventArgs e)
        {
            string sleepUrl = "";
            string[] imageUrl = { "http://i.imgur.com/RKlZlOQ.jpg", "http://i.imgur.com/RG4umjv.png", "http://i.imgur.com/qcCNi62.jpg", "http://i.imgur.com/mZm7u5Y.png", "http://i.imgur.com/tEL3LID.jpg", "http://i.imgur.com/I5Wc8JB.jpg" };

            try
            {
                int request = Int32.Parse(e.Args[0]);

                if (request >= 0 && request < 6)
                {
                    sleepUrl = imageUrl[request];
                }
                else
                {
                    sleepUrl = imageUrl[x.Next(0, 6)];
                }
            }
            catch (Exception ex)
            {
                sleepUrl = imageUrl[x.Next(0, 6)];
            }

            return sleepUrl;
        }

    }
}
