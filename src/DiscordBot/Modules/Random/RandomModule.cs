using Discord;
using Discord.Commands;
using Discord.Modules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules.Random
{
    internal class RandomModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private System.Random x = new System.Random();
        private string imagesJson = "./config/images.json";

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
                group.CreateCommand("Neal nudes")
                       .Description("Random 005 for Neal")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"http://i.imgur.com/I4tI1tL.gif");
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
                group.CreateCommand("gj")
                       .Parameter("Text", ParameterType.Multiple)
                       .Description("Gets a GJ-bu gif")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"{GetGj(e)}");
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

        private string GetGj(CommandEventArgs e)
        {
            string gjUrl = "";
            LoadGjGif(imagesJson);
            string girl = "";
            int image = 0;

            try
            {
                image = Int32.Parse(e.Args[1]);
            }
            catch (Exception ex)
            {
                image = -1;
            }

            try
            {
                girl = e.Args[0];
                switch (girl)
                {
                    case "mao":
                        if (image == -1)
                            image = x.Next(0, gj.mao.Count());
                        gjUrl = gj.mao[image];
                        break;
                    case "shi":
                    case "shion":
                        if (image == -1)
                            image = x.Next(0, gj.shi.Count());
                        gjUrl = gj.shi[image];
                        break;
                    case "kirara":
                        if (image == -1)
                            image = x.Next(0, gj.kirara.Count());
                        gjUrl = gj.kirara[image];
                        break;
                    case "megu":
                    case "megumi":
                        if (image == -1)
                            image = x.Next(0, gj.megu.Count());
                        gjUrl = gj.megu[image];
                        break;
                    case "tama":
                    case "tamaki":
                        if (image == -1)
                            image = x.Next(0, gj.tama.Count());
                        gjUrl = gj.tama[image];
                        break;
                    case "other":
                        if (image == -1)
                            image = x.Next(0, gj.other.Count());
                        gjUrl = gj.other[image];
                        break;
                    default:
                        throw new KeyNotFoundException("girl not found");
                }
            }
            catch (Exception ex)
            {
                int randGrill = x.Next(0, 6);
                switch (randGrill)
                {
                    case 0:
                        image = x.Next(0, gj.mao.Count());
                        gjUrl = gj.mao[image];
                        break;
                    case 1:
                        image = x.Next(0, gj.shi.Count());
                        gjUrl = gj.shi[image];
                        break;
                    case 2:
                        image = x.Next(0, gj.kirara.Count());
                        gjUrl = gj.kirara[image];
                        break;
                    case 3:
                        image = x.Next(0, gj.megu.Count());
                        gjUrl = gj.megu[image];
                        break;
                    case 4:
                        image = x.Next(0, gj.tama.Count());
                        gjUrl = gj.tama[image];
                        break;
                    case 5:
                        image = x.Next(0, gj.other.Count());
                        gjUrl = gj.other[image];
                        break;
                }
            }
            return gjUrl;

        }

        private void LoadGjGif(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"{filePath} is missing.");
            gj = JsonConvert.DeserializeObject<GjGif>(File.ReadAllText(filePath));
        }

        internal class GjGif
        {
            public string[] mao;
            public string[] shi;
            public string[] kirara;
            public string[] megu;
            public string[] tama;
            public string[] other;
        }
        private GjGif gj = new GjGif();
    }
}
