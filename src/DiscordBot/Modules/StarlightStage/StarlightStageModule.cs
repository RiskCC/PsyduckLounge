using Discord;
using Discord.Commands;
using Discord.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBot.Modules.StarlightStage
{
    public class StarlightStageModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private string filePath = Environment.GetEnvironmentVariable("LocalAppData") + "\\UmiBot.bin";
        private Dictionary<string, string> writeDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> readDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private string result, name, id;

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;

            manager.CreateCommands("ss", group =>
            {
                group.CreateCommand("get")
                       .Parameter("Text", ParameterType.Required)
                       .Description("Gets user info based off account ID")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           GetMe(e);
                       });
                group.CreateCommand("add")
                       .Parameter("Text", ParameterType.Unparsed)
                       .Description("Adds user info to an account list")
                       .Do(async e =>
                       {
                           await e.Channel.SendIsTyping();
                           AddMe(e);
                           //await e.Channel.SendMessage($"coming soon (๑◕︵◕๑)");
                       });
            });
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

                writeDictionary[name] = id;
                Write(writeDictionary, filePath);

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
            try
            {               
                readDictionary = Read(filePath);
                readDictionary.TryGetValue(e.Args[0], out id);

                if (id == null)
                {
                    throw new SystemException();
                }

                result = $"https://deresute.me/{id}/medium";
            }
            catch (Exception ex)
            {
                if (Regex.IsMatch(e.Args[0], "^[0-9]{9}$"))
                {
                    result = $"https://deresute.me/{e.Args[0]}/medium";
                }
                else
                {
                    result = "Try again (๑◕︵◕๑)";
                }
            }

            await e.Channel.SendMessage($"{result}");
        }

        private static void Write(Dictionary<string, string> dictionary, string file)
        {
            using (FileStream fs = File.OpenWrite(file))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // Put count.
                writer.Write(dictionary.Count);
                // Write pairs.
                foreach (var pair in dictionary)
                {
                    writer.Write(pair.Key);
                    writer.Write(pair.Value);
                }
            }
        }

        private static Dictionary<string, string> Read(string file)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            using (FileStream fs = File.OpenRead(file))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // Get count.
                int count = reader.ReadInt32();
                // Read in all pairs.
                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    string value = reader.ReadString();
                    result[key] = value;
                }
            }
            return result;
        }
    }
}
