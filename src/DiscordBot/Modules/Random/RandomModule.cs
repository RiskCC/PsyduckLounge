using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                group.CreateCommand("bad pull")
                       .Description("For when you get a bad pull...")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           switch (x.Next(3))
                           {
                               case 0:
                                   await e.Channel.SendMessage($"http://i.imgur.com/OpQjMan.gif");
                                   break;
                               case 1:
                                   await e.Channel.SendMessage($"http://i.imgur.com/5hfeKVU.gif");
                                   break;
                               case 2:
                                   await e.Channel.SendMessage($"http://i.imgur.com/zBnDDQ0.gif");
                                   break;
                               default:
                                   await e.Channel.SendMessage($"http://i.imgur.com/zBnDDQ0.gif");
                                   break;
                           }
                       });
                group.CreateCommand("jew")
                       .Description("A picture of a Jew")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"http://i.imgur.com/DUJ0IG5.png");
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
            string[] imageUrl = {
                "http://i.imgur.com/RKlZlOQ.jpg",
                "http://i.imgur.com/RG4umjv.png",
                "http://i.imgur.com/qcCNi62.jpg",
                "http://i.imgur.com/mZm7u5Y.png",
                "http://i.imgur.com/tEL3LID.jpg",
                "http://i.imgur.com/I5Wc8JB.jpg"
            };

            int maxImageUrls = 6;
            try
            {
                int request = Int32.Parse(e.Args[0]);

                if (request >= 0 && request < maxImageUrls)
                {
                    sleepUrl = imageUrl[request];
                }
                else
                {
                    sleepUrl = imageUrl[x.Next(0, maxImageUrls)];
                }
            }
            catch (Exception ex)
            {
                sleepUrl = imageUrl[x.Next(0, maxImageUrls)];
            }

            return sleepUrl;
        }
    }
}
