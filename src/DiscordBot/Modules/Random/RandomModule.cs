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
        private string imagesJson = "./config/images.json";

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;

            LoadGifJson(imagesJson);

            manager.CreateCommands("", group =>
            {
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
                       .Parameter("girl #", ParameterType.Multiple)
                       .Description("Gets a GJ-bu gif")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"{GetGj(e)}");
                       });
                group.CreateCommand("yryr")
                       .Parameter("girl #", ParameterType.Multiple)
                       .Description("Gets a YuruYuri gif")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"{GetYryr(e)}");
                       });
                group.CreateCommand("addgif")
                       .Parameter("list", ParameterType.Required)
                       .Parameter("girl", ParameterType.Required)
                       .Parameter("url", ParameterType.Required)
                       .Description("Adds a gif to a list")
                       .MinPermissions((int)PermissionLevel.UserPlus)
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           await e.Channel.SendMessage($"{AddGif(e)}");
                       });
            });
        }

        private void WriteJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(giflist, Formatting.Indented);
            System.IO.File.WriteAllText($"{filePath}", json);
        }

        private string AddGif(CommandEventArgs e)
        {
            string series = e.Args[0];
            string girl = e.Args[1];
            string url = e.Args[2];

            switch (series)
            {
                case "gj":
                    try
                    {
                        string[] urlArray = gj[girl] as string[];
                        List<string> urlList = urlArray.ToList<string>();
                        urlList.Add(url);
                        gj[girl] = urlList.ToArray();
                    }
                    catch (Exception ex)
                    {
                        return $"Unsuccessful addgif using {series} {girl} {url}";
                    }
                    break;
                case "yryr":
                    try
                    {
                        string[] urlArray = yryr[girl] as string[];
                        List<string> urlList = urlArray.ToList<string>();
                        urlList.Add(url);
                        yryr[girl] = urlList.ToArray();
                    }
                    catch (Exception ex)
                    {
                        return $"Unsuccessful addgif using {series} {girl}";
                    }
                    break;
                default:
                    break;
            }

            WriteJson(imagesJson);
            return $"Add successful to {series} {girl}";
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

        private string GetYryr(CommandEventArgs e)
        {
            LoadGifJson(imagesJson);
            string girl = "";
            int image = 0;
            string[] urls = null;
            string url;

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
                urls = yryr[girl] as string[];
            }
            catch (Exception ex)
            {
                int randGrill = x.Next(0, yryr.GetType().GetProperties().Count() - 1);
                switch (randGrill)
                {
                    case 0:
                        image = x.Next(0, yryr.akari.Count());
                        url = yryr.akari[image];
                        break;
                    case 1:
                        image = x.Next(0, yryr.kyoko.Count());
                        url = yryr.kyoko[image];
                        break;
                    case 2:
                        image = x.Next(0, yryr.yui.Count());
                        url = yryr.yui[image];
                        break;
                    case 3:
                        image = x.Next(0, yryr.chinatsu.Count());
                        url = yryr.chinatsu[image];
                        break;
                    case 4:
                        image = x.Next(0, yryr.ayano.Count());
                        url = yryr.ayano[image];
                        break;
                    case 6:
                        image = x.Next(0, yryr.chitose.Count());
                        url = yryr.chitose[image];
                        break;
                    case 7:
                        image = x.Next(0, yryr.sakurako.Count());
                        url = yryr.sakurako[image];
                        break;
                    case 8:
                        image = x.Next(0, yryr.himawari.Count());
                        url = yryr.himawari[image];
                        break;
                    case 9:
                        image = x.Next(0, yryr.other.Count());
                        url = yryr.other[image];
                        break;
                    default:
                        image = x.Next(0, yryr.himawari.Count());
                        url = yryr.himawari[image];
                        break;
                }
                return $"{url}";
            }

            try
            {
                return $"{urls[image]}";
            }
            catch (Exception ex)
            {
                image = x.Next(0, urls.Count());
                return $"{urls[image]}";
            }

        }

        private string GetGj(CommandEventArgs e)
        {
            LoadGifJson(imagesJson);
            string url = "";
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
                        url = gj.mao[image];
                        break;
                    case "shi":
                    case "shion":
                        if (image == -1)
                            image = x.Next(0, gj.shi.Count());
                        url = gj.shi[image];
                        break;
                    case "kirara":
                        if (image == -1)
                            image = x.Next(0, gj.kirara.Count());
                        url = gj.kirara[image];
                        break;
                    case "megu":
                    case "megumi":
                        if (image == -1)
                            image = x.Next(0, gj.megu.Count());
                        url = gj.megu[image];
                        break;
                    case "tama":
                    case "tamaki":
                        if (image == -1)
                            image = x.Next(0, gj.tama.Count());
                        url = gj.tama[image];
                        break;
                    case "other":
                        if (image == -1)
                            image = x.Next(0, gj.other.Count());
                        url = gj.other[image];
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
                        url = gj.mao[image];
                        break;
                    case 1:
                        image = x.Next(0, gj.shi.Count());
                        url = gj.shi[image];
                        break;
                    case 2:
                        image = x.Next(0, gj.kirara.Count());
                        url = gj.kirara[image];
                        break;
                    case 3:
                        image = x.Next(0, gj.megu.Count());
                        url = gj.megu[image];
                        break;
                    case 4:
                        image = x.Next(0, gj.tama.Count());
                        url = gj.tama[image];
                        break;
                    case 5:
                        image = x.Next(0, gj.other.Count());
                        url = gj.other[image];
                        break;
                }
            }
            return url;

        }

        private void LoadGjGif(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"{filePath} is missing.");
            gj = JsonConvert.DeserializeObject<GjGif>(File.ReadAllText(filePath));
        }
        private void LoadGifJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"{filePath} is missing.");
            giflist = JsonConvert.DeserializeObject<Gif>(File.ReadAllText(filePath));
            gj = giflist.gjbu;
            yryr = giflist.yryr;
        }

        internal class Gif
        {
            [JsonProperty("gj")]
            public GjGif gjbu;
            [JsonProperty("yryr")]
            public YryrGif yryr;
        }
        private Gif giflist = new Gif();

        internal class YryrGif
        {
            public object this[string propertyName]
            {
                get
                {
                    Type myType = typeof(YryrGif);
                    PropertyInfo myPropertyInfo = myType.GetProperty(propertyName);
                    return myPropertyInfo.GetValue(this, null);
                }
                set
                {
                    Type myType = typeof(YryrGif);
                    PropertyInfo myPropertyInfo = myType.GetProperty(propertyName);
                    myPropertyInfo.SetValue(this, value, null);
                }
            }
            public string[] akari { get; set; }
            public string[] kyoko { get; set; }
            public string[] yui { get; set; }
            public string[] chinatsu { get; set; }
            public string[] ayano { get; set; }
            public string[] chitose { get; set; }
            public string[] sakurako { get; set; }
            public string[] himawari { get; set; }
            public string[] other { get; set; }
        }
        private YryrGif yryr = new YryrGif();

        internal class GjGif
        {
            public object this[string propertyName]
            {
                get
                {
                    Type myType = typeof(GjGif);
                    PropertyInfo myPropertyInfo = myType.GetProperty(propertyName);
                    return myPropertyInfo.GetValue(this, null);
                }
                set
                {
                    Type myType = typeof(GjGif);
                    PropertyInfo myPropertyInfo = myType.GetProperty(propertyName);
                    myPropertyInfo.SetValue(this, value, null);
                }
            }
            public string[] mao { get; set; }
            public string[] shi { get; set; }
            public string[] kirara { get; set; }
            public string[] megu { get; set; }
            public string[] tama { get; set; }
            public string[] other { get; set; }
        }
        private GjGif gj = new GjGif();
    }
}
