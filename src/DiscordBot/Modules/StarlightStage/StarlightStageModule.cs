using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using DiscordBot.Modules.Border;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace DiscordBot.Modules.StarlightStage
{
    internal class StarlightStageModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private string filePath = "UmiBot.json";
        private string filePath2 = "tweetinvi.json";
        private List<Account> accounts = new List<Account>();
        private List<Keys> keys = new List<Keys>();
        private string result, name, id;
        private string consumerKey, consumerSecret, accessToken, accessTokenSecret;


        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;

            LoadJson();
            LoadKeys();
            Auth.SetUserCredentials(keys[0].consumerKey, keys[0].consumerSecret, keys[0].accessToken, keys[0].accessTokenSecret);

            manager.CreateCommands("ss", group =>
            {
                group.CreateCommand("help")
                       .Description("Returns the usage for SS module.")
                       .Do(e =>
                       {
                           return _client.Reply(e,
                               $"Usage:\n" +
                               "ss get [name or id] : get user by name or account ID\n" +
                               "ss add [name] [id]  : associate name to account ID\n");
                       });
                group.CreateCommand("get")
                       .Parameter("name | id", ParameterType.Required)
                       .Description("Gets user info based off account ID")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           GetMe(e);
                       });
                group.CreateCommand("add")
                       .Parameter("name id", ParameterType.Unparsed)
                       .Description("Adds user info to the account list")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           AddMe(e);
                       });
                group.CreateCommand("remove")
                       .Parameter("Text", ParameterType.Unparsed)
                       .Description("Removes a user from the account list")
                       .MinPermissions((int)PermissionLevel.ChannelAdmin)
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           RemoveMe(e);
                       });
                group.CreateCommand("border")
                       .Description("Alternate method to call border ss")
                       .Do(e =>
                       {
                           BorderModule bm = new BorderModule();
                           return bm.GetBorderSS(e, "");
                       });
                group.CreateCommand("prediction")
                       .Description("Get last prediction tweet")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           GetLastPredictionTweet(e);
                       });
            });
        }

        private async void GetLastPredictionTweet(CommandEventArgs e)
        {
            var accts = Search.SearchUsers("cindere_border");
            var acct = accts.First();
            var lastTweets = acct.GetUserTimeline(1);
            var lastTweet = lastTweets.FirstOrDefault();
            await e.Channel.SendMessage($"{lastTweet.Text.ToString()}");

        }

        private void LoadJson()
        {
            try
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    accounts = JsonConvert.DeserializeObject<List<Account>>(json);
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(filePath))
                    File.Create(filePath);
            }
        }
        private void LoadKeys()
        {
            using (StreamReader r = new StreamReader(filePath2))
            {
                string json = r.ReadToEnd();
                keys = JsonConvert.DeserializeObject<List<Keys>>(json);
            }
        }
        private async void AddMe(CommandEventArgs e)
        {
            try
            {
                string[] parsedArgs = e.Args[0].Split(' ');
                name = parsedArgs[0];

                if (Regex.IsMatch(parsedArgs[1], "^[0-9]{9}$"))
                {
                    id = parsedArgs[1];
                }
                else
                {
                    throw new SystemException("Invalid ID");
                }

                Write(name, id);

                result = $"{name.ToString()} added";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            await e.Channel.SendMessage($"{result}");
        }

        private async void GetMe(CommandEventArgs e)
        {
            // Discord caches images so we need to force a new image get
            var duck = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            try
            {
                id = Read(e.Args[0]);

                if (String.IsNullOrWhiteSpace(id))
                {
                    throw new SystemException();
                }
                result = $"https://deresute.me/{id}/medium?{duck}";
            }
            catch (Exception ex)
            {
                if (Regex.IsMatch(e.Args[0], "^[0-9]{9}$"))
                {
                    result = $"https://deresute.me/{e.Args[0]}/medium?{duck}";
                }
                else
                {
                    result = "not found; try again (๑◕︵◕๑)";
                }
            }

            await e.Channel.SendMessage($"{result}");
        }

        private async void RemoveMe(CommandEventArgs e)
        {
            try
            {
                //accounts.Remove(new Account() { name = e.Args[0], id = "000000000" });
                //await e.Channel.SendMessage($"{e.Args[0]} removed");
                await e.Channel.SendMessage($"this doesn't do anything yet, sorry~");

            }
            catch (Exception ex)
            {
                await e.Channel.SendMessage($"{e.Args[0]} not removed");
            }
        }


        private void Write(string name, string id)
        {
            Account psyduck = new Account()
            {
                name = name,
                id = id
            };

            int index = accounts.FindLastIndex(s => s.name == psyduck.name);
            if (index != -1)
            {
                accounts[index] = psyduck;
            }
            else
            {
                accounts.Add(psyduck);
            }

            string json = JsonConvert.SerializeObject(accounts.ToArray());

            System.IO.File.WriteAllText($"{filePath}", json);
        }

        private string Read(string name)
        {
            foreach (Account a in accounts)
            {
                if (String.Equals(a.name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return a.id;
                }
            }
            return "";
        }
    }

    internal class Keys
    {
        public string consumerKey;
        public string consumerSecret;
        public string accessToken;
        public string accessTokenSecret;
    }

    internal class Account
    {
        public string name;
        public string id;
    }
}
