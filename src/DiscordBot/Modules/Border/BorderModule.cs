using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;

namespace DiscordBot.Modules.Border
{
    public class BorderModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private string ssCsv;

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;

            manager.CreateCommands("border", group =>
            {
                group.CreateCommand("ss")
                    .Description("Returns the current Starlight Stage tier borders.")
                    .Do(e =>
                    {
                        return GetBorder(e, "ss");
                    });
                //group.CreateCommand("sif")
               //     .Description("Returns the current School Idol Festival tier borders.")
                //    .Do(e =>
               //     {
                //        return GetBorder(e, "ss");
                //    });
            });
        }

        private async Task GetBorder(CommandEventArgs e, string target)
        {
            ssCsv = "http://deresuteborder.web.fc2.com/csv/event_latest.csv";

            string csv, entry, prevEntry, formattedResult;
            string[] splitCsv, a, b;
            int[] delta = { };
            object[] args;
            TimeSpan elapsed;
            csv = GetCSV(ssCsv);
            splitCsv = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            entry = splitCsv.GetValue(splitCsv.Count() - 2).ToString();
            prevEntry = splitCsv.GetValue(splitCsv.Count() - 3).ToString();
            a = entry.Split(',');
            b = prevEntry.Split(',');

            delta = new int[] { 0, Int32.Parse(a[1]) - Int32.Parse(b[1]), Int32.Parse(a[2]) - Int32.Parse(b[2]), Int32.Parse(a[3]) - Int32.Parse(b[3]), Int32.Parse(a[4]) - Int32.Parse(b[4]), Int32.Parse(a[5]) - Int32.Parse(b[5]) };
            elapsed = DateTime.Parse(a[0]) - DateTime.Parse(b[0]);
            args = new object[] { a[0], a[1], a[2], a[3], a[4], a[5], delta[1], delta[2], delta[3], delta[4], delta[5], elapsed.TotalMinutes };

            formattedResult = String.Format("\nLast Updated: {0} (+{11} min)\nT1: {1} (+{6})\nT2: {2} (+{7})\nT3: {3} (+{8})\nT4: {4} (+{9})\nT5: {5} (+{10})", args);

            await _client.Reply(e, $"{formattedResult}");

        }
        public string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }
    }
}
